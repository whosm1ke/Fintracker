import {useMutation, useQueryClient} from "@tanstack/react-query";
import ApiClient from "../../services/ApiClient.ts";
import {Budget} from "../../entities/Budget.ts";

type Context = {
    prevBudgets: Budget[]
}
const apiClinet = new ApiClient<Budget, Budget>('budget');
const useDeleteBudget = (id: string) => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<DeleteCommandResponse<Budget>>, Error, {
        id: string,
        walletId: string | undefined
    }, Context>({
        mutationFn: async () => await apiClinet.delete(id),
        onMutate: async ({id,walletId}) => {
            await queryClient.cancelQueries({queryKey: ['budgets', walletId]});

            const prevData = queryClient.getQueryData<Budget[]>(['budgets', walletId]) || [];

            queryClient.setQueryData(['budgets', walletId], (oldQueryData: Budget[]) => oldQueryData ? oldQueryData.filter(b => b.id !== id) : []);

            return {prevBudgets: prevData};
        },
        // @ts-ignore
        onError: (err, budgetToDelete, context) => {
            queryClient.setQueryData(['budgets', budgetToDelete.walletId], context?.prevBudgets)
            return err;
        },
        onSettled: async (_data, _error, budgetToDelete, _context) => {
            await queryClient.invalidateQueries({queryKey: ['budgets', budgetToDelete.walletId]})
        }
    })
}

export default useDeleteBudget;