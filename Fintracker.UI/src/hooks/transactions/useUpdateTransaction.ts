import {useMutation, useQueryClient} from "@tanstack/react-query";
import {Transaction} from "../../entities/Transaction.ts";
import ApiClient from "../../services/ApiClient.ts";
import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";

const apiClient = new ApiClient<Transaction, Transaction>('transaction')

type Context = {
    previousTransactions: Transaction[]
}
const useUpdateTransaction = () => {
    const queryClient = useQueryClient();
    const transQuery = useTransactionQueryStore(x => x.query);
    return useMutation<ClientWrapper<UpdateCommandResponse<Transaction>>, Error, Transaction, Context>({
        mutationFn: async (model) => await apiClient.update(model),
        onMutate: async (newTransaction: Transaction) => {
            await queryClient.cancelQueries({queryKey: ['transactions',newTransaction.walletId, {...transQuery!}]});

            const prevData = queryClient.getQueryData<Transaction[]>(['transactions',newTransaction.walletId, {...transQuery!}]) || [];

            queryClient.setQueryData(['transactions',newTransaction.walletId, {...transQuery!}], (oldQueryData: Transaction[]) => {
                return oldQueryData.map(t => t.id === newTransaction.id ? newTransaction : t);
            });
            return {previousTransactions: prevData};
        },
        // @ts-ignore
        onError: (err, newTransaction, context) => {
            queryClient.setQueryData(['transactions',newTransaction.walletId, {...transQuery!}], context?.previousTransactions)
            return err;
        },
        onSettled: async (_resp, _error, newTransaction) => {
            await queryClient.invalidateQueries({queryKey: ['transactions',newTransaction.walletId, {...transQuery!}]})
            await queryClient.invalidateQueries({queryKey: ['wallets']})
            await queryClient.invalidateQueries({queryKey: ['budgets', newTransaction.walletId]})
        },
    });
}

export default useUpdateTransaction;