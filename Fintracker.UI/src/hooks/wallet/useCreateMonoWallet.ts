import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";
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
    return useMutation<ClientWrapper<CreateCommandResponse<Wallet>>, Error, MonobankConfiguration, Context>({
        mutationKey: ['monobank'],
        mutationFn: async (model: MonobankConfiguration) => await apiClient.addInitialMonobankTransaction(model)
    })

}



export default useCreateMonoWallet;