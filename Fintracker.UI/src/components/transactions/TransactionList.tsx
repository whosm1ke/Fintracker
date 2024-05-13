import {useId} from "react";
import {Transaction} from "../../entities/Transaction";
import {useCurrencyConvertAll} from "../../hooks/currencies/useCurrenctConvertAll";
import {TransactionItem} from "./TransactionItem.tsx";
import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";
import {
    calculateTotalExpense,
    filterTransactions,
    getCurrencyRates,
    getUniqueCurrencySymbols,
    groupTransactionsByDate
} from "../../helpers/globalHelper.ts";


interface TransactionListProps {
    transactions: Transaction[];
    walletSymbol: string;
}
export default function TransactionList({transactions, walletSymbol}: TransactionListProps) {


    
    const filters = useTransactionQueryStore(x => x.filters);
    const filteredTransactions = filterTransactions(transactions, filters);
    const groupedTransactions = groupTransactionsByDate(filteredTransactions);
    const allTransactions = groupedTransactions.flatMap(group => group.transactions);
    const uniqueSymbols = getUniqueCurrencySymbols(allTransactions);
    const {data: convertedCurrencies} = useCurrencyConvertAll({from: uniqueSymbols, to: walletSymbol, amount: [1]})
    const currencyRates = getCurrencyRates(convertedCurrencies, uniqueSymbols);

    
    if (!currencyRates) return null;

    return (
        <div className={'flex flex-col gap-y-2'}>
            {groupedTransactions.map((group, i) =>
                <div className={''} key={i}>
                    <TransactionBlock transactions={group.transactions} date={group.date}
                                      walletSymbol={walletSymbol}
                                      totalSpent={calculateTotalExpense(group.transactions, currencyRates)}/>
                </div>
            )}
        </div>
    )
}


interface TransactionBlockProps {
    transactions: Transaction[],
    walletSymbol: string;
    date: Date,
    totalSpent: number;
}

export function TransactionBlock({
                                     transactions,
                                     walletSymbol,
                                     date,
                                     totalSpent,
                                 }: TransactionBlockProps) {
    const uniqueSymbols = getUniqueCurrencySymbols(transactions);
    const {data: convertedCurrencies} = useCurrencyConvertAll({from: uniqueSymbols, to: walletSymbol, amount: [1]})
    const currencyRates = getCurrencyRates(convertedCurrencies, uniqueSymbols);
    const id = useId()

    if (!currencyRates) return null;


    return (
        <div className={'flex flex-col border-b-2 border-b-blue-300'}>
            <TransactionBlockHeader date={date} totalSpent={totalSpent}
                                    walletSymbol={walletSymbol}/>
            <div className={'flex flex-col'}>
                {
                    transactions.map(tran =>
                        <TransactionItem key={tran.id + id} transaction={tran} parentCurrencySymbol={walletSymbol}
                                         conversionRate={currencyRates[tran.currency.symbol] || 1} walletOwnerId={tran.wallet.ownerId}/>
                    )
                }
            </div>
        </div>
    )
}

interface TransactionBlockHeaderProps {
    date: Date;
    totalSpent: number;
    walletSymbol: string;
}

const TransactionBlockHeader = ({date, totalSpent, walletSymbol}: TransactionBlockHeaderProps) => {
    const datePeriod = new Date(date).toLocaleDateString();
    const totalSpentText = Math.abs(totalSpent).toFixed(2);
    const isPositive = totalSpent > 0;
    const classNameForTotalSpent = isPositive ? "text-green-400 font-bold text-right" : "text-red-400 font-bold text-right";


    return (
        <div className={'flex justify-between items-center text-sm sm:text-[15px] px-2 py-4'}>
            <div>
                <p className={'font-bold text-left'}>{datePeriod}</p>
            </div>
            <div className={''}>
                <p className={classNameForTotalSpent}>{isPositive ? "" : "-"} {totalSpentText} {walletSymbol}</p>
            </div>
        </div>

    )
}