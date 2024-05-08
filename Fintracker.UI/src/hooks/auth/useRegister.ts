import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";
import {RegisterSchema} from "../../models/RegisterSchema.ts";
import useUserStore from "../../stores/userStore.ts";

const apiClient = new ApiClient<RegisterSchema>('account/register');

const useRegister = () => {
    const [setId] = useUserStore(x => ([x.setUserId]));
    return useMutation({
        mutationFn: (model: RegisterSchema) => apiClient.register(model),
        onSuccess: (model) => {
            if (!model.hasError) {
                setId(model.response!.userId);
            }
        }
    });
};

export default useRegister;