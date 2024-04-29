import { Budget } from "../../entities/Budget.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";

const useBudget = (budgetId: string) => {
    const apiClient = new ApiClient<ClientWrapper<Budget>,Budget>("budget");
    return useQuery({
        queryKey:["budget", budgetId],
        queryFn: async () => await apiClient.getById(budgetId),
    })
}

export default useBudget;