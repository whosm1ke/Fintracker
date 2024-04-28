import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";
import {formatDate} from "../helpers/globalHelper.ts";
import {Category} from "../entities/Category.ts";
import {MinMaxRange} from "./transactionQueryStore.ts";
import { Wallet } from "../entities/Wallet.ts";

interface WalletsOverview {
    totalBalance: number;
    totalChangeForPeriod: number;
    totalExpenseForPeriod: number;
    totalIncomeForPeriod: number;
}

interface WalletFilters {
    startDate: string;
    endDate: string;
    categories: Category[];
    minMaxRange: MinMaxRange;
    note: string,
    selectedWallets: Wallet[]
}

interface WalletInfoStore {
    overview: WalletsOverview;
    setBalance: (balance: number) => void;
    setChangeForPeriod: (total: number) => void;
    setExpenseForPeriod: (expense: number) => void;
    setIncomeForPeriod: (income: number) => void;
    filters: WalletFilters;
    setStartDate: (date: string) => void;
    setEndDate: (date: string) => void;
    setCategories: (categories: Category[]) => void;
    setWallets: (wallets: Wallet[]) => void;
    setMinMaxRange: (minMax: MinMaxRange) => void;
    setNote: (note: string) => void;
}

const useWalletInfoStore = createWithEqualityFn<WalletInfoStore>(set => {
    // Set end date to today at 00:00:00
    let endDate = new Date();
    endDate.setHours(0, 0, 0, 0);

    // Set start date to one week before end date at 00:00:00
    let startDate = new Date(endDate);
    startDate.setDate(startDate.getDate() - 7);


    return {

        overview: {
            totalBalance: 0,
            totalChangeForPeriod: 0,
            totalExpenseForPeriod: 0,
            totalIncomeForPeriod: 0
        },
        setBalance: (num: number) => set(store => ({overview: {...store.overview, totalBalance: num}})),
        setChangeForPeriod: (num: number) => set(store => ({overview: {...store.overview, totalChangeForPeriod: num}})),
        setExpenseForPeriod: (num: number) => set(store => ({overview: {...store.overview, totalExpenseForPeriod: num}})),
        setIncomeForPeriod: (num: number) => set(store => ({overview: {...store.overview, totalIncomeForPeriod: num}})),


        filters: {
            startDate: formatDate(startDate),
            endDate: formatDate(endDate),
            note: "",
            categories: [],
            minMaxRange: {min: 1, max: 1000},
            selectedWallets: []
        },
        setEndDate: (date: string) => set(store => ({filters: {...store.filters, endDate: date}})),
        setStartDate: (date: string) => set(store => ({filters: {...store.filters, startDate: date}})),
        setCategories: (categories: Category[]) => set(store => ({
            filters: {
                ...store.filters,
                categories: categories
            }
        })),
        setWallets: (wallets: Wallet[]) => set(store => ({filters: {...store.filters, selectedWallets: wallets}})),
        setMinMaxRange: (minMax: MinMaxRange) => set(store => ({filters: {...store.filters, minMaxRange: minMax}})),
        setNote: (note: string) => set(store => ({filters: {...store.filters, note: note}})),
    }
}, shallow);

export default useWalletInfoStore;