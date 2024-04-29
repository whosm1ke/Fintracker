import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";

type InviteModel = {
    email: string;
    walletId: string;
    urlCallback: string;
}


const apiClient = new ApiClient<undefined, InviteModel>('account/invite')
const useInviteUser = () => {
    return useMutation({
        mutationFn: async (model: InviteModel) => await apiClient.create(model),
    })
}

export default useInviteUser;