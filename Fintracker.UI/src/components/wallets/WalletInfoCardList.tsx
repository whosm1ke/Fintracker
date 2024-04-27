import useWalletInfoStore from "../../stores/walletStore.ts";
import WalletInfoCard from "./WalletInfoCard.tsx";

interface WalletInfoCardList {
    walletCurrency: string;
    walletBalance: number;
}
export default function WalletInfoCardList ({walletCurrency, walletBalance}: WalletInfoCardList) {
    const walletInfo = useWalletInfoStore(x => x.info);
    return (
        <div className={'grid sm:grid-cols-2 lg:grid-cols-4 gap-3'}>
            <WalletInfoCard title={"Wallet balance"} amount={walletBalance} currencySymbol={walletCurrency}/>
            <WalletInfoCard title={"Change for period"} amount={walletInfo.walletChangeForPeriod}
                            currencySymbol={walletCurrency}/>
            <WalletInfoCard title={"Expense for period"} amount={walletInfo.walletExpenseForPeriod}
                            currencySymbol={walletCurrency}/>
            <WalletInfoCard title={"Income for period"} amount={walletInfo.walletIncomeForPeriod}
                            currencySymbol={walletCurrency}/>
        </div>
    )
}