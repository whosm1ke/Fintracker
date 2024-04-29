import {useQuery} from "@tanstack/react-query";
import ms from "ms";
import ApiClient from "../../services/ApiClient.ts";
import { ConvertCurrency } from "../../entities/Currency.ts";

const apiClient = new ApiClient<ConvertCurrency[]>('convert')
export const useCurrencyConvertAll =  (data: {from: string[], to: string, amount: number[]}) => {
    return useQuery({
        queryKey:['convertCurrencyAll', ...data.from],
        queryFn: async () => await apiClient.convertAll(data),
        staleTime: ms('24h'),
        refetchOnMount: false
    })
}