import ApiClient from "../../services/ApiClient.ts";
import {useQuery} from "@tanstack/react-query";
import ms from "ms";
import { User } from "../../entities/User.ts";
import ClientWrapper from "../../serverResponses/responses.ts";

const apiClient = new ApiClient<User>('user');

export function useGetUser(id: string) {
    return useQuery<ClientWrapper<User>>({
        queryKey: ['user', id],
        queryFn: async () => await apiClient.getById(id),
        staleTime: ms('3h')
    })
}