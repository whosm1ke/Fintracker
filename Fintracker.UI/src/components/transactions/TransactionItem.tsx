import {
    amountRegisterForTransaction,
    dateRegisterForTransaction, labelRegisterForTransaction, noteRegisterForTransaction,
    Transaction
} from "../../entities/Transaction.ts";
import * as Icons from 'react-icons/md'
import {IconType} from "react-icons";
import {CategoryType} from "../../entities/CategoryType.ts";
import {useState} from "react";
import {AnimatePresence, motion, Variants} from "framer-motion";
import useCategories from "../../hooks/categories/useCategories.ts";
import useUpdateTransaction from "../../hooks/transactions/useUpdateTransaction.ts";
import {Category} from "../../entities/Category.ts";
import Spinner from "../other/Spinner.tsx";
import {SubmitHandler, useForm} from "react-hook-form";
import SingleSelectDropDownMenu from "../other/SingleSelectDropDownMenu.tsx";
import CategoryItem from "../categories/CategoryItem.tsx";
import currencies from "../../data/currencies.ts";
import CurrencyItem from "../currencies/CurrencyItem.tsx";
import {Currency} from "../../entities/Currency.ts";
import CategoryHeadingItem from "../categories/CategoryHeadingItem.tsx";

interface TransactionItemProps {
    transaction: Transaction,
    walletCurrencySymbol: string;
    conversionRate: number;
}

const variants : Variants = {
    editing: { backgroundColor: ["#ffef00", "#ffdf00"], transition: { duration: 0.5, repeatType: 'reverse', repeat: Infinity } },
    idle: {backgroundColor: 'rgb(245,245,245)'},
};
export function TransactionItem({transaction, conversionRate, walletCurrencySymbol}: TransactionItemProps) {
    const Icon = (Icons as any)[transaction.category.image] as IconType;
    const convertedAmount = transaction.amount * conversionRate;
    const {data: categories} = useCategories();
    const [isEditing, setIsEditing] = useState(false);

    if (!categories) return <Spinner/>
    const handleIsEditing = () => setIsEditing(p => !p);
    return (
        <AnimatePresence>
            <motion.div
                initial={{position: 'relative', height: 'auto'}}
                exit={{height: 'auto'}}
                animate={isEditing ? {height: "auto"} : {}}
                transition={{duration: 0.8}}
            >
                <motion.div
                    variants={variants}
                    animate={isEditing ? "editing" : "idle"}
                    onClick={handleIsEditing}
                    whileHover={isEditing ? {} : {backgroundColor: 'rgb(229,229,229)'}}
                    className="grid grid-cols-2 md:grid-cols-4 justify-between items-center px-2 border rounded shadow hover:bg-neutral-200">
                    <div className="justify-self-start flex items-center gap-x-2">
                        <Icon size="2rem" color={transaction.category.iconColour} className={'hidden sm:block'}/>
                        <p>{transaction.category.name}</p>
                    </div>
                    <div className="hidden md:flex">
                        <p className="">{transaction.user.userName}</p>
                    </div>
                    <div className="justify-self-center hidden md:flex-grow md:flex justify-between min-w-full gap-x-5">
                        <p className="break-words">{transaction.note || "No note"}</p>
                        <p className="">{transaction.label || "No note"}</p>
                    </div>
                    <div className="justify-self-end flex flex-col">
                        <p className={`${transaction.category.type === CategoryType.EXPENSE ? "text-red-400" : 'text-green-400'} font-bold`}>
                            {transaction.category.type === CategoryType.EXPENSE ? "-" : ""}
                            {transaction.amount} {transaction.currency.symbol}
                        </p>
                        {transaction.currency.symbol !== walletCurrencySymbol &&
                            <p className={`text-gray-400 font-bold text-sm`}>
                                {convertedAmount > 0 ? "-" : ""}
                                {convertedAmount.toFixed(2)} {walletCurrencySymbol}
                            </p>}
                    </div>
                </motion.div>
                {isEditing && <TransactionEditingBlock transaction={transaction} categories={categories}/>}
            </motion.div>
        </AnimatePresence>
    );
}

interface TransactionEditingBlockProps {
    transaction: Transaction,
    categories: Category[]
}

const TransactionEditingBlock = ({transaction, categories}: TransactionEditingBlockProps) => {
    const updateTransactionMutation = useUpdateTransaction();
    const {handleSubmit, register, reset, clearErrors, setError, formState: {errors}} = useForm<Transaction>();
    const [selectedCategory, setSelectedCategory] = useState<Category>(transaction.category);
    const [selectedCurrency, setSelectedCurrency] = useState<Currency>(transaction.currency);

    const handleSelectedCategory = (cat: Category) => setSelectedCategory(cat);
    const handleSelectedCurrency = (currency: Currency) => setSelectedCurrency(currency);
    

    const onSubmit: SubmitHandler<Transaction> = (model: Transaction) => {
        if(!selectedCategory)
            setError("categoryId", {message: "Transaction must have a category"})
        else
            clearErrors("categoryId")

        if(!selectedCurrency)
            setError("currencyId", {message: "Transaction must have a currency"})
        else
            clearErrors("currencyId")
        
        model.id = transaction.id
        model.categoryId = selectedCategory.id;
        model.currencyId = selectedCurrency.id;
        
        console.log(model)
        
        const res = updateTransactionMutation.mutate(model);
        
        console.log("res: ", res)
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
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="Date"
                            type="date"
                            defaultValue={new Date().toLocaleDateString('en-CA')}
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
                            defaultValue={transaction.note || "---"}
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
                            defaultValue={transaction.label || "---"}
                            type="text"
                            {...register("label", labelRegisterForTransaction)}
                        />
                    </div>
                    <div>
                        <label className="flex justify-between text-gray-700 text-sm font-bold mb-1" htmlFor="Amount">
                            Amount {errors.amount && <p className={'text-red-400 italic'}>{errors.amount.message}</p>}
                        </label>
                        <input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="Amount"
                            type="number"
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
                                                      heading={"Currency"}
                                                      onItemSelected={handleSelectedCurrency}/>
                        </div>
                    </div>
                    <div className={'flex justify-center items-end'}>
                        <button type={'submit'}
                                className={'py-2 bg-green-400 h-full rounded w-full text-stone-50 font-semibold'}>Save
                            transaction
                        </button>
                    </div>
                    <div className={'flex justify-center items-end'}>
                        <button type={'button'}
                                className={'py-2 bg-red-500 rounded w-full h-full text-stone-50 font-semibold'}>Delete
                            transaction
                        </button>
                    </div>
                </div>
            </form>
        </motion.div>
    )
}
