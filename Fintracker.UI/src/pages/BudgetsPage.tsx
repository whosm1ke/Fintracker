import { useParams} from "react-router-dom";
import {useBudgets} from "../hooks/useBudgets.ts";
import {BudgetCard} from "../components/BudgetCard.tsx";
import {CreateBudgetModal} from "../components/CreateBudgetModal.tsx";
import BudgetOverview from "../components/BudgetOverview.tsx";

export default function BudgetsPage() {
    const {walletId} = useParams();
    const {data: budgets, isFetching, isLoading, isError} = useBudgets(walletId!, null);

    if(isFetching || isLoading || isError)
        return null;

    return (
        <div className={'container mx-auto p-4 overflow-hidden'}>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Budgets</h2>
                    {walletId && <CreateBudgetModal/>}
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                    {budgets && budgets.map(b =>
                        <BudgetCard walletName={b.wallet.name}
                                    balance={b.balance} currencySymbol={b.currency.symbol}
                                    endDate={b.endDate} startDate={b.startDate} name={b.name}
                                    totalSpent={b.totalSpent} budgetId={b.id} isPublic={b.isPublic}
                                    key={b.id}/>
                    )}
                </div>
            </section>
            <BudgetOverview/>
        </div>
    )
}






