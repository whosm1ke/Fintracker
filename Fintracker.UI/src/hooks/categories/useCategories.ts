import ApiClient from "../../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import useCategoryQueryStore from "../../stores/categoryQueryStore.ts";
import { Category } from "../../entities/Category.ts";

const useCategories = (userId: string) => {
const apiClient = new ApiClient<Category[]>(`category/user/${userId}`)
    const query = useCategoryQueryStore(x => x.query);
    return useQuery({
        queryKey: ["categories","user", userId],
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