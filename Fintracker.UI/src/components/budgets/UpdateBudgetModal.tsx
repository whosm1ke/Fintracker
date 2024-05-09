import {SubmitHandler, useForm} from "react-hook-form";
import {
    balanceRegisterOptionsForBudget,
    Budget, endDateRegisterOptionsForBudget,
    nameRegisterOptionsForBudget,
    startDateRegisterOptionsForBudget
} from "../../entities/Budget.ts";
import {useParams} from "react-router-dom";
import useWallets from "../../hooks/wallet/useWallets.ts";
import useExpenseCategories from "../../hooks/categories/useExpenseCategories.ts";
import {useLayoutEffect, useState} from "react";
import {Wallet} from "../../entities/Wallet.ts";
import {Currency} from "../../entities/Currency.ts";
import {Category} from "../../entities/Category.ts";
import useStopScrolling from "../../hooks/other/useStopScrolling.ts";
import {ActionButton} from "../other/ActionButton.tsx";
import {HiX} from "react-icons/hi";
import SingleSelectDropDownMenu from "../other/SingleSelectDropDownMenu.tsx";
import currencies from "../../data/currencies.ts";
import CurrencyItem from "../currencies/CurrencyItem.tsx";
import WalletItem from "../wallets/WalletItem.tsx";
import MultiSelectDropDownMenu from "../other/MultiSelectDropDownMenu.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";
import useUpdateBudget from "../../hooks/budgets/useUpdateBudget.ts";
import {dateToString} from "../../helpers/globalHelper.ts";
import useUserStore from "../../stores/userStore.ts";

interface UpdateBudgetModalProps {
    userId: string,
    budget: Budget;
}

const UpdateBudgetModal = ({userId, budget}: UpdateBudgetModalProps) => {

    const {register, handleSubmit, clearErrors, reset, setError, formState: {errors}} = useForm<Budget>();
    const currentUserId = useUserStore(x => x.getUserId())
    const {walletId} = useParams();
    const budgetMutation = useUpdateBudget();
    const {data: wallets} = useWallets(userId)
    const {data: categories} = useExpenseCategories(budget.ownerId);
    const [isOpen, setIsOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [selectedWallet, setSelectedWallet] = useState<Wallet | undefined>(undefined)
    const [selectedCurrency, setSelectedCurrency] = useState<Currency | undefined>(undefined)
    const [selectedCategories, setSelectedCategories] = useState<Category[]>([])
    useStopScrolling(isOpen);

    useLayoutEffect(() => {
        if (isOpen) {
            setSelectedWallet(budget.wallet);
            setSelectedCurrency(budget.currency);
            setSelectedCategories(budget.categories);
        }
    }, [isOpen]);


    if (!wallets || !categories) return null;
    const start = dateToString(new Date(budget.startDate));
    const end = dateToString(new Date(budget.endDate));


    function handleOpenModal() {
        handleSelectedCurrency(undefined);
        setSelectedCategories([])
        setIsOpen(p => !p);
    }

    const handleSelectedWallet = (wallet: Wallet) => setSelectedWallet(wallet);

    function handleSelectedCurrency(currency: Currency | undefined) {
        setSelectedCurrency(currency);
    }

    const handleToggleCategoryId = (category: Category) => {

        if (selectedCategories.includes(category)) {
            setSelectedCategories(prev => prev.filter(c => c.id !== category.id));
        } else {
            setSelectedCategories(prev => [...prev, category!]);
        }
    };

    const handleSelectAllCategories = () => {
        if (selectedCategories.length === categories.length) {
            setSelectedCategories([]);
        } else {
            setSelectedCategories(categories);
        }
    }

    const onSubmit: SubmitHandler<Budget> = async (model: Budget) => {
        if (selectedCategories.length === 0) {
            setError("categoryIds", {message: "Budget must contain at least one category"})
            return;
        } else {
            clearErrors("categoryIds")
            model.categoryIds = selectedCategories.map(c => c.id);
        }

        if (!selectedCurrency) {
            setError("currencyId", {message: "Currency is required"})
            return;
        } else {
            clearErrors("currencyId")
            model.currencyId = selectedCurrency.id;
        }

        if (walletId)
            model.walletId = walletId
        else
            model.walletId = selectedWallet!.id

        setIsLoading(true)
        model.id = budget.id;
        model.ownerId = userId;
        model.currencyId = selectedCurrency!.id;
        model.currency = selectedCurrency!;
        model.categories = selectedCategories
        model.members = budget.members
        model.wallet = selectedWallet || wallets.find(w => w.id === walletId)!;
        await budgetMutation.mutateAsync(model, {
            onSuccess: () => {
                reset();
                clearErrors();
                handleOpenModal();
                setIsLoading(false)
            },
            onError: () => {
                setIsLoading(false)
            }
        });
    };

    return (
        <>
            <ActionButton text={"Update budget"} onModalOpen={handleOpenModal}/>
            {isOpen && <div
                className={'absolute inset-0 flex justify-center items-start px-4 lg:px-0 visible bg-black/20 z-50'}>
                <div className="bg-white p-4 rounded-md shadow-lg max-w-full mx-auto mt-4">
                    <h2 className="text-2xl font-bold mb-4 flex justify-between">Update budget
                        <HiX size={'2rem'} color={'red'} onClick={() => {
                            reset()
                            clearErrors()
                            handleOpenModal()
                            setSelectedCategories([])
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
                                    defaultValue={budget.name}
                                    {...register("name", nameRegisterOptionsForBudget)}
                                />
                                {errors.name && <p className={'text-red-400 italic'}>{errors.name.message}</p>}
                            </div>

                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="balance">
                                    Start balance
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="balance"
                                    type="number"
                                    defaultValue={budget.startBalance}
                                    {...register("startBalance", balanceRegisterOptionsForBudget)}
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
                                    defaultValue={start}
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
                                    defaultValue={end}
                                    {...register("endDate", endDateRegisterOptionsForBudget)}
                                />

                                {errors.endDate && <p className={'text-red-400 italic'}>{errors.endDate.message}</p>}
                            </div>
                            <div className="mb-4 col-start-1 col-span-2">
                                <label className="block text-gray-700 text-sm font-bold mb-2"
                                       htmlFor="CurrencyId">Currency</label>
                                <div {...register("currencyId")}>
                                    <SingleSelectDropDownMenu items={currencies} ItemComponent={CurrencyItem}
                                                              defaultSelectedItem={selectedCurrency}
                                                              heading={"Currency"}
                                                              onItemSelected={handleSelectedCurrency}/>
                                    {errors.currencyId &&
                                        <p className={'text-red-400 italic'}>{errors.currencyId.message}</p>}
                                </div>
                            </div>
                            <div className="mb-4 col-start-1 col-span-2">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="walletId">
                                    Wallet
                                </label>
                                <div {...register("walletId")}>
                                    <SingleSelectDropDownMenu items={wallets} ItemComponent={WalletItem}
                                                              defaultSelectedItem={selectedWallet}
                                                              heading={"Wallet"} onItemSelected={handleSelectedWallet}/>
                                    {errors.walletId &&
                                        <p className={'text-red-400 italic'}>{errors.walletId.message}</p>}
                                </div>
                            </div>

                            <div className={'mb-4 col-start-1 col-span-2 sm:col-start-0 sm:col-span-0'}
                                 {...register("categoryIds")}>
                                <span className="block text-gray-700 text-sm font-bold mb-2">
                                    Select categories
                                </span>
                                <MultiSelectDropDownMenu items={categories} onItemSelected={handleToggleCategoryId}
                                                         selectedItems={selectedCategories} heading={"Categories"}
                                                         onAllItemsSelected={handleSelectAllCategories}
                                                         ItemComponent={CategoryItem}/>

                                {errors.categoryIds &&
                                    <p className={'text-red-400 italic'}>{errors.categoryIds.message}</p>}
                            </div>
                        </div>
                        <div className="flex items-center justify-between">
                            <button
                                className={isLoading ? "inactive-create-cash-wallet-button" : "create-cash-wallet-button"}
                                type="submit"
                            >
                                Update budget
                            </button>
                            {<div className={`flex gap-4 ${currentUserId !== budget.ownerId ? 'hidden' : ''}`}>
                                <label className="block text-sm font-bold" htmlFor="isPublic">
                                    Public?
                                </label>
                                <div className="relative">
                                    <input
                                        className="h-6 w-6 border-blue-500 rounded-full cursor-pointer checked:border-transparent 
                                        checked:bg-blue-500"
                                        id="isPublic"
                                        type="checkbox"
                                        defaultChecked={budget.isPublic}
                                        {...register("isPublic")}
                                    />
                                </div>
                            </div>}
                        </div>
                    </form>
                </div>
            </div>}
        </>
    );
}

export default UpdateBudgetModal;