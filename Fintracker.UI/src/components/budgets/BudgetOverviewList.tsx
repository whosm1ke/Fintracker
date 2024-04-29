import {Budget} from "../../entities/Budget.ts";
import OverviewListBase from "../other/OverviewListBase.tsx";
import {calculateDailyBudget} from "../../helpers/globalHelper.ts";

interface BudgetOverviewListProps {
    budget: Budget
}

export default function BudgetOverviewList({budget}: BudgetOverviewListProps) {
const spentPerDay = calculateDailyBudget(budget);
    return (
        <OverviewListBase currency={budget.currency.symbol} balance={budget.startBalance} changeForPeriod={-budget.totalSpent}
                          expenseForPeriod={budget.balance} incomeForPeriod={spentPerDay} numberPostfix={"/ PER DAY"}
                          balanceTitle={"Start balance"} changeForPeriodTitle={"Already spent"}
                          incomeTitle={"Spent per day"} expenseTitle={"You can spent"}/>
    )
}