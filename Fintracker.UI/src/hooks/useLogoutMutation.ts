import ApiClient from "../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";

const apiClient = new ApiClient<void, void>('account/logout');
const useLogoutMutation = () => {
    return useMutation<null,Error, null>({
        mutationFn: async () => {
            return await apiClient.logout();
        },
        onSuccess: () => {
            localStorage.removeItem('userToken')
            localStorage.removeItem('userEmail')
            localStorage.removeItem('userId')
        },
        onError: () => {
            localStorage.removeItem('userToken')
            localStorage.removeItem('userEmail')
            localStorage.removeItem('userId')
        }
    })
}

export default useLogoutMutation;