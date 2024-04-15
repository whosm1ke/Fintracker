import ApiClient from "../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";
import {LoginResponse, LoginSchema} from "../models/LoginSchema.ts";
import useUserStore from "../stores/userStore.ts";
import {UseFormSetError} from "react-hook-form";

const apiClient = new ApiClient<LoginSchema, LoginResponse>('account/login');
const useLogin = (setError : UseFormSetError<LoginSchema>) => {
    const [setEmail, setId, setToken] = useUserStore(x => ([x.setUserEmail, x.setUserId, x.setUserToken]));
    return useMutation({
        mutationFn: (model: LoginSchema) => apiClient.login(model),
        onSuccess: (data) => {
            setId(data.response!.userId);
            setEmail(data.response!.userEmail);
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