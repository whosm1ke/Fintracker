import ApiClient from "../../services/ApiClient.ts";
import {Category} from "../../entities/Category.ts";
import {useQuery} from "@tanstack/react-query";
import {CategoryType} from "../../entities/CategoryType.ts";

const apiClient = new ApiClient<Category[]>(`category/${CategoryType.EXPENSE}`)
const useExpenseCategories = () => {
    return useQuery({
        queryKey: ["categories", 'expense'],
        queryFn: async () => await apiClient.getAll()
    })
}

export default useExpenseCategories;