import {useQuery} from "@tanstack/react-query";
import ApiClient from "../../services/ApiClient.ts";
import ms from "ms";
import { ConvertCurrency } from "../../entities/Currency.ts";

const apiClient = new ApiClient<ConvertCurrency, ConvertCurrency>('convert')
const useCurrencyConvert = (from: string, to: string, amount: number = 1) => {
    return useQuery({
        queryKey:['convertCurrency', from,to,amount],
        queryFn: async () => await apiClient.convert(from, to, amount),
        staleTime: ms('24h'),
        refetchOnMount: true,
        placeholderData: {amount: 1, from: from, to: to, value: 1},
    })
}




export default useCurrencyConvert;