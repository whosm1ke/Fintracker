import ApiClient from "../../services/ApiClient.ts";
import {Budget} from "../../entities/Budget.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";

const apiClient = new ApiClient<Budget, Budget>('budget')
type Context = {
    previousWallets: Budget[]
}
const useCreateBudget = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<CreateCommandResponse<Budget>>, Error, Budget, Context>({
        mutationKey: ['wallets'],
        mutationFn: async (model: Budget) => await apiClient.create(model),
        onMutate: async (newWallet: Budget) => {
            await queryClient.cancelQueries({queryKey: ['wallets']});

            const prevData = queryClient.getQueryData<Budget[]>(['wallets']) || [];

            queryClient.setQueryData(['wallets'], (oldQueryData: Budget[]) => [...oldQueryData, newWallet]);
            return {previousWallets: prevData};
        },
        // @ts-ignore
        onError: (err, newWallet, context) => {
            queryClient.setQueryData(['wallets'], context?.previousWallets)
            return err;
        },
        onSettled: async () => {
            await queryClient.invalidateQueries({queryKey: ['wallets']})
        },
    })
}

export default useCreateBudget;