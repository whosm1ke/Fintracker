import {useQuery} from "@tanstack/react-query";
import currencies from "../data/currencies.ts";

const useCurrencies = () => {
    return useQuery({
        queryKey: ['currencies'],
        initialData: currencies
    })
}

export default useCurrencies;