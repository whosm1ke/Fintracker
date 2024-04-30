import ApiClient from "../../services/ApiClient.ts";
import {Category} from "../../entities/Category.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import useUserStore from "../../stores/userStore.ts";

const apiClient = new ApiClient<Category, Category>('category');
const useUpdateCategory = () => {
    const queryClient = useQueryClient();
    const userId = useUserStore(x => x.getUserId());
    return useMutation({
        mutationFn: async (model: Category) => await apiClient.update(model),
        onMutate: async (newCategory: Category) => {
            console.log('update category hook: ', newCategory)
            await queryClient.cancelQueries({queryKey: ["categories", "user", userId]});

            const prevData = queryClient.getQueryData<Category[]>(["categories", "user", userId]) || [];
            queryClient.setQueryData(["categories", "user", userId], (oldQueryData: Category[]) => oldQueryData?.map(t => t.id === newCategory.id ? newCategory : t));
            
            return {previousCats: prevData};
        },
        onError: (err, _newBudget, context) => {
            queryClient.setQueryData(["categories", "user", userId], context?.previousCats)
            return err;
        },
        onSettled: async (_data, _error, _newBudget, _context) => {
            await queryClient.invalidateQueries({queryKey: ["categories", "user", userId]})
            await queryClient.invalidateQueries({queryKey: ["wallets"]})
            await queryClient.invalidateQueries({queryKey: ["transactions"]})
        },
    });
}

export default useUpdateCategory;
