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
}

interface TransactionQueryStore {
    query: TransactionQueryParams;
    setPageNumber: (num: number) => void;
    setPageSize: (num: number) => void;
    setSortBy: (sortBy: string) => void;
    setIsDescending: (isDescending: boolean) => void;
    setTransactionsPerDate: (num: number) => void;
}


const useTransactionQueryStore = createWithEqualityFn<TransactionQueryStore>((set) => ({
    query: {
        pageNumber: 1,
        pageSize: 50,
        sortBy: '',
        isDescending: false,
        transactionsPerDate: 10,
    },
    setPageNumber: (num: number) => set(store => ({query: {...store.query, pageNumber: num}})),
    setPageSize: (num: number) => set(store => ({query: {...store.query, page_size: num}})),
    setTransactionsPerDate: (num: number) => set(store => ({query: {...store.query, transactionsPerDate: num}})),
    setIsDescending: (isDescending: boolean) => set(store => ({query: {...store.query, isDescending: isDescending}})),
    setSortBy: (sort: string) => set(store => ({query: {...store.query, sortBy: sort}})),
}), shallow);

export default useTransactionQueryStore;