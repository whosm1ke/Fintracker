import ApiClient from "../../services/ApiClient.ts";
import {Budget} from "../../entities/Budget.ts";
import {useQuery} from "@tanstack/react-query";
import useUserStore from "../../stores/userStore.ts";

export const useBudgets = (id: string | undefined, isPublic: boolean | null) => {
    
    let apiClient;
    if (id === undefined) {
        const userId = useUserStore(x => x.getUserId());
        apiClient = new ApiClient<Budget, Budget[]>(`budget/user/${userId}/sorted`)
    }else{
        apiClient = new ApiClient<Budget, Budget[]>(`budget/wallet/${id}/sorted`)
    }
    return useQuery({
        queryKey: ['budgets'],
        queryFn: async () => await apiClient.getAll({
            params: {
                isPublic: isPublic,

            }
        })
    })
}

