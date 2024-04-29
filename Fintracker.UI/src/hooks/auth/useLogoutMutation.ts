import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";

const apiClient = new ApiClient<void, void>('account/logout');
const useLogoutMutation = () => {
    return useMutation<null,Error, null>({
        mutationFn: async () => {
            return await apiClient.logout();
        },
        onSuccess: () => {
            sessionStorage.removeItem('userToken')
            sessionStorage.removeItem('userEmail')
            sessionStorage.removeItem('userId')
        },
        onError: () => {
            sessionStorage.removeItem('userToken')
            sessionStorage.removeItem('userEmail')
            sessionStorage.removeItem('userId')
        }
    })
}

export default useLogoutMutation;