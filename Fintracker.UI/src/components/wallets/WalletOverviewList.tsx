import useWalletInfoStore from "../../stores/walletStore.ts";
import OverviewListBase from "../other/OverviewListBase.tsx";
import {
    calcExpenseAndIncome,
    calculateWalletsBalance, filterTransactionsFrowWallets,
    getCurrencyRates,
    getUniqueCurrencySymbols,
} from "../../helpers/globalHelper.ts";
import {useCurrencyConvertAll} from "../../hooks/currencies/useCurrenctConvertAll.tsx";
import Spinner from "../other/Spinner.tsx";


interface WalletOverviewListProps {
    globalCurrency: string;
}
export default function WalletOverviewList({ globalCurrency}: WalletOverviewListProps) {
    const walletFilters = useWalletInfoStore(x => x.filters);


    const allTransactions = walletFilters.selectedWallets.flatMap(w => w.transactions);
    const uniqueSymbols = getUniqueCurrencySymbols(allTransactions);
    const {data: convertedCurrencies, isLoading} = useCurrencyConvertAll({from: uniqueSymbols, to: globalCurrency, amount: [1]})
    
    if(isLoading) return <Spinner/>
    const currencyRates = getCurrencyRates(convertedCurrencies, uniqueSymbols);
    const totalBalance = calculateWalletsBalance(walletFilters.selectedWallets, currencyRates);
    const filteredTransactions = filterTransactionsFrowWallets(walletFilters.selectedWallets, {
        minMaxRange: walletFilters.minMaxRange,
        note: walletFilters.note,
        categories: walletFilters.categories,
        users: []
    }, walletFilters.startDate, walletFilters.endDate)

    console.log("filteredTransactions: ", filteredTransactions)
    console.log("walletFilters: ", walletFilters)
    
    const expsenseAndIncome = calcExpenseAndIncome(filteredTransactions, currencyRates);
    return (
        <OverviewListBase balance={totalBalance} changeForPeriod={expsenseAndIncome.expense + expsenseAndIncome.income} currency={globalCurrency}
                          expenseForPeriod={expsenseAndIncome.expense} incomeForPeriod={expsenseAndIncome.income}/>
    )
}