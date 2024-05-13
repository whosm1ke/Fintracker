import {Wallet} from "../../entities/Wallet.ts";
import {Currency} from "../../entities/Currency.ts";
import {Category} from "../../entities/Category.ts";
import {SubmitHandler, useForm} from "react-hook-form";
import {
    balanceRegisterOptionsForBudget,
    Budget, endDateRegisterOptionsForBudget,
    nameRegisterOptionsForBudget,
    startDateRegisterOptionsForBudget
} from "../../entities/Budget.ts";
import useCreateBudget from "../../hooks/budgets/useCreateBudget.ts";
import {useState} from "react";
import {useNavigate} from "react-router-dom";
import {ActionButton} from "../other/ActionButton.tsx";
import {HiX} from "react-icons/hi";
import SingleSelectDropDownMenu from "../other/SingleSelectDropDownMenu.tsx";
import currencies from "../../data/currencies.ts";
import CurrencyItem from "../currencies/CurrencyItem.tsx";
import WalletItem from "../wallets/WalletItem.tsx";
import MultiSelectDropDownMenu from "../other/MultiSelectDropDownMenu.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";

interface CreateBudgetModalBaseProps {
    userId: string,
    walletId: string | undefined,
    handleSelectedWallet: (w: Wallet) => void;
    handleSelectedCurrency: (currency: Currency | undefined) => void
    handleToggleCategoryId: (category: Category) => void;
    handleSelectAllCategories: () => void;
    refreshCategories: () => void;
    categoriesToShow: Category[];
    isActionButtonActive: boolean;
    showWallets: boolean;
    walletsToShow: Wallet[];
    selectedWallet: Wallet | undefined;
    selectedCurrency: Currency | undefined;
    showAddWalletBtn: boolean;
    selectedCategories: Category[];
}


export default function CreateBudgetModalBase ({
                                          userId,
                                          handleSelectAllCategories,
                                          handleToggleCategoryId,
                                          handleSelectedCurrency,
                                          handleSelectedWallet,
                                          refreshCategories,
                                          isActionButtonActive,
                                          showWallets,
                                          walletsToShow,
                                          selectedWallet,
                                          showAddWalletBtn,
                                          categoriesToShow,
                                          selectedCurrency,
                                          selectedCategories,
                                          walletId
                                      }: CreateBudgetModalBaseProps) {
    const {register, handleSubmit, clearErrors, reset, setError, formState: {errors}} = useForm<Budget>();
    const budgetMutation = useCreateBudget();
    const [isOpen, setIsOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();


    function handleOpenModal() {
        handleSelectedCurrency(undefined);
        refreshCategories()
        setIsOpen(p => !p);
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
        else {
            if (!selectedWallet) {
                setError("walletId", {message: "Wallet is required"})
                return;
            } else {
                clearErrors("walletId")
                model.walletId = selectedWallet!.id
            }
        }
        setIsLoading(true)
        model.ownerId = userId;
        model.currencyId = selectedCurrency!.id;
        model.currency = selectedCurrency!;
        model.categories = selectedCategories
        model.wallet = selectedWallet || walletsToShow.find(w => w.id === walletId)!;
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
            <ActionButton text={"Add new budget"} onModalOpen={handleOpenModal}
                          isActive={isActionButtonActive}/>
            {isOpen && <div
                className={'fixed inset-0 flex justify-center items-center px-4 visible bg-black/20 z-50'}>
                <div className="bg-white p-4 rounded-md shadow-lg max-w-full mx-auto mt-4">
                    <h2 className="text-2xl font-bold mb-4 flex justify-between">Add budget
                        <HiX size={'2rem'} color={'red'} onClick={() => {
                            reset()
                            clearErrors()
                            handleOpenModal()
                            refreshCategories()
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
                                    Start balance
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="balance"
                                    type="number"
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
                                    defaultValue={(function () {
                                        const tomorrow = new Date();
                                        tomorrow.setDate(tomorrow.getDate() + 1);
                                        return tomorrow.toLocaleDateString('en-CA');
                                    })()}

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
                            {showWallets &&
                                <div className="mb-4 col-start-1 col-span-2">
                                    <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="walletId">
                                        Wallet
                                    </label>
                                    <div {...register("walletId")}>
                                        <SingleSelectDropDownMenu items={walletsToShow}
                                                                  ItemComponent={WalletItem}
                                                                  defaultSelectedItem={selectedWallet}
                                                                  heading={"Wallet"}
                                                                  onItemSelected={handleSelectedWallet}/>
                                        {showAddWalletBtn ? (<div
                                            className={'px-4 py-2 mt-4 bg-green-400 w-1/3 text-center rounded-full font-semibold'}>
                                            <button className={'text-white text-sm'}
                                                    onClick={() => navigate("/dashboard", {state: "showArrow"})}
                                            >Add new Wallet
                                            </button>
                                        </div>) : null}
                                        {errors.walletId &&
                                            <p className={'text-red-400 italic'}>{errors.walletId.message}</p>}
                                    </div>
                                </div>}

                            <div className={'mb-4 col-start-1 col-span-2 sm:col-start-0 sm:col-span-0'}
                                 {...register("categoryIds")}>
                                <span className="block text-gray-700 text-sm font-bold mb-2">
                                    Select categories
                                </span>
                                <MultiSelectDropDownMenu items={categoriesToShow}
                                                         onItemSelected={handleToggleCategoryId}
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
                                        {...register("isPublic")}
                                    />
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>}
        </>
    );
}
