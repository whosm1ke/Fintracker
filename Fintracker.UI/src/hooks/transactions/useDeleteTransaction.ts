﻿import { Transaction } from "../../entities/Transaction.ts";
import ApiClient from "../../services/ApiClient.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";

type Context = {
    prevTransactions: Transaction[]
}
const apiClinet = new ApiClient<Transaction, Transaction>('transaction');
const useDeleteTransaction = (id: string) => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<DeleteCommandResponse<Transaction>>, Error, {id: string, walletId: string}, Context>({
        mutationFn: async () => await apiClinet.delete(id),
        onMutate: async (transToDelete: {id: string, walletId: string}) => {
            await queryClient.cancelQueries({queryKey: ['transactions',transToDelete.walletId]});

            const prevData = queryClient.getQueryData<Transaction[]>(['transactions', transToDelete.walletId]) || [];

            queryClient.setQueryData(['transactions',  transToDelete.walletId], (oldQueryData: Transaction[]) => oldQueryData?.filter(t => t.id !== id));
            return {prevTransactions: prevData};
        },
        // @ts-ignore
        onError: (err, transToDelete, context) => {
            queryClient.setQueryData(['transactions',  transToDelete.walletId], context?.prevTransactions)
            return err;
        },
        onSettled: async (_resp, _error, transToDelete) => {
            await queryClient.invalidateQueries({queryKey: ['transactions', transToDelete.walletId]})
            await queryClient.invalidateQueries({queryKey: ['wallets']})
            await queryClient.invalidateQueries({queryKey: ['budgets']})
        },
    })
}

export default useDeleteTransaction;