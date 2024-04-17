import ApiClient from "../services/ApiClient.ts";
import {Budget} from "../entities/Budget.ts";
import {useQuery} from "@tanstack/react-query";

export const useBudgets = (id: string, type: "wallet" | "user", isPublic: boolean | null) => {
const apiClient = new ApiClient<Budget,Budget[]>(`budget/${id}/list`)
    return useQuery({
        queryKey: ['budgets'],
        queryFn: async () => await apiClient.getAllSorted({
            params: {
                type: type,
                isPublic: isPublic
            }
        })
    })
}

export const useBudgetsWithWallets = () => {
    const apiClient = new ApiClient<Budget,Budget[]>(`budget/with-wallet`)
    return useQuery({
        queryKey: ['budgets-with-wallets'],
        queryFn: async () => await apiClient.getAll()
    })
}

