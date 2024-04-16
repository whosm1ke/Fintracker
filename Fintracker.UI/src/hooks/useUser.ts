import ApiClient from "../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import ms from "ms";

const apiClient = new ApiClient<void, User>('user');

export function useGetUser(id: string) {
    return useQuery<ClientWrapper<User>>({
        queryKey: ['user', id],
        queryFn: async () => await apiClient.getById(id),
        staleTime: ms('3h')
    })
}