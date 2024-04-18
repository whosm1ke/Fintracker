import {Transaction} from "../entities/Transaction.ts";
import * as Icons from 'react-icons/md'
import {IconType} from "react-icons";

interface TransactionItemProps {
    transaction: Transaction,
    walletCurrencySymbol: string;
    conversionRate: number;
}

export function TransactionItem({transaction, conversionRate, walletCurrencySymbol}: TransactionItemProps) {
    const Icon = (Icons as any)[transaction.category.image] as IconType;
    const convertedAmount = transaction.amount * conversionRate;

    return (
        <div className="flex justify-between items-center gap-x-2 p-4 bg-neutral-100 border rounded shadow">
            <div className="flex items-center gap-x-2">
                <Icon size="2rem" color={transaction.category.iconColour}/>
                <p>{transaction.category.name}</p>
            </div>
            <div className="hidden md:flex-grow md:flex justify-center gap-x-10">
                <p>{transaction.note || "---"}</p>
                <p>{transaction.label || "---"}</p>
            </div>
            <div className="flex flex-col">
                <p className={`${transaction.amount > 0 ? "text-red-400" : 'text-green-400'} font-bold`}>
                    {transaction.amount > 0 ? "-" : ""}
                    {transaction.amount} {transaction.currency.symbol}
                </p>
                {transaction.currency.symbol !== walletCurrencySymbol &&
                    <p className={`text-gray-400 font-bold text-sm`}>
                        {convertedAmount > 0 ? "-" : ""}
                        {convertedAmount.toFixed(2)} {walletCurrencySymbol}
                    </p>}
            </div>
        </div>
    );
}

