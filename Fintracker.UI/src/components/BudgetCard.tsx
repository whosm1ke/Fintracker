import {Link} from "react-router-dom";

interface BudgetCardProps {
    budgetId: string,
    name: string
    walletName: string;
    balance: number,
    totalSpent: number,
    currencySymbol: string,
    startDate: Date,
    endDate: Date,
    isPublic: boolean
}

export const BudgetCard = ({
                        isPublic,
                        budgetId,
                        name,
                        balance,
                        walletName,
                        currencySymbol,
                        endDate,
                        startDate,
                        totalSpent,
                    }: BudgetCardProps) => {
    const start = new Date(startDate).toLocaleDateString()
    const end = new Date(endDate).toLocaleDateString()
    const spentPercentage = (totalSpent * 100) / (totalSpent + balance);
    const spentBgColor = spentPercentage < 33 ? "bg-green-400" : spentPercentage < 66 ? "bg-yellow-200" : "bg-red-400"
    const cardBgColor = isPublic ? "bg-green-100" : "bg-orange-100"
    return (
        <Link
            to={`${budgetId}`}
            className={`${cardBgColor} p-4 rounded-lg w-full shadow-md`}>
            <h2 className="flex justify-between font-bold">
                <p className={'text-lg text-black'}>{name}</p>
                <p className={isPublic ? "text-green-600" : "text-red-600"}>{isPublic ? "PUBLIC" : "PRIVATE"}</p>
            </h2>
            <p className="text-sm text-gray-500">{walletName}</p>
            <p className="text-lg font-bold text-black">{balance} {currencySymbol} left</p>
            <p className="text-sm text-gray-500">of {balance + totalSpent} {currencySymbol}</p>
            <div className="h-7 w-full bg-gray-200 mt-2 rounded">
                <div style={{width: `${spentPercentage}%`, maxWidth: '100%'}}
                     className={`h-full ${spentBgColor} rounded`}></div>
            </div>
            <div className="flex justify-between text-sm text-gray-500 mt-2">
                <span>{start}</span>
                <span>{end}</span>
            </div>
        </Link>
    )
}