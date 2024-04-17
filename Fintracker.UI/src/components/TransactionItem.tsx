import {Transaction} from "../entities/Transaction.ts";
import * as Icons from 'react-icons/md'
import {IconType} from "react-icons";
import useCurrencyConvert from "../hooks/useCurrencyConvert.ts";
import {useMemo} from "react";

interface TransactionItemProps {
    transaction: Transaction,
    walletCurrencySymbol: string;
}

export function TransactionItem({ transaction, walletCurrencySymbol }: TransactionItemProps) {
    const Icon = (Icons as any)[transaction.category.image] as IconType;
    let convertedCurrency = null;
    if (transaction.currency.symbol !== walletCurrencySymbol)
        convertedCurrency = useCurrencyConvert(transaction.currency.symbol, walletCurrencySymbol, transaction.amount);

    // Оптимізація: Використання memoization для запобігання повторному рендерингу
    const convertedAmountText = useMemo(() => {
        if (!convertedCurrency) {
            return null;
        }

        if (convertedCurrency.isLoading) {
            return <span className="text-gray-500">Converting...</span>;
        }

        return convertedCurrency.data?.value?.toFixed(2);
    }, [convertedCurrency]);

    return (
        <div className="flex justify-between items-center gap-x-2 p-4 bg-neutral-100 border rounded shadow">
            <div className="flex items-center gap-x-2">
                <Icon size="2rem" color={transaction.category.iconColour} />
                <p>{transaction.category.name}</p>
            </div>
            <div className="flex-grow flex justify-center gap-x-10">
                <p>{transaction.note || "---"}</p>
                <p>{transaction.label || "---"}</p>
            </div>
            <div className="flex flex-col">
                <p style={{ color: transaction.amount > 0 ? "red" : "black" }}>
                    {transaction.amount > 0 ? "-" : ""}
                    {transaction.amount} {transaction.currency.symbol}
                </p>
                {convertedAmountText && (
                    <p className="text-[16px] text-gray-500">
                        {!convertedCurrency?.isLoading && transaction.amount > 0 ? "-" : ""}
                        {convertedAmountText} {!convertedCurrency?.isLoading && walletCurrencySymbol}
                    </p>
                )}
            </div>
        </div>
    );
}

interface TransactionListProps {
    transactions: Transaction[],
    walletCurrencySymbol: string;
}

export const TransactionList = ({transactions, walletCurrencySymbol}: TransactionListProps) => {
    const groupedByDate = transactions.reduce((acc, transaction) => {
        const date = new Date(transaction.date);
        const dateKey = date.toLocaleDateString().split('T')[0];
        if (!acc[dateKey]) {
            acc[dateKey] = {transactions: [], total: 0};
        }
        const {data: conversionRate} = useCurrencyConvert(transaction.currency.symbol, walletCurrencySymbol, transaction.amount);


        const convertedAmount = conversionRate?.value!;
        acc[dateKey].transactions.push(transaction);
        acc[dateKey].total += convertedAmount;
        return acc;
    }, {} as Record<string, { transactions: Transaction[], total: number }>);

    return (
        <div className={'flex flex-col gap-y-5 mx-auto w-[70%]'}>
            {Object.keys(groupedByDate).sort().reverse().map(date => (
                <div key={date} className={'w-full flex flex-col gap-y-2 bg-slate-50 p-4 shadow-lg rounded-lg'}>
                    <TransactionListHeader total={parseFloat(groupedByDate[date].total.toFixed(2))} date={date}
                                           walletCurrencySymbol={walletCurrencySymbol}/>
                    {groupedByDate[date].transactions.map(transaction => (
                        <TransactionItem key={transaction.id} transaction={transaction}
                                         walletCurrencySymbol={walletCurrencySymbol}
                        />
                    ))}
                </div>
            ))}
        </div>
    )
}


interface TransactionListHeaderProps {
    total: number;
    date: string;
    walletCurrencySymbol: string
}

const TransactionListHeader = ({date, total, walletCurrencySymbol}: TransactionListHeaderProps) => {

    const isPositive = total > 0;
    const totalAmountText = !isNaN(total) ? `${isPositive ? -total : total} ${walletCurrencySymbol}` :
        `Calculating ${walletCurrencySymbol}`
    return (
        <>
            <div className={'flex justify-between items-center mb-4'}>
                <h2 className={'font-bold'}>{date}</h2>
                <p className={isPositive ? 'text-red-400 font-bold' :
                    'text-green-400 font-bold'}>{totalAmountText}</p>
            </div>
        </>
    )
}
