import {User} from "../../entities/User.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import ClientWrapper from "../../serverResponses/responses.ts";
import {UserDetails} from "../../entities/UserDetails.ts";



const apiClient = new ApiClient<User, FormData>('user')
const useUpdateUser = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: async (model: FormData) => await apiClient.updateForm(model),
        onMutate: async (newUserFormData: FormData) => {
            
            const id = newUserFormData.get('Id');
            
            const globalCurrency = JSON.parse(newUserFormData.get('Currency') as string);
            const userDetails : UserDetails = {
                sex: newUserFormData.get('UserDetails.Sex') as string,
                language: +(newUserFormData.get('UserDetails.Language') as string),
                dateOfBirth: newUserFormData.get('UserDetails.DateOfBirth') as string,
            }
            
            await queryClient.cancelQueries({queryKey: ['user', id]});
            const prevData = queryClient.getQueryData<User>(['user', id]);

            queryClient.setQueryData(['user', id], (_oldQueryData: ClientWrapper<User>) => {
                const clientWarpper: ClientWrapper<User> = {
                    hasError: false,
                    response: {
                        // userName: _oldQueryData.response?.userName || "",
                        globalCurrency: globalCurrency,
                        userDetails: userDetails
                    } as User
                }
                return clientWarpper;
            });
            return {prevUser: prevData};
        },
        // @ts-ignore
        onError: (err, newUserFormData, context) => {
            const id = newUserFormData.get('Id');
            queryClient.setQueryData(['user', id], context?.prevUser)
            return err;
        },
        onSettled: async (_resp, _error, newUserFormData) => {
            const id = newUserFormData.get('Id');
            await queryClient.invalidateQueries({queryKey: ['user', id]})
        },
    })
}

export default useUpdateUser;