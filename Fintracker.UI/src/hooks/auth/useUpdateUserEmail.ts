import ApiClient from "../../services/ApiClient.ts";
import {useMutation} from "@tanstack/react-query";

const apiClinet = new ApiClient<undefined,ResetEmailRequest>('account/reset-email')

type ResetEmailRequest = {
    newEmail: string;
    urlCallback: string;
}
const useUpdateUserEmail = () => {
    return useMutation({
        mutationFn: async (model: ResetEmailRequest) => await apiClinet.create(model)
    });
}

export default useUpdateUserEmail;