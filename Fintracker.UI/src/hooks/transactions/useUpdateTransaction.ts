import {useMutation, useQueryClient} from "@tanstack/react-query";
import {Transaction} from "../../entities/Transaction.ts";
import ApiClient from "../../services/ApiClient.ts";
import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";
import {Budget} from "../../entities/Budget.ts";

//TODO Create UpdateTransactionDTO

const apiClient = new ApiClient<Transaction, Transaction>('transaction')

type Context = {
    previousTransactions: Transaction[],
    prevBudget: Budget | undefined
}
const useUpdateTransaction = (budgetId: string | undefined) => {
    const queryClient = useQueryClient();
    const transQuery = useTransactionQueryStore(x => x.query);
    return useMutation<ClientWrapper<UpdateCommandResponse<Transaction>>, Error, Transaction, Context>({
        mutationFn: async (model) => await apiClient.update(model),
        onMutate: async (newTransaction: Transaction) => {

            //For Wallet
            await queryClient.cancelQueries({queryKey: ['transactions', newTransaction.walletId, {...transQuery!}]});

            const prevTransactions = queryClient.getQueryData<Transaction[]>(['transactions', newTransaction.walletId, {...transQuery!}]) || [];
            queryClient.setQueryData(['transactions', newTransaction.walletId, {...transQuery!}], (oldQueryData: Transaction[]) => {
                return oldQueryData?.map(t => t.id === newTransaction.id ? newTransaction : t);
            });


            //For budget
            let prevBudget : Budget | undefined;
            if (budgetId) {
                await queryClient.cancelQueries({queryKey: ['budget', budgetId]});

                prevBudget = queryClient.getQueryData(['budget', budgetId]);
                queryClient.setQueryData(['budget', budgetId], (oldQueryData: ClientWrapper<Budget>) => {
                    if (oldQueryData.response) {
                        const newTransactions = oldQueryData.response.transactions.map(t =>
                            t.id === newTransaction.id ? newTransaction : t
                        );

                        return {
                            ...oldQueryData,
                            response: {
                                ...oldQueryData.response,
                                transactions: newTransactions
                            }
                        };
                    }
                });
            }


            return {
                previousTransactions: prevTransactions,
                prevBudget: prevBudget
            };
        },
        // @ts-ignore
        onError: (err, newTransaction, context) => {
            queryClient.setQueryData(['transactions', newTransaction.walletId, {...transQuery!}], context?.previousTransactions)
            if (budgetId)
                queryClient.setQueryData(['budget', budgetId], context?.prevBudget)
            return err;
        },
        onSettled: async (_resp, _error, newTransaction) => {
            await queryClient.invalidateQueries({queryKey: ['transactions', newTransaction.walletId, {...transQuery!}]})
            await queryClient.invalidateQueries({queryKey: ['wallets']})
            await queryClient.invalidateQueries({queryKey: ['wallet', newTransaction.walletId]})
            await queryClient.invalidateQueries({queryKey: ['budgets']})
            await queryClient.invalidateQueries({queryKey: ['budget', budgetId]})
        },
    });
}

export default useUpdateTransaction;