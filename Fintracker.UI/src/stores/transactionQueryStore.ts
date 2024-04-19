import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";

export interface QueryParams {
    pageNumber?: number;
    pageSize?: number;
    sortBy?: string;
    isDescending?: boolean;
}

interface TransactionQueryParams extends QueryParams {
    transactionsPerDate?: number;
    startDate?: Date;
    endDate?: Date;
}

interface TransactionQueryStore {
    query: TransactionQueryParams;
    setPageNumber: (num: number) => void;
    setPageSize: (num: number) => void;
    setSortBy: (sortBy: string) => void;
    setIsDescending: (isDescending: boolean) => void;
    setTransactionsPerDate: (num: number) => void;
    setStartDate: (date: Date) => void;
    setEndDate: (date: Date) => void;
}


const useTransactionQueryStore = createWithEqualityFn<TransactionQueryStore>((set) => ({
    query: {
        pageNumber: 1,
        pageSize: 50,
        sortBy: '',
        isDescending: false,
        transactionsPerDate: 10,
        startDate: undefined,
        endDate: new Date()
    },
    setPageNumber: (num: number) => set(store => ({query: {...store.query, pageNumber: num}})),
    setPageSize: (num: number) => set(store => ({query: {...store.query, page_size: num}})),
    setTransactionsPerDate: (num: number) => set(store => ({query: {...store.query, transactionsPerDate: num}})),
    setIsDescending: (isDescending: boolean) => set(store => ({query: {...store.query, isDescending: isDescending}})),
    setSortBy: (sort: string) => set(store => ({query: {...store.query, sortBy: sort}})),
    setEndDate: (date: Date) => set(store => ({query: {...store.query, endDate: date}})),
    setStartDate: (date: Date) => set(store => ({query: {...store.query, startDate: date}})),
}), shallow);

export default useTransactionQueryStore;