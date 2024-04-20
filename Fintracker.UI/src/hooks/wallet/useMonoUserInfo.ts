import {useMutation} from "@tanstack/react-query";
import ms from "ms";
import {MonoWalletToken} from "./useCreateMonoWallet.ts";
import ApiClient from "../../services/ApiClient.ts";
import { MonobankUserInfo } from "../../entities/MonobankUserInfo.ts";

const apiClient = new ApiClient<MonoWalletToken, MonobankUserInfo>('bank/monobank')
export const useMonoUserInfo = () => {
    return useMutation({
        mutationKey: ['monobank'],
        mutationFn: async (monoToken: MonoWalletToken) => await apiClient.getMonobankUserInfo(monoToken),
        retryDelay: ms('5s'),
        retry: 3
    })
}