import { User } from "../../entities/User.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import ClientWrapper from "../../serverResponses/responses.ts";

const apiClinet = new ApiClient<User,User>('account/username')
const useUpdateUserUsername = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: async (model: User) => await apiClinet.update(model),
        onMutate: async (newUser: User) => {


            await queryClient.cancelQueries({queryKey: ['user', newUser.id]});
            const prevData = queryClient.getQueryData<User>(['user', newUser.id]);

            queryClient.setQueryData(['user', newUser.id], (_oldQueryData: ClientWrapper<User>) => {
                const clientWarpper: ClientWrapper<User> = {
                    hasError: false,
                    response: {
                        userName: newUser.userName,
                        globalCurrency: newUser.globalCurrency,
                        userDetails: newUser.userDetails
                    } as User
                }
                return clientWarpper;
            });
            return {prevUser: prevData};
        },
        // @ts-ignore
        onError: (err, newUser, context) => {
            queryClient.setQueryData(['user', newUser.id], context?.prevUser)
            return err;
        },
        onSettled: async (_resp, _error, newUser) => {
            await queryClient.invalidateQueries({queryKey: ['user', newUser.id]})
        },
    })
}

export default useUpdateUserUsername;