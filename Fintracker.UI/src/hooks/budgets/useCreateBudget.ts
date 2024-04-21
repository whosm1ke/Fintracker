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
        mutationKey: ['budgets'],
        mutationFn: async (model: Budget) => await apiClient.create(model),
        onMutate: async (newBudget: Budget) => {
            await queryClient.cancelQueries({queryKey: ['budgets']});

            const prevData = queryClient.getQueryData<Budget[]>(['budgets']) || [];
            console.log("prevData: ", prevData)
            queryClient.setQueryData(['budgets'], (oldQueryData: Budget[]) => [...oldQueryData, newBudget]);
            return {previousWallets: prevData};
        },
        onError: (err, _newWallet, context) => {
            queryClient.setQueryData(['budgets'], context?.previousWallets)
            return err;
        },
        onSettled: async () => {
            await queryClient.invalidateQueries({queryKey: ['budgets']})
        },
    })
}

export default useCreateBudget;