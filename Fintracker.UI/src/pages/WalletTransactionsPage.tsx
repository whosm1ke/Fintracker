import useTransactions from "../hooks/useTransactions.ts";
import {Navigate, useParams} from "react-router-dom";
import {TransactionList} from "../components/TransactionItem.tsx";
import useWallet from "../hooks/useWallet.ts";

export default function WalletTransactionsPage(){
    const {walletId} = useParams();
    
    if(!walletId)
        return <Navigate to={'/dashboard'}/>
    
    const {data: wallet} = useWallet(walletId);
    const {data: transactions, isLoading} = useTransactions(walletId)
    
    if(isLoading)
        return <p>Loading</p>
    
    
    console.log(transactions)
    return (
        <div className={'container mx-auto p-4'}>
            <TransactionList transactions={transactions!} walletCurrencySymbol={wallet?.response?.currency.symbol!}/>
        </div>
    )
}

