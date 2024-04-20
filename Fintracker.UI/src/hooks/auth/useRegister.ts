import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";
import {RegisterResponse, RegisterSchema} from "../../models/RegisterSchema.ts";
import useUserStore from "../../stores/userStore.ts";
import {UseFormSetError} from "react-hook-form";

const apiClient = new ApiClient<RegisterSchema, RegisterResponse>('account/register');

const useRegister = (setError : UseFormSetError<RegisterSchema>) => {
    const [setEmail, setId] = useUserStore(x => ([x.setUserEmail, x.setUserId]));
    return useMutation({
        mutationFn: (model: RegisterSchema) => apiClient.register(model),
        onSuccess: (model, variables) => {
            setId(model.response!.userId);
            setEmail(variables.email);
        },
        onError: (error: any) => {
            if (error && 'details' in error) {
                const serverErrors = error.details;
                serverErrors.forEach((errorDetail: any) => {
                    if (errorDetail.propertyName) {
                        setError(errorDetail.propertyName as keyof RegisterSchema, {
                            type: 'server',
                            message: errorDetail.errorMessage,
                        });
                    }
                });
            } else {
                console.log(error);
            }
        }
    });
};

export default useRegister;