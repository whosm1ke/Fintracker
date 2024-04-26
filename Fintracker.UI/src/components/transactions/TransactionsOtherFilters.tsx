import MultiSelectDropDownMenu from "../other/MultiSelectDropDownMenu.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";
import UserItem from "../auth/UserItem.tsx";
import ReactSlider from "react-slider";
import {Category} from "../../entities/Category.ts";
import {User} from "../../entities/User.ts";
import {useEffect, useMemo, useRef, useState} from "react";
import useTransactionQueryStore, {MinMaxRange} from "../../stores/transactionQueryStore.ts";
import {Transaction} from "../../entities/Transaction.ts";

interface TransactionsOtherFiltersProps {
    transactions: Transaction[]
}

function getUniqueCategories(transactions: Transaction[]): Category[] {
    const categories = transactions.map(transaction => transaction.category);
    return Array.from(new Set(categories.map(category => category.name)))
        .map(name => categories.find(category => category.name === name)!);
}

function getUniqueUsers(transactions: Transaction[]): User[] {
    const users = transactions.map(t => t.user);
    const uniqueUsers = Array.from(new Set(users.map(u => u.id)))
        .map(id => users.find(u => u.id === id)!);
    return uniqueUsers;
}


function getMinMaxRange(transactions: Transaction[]): MinMaxRange {

    if (transactions.length === 0)
        return {min: 1, max: 1000}
    const amounts = transactions.map(transaction => transaction.amount);
    const min = Math.min(...amounts);
    const max = Math.max(...amounts);


    return {min, max};
}

export default function TransactionsOtherFilters({transactions}: TransactionsOtherFiltersProps) {
    const [filterCategories, filterUsers, filterMinMax, filterNote, setFilterCategories, setFilterUsers, setFilterMinMax, setFilterNote
    ] = useTransactionQueryStore(x =>
        [x.filters.categories,
            x.filters.users, x.filters.minMaxRange, x.filters.note, x.setCategories, x.setUsers, x.setMinMaxRange, x.setNote]);

    const [isFilterMenuOpen, setIsFilterMenuOpen] = useState(true);
    const initialCategories = useRef<Category[]>([]);
    const initialUsers = useRef<User[]>([]);
    const initialMinMax = useRef<MinMaxRange>({min: 1, max: 1000});

    useEffect(() => {
        if (transactions) {
            const uniqueCategories = getUniqueCategories(transactions);
            const uniqueUsers = getUniqueUsers(transactions);
            const minMaxRange = getMinMaxRange(transactions);

            initialCategories.current = uniqueCategories;
            initialUsers.current = uniqueUsers;
            initialMinMax.current = minMaxRange;

            setFilterCategories(uniqueCategories);
            setFilterUsers(uniqueUsers);
            setFilterMinMax(minMaxRange);
        }
    }, [transactions]);
    

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
    };

    const toggleFilterMenu = () => setIsFilterMenuOpen(p => !p);

    return (
        <div
            onClick={toggleFilterMenu}
            className={'mt-8 bg-slate-200 rounded shadow-lg py-4 px-3'}>
            <header className={'flex justify-between px-1'}>
                <p className={'font-bold text-lg'}>Filters</p>
                <button onClick={resetFilters} className={'text-center underline underline-offset-2'}>
                    Reset filters
                </button>
            </header>
            {isFilterMenuOpen && <div
                className={'grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 justify-center items-center gap-y-3 gap-x-10 mt-2'}>
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
                           onChange={e => handleNoteFilterChanged(e.target.value)}
                    />
                </div>
                <div className={'w-full flex flex-col gap-y-2'}>
                    <p className={'font-semibold'}>By Amount</p>
                    <div>
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