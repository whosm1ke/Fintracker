import {useQuery} from "@tanstack/react-query";
import ms from "ms";
import ApiClient from "../services/ApiClient.ts";

const apiClient = new ApiClient<ConvertCurrency, ConvertCurrency[]>('convert')
export const useCurrencyConvertAll =  (data: {from: string[], to: string, amount: number[]}) => {
    return useQuery({
        queryKey:['convertCurrencyAll'],
        queryFn: async () => await apiClient.convertAll(data),
        staleTime: ms('24h'),
        refetchOnMount: true
    })
}