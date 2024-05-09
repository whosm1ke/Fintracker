import {useParams} from "react-router-dom";
import {useBudgets} from "../../hooks/budgets/useBudgets";
import useUserStore from "../../stores/userStore.ts";
import {BudgetCard} from "../../components/budgets/BudgetCard.tsx";
import CreateBudgetModalWalletPersonal from "../../components/budgets/CreateBudgetModalWalletPersonal.tsx";
import BudgetOverview from "../../components/budgets/BudgetOverview.tsx";
import CreateBudgetModalGlobal from "../../components/budgets/CreateBudgetModalGlobal.tsx";
import useExpenseCategories from "../../hooks/categories/useExpenseCategories.ts";

export default function BudgetsPage() {
    const {walletId} = useParams();
    const userId = useUserStore(x => x.getUserId());
    const {data: budgets} = useBudgets(walletId!);
    const {data: categories} = useExpenseCategories(userId!);


    if (budgets === undefined || categories === undefined ) return null;

    return (
        <div className={'container mx-auto p-4 overflow-hidden'}>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Budgets</h2>
                    {walletId ? <CreateBudgetModalWalletPersonal walletId={walletId} userId={userId!} categories={categories}/> :
                        <CreateBudgetModalGlobal userId={userId!} categories={categories}/>
                    }
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                    {budgets && budgets.map(b => {
                        return (<BudgetCard walletName={b.wallet.name}
                                            balance={b.balance} currencySymbol={b.currency.symbol}
                                            endDate={b.endDate} startDate={b.startDate} name={b.name}
                                            totalSpent={b.totalSpent} budgetId={b.id} isPublic={b.isPublic}
                                            key={b.id || "Pipa"} userId={userId!} startBalance={b.startBalance}
                                            budgetUserId={b.ownerId}/>)

                    })}
                </div>
            </section>
            <BudgetOverview/>
        </div>
    )
}






