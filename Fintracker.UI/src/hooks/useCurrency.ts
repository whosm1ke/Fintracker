import ApiClient from "../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import ms from "ms";

const apiClient = new ApiClient<Currency, Currency>('currency')
const useCurrency = (id: string) => {
    return useQuery({
        queryKey: ['currency', id],
        queryFn: async () => await apiClient.getById(id),
        staleTime: ms('10h')
    })
}

export default useCurrency;