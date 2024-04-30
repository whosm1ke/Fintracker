import AcceptInvite from "../../models/AcceptInvite.ts";
import { BaseCommandResponse } from "../../serverResponses/responses.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";



const apiClient = new ApiClient<BaseCommandResponse, AcceptInvite>('account/invite/accept')
const useAcceptInvite = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: async (model: AcceptInvite) => await apiClient.acceptInvite(model),
        onSettled: async (_data, _error, variables, _context) => {
            await queryClient.invalidateQueries({queryKey: ['wallet', variables.walletId]})
        }
    })
}

export default useAcceptInvite;