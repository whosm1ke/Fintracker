import ApiClient from "../../services/ApiClient.ts";
import {Wallet} from "../../entities/Wallet.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
//TODO Create UpdateWalletDTO
export type UpdateWalletDTO = {
    name: string;
    startBalance: number;
    currencyId: string;
    id: string;
    userIds: string[];
    deleteUserTransaction: boolean;
}

type Context = {
    prevWallet: Wallet | undefined
}

const apiClient = new ApiClient<Wallet,Wallet>('wallet');
const useUpdateWallet = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<UpdateCommandResponse<Wallet>>, Error, Wallet, Context>({
        mutationKey: ['wallets'],
        mutationFn: async (model: Wallet) => await apiClient.update(model),
        onMutate: async (newWallet: Wallet) => {
            await queryClient.cancelQueries({queryKey: ['wallet', newWallet.id]});

            const prevData = queryClient.getQueryData<Wallet>(['wallet', newWallet.id]);

            queryClient.setQueryData(['wallet', newWallet.id], (oldQueryData: ClientWrapper<Wallet>) => {
                newWallet.transactions = oldQueryData.response!.transactions
                const clientWarpper : ClientWrapper<Wallet> = {
                    hasError: false,
                    response: newWallet
                }
                return clientWarpper;
            });
            return {prevWallet: prevData};
        },
        // @ts-ignore
        onError: (err, _newWallet, _context) => {
            return err;
        },
        onSettled: async (_data, _error, variables, _context) => {
            await queryClient.invalidateQueries({queryKey: ['wallets']})
            await queryClient.invalidateQueries({queryKey: ['wallet', variables.id]})
        }
    })
}

export default useUpdateWallet;