import {useParams} from "react-router-dom";
import useWallet from "../../hooks/wallet/useWallet.ts";
import useUserStore from "../../stores/userStore.ts";
import useTransactions from "../../hooks/transactions/useTransactions.ts";
import useTransactionQueryStore, {MinMaxRange} from "../../stores/transactionQueryStore.ts";
import TransactionsDateFilters from "../../components/transactions/TransactionsDateFilters.tsx";
import Spinner from "../../components/other/Spinner.tsx";
import CreateTransactionModal from "../../components/transactions/CreateTransactionModal.tsx";
import TransactionList from "../../components/transactions/TransactionList.tsx";
import {Category} from "../../entities/Category.ts";
import MultiSelectDropDownMenu from "../../components/other/MultiSelectDropDownMenu.tsx";
import CategoryItem from "../../components/categories/CategoryItem.tsx";
import {useEffect, useMemo, useRef, useState} from "react";
import {User} from "../../entities/User.ts";
import UserItem from "../../components/auth/UserItem.tsx";
import ReactSlider from "react-slider";
import {Transaction} from "../../entities/Transaction.ts";
import {formatDate} from "../../helpers/globalHelper.ts";


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

export default function WalletTransactionsPage() {
    const {walletId} = useParams();
    const userId = useUserStore(x => x.getUserId());
    const [
        startDate, endDate, setStartDate, setEndDate, filterCategories,
        filterUsers, filterMinMax, setFilterCategories, setFilterUsers, setFilterMinMax
    ] = useTransactionQueryStore(x =>
        [x.query.startDate, x.query.endDate, x.setStartDate, x.setEndDate, x.filters.categories,
            x.filters.users, x.filters.minMaxRange, x.setCategories, x.setUsers, x.setMinMaxRange]);
    const {data: transactions} = useTransactions(walletId!)
    const {data: walletResponse} = useWallet(walletId!);
    const [noteFilter, setNoteFilter] = useState("");


    const initialCategories = useRef<Category[]>([]);
    const initialUsers = useRef<User[]>([]);
    const initialMinMax = useRef<MinMaxRange>({min: 1, max: 1000});

    useEffect(() => {
        if (transactions) {
            const uniqueCategories = getUniqueCategories(transactions);
            const uniqueUsers = getUniqueUsers(transactions);
            const minMaxRange = getMinMaxRange(transactions);

            // Зберігаємо початкові значення в refs
            initialCategories.current = uniqueCategories;
            initialUsers.current = uniqueUsers;
            initialMinMax.current = minMaxRange;

            // Встановлюємо початкові значення фільтрів
            setFilterCategories(uniqueCategories);
            setFilterUsers(uniqueUsers);
            setFilterMinMax(minMaxRange);
        }
    }, [transactions]);

    const minMaxRange = useMemo(() => getMinMaxRange(transactions || []), [transactions]);


    if (!transactions || !walletResponse || !walletResponse.response) return <Spinner/>
    const wallet = walletResponse.response;

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
    const handleDateFilterChange = (date: Date, isStartDate: boolean) => {
        if (isStartDate) setStartDate(formatDate(date))
        if (!isStartDate) setEndDate(formatDate(date))
    }
    const handleNoteFilterChanged = (note: string) => setNoteFilter(note);

    const resetFilters = () => {
        setFilterCategories(initialCategories.current);
        setFilterUsers(initialUsers.current);
        setFilterMinMax(initialMinMax.current);
        setNoteFilter("");
    };

    return (
        <div className={'container mx-auto p-4'}>
            <div className={'flex flex-col sm:flex-row justify-between items-center gap-5'}>
                <div className={''}>
                    {!wallet.isBanking &&
                        <CreateTransactionModal userId={userId!} walletId={wallet.id}
                                                walletCurrency={wallet.currency}/>}
                </div>
                <div>
                    <TransactionsDateFilters startDate={startDate!} endDate={endDate!}
                                             handleDateFilterChange={handleDateFilterChange}/>
                </div>
            </div>
            <div className={'mt-8 bg-slate-200 rounded shadow-lg py-4 px-3'}>
                <header className={'flex justify-between'}>
                    <p className={'font-bold text-lg px-1'}>Filters</p>
                    <button onClick={resetFilters} className={'text-center underline underline-offset-2'}>Reset filters</button>
                </header>
                <div className={'grid grid-cols-1 sm:grid-cols-4 justify-center items-center gap-10'}>
                    <div className={'w-full flex flex-col gap-y-2'}>
                        <p className={'font-semibold px-1'}>By category</p>
                        <MultiSelectDropDownMenu items={uniqueCategories} ItemComponent={CategoryItem}
                                                 heading={"Categories"} onItemSelected={handleToggleCategory}
                                                 onAllItemsSelected={handleSelectAllCategories}
                                                 selectedItems={filterCategories}/>
                    </div>
                    <div className={'w-full flex flex-col gap-y-2'}>
                        <p className={'font-semibold px-1'}>By people</p>
                        <MultiSelectDropDownMenu items={uniqueUsers} ItemComponent={UserItem}
                                                 heading={"Users"} onItemSelected={handleToggleUser}
                                                 onAllItemsSelected={handleSelectAllUsers}
                                                 selectedItems={filterUsers}/>
                    </div>
                    <div className={'w-full flex flex-col gap-y-2'}>
                        <p className={'font-semibold px-1'}>By Note</p>
                        <input type="text"
                               value={noteFilter}
                               className={'w-full px-2 py-1 rounded text-gray-400 focus:outline-0'}
                               onChange={e => handleNoteFilterChanged(e.target.value)}
                        />
                    </div>
                    <div className={'w-full flex flex-col gap-y-2'}>
                        <p className={'font-semibold px-1'}>By Amount</p>
                        <div >
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
                </div>
            </div>
            <div className={'mt-4'}>
                <TransactionList transactions={transactions} walletSymbol={wallet.currency.symbol}/>
            </div>
        </div>
    )
}






