import {useParams} from "react-router-dom";
import useUserStore from "../stores/userStore.ts";

export default function BudgetsPage() {
    const {id: walletId} = useParams();
    const userId = useUserStore(x => x.getUserId());
    return (
        <div className={'container mx-auto p-4 overflow-hidden'}>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Budgets</h2>
                    {walletId && <button>Create</button>}
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                    Show all budgets
                </div>
            </section>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Overview</h2>
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                </div>
            </section>
        </div>
    )
}

