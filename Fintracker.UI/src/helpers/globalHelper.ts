import {ConvertCurrency} from "../entities/Currency.ts";
import {GroupedTransactionByDate, Transaction} from "../entities/Transaction.ts";
import {CategoryType} from "../entities/CategoryType.ts";
import {User} from "../entities/User.ts";
import {MinMaxRange} from "../stores/transactionQueryStore.ts";
import {Category} from "../entities/Category.ts";
import {Wallet} from "../entities/Wallet.ts";
import { Budget } from "../entities/Budget.ts";

export function dateToString(date: Date): string {
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

export const calculateWalletsBalance = (wallets: Wallet[], convertionRate: UniqueCurrencyRates | null): number => {
    let result = 0;


    wallets.forEach(w => {
        result += w.balance * (convertionRate ? convertionRate[w.currency.symbol] || 1 : 1)
    })

    return result;
}

export const calculateTotalExpense = (transactions: Transaction[]) => {
    let total = 0;
    transactions.forEach(t => {
        if (t.category.type === CategoryType.EXPENSE)
            total -= t.amountInWalletCurrency;
        else
            total += t.amountInWalletCurrency;
    });
    return total;
}


export const getUniqueCurrencySymbolsFromWallets = (wallets: Wallet[]) => {
    const symbols = wallets.map(w => {
        if (w && w.currency)
            return w.currency.symbol;
        return "";
    });
    return [...new Set(symbols)]
}

export const getUniqueCurrencySymbols = (items: Transaction[] | Wallet[]) => {

    const symbols = items.map(t => {
        if (t && t.currency)
            return t.currency.symbol;
        return "";
    });
    return [...new Set(symbols)]
}

export type ExpenseAndIncome = {
    expense: number;
    income: number;
}

export function calcExpenseAndIncome(transactions: Transaction[]): ExpenseAndIncome {
    let expense = 0;
    let income = 0;


    transactions.forEach(t => {
        if (t.category.type === CategoryType.EXPENSE)
            expense -= t.amountInWalletCurrency;
        if (t.category.type === CategoryType.INCOME)
            income += t.amountInWalletCurrency
    });


    return {
        expense: expense,
        income: income
    }
}

export function getUniqueCategories(transactions: Transaction[]): Category[] {
    const transWithCats = transactions.filter(transaction => {
        if (transaction && transaction.category)
            return transaction;
    });

    const categories = transWithCats.map(t => t.category)

    return Array.from(new Set(categories.map(category => category.name)))
        .map(name => categories.find(category => category.name === name)!);
}

export function getUniqueCategoriesFromWallets(wallets: Wallet[]): Category[] {
    return getUniqueCategories(wallets.flatMap(w => w.transactions))
}

export function getUniqueUsers(transactions: Transaction[]): User[] {
    const users = transactions.map(t => t.user);
    return Array.from(new Set(users.map(u => u.id)))
        .map(id => users.find(u => u.id === id)!);
}


export function getMinMaxRange(transactions: Transaction[]): MinMaxRange {


    if (transactions && transactions.length === 0)
        return {min: 1, max: 1000}

    const incomeTransactions = transactions.filter(t => t?.category.type === CategoryType.INCOME)
    const expenseTransactions = transactions.filter(t => t?.category.type === CategoryType.EXPENSE)

    const incomeAmounts = incomeTransactions.map(transaction => {
        if (transaction)
            return transaction.amount;
        return 0;
    });

    const expenseAmounts = expenseTransactions.map(transaction => {
        if (transaction)
            return -transaction.amount;
        return 0;
    });
    let min = Math.min(...expenseAmounts, ...incomeAmounts);
    let max = Math.max(...expenseAmounts, ...incomeAmounts);

    if (min === Infinity || min === -Infinity)
        min = 0;
    if (max === Infinity || max === -Infinity)
        max = 1;
    return {min, max};
}

export function getMinMaxRangeFromWallets(wallets: Wallet[]): MinMaxRange {

    return getMinMaxRange(wallets.flatMap(w => w.transactions))
}

type CommonFilters = {
    categories: Category[];
    users: User[];
    minMaxRange: MinMaxRange;
    note: string
}

export function filterTransactions(transactions: Transaction[], filters: CommonFilters, includeUsers: boolean = true): Transaction[] {
    return transactions.filter(transaction => {

        if (!transaction) return false;

        // Фільтрація за категоріями
        if (!filters.categories.map(c => c?.id).includes(transaction.category.id)) {
            return false;
        }

        if (includeUsers) {
            // Фільтрація за користувачами
            if (!filters.users.map(u => u.id).includes(transaction.userId)) {
                return false;
            }
        }

        const amount = transaction.category.type === CategoryType.EXPENSE ? transaction.amount * -1 : transaction.amount

        // Фільтрація за мінімальним і максимальним значеннями
        if (amount < filters.minMaxRange.min || amount > filters.minMaxRange.max) {
            return false;
        }

        // Фільтрація за нотатками
        if (filters.note && (!transaction.note || !transaction.note.toLowerCase().includes(filters.note.toLowerCase()))) {
            return false;
        }
        return true;

    });
}

export const calculateDailyBudget = (budget: Budget) => {
    const startDate = new Date(budget.startDate);
    const endDate = new Date(budget.endDate);

    const diffTime = Math.abs(endDate.getTime() - startDate.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    const dailyBudget = budget.startBalance / diffDays;

    return dailyBudget;
}




export function filterTransactionsByDate(wallets: Wallet[], startDate: string, endDate: string): Transaction[] {
    // Convert the start and end dates to Date objects
    const start = new Date(startDate);
    start.setHours(0, 0, 0, 0)
    const end = new Date(endDate);
    end.setHours(0, 0, 0, 0)

    // Get all transactions from all wallets
    const allTransactions = wallets.flatMap(wallet => wallet.transactions);

    // Filter the transactions by date
    return allTransactions.filter(transaction => {
        if (transaction && transaction.date) {
            const transactionDate = new Date(transaction.date);
            return transactionDate >= start && transactionDate <= end;
        }
    });
}


export function filterTransactionsFrowWallets(wallets: Wallet[], filters: CommonFilters, startDate: string, endDate: string): Transaction[] {
    const filteredByDate = filterTransactionsByDate(wallets, startDate, endDate)
    return filterTransactions(filteredByDate, filters, false)
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