import {useQuery} from "@tanstack/react-query";
import ApiClient from "../../services/ApiClient.ts";
import ms from 'ms'
import {Wallet} from "../../entities/Wallet.ts";

const useWallets = (ownerId: string) => {
    const apiClient = new ApiClient<Wallet[]>(`wallet/owner/${ownerId}`);
    return useQuery({
        queryKey: ['wallets'],
        queryFn: async () => apiClient.getAll(),
        staleTime: ms('20m'),
        retryDelay: 60
    });
}

export default useWallets;