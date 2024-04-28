import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";
import OverviewListBase from "../other/OverviewListBase.tsx";

export interface OverviewListProps {
    walletCurrency: string;
    balance: number;
}

export default function TransactionOverviewList({walletCurrency, balance}: OverviewListProps) {
    const transInfo = useTransactionQueryStore(x => x.overview);
    return (
        <OverviewListBase balance={balance} changeForPeriod={transInfo.changeForPeriod} currency={walletCurrency}
                          expenseForPeriod={transInfo.expenseForPeriod} incomeForPeriod={transInfo.incomeForPeriod}/>
    )
}

