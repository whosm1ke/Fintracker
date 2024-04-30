import ApiClient from "../../services/ApiClient.ts";
import {Budget} from "../../entities/Budget.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import ClientWrapper, { CreateCommandResponse } from "../../serverResponses/responses.ts";

//TODO Create CreateBudgetDTO
const apiClient = new ApiClient<Budget, Budget>('budget')

type Context = {
    previousWallets: Budget[]
}

const useCreateBudget = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<CreateCommandResponse<Budget>>, Error, Budget, Context>({
        mutationFn: async (model: Budget) => await apiClient.create(model),
        onMutate: async (newBudget: Budget) => {
            await queryClient.cancelQueries({queryKey: ['budgets']});

            const prevData = queryClient.getQueryData<Budget[]>(['budgets']) || [];
            queryClient.setQueryData(['budgets'], (oldQueryData: Budget[]) => [...oldQueryData || [], newBudget]);
            return {previousWallets: prevData};
        },
        onError: (err, _newBudget, context) => {
            queryClient.setQueryData(['budgets'], context?.previousWallets)
            return err;
        },
        onSettled: async (_data, _error, _newBudget, _context) => {
            await queryClient.invalidateQueries({queryKey: ['budgets']})
        },
    })
}

export default useCreateBudget;