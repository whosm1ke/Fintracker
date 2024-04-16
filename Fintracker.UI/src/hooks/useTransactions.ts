import ApiClient from "../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import useQueryStore from "../stores/queryStore.ts";

const useTransactions = (userId: string) => {
    const apiClient = new ApiClient<Transaction, Transaction[]>(`transaction/user/${userId}/all`);
    const queryStore = useQueryStore(x => x.query);
    return useQuery({
        queryKey: ['transaction', 'user', userId, 'all'],
        queryFn: async () => await apiClient.getAllSorted({
            params: {
                pageNumber: queryStore.pageNumber || 1,
                pageSize: queryStore.pageSize || 10,
                sortBy: queryStore.sortBy || 'createdat',
                isDescending: queryStore.isDescending || false
            }
        })
    })
}

export default useTransactions;