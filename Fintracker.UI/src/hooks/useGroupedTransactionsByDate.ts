import ApiClient from "../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import {GroupedTransactionByDate} from "../entities/Transaction.ts";

const useGroupedTransactionsByDate = (walletId: string) => {
    const apiClient = new ApiClient<GroupedTransactionByDate, GroupedTransactionByDate[]>(`transaction/wallet/${walletId}/grouped`);
    return useQuery({
        queryKey: ['transaction', 'wallet', walletId, 'grouped'],
        queryFn: async () => await apiClient.getAll()
    })
}

export default useGroupedTransactionsByDate;