interface WalletInfoCardProps {
    title: string;
    amount: number;
    currencySymbol: string
}
export default function WalletInfoCard ({title, amount, currencySymbol}: WalletInfoCardProps)  {
    const isPositiveAmount = amount > 0;
    const bgColorForAmount = isPositiveAmount ? "text-green-400" : "text-red-400";
    const amountText = (isPositiveAmount ? "+ " : "- ") + Math.abs(amount).toFixed(2) + " " + currencySymbol;

    return (
        <div className={'w-full bg-white px-4 py-2 rounded shadow'}>
            <p className={'font-bold text-lg'}>{title}</p>
            <p className={`text-xl font-semibold ${bgColorForAmount}`}>{amountText}</p>
        </div>
    )
}