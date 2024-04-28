import {MinMaxRange} from "../../stores/transactionQueryStore.ts";
import {useEffect, useMemo, useRef, useState} from "react";
import {Category} from "../../entities/Category.ts";
import { getMinMaxRangeFromWallets, getUniqueCategoriesFromWallets} from "../../helpers/globalHelper.ts";
import MultiSelectDropDownMenu from "../other/MultiSelectDropDownMenu.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";
import ReactSlider from "react-slider";
import { Wallet } from "../../entities/Wallet.ts";
import useWalletInfoStore from "../../stores/walletStore.ts";
import WalletItem from "./WalletItem.tsx";

interface WalletsOtherFiltersProps {
    wallets: Wallet[]
}

export default function WalletsOtherFilters({wallets} : WalletsOtherFiltersProps){

    const [filterCategories, filterWallets, filterMinMax, filterNote, setFilterCategories, setFilterWallets, setFilterMinMax, setFilterNote
    ] = useWalletInfoStore(x =>
        [x.filters.categories,
            x.filters.selectedWallets, x.filters.minMaxRange, x.filters.note, x.setCategories, x.setWallets, x.setMinMaxRange, x.setNote]);


    const [isFilterMenuOpen, setIsFilterMenuOpen] = useState(true);
    const initialCategories = useRef<Category[]>([]);
    const initialWallets = useRef<Wallet[]>([]);
    const initialMinMax = useRef<MinMaxRange>({min: 1, max: 1000});

    useEffect(() => {
        if (wallets) {
            const uniqueCategories = getUniqueCategoriesFromWallets(wallets);
            const minMaxRange = getMinMaxRangeFromWallets(wallets);
            initialCategories.current = uniqueCategories;
            initialWallets.current = wallets;
            initialMinMax.current = minMaxRange;

            setFilterCategories(uniqueCategories);
            setFilterWallets(wallets)
            setFilterMinMax(minMaxRange);
        }
    }, [wallets]);

//Getting default filters
    const minMaxRange = useMemo(() => getMinMaxRangeFromWallets(wallets || []), [wallets]);
    const uniqueCategories = getUniqueCategoriesFromWallets(filterWallets);


    const handleToggleCategory = (category: Category) => {

        if (filterCategories.includes(category)) {
            const newCategories = filterCategories.filter(u => u.id !== category.id);
            setFilterCategories(newCategories);
        } else {
            setFilterCategories([...filterCategories, category]);
        }
    };
    const handleSelectAllCategories = () => {
        if (filterCategories.length === uniqueCategories.length) {
            setFilterCategories([]);
        } else {
            setFilterCategories(uniqueCategories);
        }
    }
    const handleToggleWallet = (wallet: Wallet) => {

        if (filterWallets.includes(wallet)) {
            const newWallets = filterWallets.filter(u => u.id !== wallet.id);
            const newMinMax = getMinMaxRangeFromWallets(newWallets)
            setFilterMinMax(newMinMax)
            setFilterWallets(newWallets);
        } else {
            const newMinMax = getMinMaxRangeFromWallets([...filterWallets, wallet])
            setFilterMinMax(newMinMax)
            setFilterWallets([...filterWallets, wallet]);
        }
    };
    const handleSelectAllWallets = () => {
        if (filterWallets.length === wallets.length) {
            setFilterWallets([]);
            const newMinMax = getMinMaxRangeFromWallets([])
            setFilterMinMax(newMinMax)
        } else {
            const newMinMax = getMinMaxRangeFromWallets(wallets)
            setFilterMinMax(newMinMax)
            setFilterWallets(wallets);
        }
    }
    const handleNoteFilterChanged = (note: string) => setFilterNote(note);
    const resetFilters = () => {
        setFilterCategories(initialCategories.current);
        setFilterWallets(initialWallets.current);
        setFilterMinMax(initialMinMax.current);
        setFilterNote("");
    };

    const toggleFilterMenu = () => setIsFilterMenuOpen(p => !p);

    return (
        <div
            className={'bg-slate-200 rounded-xl shadow-lg'}>
            <div className={'w-full h-full'}
                 onClick={toggleFilterMenu}
            >
                <header className={'flex justify-between  py-4 px-3'}>
                    <p className={'font-bold text-lg'}>Filters</p>
                    <button onClick={e => {
                        e.preventDefault()
                        resetFilters()
                    }} className={'text-center underline underline-offset-2'}>
                        Reset filters
                    </button>
                </header>
            </div>
            {isFilterMenuOpen && <div
                className={'grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 justify-center items-center gap-y-3 gap-x-10 px-3 pb-4'}>
                <div className={'w-full flex flex-col gap-y-2'}>
                    <p className={'font-semibold'}>By wallet</p>
                    <MultiSelectDropDownMenu items={wallets} ItemComponent={WalletItem}
                                             heading={"Wallets"} onItemSelected={handleToggleWallet}
                                             onAllItemsSelected={handleSelectAllWallets}
                                             selectedItems={filterWallets}/>
                </div>
                <div className={'w-full flex flex-col gap-y-2'}>
                    <p className={'font-semibold'}>By category</p>
                    <MultiSelectDropDownMenu items={uniqueCategories} ItemComponent={CategoryItem}
                                             heading={"Categories"} onItemSelected={handleToggleCategory}
                                             onAllItemsSelected={handleSelectAllCategories}
                                             selectedItems={filterCategories}/>
                </div>
                <div className={'w-full flex flex-col gap-y-2'}>
                    <p className={'font-semibold'}>By Note</p>
                    <input type="text"
                           value={filterNote}
                           className={'w-full px-2 py-1 rounded text-gray-400 focus:outline-0'}
                           onClick={e => e.stopPropagation()}
                           onChange={e => handleNoteFilterChanged(e.target.value)}
                    />
                </div>
                <div className={'w-full flex flex-col gap-y-2'}>
                    <p className={'font-semibold'}>By Amount</p>
                    <div
                        onClick={e => e.stopPropagation()}
                    >
                        <ReactSlider
                            className="horizontal-slider"
                            thumbClassName="example-thumb"
                            trackClassName="example-track"
                            ariaLabel={['Lower thumb', 'Upper thumb']}
                            value={[filterMinMax.min, filterMinMax.max]}
                            min={minMaxRange.min}
                            max={minMaxRange.max}
                            minDistance={1}
                            pearling
                            onChange={(n) => {
                                const [minValue, maxValue] = n;
                                setFilterMinMax({min: minValue, max: maxValue})
                            }}
                        />
                        <div className={'flex justify-between items-center px-1'}>
                            <p>{filterMinMax.min}</p>
                            <p>{filterMinMax.max}</p>
                        </div>
                    </div>
                </div>
            </div>}
        </div>
    )
}