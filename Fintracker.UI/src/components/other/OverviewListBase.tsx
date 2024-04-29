import WalletInfoCard from "../wallets/WalletInfoCard.tsx";

interface OverviewListBaseProps {
    currency: string;
    balance: number;
    changeForPeriod: number;
    expenseForPeriod: number;
    incomeForPeriod: number;
    balanceTitle: string;
    changeForPeriodTitle: string;
    incomeTitle: string;
    expenseTitle: string;
    numberPostfix?: string;
}

export default function OverviewListBase({
                                             currency,
                                             balance,
                                             changeForPeriod,
                                             expenseForPeriod,
                                             incomeForPeriod,
                                             changeForPeriodTitle,
                                             expenseTitle,
                                             incomeTitle,
                                             balanceTitle,
                                             numberPostfix
                                         }: OverviewListBaseProps) {
    return (
        <div className={'grid sm:grid-cols-2 lg:grid-cols-4 gap-3'}>
            <WalletInfoCard title={balanceTitle} amount={balance} currencySymbol={currency}/>
            <WalletInfoCard title={changeForPeriodTitle} amount={changeForPeriod}
                            currencySymbol={currency}/>
            <WalletInfoCard title={expenseTitle} amount={expenseForPeriod}
                            currencySymbol={currency}/>
            <WalletInfoCard title={incomeTitle} amount={incomeForPeriod} amountPostfix={numberPostfix}
                            currencySymbol={currency}/>
        </div>
    )
}