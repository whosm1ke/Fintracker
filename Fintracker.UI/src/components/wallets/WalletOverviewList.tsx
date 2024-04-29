import useWalletInfoStore from "../../stores/walletStore.ts";
import OverviewListBase from "../other/OverviewListBase.tsx";
import {
    calcExpenseAndIncome,
    calculateWalletsBalance, filterTransactionsFrowWallets,
    getCurrencyRates,
    getUniqueCurrencySymbolsFromWallets,
} from "../../helpers/globalHelper.ts";
import {useCurrencyConvertAll} from "../../hooks/currencies/useCurrenctConvertAll.tsx";
import Spinner from "../other/Spinner.tsx";


interface WalletOverviewListProps {
    globalCurrency: string;
}

export default function WalletOverviewList({globalCurrency}: WalletOverviewListProps) {
    const walletFilters = useWalletInfoStore(x => x.filters);


    const uniqueSymbols = getUniqueCurrencySymbolsFromWallets(walletFilters.selectedWallets);
    const {data: convertedCurrencies, isLoading} = useCurrencyConvertAll({
        from: uniqueSymbols,
        to: globalCurrency,
        amount: [1]
    })

    if (isLoading) return <Spinner/>
    const currencyRates = getCurrencyRates(convertedCurrencies, uniqueSymbols);
    const totalBalance = calculateWalletsBalance(walletFilters.selectedWallets, currencyRates);
    const filteredTransactions = filterTransactionsFrowWallets(walletFilters.selectedWallets, {
        minMaxRange: walletFilters.minMaxRange,
        note: walletFilters.note,
        categories: walletFilters.categories,
        users: []
    }, walletFilters.startDate, walletFilters.endDate)


    const expsenseAndIncome = calcExpenseAndIncome(filteredTransactions, currencyRates);
    return (
        <OverviewListBase
            balanceTitle={"Total balance"} changeForPeriodTitle={"Change for period"}
            expenseTitle={"Expense for period"} incomeTitle={"Income for period"}
            balance={totalBalance} changeForPeriod={expsenseAndIncome.expense + expsenseAndIncome.income}
            currency={globalCurrency}
            expenseForPeriod={expsenseAndIncome.expense} incomeForPeriod={expsenseAndIncome.income}/>
    )
}