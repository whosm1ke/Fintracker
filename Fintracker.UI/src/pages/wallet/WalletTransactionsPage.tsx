import {useParams} from "react-router-dom";
import useWallet from "../../hooks/wallet/useWallet.ts";
import useUserStore from "../../stores/userStore.ts";
import useTransactions from "../../hooks/transactions/useTransactions.ts";
import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";
import TransactionFilters from "../../components/transactions/TransactionFilters.tsx";
import Spinner from "../../components/other/Spinner.tsx";
import CreateTransactionModal from "../../components/transactions/CreateTransactionModal.tsx";
import TransactionList from "../../components/transactions/TransactionList.tsx";

export default function WalletTransactionsPage() {
    const {walletId} = useParams();
    const userId = useUserStore(x => x.getUserId());
    const [
        transPerDate, startDate, endDate,
        setTransPerDay, setStartDate, setEndDate
    ] = useTransactionQueryStore(x =>
        [x.query.transactionsPerDate, x.query.startDate, x.query.endDate,
            x.setTransactionsPerDate, x.setStartDate, x.setEndDate]);
    const {data: transactions} = useTransactions(walletId!)
    const {data: wallet} = useWallet(walletId!);

    if (!transactions || !wallet || !wallet.response) return <Spinner/>


    const handleDateFilterChange = (date: Date, isStartDate: boolean) => {
        if (isStartDate) setStartDate(date)
        if (!isStartDate) setEndDate(date)
    }

    const handleTransactionPerDateChange = (num: number) => {
        setTransPerDay(num);
    }

    return (
        <div className={'container mx-auto p-4'}>
            <div className={'flex justify-between'}>
                <div className={'grid grid-cols-2 grid-rows-2'}>
                    {!wallet.response.isBanking && <CreateTransactionModal userId={userId!} walletId={wallet.response.id}
                                             walletCurrency={wallet.response.currency}/>}
                </div>
                <div className={'grid grid-cols-3 grid-rows-2 gap-x-4'}>
                    <TransactionFilters startDate={startDate!} endDate={endDate!}
                                        handleDateFilterChange={handleDateFilterChange}
                                        transPerPage={transPerDate || 1}
                                        handleTransPerDateChangle={handleTransactionPerDateChange}
                    />
                </div>

            </div>

            <div className={''}>
                <TransactionList transactions={transactions} walletSymbol={wallet.response.currency.symbol}/>
            </div>
        </div>
    )
}






