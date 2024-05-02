import ApiClient from "../../services/ApiClient.ts";
import {Wallet} from "../../entities/Wallet.ts";
import {useQuery} from "@tanstack/react-query";

const apiClient = new ApiClient<Wallet>('wallet')
const useWallet = (id: string) => {
    return useQuery({
        queryKey: ['wallet', id],
        queryFn: async () => {
            try {
                return await apiClient.getById(id)
            }
            catch{
                
            }
        }
    })
}

export default useWallet;