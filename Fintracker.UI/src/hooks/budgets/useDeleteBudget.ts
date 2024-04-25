import {useMutation, useQueryClient} from "@tanstack/react-query";
import ApiClient from "../../services/ApiClient.ts";
import {Budget} from "../../entities/Budget.ts";
import useUserStore from "../../stores/userStore.ts";

type Context = {
    prevBudgets: Budget[]
}
const apiClinet = new ApiClient<Budget, Budget>('budget');
const useDeleteBudget = (id: string) => {
    const queryClient = useQueryClient();
    const userId = useUserStore(x => x.getUserId());
    return useMutation<ClientWrapper<DeleteCommandResponse<Budget>>, Error, { id: string, walletId: string | undefined}, Context>({
        mutationFn: async () => await apiClinet.delete(id),
        onMutate: async ({id, walletId}) => {
            await queryClient.cancelQueries({queryKey: ['budgets', walletId ? walletId : userId]});

            const prevData = queryClient.getQueryData<Budget[]>(['budgets', walletId ? walletId : userId]) || [];

            queryClient.setQueryData(['budgets', walletId ? walletId : userId], (oldQueryData: Budget[]) => oldQueryData.filter(b => b.id !== id));
            return {prevBudgets: prevData};
        },
        // @ts-ignore
        onError: (err, budgetToDelete, context) => {
            queryClient.setQueryData(['budgets', budgetToDelete.walletId ? budgetToDelete.walletId : userId], context?.prevBudgets)
            return err;
        },
        onSettled: async (_data, _error, variables, _context) => {
            await queryClient.invalidateQueries({queryKey: ['budgets', variables.walletId ? variables.walletId : userId]})
        }
    })
}

export default useDeleteBudget;