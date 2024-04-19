import ApiClient from "../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import {Transaction} from "../entities/Transaction.ts";
import useTransactionQueryStore from "../stores/transactionQueryStore.ts";


const useTransactions = (walletId: string) => {
    const apiClient = new ApiClient<Transaction, Transaction[]>(`transaction/wallet/${walletId}`);
    const query = useTransactionQueryStore(x => x.query);
    return useQuery({
        queryKey: ['transactions', walletId],
        queryFn: async () => await apiClient.getAllSorted({
            params:{
                sortBy: query.sortBy || "date",
                isDescending: query.isDescending || true,
                pageSize: query.pageSize || 50,
                pageNumber: query.pageNumber || 1,
                transactionsPerDate: query.transactionsPerDate || 10
            }
        })
    })
}

export default useTransactions;