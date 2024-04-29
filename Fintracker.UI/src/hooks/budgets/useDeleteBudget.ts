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
        id: string}, Context>({
        mutationFn: async () => await apiClinet.delete(id),
        onMutate: async ({id}) => {
            await queryClient.cancelQueries({queryKey: ['budgets']});

            const prevData = queryClient.getQueryData<Budget[]>(['budgets']) || [];

            queryClient.setQueryData(['budgets'], (oldQueryData: Budget[]) => oldQueryData ? oldQueryData.filter(b => b.id !== id) : []);

            return {prevBudgets: prevData};
        },
        // @ts-ignore
        onError: (err, budgetToDelete, context) => {
            queryClient.setQueryData(['budgets'], context?.prevBudgets)
            return err;
        },
        onSettled: async (_data, _error, _budgetToDelete, _context) => {
            await queryClient.invalidateQueries({queryKey: ['budgets']})
        }
    })
}

export default useDeleteBudget;