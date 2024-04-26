import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";
import {Category} from "../entities/Category.ts";
import {User} from "../entities/User.ts";
import {formatDate} from "../helpers/globalHelper.ts";

export interface QueryParams {
    pageNumber?: number;
    pageSize?: number;
    sortBy?: string;
    isDescending?: boolean;
}

interface TransactionQueryParams extends QueryParams {
    transactionsPerDate?: number;
    startDate?: string;
    endDate?: string;
}

export type MinMaxRange = {
    min: number;
    max: number
}

export interface TransactionFilters {
    categories: Category[];
    users: User[];
    minMaxRange: MinMaxRange,
    note: string
}

interface TransactionQueryStore {
    query: TransactionQueryParams;
    filters: TransactionFilters;
    setPageNumber: (num: number) => void;
    setPageSize: (num: number) => void;
    setSortBy: (sortBy: string) => void;
    setIsDescending: (isDescending: boolean) => void;
    setTransactionsPerDate: (num: number) => void;
    setStartDate: (date: string) => void;
    setEndDate: (date: string) => void;

    setCategories: (categories: Category[]) => void;
    setUsers: (users: User[]) => void;
    setMinMaxRange: (minMax: MinMaxRange) => void;
    setNote: (note: string) => void;
}


const useTransactionQueryStore = createWithEqualityFn<TransactionQueryStore>((set) => {
    // Set end date to today at 00:00:00
    let endDate = new Date();
    endDate.setHours(0, 0, 0, 0);

    // Set start date to one week before end date at 00:00:00
    let startDate = new Date(endDate);
    startDate.setDate(startDate.getDate() - 7);

    return {
        query: {
            pageNumber: 1,
            pageSize: 50,
            sortBy: '',
            isDescending: false,
            transactionsPerDate: 10,
            startDate: formatDate(startDate),
            endDate: formatDate(endDate)
        },
        filters: {
            categories: [],
            minMaxRange: {min: 1, max: 1000},
            users: [],
            note: ""
        },
        setPageNumber: (num: number) => set(store => ({query: {...store.query, pageNumber: num}})),
        setPageSize: (num: number) => set(store => ({query: {...store.query, page_size: num}})),
        setTransactionsPerDate: (num: number) => set(store => ({query: {...store.query, transactionsPerDate: num}})),
        setIsDescending: (isDescending: boolean) => set(store => ({
            query: {
                ...store.query,
                isDescending: isDescending
            }
        })),
        setSortBy: (sort: string) => set(store => ({query: {...store.query, sortBy: sort}})),
        setEndDate: (date: string) => set(store => ({query: {...store.query, endDate: date}})),
        setStartDate: (date: string) => set(store => ({query: {...store.query, startDate: date}})),
        setCategories: (categories: Category[]) => set(store => ({
            filters: {
                ...store.filters,
                categories: categories
            }
        })),
        setUsers: (users: User[]) => set(store => ({filters: {...store.filters, users: users}})),
        setMinMaxRange: (minMax: MinMaxRange) => set(store => ({filters: {...store.filters, minMaxRange: minMax}})),
        setNote: (note: string) => set(store => ({filters: {...store.filters, note: note}})),
    }
}, shallow);


export default useTransactionQueryStore;