import {GroupedTransactionByDate, Transaction} from "../entities/Transaction.ts";
import {TransactionItem} from "./TransactionItem.tsx";
import {useCurrencyConvertAll} from "../hooks/useCurrenctConvertAll.tsx";
import {useId} from "react";

interface TransactionListProps {
    transactions: Transaction[];
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
    if (start === null || end === null)
        return groupedTransactions
    const adjustedStart = new Date(start);
    adjustedStart.setDate(adjustedStart.getDate() - 1)
    return groupedTransactions.filter(x => new Date(x.date) <= end && new Date(x.date) >= adjustedStart);
}

const groupTransactionsByDate = (transactions: Transaction[]) => {
    let transactionContainer: GroupedTransactionByDate[] = [];
    transactions.forEach(transaction => {
        const transDate = new Date(transaction.date);
        const date = transDate.toLocaleDateString('en-CA'); // format: YYYY-MM-DD

        let group = transactionContainer.find(x => new Date(x.date).toLocaleDateString('en-CA') === date);

        if (!group) {
            group = {
                date: new Date(date),
                transactions: []
            };
            transactionContainer.push(group);
        }

        group.transactions.push(transaction);
    });

    return transactionContainer;
}


export default function TransactionList({transactions, walletSymbol, endDate, startDate}: TransactionListProps) {

    const groupedTransactions = groupTransactionsByDate(transactions);
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
    const id = useId()

    return (
        <>
            <div className={'flex flex-col gap-y-2'}>
                {
                    transactions.map(tran =>
                        <TransactionItem key={tran.id + id} transaction={tran} walletCurrencySymbol={walletSymbol}
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
    const sortOptions = ["Label", "Note", "Amount"];
    
    
    return (
        <div className={'flex justify-between mb-1 p-4 text-sm sm:text-[15px]'}>
            <div>
                <h1 className={'font-bold'}>Date</h1>
                <p className={'font-bold text-left'}>{datePeriod}</p>
            </div>
            <div className={'hidden sm:block'}>
                <h1 className={'font-bold'}>Total transactions</h1>
                <p className={'font-bold text-center'}>{totalTransactions}</p>
            </div>
            <div className={'hidden sm:flex gap-x-2'}>
                <h1 className={'font-bold'}>Sort by:</h1>
                <select>
                    {sortOptions.map(s =>
                        <option value={s.toLowerCase()}>{s}</option>)}
                </select>
            </div>
            <div className={'grid grid-rows-2'}>
                <h1 className={'font-bold '}>Total spent</h1>
                <p className={'text-red-400 font-bold text-right'}>- {totalSpent} {walletSymbol}</p>
            </div>
        </div>

    )
}