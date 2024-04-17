import {useQuery} from "@tanstack/react-query";
import ApiClient from "../services/ApiClient.ts";

const apiClient = new ApiClient<ConvertCurrency, ConvertCurrency>('currency/convert')
const useCurrencyConvert = (from: string, to: string, amount: number = 1) => {
    return useQuery({
        queryKey:['convertCurrency', from,to,amount],
        queryFn: async () => await apiClient.get({
           params:{
               from:from,
               to: to,
               amount: amount
           } 
        })
    })
}

export default useCurrencyConvert;