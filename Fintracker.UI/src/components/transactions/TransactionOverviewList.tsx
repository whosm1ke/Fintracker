import {Transaction} from "../../entities/Transaction.ts";
import OverviewListBase from "../other/OverviewListBase.tsx";
import {
    calcExpenseAndIncome,
    filterTransactions,
} from "../../helpers/globalHelper.ts";
import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";
import {useMemo} from "react";

export interface OverviewListProps {
    walletCurrency: string;
    balance: number;
    transactions: Transaction[]
}

export default function TransactionOverviewList({walletCurrency, balance, transactions}: OverviewListProps) {

    const filters = useTransactionQueryStore(x => x.filters);

    const filteredTransactions = useMemo(() => filterTransactions(transactions, filters), [transactions, filters]);




    const expAndInc = useMemo(() => calcExpenseAndIncome(filteredTransactions, null), [filteredTransactions]);

    return (
        <OverviewListBase balanceTitle={"Wallet balance"} changeForPeriodTitle={"Change for period"}
                          expenseTitle={"Expense for period"} incomeTitle={"Income for period"} balance={balance}
                          changeForPeriod={expAndInc.expense + expAndInc.income} currency={walletCurrency}
                          expenseForPeriod={expAndInc.expense} incomeForPeriod={expAndInc.income}/>
    )
}


