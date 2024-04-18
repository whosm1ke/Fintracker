import {GroupedTransactionByDate, Transaction} from "../entities/Transaction.ts";
import {TransactionItem} from "./TransactionItem.tsx";
import {useCurrencyConvertAll} from "../hooks/useCurrenctConvertAll.tsx";

interface TransactionListProps {
    groupedTransactions: GroupedTransactionByDate[];
    walletSymbol: string;
    startDate: Date | null;
    endDate: Date | null;
}

const calculateTotalExpenseByDate = (transactions: Transaction[], convertionRate: { [key: string]: number }) => {
    let total = 0;
    transactions.forEach(t => {
        total += t.amount * (convertionRate[t.currency.symbol] || 1)
    });
    return total.toFixed(2);
}

const getUniqueCurrencySymbols = (trans: Transaction[]) => {
    const symbols = trans.map(t => t.currency.symbol);
    return [...new Set(symbols)]
}

const getTransactionGroupsFilteredByDate = (groupedTransactions: GroupedTransactionByDate[], start: Date | null, end: Date | null) => {
    if(start === null || end === null)
        return groupedTransactions
    const adjustedStart = new Date(start);
    adjustedStart.setDate(adjustedStart.getDate() - 1)
    return groupedTransactions.filter(x => new Date(x.date) <= end && new Date(x.date) >= adjustedStart);
}


export default function TransactionList({groupedTransactions, walletSymbol, endDate, startDate}: TransactionListProps) {


    const filteredTransactions = getTransactionGroupsFilteredByDate(groupedTransactions, startDate, endDate);
    const allTransactions = filteredTransactions.flatMap(group => group.transactions);
    const uniqueSymbols = getUniqueCurrencySymbols(allTransactions);
    const {data: convertedCurrencies} = useCurrencyConvertAll({from: uniqueSymbols, to: walletSymbol, amount: [1]})
    const currencyRates: { [key: string]: number } = {};
    if (convertedCurrencies) {
        convertedCurrencies.forEach((rate, i) => {
            currencyRates[uniqueSymbols[i]] = rate.value;
        });
    }
    return (
        <div className={'flex flex-col gap-y-7'}>
            {filteredTransactions.map((group, i) =>
                <div className={''} key={i}>
                    <TransactionBlockHeader date={group.date}
                                            walletSymbol={walletSymbol}
                                            totalSpent={calculateTotalExpenseByDate(group.transactions, currencyRates)}
                                            totalTransactions={group.transactions.length}/>
                    <TransactionBlock transactions={group.transactions} walletSymbol={walletSymbol}/>
                </div>
            )}
        </div>
    )
}


interface TransactionBlockProps {
    transactions: Transaction[],
    walletSymbol: string;
}

export function TransactionBlock({transactions, walletSymbol}: TransactionBlockProps) {
    const uniqueSymbols = getUniqueCurrencySymbols(transactions);
    const {data: convertedCurrencies} = useCurrencyConvertAll({from: uniqueSymbols, to: walletSymbol, amount: [1]})
    const currencyRates: { [key: string]: number } = {};
    if (convertedCurrencies) {
        convertedCurrencies.forEach((rate, i) => {
            currencyRates[uniqueSymbols[i]] = rate.value;
        });
    }


    return (
        <>
            <div className={'flex flex-col gap-y-2'}>
                {
                    transactions.map(tran =>
                        <TransactionItem key={tran.id} transaction={tran} walletCurrencySymbol={walletSymbol}
                                         conversionRate={currencyRates[tran.currency.symbol] || 1}/>
                    )
                }
            </div>
        </>
    )
}

interface TransactionBlockHeaderProps {
    date: Date;
    totalSpent: string;
    totalTransactions: number;
    walletSymbol: string;
}

const TransactionBlockHeader = ({date, totalTransactions, totalSpent, walletSymbol}: TransactionBlockHeaderProps) => {
    const datePeriod = new Date(date).toLocaleDateString();

    return (
        <div className={'flex justify-between mb-4 p-4'}>
            <div>
                <h1 className={'font-bold text-lg'}>Date</h1>
                <p className={'text-lg font-bold text-left'}>{datePeriod}</p>
            </div>
            <div className={'hidden sm:block'}>
                <h1 className={'font-bold text-lg'}>Total transactions</h1>
                <p className={'text-lg font-bold text-center'}>{totalTransactions}</p>
            </div>
            <div>
                <h1 className={'font-bold text-lg'}>Total spent</h1>
                <p className={'text-lg text-red-400 font-bold text-right'}>- {totalSpent} {walletSymbol}</p>
            </div>
        </div>

    )
}