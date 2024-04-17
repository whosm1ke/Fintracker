import {useMutation, useQueryClient} from "@tanstack/react-query";
import ApiClient from "../services/ApiClient.ts";
import {Wallet} from "../entities/Wallet.ts";

const apiClient = new ApiClient<Wallet, Wallet>('wallet')

type Context = {
    previousWallets: Wallet[]
}
const useCreateWallet = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<CreateCommandResponse<Wallet>>, Error, Wallet, Context>({
        mutationKey: ['wallets'],
        mutationFn: async (model: Wallet) => await apiClient.create(model),
        onMutate: async (newWallet: Wallet) => {
            await queryClient.cancelQueries({queryKey: ['wallets']});

            const prevData = queryClient.getQueryData<Wallet[]>(['wallets']) || [];

            queryClient.setQueryData(['wallets'], (oldQueryData: Wallet[]) => [...oldQueryData, newWallet]);
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


export default useCreateWallet;