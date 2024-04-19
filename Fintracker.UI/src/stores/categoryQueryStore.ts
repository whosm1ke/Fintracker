import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";
import {QueryParams} from "./transactionQueryStore.ts";

interface CategoryQueryStore {
    query: QueryParams;
    setPageNumber: (num: number) => void;
    setPageSize: (num: number) => void;
    setSortBy: (sortBy: string) => void;
    setIsDescending: (isDescending: boolean) => void;
}

const useCategoryQueryStore = createWithEqualityFn<CategoryQueryStore>((set) => ({
    query: {
        pageNumber: 1,
        pageSize: 50,
        sortBy: '',
        isDescending: false,
    },
    setPageNumber: (num: number) => set(store => ({query: {...store.query, pageNumber: num}})),
    setPageSize: (num: number) => set(store => ({query: {...store.query, page_size: num}})),
    setIsDescending: (isDescending: boolean) => set(store => ({query: {...store.query, isDescending: isDescending}})),
    setSortBy: (sort: string) => set(store => ({query: {...store.query, sortBy: sort}})),
}), shallow);

export default useCategoryQueryStore;