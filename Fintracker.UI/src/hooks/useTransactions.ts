import ApiClient from "../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import useQueryStore from "../stores/queryStore.ts";
import {Transaction} from "../entities/Transaction.ts";

const useTransactions = (walletId: string) => {
    const apiClient = new ApiClient<Transaction, Transaction[]>(`transaction/wallet/${walletId}`);
    const queryStore = useQueryStore(x => x.query);
    return useQuery({
        queryKey: ['transaction', 'wallet', walletId],
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