import ApiClient from "../../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import useCategoryQueryStore from "../../stores/categoryQueryStore.ts";
import { Category } from "../../entities/Category.ts";

const apiClient = new ApiClient<Category, Category[]>('category')
const useCategories = () => {
    const query = useCategoryQueryStore(x => x.query);
    return useQuery({
        queryKey: ["categories"],
        queryFn: async () => await apiClient.getAll({
            params: {
                sortBy: query.sortBy || "name",
                isDescending: query.isDescending || true,
                pageSize: query.pageSize || 50,
                pageNumber: query.pageNumber || 1
            }
        })
    })
}



export default useCategories;