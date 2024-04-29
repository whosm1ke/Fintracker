import AcceptInvite from "../../models/AcceptInvite.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";



const apiClient = new ApiClient<BaseCommandResponse, AcceptInvite>('account/invite/accept')
const useAcceptInvite = () => {
    return useMutation({
        mutationFn: async (model: AcceptInvite) => await apiClient.acceptInvite(model)
    })
}

export default useAcceptInvite;