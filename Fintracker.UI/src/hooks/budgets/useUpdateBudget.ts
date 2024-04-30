import ApiClient from "../../services/ApiClient.ts";
import {Budget} from "../../entities/Budget.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import ClientWrapper, { UpdateCommandResponse } from "../../serverResponses/responses.ts";

const apiClient = new ApiClient<Budget, Budget>('budget')
type Context = {
    prevBudget: Budget | undefined
}
const useUpdateBudget = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<UpdateCommandResponse<Budget>>, Error, Budget, Context>({
        mutationFn: async (model: Budget) => await apiClient.update(model),
        onMutate: async (newBudget: Budget) => {
            await queryClient.cancelQueries({queryKey: ['budget', newBudget.id]});

            const prevData = queryClient.getQueryData<Budget>(['budget', newBudget.id]);
            
            queryClient.setQueryData(['budget', newBudget.id], (_oldQueryData: ClientWrapper<Budget>) => {
                newBudget.transactions = _oldQueryData.response!.transactions
                const clientWarpper : ClientWrapper<Budget> = {
                    hasError: false,
                    response: newBudget
                }
                return clientWarpper;
            });
            return {prevBudget: prevData};
        },
        // @ts-ignore
        onError: (err, _newBudget, context) => {
            queryClient.setQueryData(['budget', _newBudget.id], context?.prevBudget)
            return err;
        },
        onSettled: async (_resp, _error, oldBudget) => {
            await queryClient.invalidateQueries({queryKey: ['budgets']})
            await queryClient.invalidateQueries({queryKey: ['budget', oldBudget.id]})
        },
    })
}

export default useUpdateBudget;