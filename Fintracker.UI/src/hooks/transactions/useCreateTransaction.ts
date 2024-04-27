import ApiClient from "../../services/ApiClient.ts";
import {Transaction} from "../../entities/Transaction.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";

type Context = {
    previousTransactions: Transaction[]
}

const apiClient = new ApiClient<Transaction, Transaction>('transaction')
const useCreateTransaction = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<CreateCommandResponse<Transaction>>, Error, Transaction, Context>({
        mutationFn: async (model) => await apiClient.create(model),
        onMutate: async (newTransaction: Transaction) => {
            await queryClient.cancelQueries({queryKey: ['transactions',newTransaction.walletId]});

            const prevData = queryClient.getQueryData<Transaction[]>(['transactions', newTransaction.walletId]) || [];
            
            queryClient.setQueryData(['transactions',  newTransaction.walletId], (oldQueryData: Transaction[]) => [...oldQueryData || [], newTransaction]);
            return {previousTransactions: prevData};
        },
        // @ts-ignore
        onError: (err, newTransaction, context) => {
            queryClient.setQueryData(['transactions',  newTransaction.walletId], context?.previousTransactions)
            return err;
        },
        onSettled: async (_resp, _error, newTransaction) => {
            await queryClient.invalidateQueries({queryKey: ['transactions', newTransaction.walletId]})
            await queryClient.invalidateQueries({queryKey: ['wallets']})
            await queryClient.invalidateQueries({queryKey: ['wallet', newTransaction.walletId]})
            await queryClient.invalidateQueries({queryKey: ['budgets', newTransaction.walletId]})
        },
    });
}

export default useCreateTransaction;