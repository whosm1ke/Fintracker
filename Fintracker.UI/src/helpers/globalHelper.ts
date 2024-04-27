import {ConvertCurrency} from "../entities/Currency.ts";
import {GroupedTransactionByDate, Transaction} from "../entities/Transaction.ts";
import {CategoryType} from "../entities/CategoryType.ts";
import {User} from "../entities/User.ts";
import {MinMaxRange, CommonFilters} from "../stores/transactionQueryStore.ts";
import {Category} from "../entities/Category.ts";
import { Wallet } from "../entities/Wallet.ts";

export function formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

export const getCurrencyRates = (convertedCurrencies: ConvertCurrency[] | undefined, uniqueSymbols: string[]) => {
    const currencyRates: UniqueCurrencyRates = {};
    if (convertedCurrencies) {
        convertedCurrencies.forEach((rate, i) => {
            currencyRates[uniqueSymbols[i]] = rate.value;
        });

        return currencyRates;
    }

    return null;
}

export const calculateWalletsBalance = (wallets: Wallet[], convertionRate: UniqueCurrencyRates) : number => {
    let result = 0;
    
    wallets.forEach(w => {
        result += w.balance * (convertionRate[w.currency.symbol] || 1)
    })
    
    return result;
}

export const calculateTotalExpense = (transactions: Transaction[], convertionRate: UniqueCurrencyRates) => {
    let total = 0;
    transactions.forEach(t => {
        if (t.category.type === CategoryType.EXPENSE)
            total -= t.amount * (convertionRate[t.currency.symbol] || 1)
        else
            total += t.amount * (convertionRate[t.currency.symbol] || 1)
    });
    return total;
}

export const getUniqueCurrencySymbols = (items: Transaction[] | Wallet[]) => {
    
    const symbols = items.map(t => t.currency.symbol);
    return [...new Set(symbols)]
}

export type ExpenseAndIncome = {
    expense: number;
    income: number;
}
export function calcExpenseAndIncome(transactions: Transaction[], currencyRates: UniqueCurrencyRates | null): ExpenseAndIncome {
    let expense = 0;
    let income = 0;

    console.log("currencyRates: ", currencyRates)

    transactions.forEach(t => {
        if (t.category.type === CategoryType.EXPENSE)
            expense -= t.amount * (currencyRates ? currencyRates[t.currency.symbol] : 1);
        if (t.category.type === CategoryType.INCOME)
            income += t.amount * (currencyRates ? currencyRates[t.currency.symbol] : 1);
    });


    return {
        expense: expense,
        income: income
    }
}

export function getUniqueCategories(transactions: Transaction[]): Category[] {
    const categories = transactions.map(transaction => transaction.category);
    return Array.from(new Set(categories.map(category => category.name)))
        .map(name => categories.find(category => category.name === name)!);
}

export function getUniqueUsers(transactions: Transaction[]): User[] {
    const users = transactions.map(t => t.user);
    return Array.from(new Set(users.map(u => u.id)))
        .map(id => users.find(u => u.id === id)!);
}

export function getUniqueUsersAndOwners(wallets: Wallet[]): User[] {
    const uniqueUsersAndOwners = new Set<User>();
    wallets.forEach(wallet => {
        uniqueUsersAndOwners.add(wallet.owner);

        wallet.users.forEach(user => uniqueUsersAndOwners.add(user));
    });

    return Array.from(uniqueUsersAndOwners);
}

export function getMinMaxRange(transactions: Transaction[]): MinMaxRange {

    if (transactions.length === 0)
        return {min: 1, max: 1000}
    const amounts = transactions.map(transaction => transaction.amount);
    const min = Math.min(...amounts);
    const max = Math.max(...amounts);


    return {min, max};
}

export function filterTransactions(transactions: Transaction[], filters: CommonFilters): Transaction[] {
    return transactions.filter(transaction => {
        // Фільтрація за категоріями
        if (!filters.categories.map(c => c.id).includes(transaction.category.id)) {
            return false;
        }

        // Фільтрація за користувачами
        if (!filters.users.map(u => u.id).includes(transaction.userId)) {
            return false;
        }

        // Фільтрація за мінімальним і максимальним значеннями
        if (transaction.amount < filters.minMaxRange.min || transaction.amount > filters.minMaxRange.max) {
            return false;
        }

        // Фільтрація за нотатками
        if (filters.note && (!transaction.note || !transaction.note.toLowerCase().includes(filters.note.toLowerCase()))) {
            return false;
        }
        return true;
    });
}

export const groupTransactionsByDate = (transactions: Transaction[]) => {
    let transactionContainer: GroupedTransactionByDate[] = [];
    transactions.forEach(transaction => {
        const transDate = new Date(transaction.date);
        const date = transDate.toLocaleDateString('en-CA'); // format: YYYY-MM-DD

        let group = transactionContainer.find(x => new Date(x.date).toLocaleDateString('en-CA') === date);

        if (!group) {
            group = {
                date: new Date(date),
                transactions: []
            };
            transactionContainer.push(group);
        }

        group.transactions.push(transaction);
    });

    return transactionContainer;
}

export type UniqueCurrencyRates = { [key: string]: number }