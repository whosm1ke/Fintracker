import {QueryParams} from "./transactionQueryStore.ts";
import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";


interface BudgetQueryParams extends QueryParams {
    isPublic?: boolean;
}
interface BudgetQueryStore {
    query: BudgetQueryParams;
    setPageNumber: (num: number) => void;
    setPageSize: (num: number) => void;
    setSortBy: (sortBy: string) => void;
    setIsDescending: (isDescending: boolean) => void;
    setIsPublic: (isPublic: boolean) => void;
}

const useBudgetQueryStore = createWithEqualityFn<BudgetQueryStore>((set) => ({
    query: {
        pageNumber: 1,
        pageSize: 50,
        sortBy: '',
        isDescending: false,
        isPublic: undefined
    },
    setPageNumber: (num: number) => set(store => ({query: {...store.query, pageNumber: num}})),
    setPageSize: (num: number) => set(store => ({query: {...store.query, page_size: num}})),
    setIsDescending: (isDescending: boolean) => set(store => ({query: {...store.query, isDescending: isDescending}})),
    setIsPublic: (isPublic: boolean) => set(store => ({query: {...store.query, isPublic: isPublic}})),
    setSortBy: (sort: string) => set(store => ({query: {...store.query, sortBy: sort}})),
}), shallow);

export default useBudgetQueryStore;