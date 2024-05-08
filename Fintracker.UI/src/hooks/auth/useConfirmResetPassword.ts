import ApiClient from "../../services/ApiClient.ts";
import {BaseCommandResponse, BaseResponse} from "../../serverResponses/responses.ts";
import ResetPasswordModel from "../../models/ResetPasswordModel.ts";
import {useMutation} from "@tanstack/react-query";

const apiClient = new ApiClient<BaseCommandResponse | BaseResponse, ResetPasswordModel>('account/reset-password/confirm')
const useConfirmResetPassword = () => {
    return useMutation({
        mutationFn: async (model: ResetPasswordModel) => await apiClient.resetPassword(model)
    })    
}

export default useConfirmResetPassword;