interface WalletInfoCardProps {
    title: string;
    amount: number;
    currencySymbol: string;
    amountPostfix?: string;
}

export default function WalletInfoCard({title, amount, currencySymbol, amountPostfix = ""}: WalletInfoCardProps) {
    const isPositiveAmount = amount > 0 && amount !== 0;
    const isZero = amount === 0;
    const bgColorForAmount = isPositiveAmount || isZero ? "text-green-400" : "text-red-400";
    const amountText = (isPositiveAmount ? "+ " : isZero ? "" : "- ") + Math.abs(amount).toFixed(2) + " " + currencySymbol;
    
    return (
        <div className={'w-full bg-white px-4 py-2 rounded shadow'}>
            <p className={'font-bold text-lg'}>{title}</p>
            <p className={`text-xl font-semibold ${bgColorForAmount}`}>
                {isNaN(amount) ? "0" : amountText}
            <span className={'text-gray-500'}>{" " + amountPostfix}</span>
            </p>
        </div>
    )
}