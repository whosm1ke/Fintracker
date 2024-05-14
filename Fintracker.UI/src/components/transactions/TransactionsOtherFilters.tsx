import MultiSelectDropDownMenu from "../other/MultiSelectDropDownMenu.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";
import UserItem from "../auth/UserItem.tsx";
import ReactSlider from "react-slider";
import {Category} from "../../entities/Category.ts";
import {User} from "../../entities/User.ts";
import {useEffect, useMemo, useRef, useState} from "react";
import useTransactionQueryStore, {MinMaxRange} from "../../stores/transactionQueryStore.ts";
import {Transaction} from "../../entities/Transaction.ts";
import {
    getMinMaxRange,
    getUniqueCategories, getUniqueUsers
} from "../../helpers/globalHelper.ts";
import SingleSelectDropDownMenu from "../other/SingleSelectDropDownMenu.tsx";
import TransactionItemSelector, { TransactionSelectorMap } from "./TransactionItemSelector.tsx";

interface TransactionsOtherFiltersProps {
    transactions: Transaction[];
}

const transactionMap: TransactionSelectorMap[] = [
    {
        id: "1",
        value: undefined
    },
    {
        id: "2",
        value: 5
    },
    {
        id: "3",
        value: 10
    },
    {
        id: "4",
        value: 15
    },
    {
        id: "5",
        value: 20
    },
    {
        id: "6",
        value: 25
    },
    {
        id: "7",
        value: 30
    }
]

export default function TransactionsOtherFilters({transactions}: TransactionsOtherFiltersProps) {
    const [filterCategories, filterUsers, filterMinMax, filterNote, setFilterCategories, setFilterUsers, setFilterMinMax, setFilterNote, setTransactionsPerDate
    ] = useTransactionQueryStore(x =>
        [x.filters.categories,
            x.filters.users, x.filters.minMaxRange, x.filters.note, x.setCategories, x.setUsers, x.setMinMaxRange, x.setNote, x.setTransactionsPerDate]);


    const [isFilterMenuOpen, setIsFilterMenuOpen] = useState(true);
    const initialCategories = useRef<Category[]>([]);
    const initialUsers = useRef<User[]>([]);
    const initialMinMax = useRef<MinMaxRange>({min: 1, max: 1000});
    const [transactionsPerDate, setTransactionsPerDateInternal] = useState<TransactionSelectorMap>(transactionMap[0])
    useEffect(() => {
        if (transactions) {
            const uniqueCategories = getUniqueCategories(transactions);
            const uniqueUsers = getUniqueUsers(transactions);
            const minMaxRange = getMinMaxRange(transactions);
            initialCategories.current = uniqueCategories;
            initialUsers.current = uniqueUsers;
            initialMinMax.current = minMaxRange;

            setFilterCategories(uniqueCategories);
            setFilterUsers(uniqueUsers)
            setFilterMinMax(minMaxRange);
        }
    }, [transactions]);

//Getting default filters
    const minMaxRange = useMemo(() => getMinMaxRange(transactions || []), [transactions]);
    const uniqueCategories = getUniqueCategories(transactions);
    const uniqueUsers = getUniqueUsers(transactions);


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

    const handleToggleTransactionsPerPeriod = (transPerPeriod: TransactionSelectorMap) => {
        setTransactionsPerDate(transPerPeriod.value)
        setTransactionsPerDateInternal(transPerPeriod);
    }
    const handleToggleUser = (user: User) => {

        if (filterUsers.includes(user)) {
            const newUsers = filterUsers.filter(u => u.id !== user.id);
            setFilterUsers(newUsers);
        } else {
            setFilterUsers([...filterUsers, user]);
        }
    };
    const handleSelectAllUsers = () => {
        if (filterUsers.length === uniqueUsers.length) {
            setFilterUsers([]);
        } else {
            setFilterUsers(uniqueUsers);
        }
    }
    const handleNoteFilterChanged = (note: string) => setFilterNote(note);
    const resetFilters = () => {
        setFilterCategories(initialCategories.current);
        setFilterUsers(initialUsers.current);
        setFilterMinMax(initialMinMax.current);
        setFilterNote("");
        setTransactionsPerDate(0);
        setTransactionsPerDateInternal(transactionMap[0])
    };

    const toggleFilterMenu = () => setIsFilterMenuOpen(p => !p);

    if (transactions.length === 0) return null;

    return (
        <div
            className={'bg-slate-200 rounded-xl shadow-lg'}>
            <div className={'w-full h-full'}
                 onClick={toggleFilterMenu}
            >
                <header className={'flex justify-between  py-4 px-3'}>
                    <p className={'font-bold text-lg'}>Filters</p>
                    <button onClick={e => {
                        e.stopPropagation()
                        resetFilters()
                    }} className={'text-center underline underline-offset-2'}>
                        Reset filters
                    </button>
                </header>
            </div>
            {isFilterMenuOpen && <div
                className={'grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 justify-center items-center gap-y-3 gap-x-10 px-3 pb-4'}>
                <div className={'w-full flex flex-col gap-y-2'}>
                    <p className={'font-semibold'}>By category</p>
                    <MultiSelectDropDownMenu items={uniqueCategories} ItemComponent={CategoryItem}
                                             heading={"Categories"} onItemSelected={handleToggleCategory}
                                             onAllItemsSelected={handleSelectAllCategories}
                                             selectedItems={filterCategories}/>
                </div>
                <div className={'w-full flex flex-col gap-y-2'}>
                    <p className={'font-semibold'}>By people</p>
                    <MultiSelectDropDownMenu items={uniqueUsers} ItemComponent={UserItem}
                                             heading={"Users"} onItemSelected={handleToggleUser}
                                             onAllItemsSelected={handleSelectAllUsers}
                                             selectedItems={filterUsers}/>
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
                    <p className={'font-semibold'}>Transactions per period</p>
                    <SingleSelectDropDownMenu items={transactionMap} onItemSelected={handleToggleTransactionsPerPeriod}
                    ItemComponent={TransactionItemSelector} defaultSelectedItem={transactionsPerDate}
                    heading={"Transactions per period"} HeadingComponent={TransactionItemSelector}/>
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

