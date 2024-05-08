import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";

const apiClient = new ApiClient<undefined, ResetPasswordModel>('account/reset-password')
type ResetPasswordModel = {
    urlCallback: string;
}
const useUpdateUserPassword = () => {
    return useMutation({
        mutationFn: async (model: ResetPasswordModel) => await apiClient.create(model)
    })
}

export default useUpdateUserPassword;