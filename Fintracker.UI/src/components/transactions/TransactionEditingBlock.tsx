import {
    amountRegisterForTransaction,
    dateRegisterForTransaction,
    labelRegisterForTransaction,
    noteRegisterForTransaction,
    Transaction
} from "../../entities/Transaction.ts";
import {Category} from "../../entities/Category.ts";
import useUpdateTransaction from "../../hooks/transactions/useUpdateTransaction.ts";
import useDeleteTransaction from "../../hooks/transactions/useDeleteTransaction.ts";
import {SubmitHandler, useForm} from "react-hook-form";
import React, {useState} from "react";
import {Currency} from "../../entities/Currency.ts";
import {motion} from "framer-motion";
import SingleSelectDropDownMenu from "../other/SingleSelectDropDownMenu.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";
import CategoryHeadingItem from "../categories/CategoryHeadingItem.tsx";
import currencies from "../../data/currencies.ts";
import CurrencyItem from "../currencies/CurrencyItem.tsx";

interface TransactionEditingBlockProps {
    transaction: Transaction;
    categories: Category[];
    handleIsEditing: () => void;
    budgetId?: string
}

const TransactionEditingBlock = ({transaction, categories, handleIsEditing, budgetId}: TransactionEditingBlockProps) => {
    const updateTransactionMutation = useUpdateTransaction(budgetId);
    const deleteTransactionMutation = useDeleteTransaction(transaction.id, budgetId);
    const {handleSubmit, register, reset, clearErrors, setError, formState: {errors}} = useForm<Transaction>();
    const [selectedCategory, setSelectedCategory] = useState<Category>(transaction.category);
    const [selectedCurrency, setSelectedCurrency] = useState<Currency>(transaction.currency);

    const handleSelectedCategory = (cat: Category) => setSelectedCategory(cat);
    const handleSelectedCurrency = (currency: Currency) => setSelectedCurrency(currency);

    const date = new Date(transaction.date);
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2); // add leading zero
    const day = ('0' + date.getDate()).slice(-2); // add leading zero
    const formattedDate = `${year}-${month}-${day}`;
    const isBankTrans: boolean | undefined = transaction.isBankTransaction;
    const onSubmit: SubmitHandler<Transaction> = async (model: Transaction) => {
        if (!selectedCategory)
            setError("categoryId", {message: "Transaction must have a category"})
        else
            clearErrors("categoryId")

        if (!selectedCurrency)
            setError("currencyId", {message: "Transaction must have a currency"})
        else
            clearErrors("currencyId")

        model.id = transaction.id
        model.categoryId = selectedCategory.id;
        model.category = selectedCategory;
        model.currencyId = selectedCurrency.id;
        model.currency = selectedCurrency;
        model.user = transaction.user;
        model.walletId = transaction.walletId;


        const res = await updateTransactionMutation.mutateAsync(model);

        if (!res.hasError) {
            reset();
            clearErrors();
            handleIsEditing();
        }
    }

    async function onDelete(e: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        e.preventDefault();
        await deleteTransactionMutation.mutateAsync({
            id: transaction.id,
            walletId: transaction.walletId,
        })
    }

    return (
        <motion.div
            key={transaction.id}
            initial={{opacity: 0, display: 'none'}}
            animate={{opacity: 1, display: 'block'}}
            transition={{duration: 0.5}}
            className={'my-4'}>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className={'grid grid-cols-2 gap-x-3 gap-y-2'}>
                    <div>
                        <label className="flex justify-between text-gray-700 text-sm font-bold mb-1" htmlFor="Date">
                            Date {errors.date && <p className={'text-red-400 italic'}>{errors.date.message}</p>}
                        </label>
                        <input
                            className={isBankTrans === undefined || isBankTrans ? "shadow pointer-events-none appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight bg-gray-300" :
                                "shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"}
                            id="Date"
                            type="date"
                            readOnly={isBankTrans === undefined || isBankTrans}
                            defaultValue={formattedDate}
                            {...register("date", dateRegisterForTransaction)}
                        />
                    </div>
                    <div>
                        <label className="flex justify-between text-gray-700 text-sm font-bold mb-1" htmlFor="Note">
                            Note {errors.note && <p className={'text-red-400 italic'}>{errors.note.message}</p>}
                        </label>
                        <input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="Note"
                            defaultValue={transaction.note || ""}
                            type="text"
                            {...register("note", noteRegisterForTransaction)}
                        />
                    </div>
                    <div>
                        <label className="flex justify-between text-gray-700 text-sm font-bold mb-1" htmlFor="Label">
                            Label {errors.label && <p className={'text-red-400 italic'}>{errors.label.message}</p>}
                        </label>
                        <input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="Label"
                            defaultValue={transaction.label || ""}
                            type="text"
                            {...register("label", labelRegisterForTransaction)}
                        />
                    </div>
                    <div>
                        <label className="flex justify-between text-gray-700 text-sm font-bold mb-1" htmlFor="Amount">
                            Amount {errors.amount && <p className={'text-red-400 italic'}>{errors.amount.message}</p>}
                        </label>
                        <input
                            className={isBankTrans === undefined || isBankTrans ? "shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight bg-gray-300" :
                                "shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"}
                            id="Amount"
                            type="number"
                            step={0.01}
                            readOnly={isBankTrans === undefined || isBankTrans}
                            defaultValue={transaction.amount}
                            {...register("amount", amountRegisterForTransaction)}
                        />

                    </div>
                    <div className={'col-start-1 col-span-2 sm:col-auto'}>
                        <label
                            className="flex justify-between text-gray-700 text-sm font-bold mb-1 col-start-1 col-span-2"
                            htmlFor="CurrencyId">Category {errors.categoryId &&
                            <p className={'text-red-400 italic'}>{errors.categoryId.message}</p>}</label>
                        <div {...register("categoryId")}>
                            <SingleSelectDropDownMenu items={categories} ItemComponent={CategoryItem}
                                                      HeadingComponent={CategoryHeadingItem}
                                                      defaultSelectedItem={selectedCategory}
                                                      heading={"Category"}
                                                      onItemSelected={handleSelectedCategory}/>
                        </div>
                    </div>
                    <div className={'col-start-1 col-span-2 sm:col-auto'}>
                        <label
                            className="flex justify-between text-gray-700 text-sm font-bold mb-1 col-start-1 col-span-2"
                            htmlFor="CurrencyId">Currency {errors.currencyId &&
                            <p className={'text-red-400 italic'}>{errors.currencyId.message}</p>}</label>
                        <div {...register("currencyId")}>
                            <SingleSelectDropDownMenu items={currencies} ItemComponent={CurrencyItem}
                                                      defaultSelectedItem={selectedCurrency}
                                                      heading={"Currency"} canBeChanged={!isBankTrans}
                                                      onItemSelected={handleSelectedCurrency}/>
                        </div>
                    </div>
                    <div className={`flex justify-center items-end ${isBankTrans === undefined || isBankTrans ? "col-span-2" : ""}`}>
                        <button type={'submit'}
                                className={'py-2 bg-green-400 h-full rounded w-full text-stone-50 font-semibold'}>Save
                            transaction
                        </button>
                    </div>
                    {isBankTrans === undefined || !isBankTrans && <div className={'flex justify-center items-end'}>
                        <button type={'button'}
                                onClick={async (e) => await onDelete(e)}
                                className={'py-2 bg-red-500 rounded w-full h-full text-stone-50 font-semibold'}>Delete
                            transaction
                        </button>
                    </div>}
                </div>
            </form>
        </motion.div>
    )
}

export default TransactionEditingBlock;