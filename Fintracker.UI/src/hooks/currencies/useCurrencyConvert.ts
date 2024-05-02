import {useQuery} from "@tanstack/react-query";
import ApiClient from "../../services/ApiClient.ts";
import { ConvertCurrency } from "../../entities/Currency.ts";

const apiClient = new ApiClient<ConvertCurrency>('convert')
const useCurrencyConvert = (from: string, to: string, amount: number = 1) => {
    return useQuery({
        queryKey:['convertCurrency', from,to,amount],
        queryFn: async () => await apiClient.convert(from, to, amount),
        refetchOnMount: true,
        placeholderData: {amount: 1, from: from, to: to, value: 1},
    })
}




export default useCurrencyConvert;