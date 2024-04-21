import {SubmitHandler, useForm } from "react-hook-form";
import { Budget, balanceRegisterOptionsForBudget,
    endDateRegisterOptionsForBudget, nameRegisterOptionsForBudget, startDateRegisterOptionsForBudget } from "../../entities/Budget";
import { useParams } from "react-router-dom";
import useCreateBudget from "../../hooks/budgets/useCreateBudget";
import useWallets from "../../hooks/wallet/useWallets";
import Spinner from "../other/Spinner.tsx";
import useCategories from "../../hooks/categories/useCategories.ts";
import { useState } from "react";
import { Currency } from "../../entities/Currency.ts";
import currencies from "../../data/currencies.ts";
import useStopScrolling from "../../hooks/other/useStopScrolling.ts";
import {ActionButton} from "../other/ActionButton.tsx";
import {HiX} from "react-icons/hi";
import CategoriesDropDownList from "../categories/CategoriesDropDownList.tsx";


interface CreateBudgetModalProps {
    userId: string,
}

export const CreateBudgetModal = ({userId}: CreateBudgetModalProps) => {
    const {register, handleSubmit, clearErrors, reset, setError, formState: {errors}} = useForm<Budget>();
    const {walletId} = useParams();
    const budgetMutation = useCreateBudget();
    const {data: wallets} = useWallets(userId)
    const {data: categories} = useCategories();
    const [isOpen, setIsOpen] = useState(false);
    const [selectedCurrency, setSelectedCurrency] = useState<Currency>(currencies[0])
    const [selectedCategoryIds, setSelectedCategoryIds] = useState<string[]>([])
    useStopScrolling(isOpen)
    if (wallets === undefined || categories === undefined) return <Spinner/>

    function handleOpenModal() {
        setIsOpen(p => !p);
    }

    function handleSelectedCurrency(currencyId: string) {
        const currency = currencies.find(c => c.id === currencyId) || currencies[0];
        setSelectedCurrency(currency);
    }

    const handleToggleCategoryId = (categoryId: string) => {
        if (selectedCategoryIds.includes(categoryId)) {
            setSelectedCategoryIds(prev => prev.filter(id => id !== categoryId));
        } else {
            setSelectedCategoryIds(prev => [...prev, categoryId]);
        }
    };

    const handleSelectAllCategories = () => {
        if (selectedCategoryIds.length === categories.length) {
            setSelectedCategoryIds([]);
        } else {
            setSelectedCategoryIds(categories.map(c => c.id));
        }
    }

    const onSubmit: SubmitHandler<Budget> = async (model: Budget) => {
        if (selectedCategoryIds.length === 0) {
            setError("categoryIds", {message: "Budget must contain at least one category"})
            return;
        }
        else{
            clearErrors("categoryIds")
        }

        model.userId = userId;
        model.currency = selectedCurrency;
        
        if (walletId)
            model.walletId = walletId
        model.categoryIds = selectedCategoryIds
        console.log("model: ", model)
        await budgetMutation.mutateAsync(model, {
            onSuccess: () => {
                reset();
                clearErrors();
                handleOpenModal();
                setSelectedCategoryIds([])
            }
        });
    };


    return (
        <>
            <ActionButton text={"Add new budget"} onModalOpen={handleOpenModal}/>
            <div className={`absolute inset-0 flex justify-center items-start px-4 lg:px-0
                        ${isOpen ? 'visible bg-black/20' : 'invisible'}`}>
                <div className="bg-white p-4 rounded-md shadow-lg max-w-full mx-auto mt-4">
                    <h2 className="text-2xl font-bold mb-4 flex justify-between">Add budget
                        <HiX size={'2rem'} color={'red'} onClick={() => {
                            reset()
                            clearErrors()
                            handleOpenModal()
                            setSelectedCategoryIds([])
                        }}/>
                    </h2>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <div className={'grid grid-cols-2 sm:grid-cols-2 gap-x-5'}>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="name">
                                    Name
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="name"
                                    type="text"
                                    {...register("name", nameRegisterOptionsForBudget)}
                                />
                                {errors.name && <p className={'text-red-400 italic'}>{errors.name.message}</p>}
                            </div>
                            
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="balance">
                                    Balance
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="balance"
                                    type="number"
                                    {...register("balance", balanceRegisterOptionsForBudget)}
                                />
                                {errors.balance && <p className={'text-red-400 italic'}>{errors.balance.message}</p>}
                            </div>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="startDate">
                                    Start Date
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="startDate"
                                    type="date"
                                    defaultValue={new Date().toLocaleDateString('en-CA')}
                                    {...register("startDate", startDateRegisterOptionsForBudget)}
                                />

                                {errors.startDate &&
                                    <p className={'text-red-400 italic'}>{errors.startDate.message}</p>}
                            </div>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="endDate">
                                    End Date
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="endDate"
                                    type="date"
                                    defaultValue={(function() {
                                        const tomorrow = new Date();
                                        tomorrow.setDate(tomorrow.getDate() + 1);
                                        return tomorrow.toLocaleDateString('en-CA');
                                    })()}

                                    {...register("endDate", endDateRegisterOptionsForBudget)}
                                />

                                {errors.endDate && <p className={'text-red-400 italic'}>{errors.endDate.message}</p>}
                            </div>
                            <div className="mb-4 col-start-1 col-span-2 sm:col-start-0 sm:col-span-0">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="CurrencyId">
                                    Currency
                                </label>
                                <select
                                    className="shadow border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="CurrencyId"
                                    {...register("currencyId", {
                                        required: "Currency is required for transaction",
                                        value: currencies[0].id
                                    })}
                                    onChange={(e) => handleSelectedCurrency(e.target.value)}
                                >
                                    {currencies.map((currency) => (
                                        <option key={currency.id} value={currency.id}>
                                            {currency.name} ({currency.symbol})
                                        </option>
                                    ))}
                                </select>
                                {errors.currencyId &&
                                    <p className={'text-red-400 italic'}>{errors.currencyId.message}</p>}
                            </div>
                            {!walletId && <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="walletId">
                                    Wallet
                                </label>
                                <select
                                    className="shadow border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="walletId"
                                    {...register("walletId", {
                                        required: "Wallet is required for budget",
                                        value: wallets[0].id
                                    })}
                                    onChange={(e) => handleSelectedCurrency(e.target.value)}
                                >
                                    {wallets.map((wallet) => {
                                            if (!wallet.isBanking) {
                                                return (<option key={wallet.id} value={wallet.id}>
                                                    {wallet.name} ({wallet.balance})
                                                </option>)
                                            }
                                        }
                                    )}
                                </select>
                                {errors.walletId &&
                                    <p className={'text-red-400 italic'}>{errors.walletId.message}</p>}
                            </div>}

                            <div className={'mb-4 col-start-1 col-span-2 sm:col-start-0 sm:col-span-0'}
                                 {...register("categoryIds")}>
                                <span className="block text-gray-700 text-sm font-bold mb-2">
                                    Select categories
                                </span>
                                <CategoriesDropDownList categories={categories}
                                                        selectedCategoryIds={selectedCategoryIds}
                                                        handleSelectAllCategories={handleSelectAllCategories}
                                                        handleToggleCategoryId={handleToggleCategoryId}/>
                                {errors.categoryIds &&
                                    <p className={'text-red-400 italic'}>{errors.categoryIds.message}</p>}
                            </div>
                        </div>
                        <div className="flex items-center justify-between">
                            <button
                                className="bg-blue-500 hover:bg-blue-700 text-white text-[1rem] sm:text-lg font-semibold sm:font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                                type="submit"
                            >
                                Add budget
                            </button>
                            <div className="flex gap-4">
                                <label className="block text-sm font-bold" htmlFor="isPublic">
                                    Public?
                                </label>
                                <div className="relative">
                                    <input
                                        className="h-6 w-6 border-blue-500 rounded-full cursor-pointer checked:border-transparent 
                                        checked:bg-blue-500"
                                        id="isPublic"
                                        type="checkbox"
                                        defaultChecked={false}
                                    />
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
}

