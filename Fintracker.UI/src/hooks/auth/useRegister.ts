import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";
import {RegisterResponse, RegisterSchema} from "../../models/RegisterSchema.ts";
import useUserStore from "../../stores/userStore.ts";

const apiClient = new ApiClient<RegisterSchema, RegisterResponse>('account/register');

const useRegister = () => {
    const [setEmail, setId] = useUserStore(x => ([x.setUserEmail, x.setUserId]));
    return useMutation({
        mutationFn: (model: RegisterSchema) => apiClient.register(model),
        onSuccess: (model, variables) => {
            if (!model.hasError) {
                setId(model.response!.userId);
                setEmail(variables.email);
            }
        }
    });
};

export default useRegister;