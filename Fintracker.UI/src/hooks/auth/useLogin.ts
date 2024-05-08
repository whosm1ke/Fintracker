import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";
import {LoginSchema} from "../../models/LoginSchema.ts";
import useUserStore from "../../stores/userStore.ts";
import {UseFormSetError} from "react-hook-form";

const apiClient = new ApiClient<LoginSchema>('account/login');
const useLogin = (setError : UseFormSetError<LoginSchema>) => {
    const [ setId, setToken] = useUserStore(x => ([ x.setUserId, x.setUserToken]));
    return useMutation({
        mutationFn: (model: LoginSchema) => apiClient.login(model),
        onSuccess: (data) => {
            setId(data.response!.userId);
            setToken(data.response!.token);
        },
        onError: () => {
            setError('root', {
                message: 'Invalid credentials',
                type: 'server'
            },{shouldFocus: true})
        }
    });
}

export default useLogin;