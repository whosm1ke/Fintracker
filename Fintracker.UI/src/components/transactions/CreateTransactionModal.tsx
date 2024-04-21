import {
    Transaction, amountRegisterForTransaction,
    dateRegisterForTransaction, labelRegisterForTransaction, noteRegisterForTransaction
} from "../../entities/Transaction";
import {SubmitHandler, useForm} from "react-hook-form";
import {useState} from "react";
import {Currency} from "../../entities/Currency";
import currencies from "../../data/currencies";
import {Category} from "../../entities/Category";
import useStopScrolling from "../../hooks/other/useStopScrolling";
import Spinner from "../other/Spinner.tsx";
import {ActionButton} from "../other/ActionButton.tsx";
import {HiX} from "react-icons/hi";
import useCreateTransaction from "../../hooks/transactions/useCreateTransaction.ts";
import useCategories from "../../hooks/categories/useCategories.ts";
import SingleSelectDropDownMenu from "../other/SingleSelectDropDownMenu.tsx";
import CurrencyItem from "../currencies/CurrencyItem.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";


interface AddTransactionModalProps {
    userId: string;
    walletId: string;
}

export const CreateTransactionModal = ({userId, walletId}: AddTransactionModalProps) => {
    const {register, handleSubmit, clearErrors, reset, setError, formState: {errors}} = useForm<Transaction>();
    const transactionMutation = useCreateTransaction();
    const {data: categories} = useCategories();
    const [isOpen, setIsOpen] = useState(false);
    const [selectedCurrency, setSelectedCurrency] = useState<Currency | undefined>(undefined);

    const [selectedCategory, setSelectedCategory] = useState<Category | undefined>(undefined)
    useStopScrolling(isOpen)

    if (!categories) return <Spinner/>

    const handleSelectedCurrency = (currency: Currency | undefined) => {
        setSelectedCurrency(currency)
    }

    const handleSelectedCategory = (category: Category | undefined) => {
        setSelectedCategory(category)
    }

    function handleOpenModal() {
        setIsOpen(p => !p);
        handleSelectedCategory(undefined);
        handleSelectedCurrency(undefined);
        reset();
        clearErrors();
    }


    const onSubmit: SubmitHandler<Transaction> = async (model: Transaction) => {
        if (!selectedCategory) {
            setError("categoryId", {message: "Category is required"});
        } else {
            model.category = selectedCategory;
            clearErrors("categoryId")
        }

        if (!selectedCurrency) {
            setError("currencyId", {message: "Currency is required"});
        } else {
            clearErrors("currencyId")
            model.currency = selectedCurrency;
        }

        model.userId = userId;
        model.currencyId = selectedCurrency!.id;
        model.categoryId = selectedCategory!.id;
        model.walletId = walletId;
        await transactionMutation.mutateAsync(model, {
            onSuccess: () => {
                reset();
                clearErrors();
                handleOpenModal();
            }
        });
    };

    return (
        <>
            <ActionButton text={"Add new transaction"} onModalOpen={handleOpenModal}/>
            <div className={`absolute inset-0 flex justify-center items-start p-4 sm:p-2 z-10
                        ${isOpen ? 'visible bg-black/20' : 'invisible'}`}>
                <div className="bg-white p-4 rounded-md shadow-lg max-w-full mx-auto">
                    <h2 className="text-2xl font-bold mb-4 flex justify-between">Add transaction
                        <HiX size={'2rem'} color={'red'} onClick={() => {
                            handleOpenModal()
                        }}/>
                    </h2>
                    <form onSubmit={handleSubmit(onSubmit)} className={''}>
                        <div className={'grid grid-cols-1 md:grid-cols-2 gap-x-10'}>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Note">
                                    Note
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="Note"
                                    type="text"
                                    {...register("note", noteRegisterForTransaction)}
                                />
                                {errors.note && <p className={'text-red-400 italic'}>{errors.note.message}</p>}
                            </div>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Label">
                                    Label
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="Label"
                                    type="text"
                                    {...register("label", labelRegisterForTransaction)}
                                />
                                {errors.label && <p className={'text-red-400 italic'}>{errors.label.message}</p>}
                            </div>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Amount">
                                    Amount
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="Amount"
                                    type="number"
                                    {...register("amount", amountRegisterForTransaction)}
                                />
                                {errors.amount && <p className={'text-red-400 italic'}>{errors.amount.message}</p>}
                            </div>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Date">
                                    Date
                                </label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="Date"
                                    type="date"
                                    defaultValue={new Date().toLocaleDateString('en-CA')}
                                    {...register("date", dateRegisterForTransaction)}
                                />

                                {errors.date && <p className={'text-red-400 italic'}>{errors.date.message}</p>}
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
                                <label className="block text-gray-700 text-sm font-bold mb-2 col-start-1 col-span-2"
                                       htmlFor="CurrencyId">Category</label>
                                <div {...register("categoryId")}>
                                    <SingleSelectDropDownMenu items={categories} ItemComponent={CategoryItem}
                                                              defaultSelectedItem={selectedCategory}
                                                              heading={"Category"}
                                                              onItemSelected={handleSelectedCategory}/>
                                    {errors.categoryId &&
                                        <p className={'text-red-400 italic'}>{errors.categoryId.message}</p>}
                                </div>
                            </div>
                        </div>
                        <div className="flex items-center justify-between">
                            <button
                                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                                type="submit"
                            >
                                Add Transaction
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
}

//@ts-ignore
export default CreateTransactionModal;