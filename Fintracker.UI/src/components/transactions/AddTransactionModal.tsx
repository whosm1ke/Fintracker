import { Transaction, amountRegisterForTransaction,
    dateRegisterForTransaction, labelRegisterForTransaction, noteRegisterForTransaction } from "../../entities/Transaction";
import {SubmitHandler, useForm } from "react-hook-form";
import { useState } from "react";
import { Currency } from "../../entities/Currency";
import currencies from "../../data/currencies";
import { Category } from "../../entities/Category";
import useStopScrolling from "../../hooks/other/useStopScrolling";
import Spinner from "../other/Spinner.tsx";
import {ActionButton} from "../other/ActionButton.tsx";
import {HiX} from "react-icons/hi";
import useCreateTransaction from "../../hooks/transactions/useCreateTransaction.ts";
import useCategories from "../../hooks/categories/useCategories.ts";


interface AddTransactionModalProps {
    userId: string;
    walletId: string;
}

const AddTransactionModal = ({userId, walletId}: AddTransactionModalProps) => {
    const {register, handleSubmit, clearErrors, reset, formState: {errors}} = useForm<Transaction>();
    const transactionMutation = useCreateTransaction();
    const {data: categories} = useCategories();
    const [isOpen, setIsOpen] = useState(false);
    const [selectedCurrency, setSelectedCurrency] = useState<Currency>(currencies[0]);
    const categoryFiller: Category = {
        iconColour: 'transparent',
        image: 'MdAdd',
        name: "",
        type: 1,
        id: ""
    }
    const [selectedCategory, setSelectedCategory] = useState<Category>(categoryFiller)
    useStopScrolling(isOpen)

    if (!categories) return <Spinner/>

    const handleSelectedCurrency = (currencyId: string) => {
        const currency = currencies.find(currency => currency.id === currencyId)
        setSelectedCurrency(currency || currencies[0])
    }

    const handleSelectedCategory = (categoryId: string) => {
        const category = categories?.find(cat => cat.id === categoryId) || selectedCategory;
        setSelectedCategory(category)
    }

    function handleOpenModal() {
        setIsOpen(p => !p);
        reset();
        clearErrors();
    }


    const onSubmit: SubmitHandler<Transaction> = async (model: Transaction) => {
        model.userId = userId;
        model.walletId = walletId;
        model.currency = selectedCurrency;
        model.category = selectedCategory;
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
            <div className={`absolute inset-0 flex justify-center items-start p-4 sm:p-2
                        ${isOpen ? 'visible bg-black/20' : 'invisible'}`}>
                <div className="bg-white p-4 rounded-md shadow-lg max-w-full mx-auto">
                    <h2 className="text-2xl font-bold mb-4 flex justify-between">Add transaction
                        <HiX size={'2rem'} color={'red'} onClick={() => {handleOpenModal()}}/>
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
                            <div className="mb-4">
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
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="CategoryId">
                                    Category
                                </label>
                                <select
                                    className="shadow border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="CategoryId"
                                    {...register("categoryId", {
                                        required: "Category is required for transaction",
                                        value: categories[0].id
                                    })}
                                    onChange={(e) => handleSelectedCategory(e.target.value)}
                                >
                                    {categories?.map((category) => (
                                        <option key={category.id} value={category.id}>
                                            {category.name} ({category.type === 1 ? "EXPENSE" : "INCOME"})
                                        </option>
                                    ))}
                                </select>
                                {errors.categoryId &&
                                    <p className={'text-red-400 italic'}>{errors.categoryId.message}</p>}
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

export default AddTransactionModal;