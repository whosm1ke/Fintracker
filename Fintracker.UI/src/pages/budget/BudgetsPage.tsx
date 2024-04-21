import {useParams} from "react-router-dom";
import {useBudgets} from "../../hooks/budgets/useBudgets";
import useUserStore from "../../stores/userStore.ts";
import {BudgetCard} from "../../components/budgets/BudgetCard.tsx";
import {CreateBudgetModal} from "../../components/budgets/CreateBudgetModal.tsx";
import BudgetOverview from "../../components/budgets/BudgetOverview.tsx";
import Spinner from "../../components/other/Spinner.tsx";

export default function BudgetsPage() {
    const {walletId} = useParams();
    const userId = useUserStore(x => x.getUserId());
    const {data: budgets, isRefetching, isLoading} = useBudgets(walletId!);

    
    if (budgets === undefined || isRefetching || isLoading)
        return <Spinner/>;

    return (
        <div className={'container mx-auto p-4 overflow-hidden'}>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Budgets</h2>
                    <CreateBudgetModal userId={userId!}/>
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                    {budgets && budgets.map(b => {
                
                        return (<BudgetCard walletName={b.wallet?.name || "Wallet name"}
                                            balance={b.balance} currencySymbol={b.currency.symbol}
                                            endDate={b.endDate} startDate={b.startDate} name={b.name || "Budget name"}
                                            totalSpent={b.totalSpent} budgetId={b.id} isPublic={b.isPublic}
                                            key={b.id}/>)

                    })}
                </div>
            </section>
            <BudgetOverview/>
        </div>
    )
}






