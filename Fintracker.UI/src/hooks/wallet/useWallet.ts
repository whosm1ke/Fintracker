import ApiClient from "../../services/ApiClient.ts";
import {Wallet} from "../../entities/Wallet.ts";
import {useQuery} from "@tanstack/react-query";

const apiClient = new ApiClient<Wallet,Wallet>('wallet')
const useWallet = (id: string) => {
    return useQuery({
        queryKey: ['wallet', id],
        queryFn: async () => await apiClient.getById(id)
    })
}

export default useWallet;