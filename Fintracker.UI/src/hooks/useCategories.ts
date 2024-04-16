import ApiClient from "../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";

const apiClient = new ApiClient<Category, Category[]>('category')
const useCategories = () => {
    return useQuery({
        queryKey: ["categories"],
        queryFn: async () => await apiClient.getAll()
    })
}

export default useCategories;