import {useId} from "react";
import {ConvertCurrency} from "../../entities/Currency";
import {GroupedTransactionByDate, Transaction} from "../../entities/Transaction";
import {useCurrencyConvertAll} from "../../hooks/currencies/useCurrenctConvertAll";
import Spinner from "../other/Spinner.tsx";
import {TransactionItem} from "./TransactionItem.tsx";
import {CategoryType} from "../../entities/CategoryType.ts";


interface TransactionListProps {
    transactions: Transaction[];
    walletSymbol: string;
}

const calculateTotalExpenseByDate = (transactions: Transaction[], convertionRate: { [key: string]: number }) => {
    let total = 0;
    transactions.forEach(t => {
        if (t.category.type === CategoryType.EXPENSE)
            total -= t.amount * (convertionRate[t.currency.symbol] || 1)
        else
            total += t.amount * (convertionRate[t.currency.symbol] || 1)
    });
    return total;
}

const getUniqueCurrencySymbols = (trans: Transaction[]) => {
    const symbols = trans.map(t => t.currency.symbol);
    return [...new Set(symbols)]
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

type uniqueCurrency = { [key: string]: number }

const getCurrencyRates = (convertedCurrencies: ConvertCurrency[] | undefined, uniqueSymbols: string[]) => {
    const currencyRates: uniqueCurrency = {};
    if (convertedCurrencies) {
        convertedCurrencies.forEach((rate, i) => {
            currencyRates[uniqueSymbols[i]] = rate.value;
        });

        return currencyRates;
    }

    return null;
}
export default function TransactionList({transactions, walletSymbol}: TransactionListProps) {


    const groupedTransactions = groupTransactionsByDate(transactions);
    const allTransactions = groupedTransactions.flatMap(group => group.transactions);
    const uniqueSymbols = getUniqueCurrencySymbols(allTransactions);
    const {data: convertedCurrencies} = useCurrencyConvertAll({from: uniqueSymbols, to: walletSymbol, amount: [1]})
    const currencyRates = getCurrencyRates(convertedCurrencies, uniqueSymbols);

    if (!currencyRates) return <Spinner/>

    return (
        <div className={'flex flex-col gap-y-2'}>
            {groupedTransactions.map((group, i) =>
                <div className={''} key={i}>
                    <TransactionBlock transactions={group.transactions} date={group.date}
                                      walletSymbol={walletSymbol}
                                      totalSpent={calculateTotalExpenseByDate(group.transactions, currencyRates)}/>
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

    if (!currencyRates) return <Spinner/>


    return (
        <div className={'flex flex-col  border-b-2 border-b-blue-300'}>
            <TransactionBlockHeader date={date} totalSpent={totalSpent}
                                    walletSymbol={walletSymbol}/>
            <div className={'flex flex-col'}>
                {
                    transactions.map(tran =>
                        <TransactionItem key={tran.id + id} transaction={tran} walletCurrencySymbol={walletSymbol}
                                         conversionRate={currencyRates[tran.currency.symbol] || 1}/>
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
    const sortOptions = ["Label", "Note", "Amount"];
    const totalSpentText = Math.abs(totalSpent).toFixed(2);
    const isPositive = totalSpent > 0;
    const classNameForTotalSpent = isPositive ? "text-green-400 font-bold text-right" : "text-red-400 font-bold text-right";


    return (
        <div className={'flex justify-between items-center text-sm sm:text-[15px] px-2 py-4'}>
            <div>
                <p className={'font-bold text-left'}>{datePeriod}</p>
            </div>
            <div className={'hidden sm:flex gap-x-2'}>
                <h1 className={'font-bold'}>Sort by:</h1>
                <select>
                    {sortOptions.map((s) =>
                        <option key={s} value={s.toLowerCase()}>{s}</option>)}
                </select>
            </div>
            <div className={''}>
                <p className={classNameForTotalSpent}>{isPositive ? "" : "-"} {totalSpentText} {walletSymbol}</p>
            </div>
        </div>

    )
}