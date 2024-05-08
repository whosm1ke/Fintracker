import ApiClient from "../../services/ApiClient.ts";
import {BaseCommandResponse} from "../../serverResponses/responses.ts";
import ResetEmailModel from "../../models/ResetEmailModel.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";

const apiClient = new ApiClient<BaseCommandResponse, ResetEmailModel>('account/reset-email/confirm')
const useConfirmResetEmail = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: async (model: ResetEmailModel) => await apiClient.resetEmail(model),
        onSettled: async (_data, _error, variables, _context) => {
            await queryClient.invalidateQueries({queryKey: ['user', variables.userId]})
        }
    })
}

export default useConfirmResetEmail;