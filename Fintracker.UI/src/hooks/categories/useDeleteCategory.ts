import {useMutation, useQueryClient} from "@tanstack/react-query";
import ApiClient from "../../services/ApiClient.ts";
import {Category} from "../../entities/Category.ts";
import useUserStore from "../../stores/userStore.ts";

const apiClient = new ApiClient<Category, Category>('category');
const useDeleteCategory = () => {
    const userId = useUserStore(x => x.getUserId());
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: async (model: string) => await apiClient.delete(model),
        onMutate: async (id: string) => {
            await queryClient.cancelQueries({queryKey: ['categories', 'user', userId]});

            const prevData = queryClient.getQueryData<Category[]>(['categories', 'user', userId]);

            queryClient.setQueryData(['categories', 'user', userId], (oldQueryData: Category[]) => oldQueryData ? oldQueryData.filter(b => b.id !== id) : []);
            return {prevBudget: prevData};
        },
        // @ts-ignore
        onError: (err, id, context) => {
            queryClient.setQueryData(['categories', 'user', userId], context?.prevBudget)
            return err;
        },
        onSettled: async (_resp, _error, _id) => {
            await queryClient.invalidateQueries({queryKey: ['budgets']})
            await queryClient.invalidateQueries({queryKey: ['wallets']})
            await queryClient.invalidateQueries({queryKey: ['transactions']})
            await queryClient.invalidateQueries({queryKey: ['categories', 'user', userId]})
        },
    })
}

export default useDeleteCategory;