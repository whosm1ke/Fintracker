import {Category} from "../../entities/Category.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import useUserStore from "../../stores/userStore.ts";
import standartCategories from "../../data/standartCategories.ts";

const apiCline = new ApiClient<Category, undefined>('category/populate')
const usePopulateCategories = () => {
    const queryClient = useQueryClient();
    const userId = useUserStore(x => x.getUserId());
    return useMutation({
        mutationFn: async () => await apiCline.create(undefined),
        onMutate: async () => {


            return {previousCats: standartCategories};
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
    })
}

export default usePopulateCategories;