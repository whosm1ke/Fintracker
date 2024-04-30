import {Navigate, useNavigate, useParams} from "react-router-dom";
import useBudget from "../../hooks/budgets/useBudget.ts";
import Spinner from "../../components/other/Spinner.tsx";
import {
    calculateDailyBudget,
    dateToString,
    getCurrencyRates, getUniqueCurrencySymbols,
} from "../../helpers/globalHelper.ts";
import DateFilterBase from "../../components/other/DateFilterBase.tsx";
import {FaChevronRight} from "react-icons/fa6";
import UpdateBudgetModal from "../../components/budgets/UpdateBudgetModal.tsx";
import BudgetOverviewList from "../../components/budgets/BudgetOverviewList.tsx";
import {Transaction} from "../../entities/Transaction.ts";
import {TransactionItem} from "../../components/transactions/TransactionItem.tsx";
import {useCurrencyConvertAll} from "../../hooks/currencies/useCurrenctConvertAll.tsx";
import useUserStore from "../../stores/userStore.ts";

export default function BudgetDetailsPage() {
    const {budgetId} = useParams()
    const navigate = useNavigate();
    const {data: budgetResponse, isLoading} = useBudget(budgetId!);
    const currenctUserId = useUserStore(x => x.getUserId());

    

    if (!budgetResponse || !budgetResponse.response || isLoading) return <Spinner/>
    const budget = budgetResponse.response;
    if (budget.ownerId != currenctUserId && !budget.members.find(u => u.id === currenctUserId)) return <Navigate
        to={'../../dashboard'}/>

    const start = dateToString(new Date(budget.startDate));
    const end = dateToString(new Date(budget.endDate));
    const spentByDay = calculateDailyBudget(budget);

    const spentPercentage = (budget.totalSpent * 100) / (budget.totalSpent + budget.balance);
    const spentBgColor = spentPercentage < 33 ? "bg-green-400" : spentPercentage < 66 ? "bg-yellow-200" : "bg-red-400"

    return (
        <div className={'container flex flex-col gap-y-5 mx-auto p-4'}>
            <div className={'w-full flex justify-end'}>
                <DateFilterBase startDate={start} endDate={end} canChangeDate={false}/>
            </div>
            <div className={'w-full bg-white flex items-center justify-between p-4 rounded-xl'}>
                <ul className={'flex items-center gap-x-2'}>
                    <li className={'flex items-center gap-x-2'}>
                        <button onClick={() => navigate(-1)} className={'underline-offset-2 hover:underline'}>Budgets
                        </button>
                        <FaChevronRight size={'1.2rem'}/>
                    </li>
                    <li>
                        <button>
                            <p className={'font-bold text-lg text-left'}>{budget.name}</p>
                            <p className={'text-sm -mt-1'}>{budget.wallet.name}</p>
                        </button>
                    </li>
                </ul>
                <div>
                    <UpdateBudgetModal userId={budget.ownerId} budget={budget}/>
                </div>
            </div>
            <BudgetOverviewList budget={budget}/>
            <div className={'w-full bg-white p-3 flex flex-col shadow rounded-lg'}>
                <h2 className={'font-semibold'}>Budget progress</h2>
                <div className={'px-7 text-center mt-5'}>
                    <span
                        className={'text-xl'}>Keep spending. You can spend <strong>{spentByDay.toFixed(2)}</strong> {budget.currency.symbol} per day until the end of period.</span>
                </div>
                <div className={'w-[80%] md:w-[50%] mt-10 flex flex-col justify-center mx-auto items-center'}>
                    <div className="h-12 w-full bg-gray-200 mt-2 rounded">
                        <div style={{width: `${spentPercentage}%`, maxWidth: '100%'}}
                             className={`h-full ${spentBgColor} rounded flex items-center justify-end`}>
                            <span className={'text-sm font-bold mr-2'}>{spentPercentage.toFixed(1)}%</span>
                        </div>
                    </div>
                    <div className="flex justify-between w-full text-sm text-gray-500 mt-2">
                        <span className={'text-gray-400'}>{new Date(start).toDateString()}</span>
                        <span className={'text-gray-400'}>{new Date(end).toDateString()}</span>
                    </div>
                </div>
            </div>
            {budget.transactions.length !== 0 && <BudgetTransactionItemList transactions={budget.transactions}
                                                                            budgetCurrencySymbol={budget.currency.symbol}
                                                                            budgetId={budgetId!}
                                                                            budgetOwnerId={budget.ownerId}/>}
        </div>
    )
}

interface BudgetTransactionItemListProps {
    transactions: Transaction[];
    budgetCurrencySymbol: string;
    budgetId: string;
    budgetOwnerId: string;
}

export function BudgetTransactionItemList({
                                              transactions,
                                              budgetCurrencySymbol,
                                              budgetId,
                                              budgetOwnerId
                                          }: BudgetTransactionItemListProps) {

    const uniqueSymbols = getUniqueCurrencySymbols(transactions);
    const {data: convertedCurrencies} = useCurrencyConvertAll({
        from: uniqueSymbols,
        to: budgetCurrencySymbol,
        amount: [1]
    })
    const currencyRates = getCurrencyRates(convertedCurrencies, uniqueSymbols);
    if (!convertedCurrencies) return <Spinner/>


    return (
        <div className={'w-full p-3 bg-white shadow rounded-lg'}>
            <h4 className={'font-semibold'}>Operations</h4>
            <div className={'mt-4'}>
                {transactions.map(t =>
                    <TransactionItem key={t.id} transaction={t} parentCurrencySymbol={budgetCurrencySymbol}
                                     conversionRate={currencyRates![t.currency.symbol]} showDate={true}
                                     budgetId={budgetId} walletOwnerId={budgetOwnerId}/>
                )}
            </div>
        </div>
    )
}
