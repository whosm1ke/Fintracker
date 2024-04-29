import ApiClient from "../../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import useUserStore from "../../stores/userStore.ts";
import useBudgetQueryStore from "../../stores/budgetQueryStore.ts";
import { Budget } from "../../entities/Budget.ts";


export const useBudgets = (walletId: string | undefined) => {

    let apiClient;
        const userId = useUserStore(x => x.getUserId());
    if (walletId === undefined) {
        apiClient = new ApiClient<Budget[]>(`budget/user/${userId}`)
    } else {
        apiClient = new ApiClient<Budget[]>(`budget/wallet/${walletId}`)
    }

    const query = useBudgetQueryStore(x => x.query);
    return useQuery({
        queryKey: ['budgets'],
        queryFn: async () => await apiClient.getAll({
            params: {
                isPublic: query.isPublic || null,
                sortBy: query.sortBy || "name",
                isDescending: query.isDescending || true,
                pageSize: query.pageSize || 50,
                pageNumber: query.pageNumber || 1,
            }
        }),
        placeholderData: (prevData) => prevData
    })
}

