import {useParams} from "react-router-dom";
import useWallet from "../hooks/useWallet.ts";
import useGroupedTransactionsByDate from "../hooks/useGroupedTransactionsByDate.ts";
import TransactionList from "../components/TransactionList.tsx";
import {useState} from "react";

export default function WalletTransactionsPage() {
    const {walletId} = useParams();

    const {data: transactions} = useGroupedTransactionsByDate(walletId!)
    const {data: wallet} = useWallet(walletId!);
    const [filterDate, setFilterDate] = useState({
        startDate: new Date(new Date().setDate(new Date().getDate() - 7)),
        endDate: new Date()
    })

    if (!transactions || !wallet || !wallet.response) return <p>Loading...</p>

    const handleStartDate = (date: Date) => setFilterDate(p => ({...p, startDate: date}));
    const handleEndDate = (date: Date) => setFilterDate(p => ({...p, endDate: date}));

    return (
        <div className={'container mx-auto p-4'}>
            <div className="flex justify-end gap-x-4">
                <div className={'grid grid-cols-1 grid-rows-2'}>
                    <label className="text-lg font-semibold">Start date</label>
                    <input type="date" value={filterDate.startDate.toISOString().slice(0, 10)}
                           onChange={e => handleStartDate(e.target.valueAsDate || filterDate.startDate)}
                           className="p-2 border-2 border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                </div>
                <div className={'grid grid-cols-1 grid-rows-2'}>
                    <label className="text-lg font-semibold">End date</label>
                    <input type="date" value={filterDate.endDate.toISOString().slice(0, 10)}
                           onChange={e => handleEndDate(e.target.valueAsDate || filterDate.endDate)}
                           className="p-2 border-2 border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                </div>
            </div>

            <div className={''}>
                <TransactionList endDate={filterDate.endDate} startDate={filterDate.startDate}
                                 groupedTransactions={transactions} walletSymbol={wallet.response.currency.symbol}/>
            </div>
        </div>
    )
}


