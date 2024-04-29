import { useQuery } from "@tanstack/react-query";
import { Transaction } from "../../entities/Transaction";
import ApiClient from "../../services/ApiClient";
import useTransactionQueryStore from "../../stores/transactionQueryStore";

const useTransactions = (walletId: string) => {
    const apiClient = new ApiClient<Transaction[]>(`transaction/wallet/${walletId}`);
    const query = useTransactionQueryStore(x => x.query);
    return useQuery({
        queryKey: ['transactions', walletId, {...query}],
        queryFn: async () => await apiClient.getAll({
            params:{
                sortBy: query.sortBy || "date",
                isDescending: query.isDescending || true,
                pageSize: query.pageSize || 50,
                pageNumber: query.pageNumber || 1,
                transactionsPerDate: query.transactionsPerDate || 10,
                startDate: query.startDate,
                endDate: query.endDate
            }
        }),
        placeholderData: (prevData) => prevData
    })
}

export default useTransactions;