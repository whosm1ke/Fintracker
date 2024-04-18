import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";

export interface QueryParams {
    pageNumber?: number;
    pageSize?: number;
    sortBy?: string;
    isDescending?: boolean;
}

interface QueryStore {
    query: QueryParams;
    setPageNumber: (num: number) => void;
    setPageSize: (num: number) => void;
    setSortBy: (sortBy: string) => void;
    setIsDescending: (isDescending: boolean) => void;
}

const useQueryStore = createWithEqualityFn<QueryStore>((set) => ({
    query: {
        pageNumber: 1,
        pageSize: 10,
        sortBy: 'name',
        isDescending: false
    },
    setPageNumber: (num: number) => set(store => ({query: {...store.query, genreId: num}})),
    setPageSize: (num: number) => set(store => ({query: {...store.query, page_size: num}})),
    setIsDescending: (isDescending: boolean) => set(store => ({query: {...store.query, isDescending: isDescending}})),
    setSortBy: (sort: string) => set(store => ({query: {...store.query, sortOrder: sort}})),
}), shallow);

export default useQueryStore;