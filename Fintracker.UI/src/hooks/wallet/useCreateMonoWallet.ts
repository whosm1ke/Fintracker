import ApiClient from "../../services/ApiClient.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import {Wallet} from "../../entities/Wallet.ts";
import { MonobankConfiguration, MonobankUserInfo } from "../../entities/MonobankUserInfo.ts";

export interface MonoWalletToken {
    xToken: string
}
type Context = {
    previousWallets: Wallet[]
}

const apiClient = new ApiClient<MonoWalletToken, MonobankUserInfo>('bank/monobank/add-initial-transactions')
const useCreateMonoWallet = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<CreateCommandResponse<Wallet>>, Error, MonobankConfiguration, Context>({
        mutationKey: ['monobank'],
        mutationFn: async (model: MonobankConfiguration) => await apiClient.addInitialMonobankTransaction(model),
        onMutate: async (newWallet: MonobankConfiguration) => {
            await queryClient.cancelQueries({queryKey: ['wallets']});

            const prevData = queryClient.getQueryData<Wallet[]>(['wallets']) || [];

            queryClient.setQueryData(['wallets'], (oldQueryData: Wallet[]) => [...oldQueryData, newWallet || null]);
            return {previousWallets: prevData};
        },
        // @ts-ignore
        onError: (err, newWallet, context) => {
            queryClient.setQueryData(['wallets'], context?.previousWallets)
            return err;
        },
        onSettled: async () => {
            await queryClient.invalidateQueries({queryKey: ['wallets']})
        }
    })

}



export default useCreateMonoWallet;