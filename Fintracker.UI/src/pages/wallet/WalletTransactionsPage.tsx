import {Navigate, useParams} from "react-router-dom";
import useWallet from "../../hooks/wallet/useWallet.ts";
import useUserStore from "../../stores/userStore.ts";
import useTransactions from "../../hooks/transactions/useTransactions.ts";
import Spinner from "../../components/other/Spinner.tsx";
import CreateTransactionModal from "../../components/transactions/CreateTransactionModal.tsx";
import TransactionList from "../../components/transactions/TransactionList.tsx";
import TransactionDateFilter from "../../components/transactions/TransactionDateFilter.tsx";
import TransactionsOtherFilters from "../../components/transactions/TransactionsOtherFilters.tsx";
import TransactionOverviewList from "../../components/transactions/TransactionOverviewList.tsx";

export default function WalletTransactionsPage() {
    const {walletId} = useParams();
    const userId = useUserStore(x => x.getUserId());
    const {data: transactions} = useTransactions(walletId!)
    const {data: walletResponse} = useWallet(walletId!);
    
    
    if (!transactions || !walletResponse || !walletResponse.response) return <Spinner/>
    const wallet = walletResponse.response;
    if (wallet.ownerId != userId && !wallet.users.find(u => u.id === userId)) return <Navigate to={'../../dashboard'}/>

    return (
        <div className={'container flex flex-col gap-y-5 mx-auto p-4'}>
            <div className={'flex flex-col sm:flex-row justify-between items-center gap-5'}>
                {!wallet.isBanking &&
                    <CreateTransactionModal userId={userId!} walletId={wallet.id}
                                            walletCurrency={wallet.currency} walletOwnerId={wallet.ownerId}/>}
                <TransactionDateFilter/>
            </div>
            <TransactionsOtherFilters transactions={transactions} wallet={wallet}/>
            <TransactionOverviewList walletCurrency={wallet.currency.symbol} balance={wallet.balance}/>
            <div className={'mt-4'}>
                {wallet.transactions.length !== 0 &&
                    <TransactionList transactions={transactions} walletSymbol={wallet.currency.symbol}/>}
            </div>
        </div>
    )
}











