import WalletInfoCard from "../wallets/WalletInfoCard.tsx";

interface OverviewListBaseProps {
    currency: string;
    balance: number;
    changeForPeriod: number;
    expenseForPeriod: number;
    incomeForPeriod: number;
}

export default function OverviewListBase({
                                     currency,
                                     balance,
                                     changeForPeriod,
                                     expenseForPeriod,
                                     incomeForPeriod
                                 }: OverviewListBaseProps) {
    return (
        <div className={'grid sm:grid-cols-2 lg:grid-cols-4 gap-3'}>
            <WalletInfoCard title={"Wallet balance"} amount={balance} currencySymbol={currency}/>
            <WalletInfoCard title={"Change for period"} amount={changeForPeriod}
                            currencySymbol={currency}/>
            <WalletInfoCard title={"Expense for period"} amount={expenseForPeriod}
                            currencySymbol={currency}/>
            <WalletInfoCard title={"Income for period"} amount={incomeForPeriod}
                            currencySymbol={currency}/>
        </div>
    )
}