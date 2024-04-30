import ApiClient from "../../services/ApiClient.ts";
import {Wallet} from "../../entities/Wallet.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import {Transaction} from "../../entities/Transaction.ts";
import {Currency} from "../../entities/Currency.ts";
import {User} from "../../entities/User.ts";
import {Budget} from "../../entities/Budget.ts";
import ClientWrapper, { UpdateCommandResponse } from "../../serverResponses/responses.ts";
//TODO Create UpdateWalletDTO
export type UpdateWalletDTO = {
    name: string;
    startBalance: number;
    currencyId: string;
    currency: Currency;
    id: string;
    userIds: string[];
    users: User[];
    owner: User;
    ownerId: string;
    transactions: Transaction[]
}

type Context = {
    prevWallet: Wallet | undefined
}

const apiClient = new ApiClient<Wallet, UpdateWalletDTO>('wallet');
const useUpdateWallet = () => {
    const queryClient = useQueryClient();
    return useMutation<ClientWrapper<UpdateCommandResponse<Wallet>>, Error, UpdateWalletDTO, Context>({
        mutationKey: ['wallets'],
        mutationFn: async (model: UpdateWalletDTO) => await apiClient.update(model),
        onMutate: async (newWallet: UpdateWalletDTO) => {
            await queryClient.cancelQueries({queryKey: ['wallet', newWallet.id]});

            const prevData = queryClient.getQueryData<Wallet>(['wallet', newWallet.id]);

            queryClient.setQueryData(['wallet', newWallet.id], (oldQueryData: ClientWrapper<Wallet>) => {
                newWallet.transactions = oldQueryData.response!.transactions
                newWallet.currency = oldQueryData.response!.currency
                newWallet.users = oldQueryData.response!.users
                newWallet.owner = oldQueryData.response!.owner
                const clientWarpper: ClientWrapper<Wallet> = {
                    hasError: false,
                    // @ts-ignore
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
            await queryClient.invalidateQueries({queryKey: ['budgets']})
            _context?.prevWallet?.budgets.map(async (b: Budget) => await queryClient.invalidateQueries({queryKey: ['budget', b.id]}))
        }
    })
}

export default useUpdateWallet;