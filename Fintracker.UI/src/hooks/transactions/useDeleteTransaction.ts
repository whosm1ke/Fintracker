import {Transaction} from "../../entities/Transaction.ts";
import { DeleteCommandResponse } from "../../serverResponses/responses.ts";
import ClientWrapper from "../../serverResponses/responses.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";

type Context = {
    prevTransactions: Transaction[]
}
const apiClinet = new ApiClient<Transaction>('transaction');
const useDeleteTransaction = (id: string, budgetId: string | undefined) => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<DeleteCommandResponse<Transaction>>, Error, {
        id: string,
        walletId: string
    }, Context>({
        mutationFn: async () => await apiClinet.delete(id),
        onMutate: async (transToDelete: { id: string, walletId: string }) => {
            await queryClient.cancelQueries({queryKey: ['transactions', transToDelete.walletId]});

            const prevData = queryClient.getQueryData<Transaction[]>(['transactions', transToDelete.walletId]) || [];

            queryClient.setQueryData(['transactions', transToDelete.walletId], (oldQueryData: Transaction[]) => oldQueryData?.filter(t => t.id !== id));
            return {prevTransactions: prevData};
        },
        // @ts-ignore
        onError: (err, transToDelete, context) => {
            queryClient.setQueryData(['transactions', transToDelete.walletId], context?.prevTransactions)
            return err;
        },
        onSettled: async (_resp, _error, transToDelete) => {
            await queryClient.invalidateQueries({queryKey: ['transactions', transToDelete.walletId]})
            await queryClient.invalidateQueries({queryKey: ['wallets']})
            await queryClient.invalidateQueries({queryKey: ['wallet', transToDelete.walletId]})
            await queryClient.invalidateQueries({queryKey: ['budgets']})
            await queryClient.invalidateQueries({queryKey: ['budget', budgetId]});
        },
    })
}

export default useDeleteTransaction;