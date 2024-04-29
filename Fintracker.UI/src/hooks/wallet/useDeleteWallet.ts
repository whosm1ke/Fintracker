import {Wallet} from "../../entities/Wallet";
import ApiClient from "../../services/ApiClient";
import {useMutation, useQueryClient} from "@tanstack/react-query";

type Context = {
    prevWallets: Wallet[]
}
const apiClinet = new ApiClient<Wallet>('wallet');
const useDeleteWallet = (id: string) => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<DeleteCommandResponse<Wallet>>, Error, string, Context>({
        mutationKey: ['wallets'],
        mutationFn: async () => await apiClinet.delete(id),
        onMutate: async (walletId) => {
            await queryClient.cancelQueries({queryKey: ['wallets']});

            const prevData = queryClient.getQueryData<Wallet[]>(['wallets']) || [];

            queryClient.setQueryData(['wallets'], (oldQueryData: Wallet[]) => oldQueryData.filter(w => w.id !== walletId));
            return {prevWallets: prevData};
        },
        // @ts-ignore
        onError: (err, newWallet, context) => {
            queryClient.setQueryData(['wallets'], context?.prevWallets)
            return err;
        },
        onSettled: async () => {
            await queryClient.invalidateQueries({queryKey: ['wallets']})
        }
    })
}

export default useDeleteWallet;