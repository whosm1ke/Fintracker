import ApiClient from "../../services/ApiClient.ts";
import {Budget} from "../../entities/Budget.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";

const apiClient = new ApiClient<Budget, Budget>('budget')

type Context = {
    previousWallets: Budget[]
}

interface CreateBudget {
    name: string;
    balance: number;
    currencyId: string;
    walletId: string;
    categoryIds: string[]
    startDate: Date;
    endDate: Date;
    userId: string;
    isPublic: boolean;
}
const useCreateBudget = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<CreateCommandResponse<CreateBudget>>, Error, Budget, Context>({
        mutationFn: async (model: Budget) => await apiClient.create(model),
        onMutate: async (newBudget: Budget) => {
            await queryClient.cancelQueries({queryKey: ['budgets',newBudget.walletId]});

            const prevData = queryClient.getQueryData<Budget[]>(['budgets',newBudget.walletId]) || [];
            queryClient.setQueryData(['budgets',newBudget.walletId], (oldQueryData: Budget[]) => [...oldQueryData || [], newBudget]);
            return {previousWallets: prevData};
        },
        onError: (err, newBudget, context) => {
            queryClient.setQueryData(['budgets',newBudget.walletId], context?.previousWallets)
            return err;
        },
        onSettled: async (_data, _error, newBudget, _context) => {
            await queryClient.invalidateQueries({queryKey: ['budgets', newBudget.walletId]})
        },
    })
}

export default useCreateBudget;