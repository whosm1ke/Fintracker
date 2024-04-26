import {
    Transaction
} from "../../entities/Transaction.ts";
import * as Icons from 'react-icons/md'
import {IconType} from "react-icons";
import {CategoryType} from "../../entities/CategoryType.ts";
import {useState} from "react";
import {AnimatePresence, motion, Variants} from "framer-motion";
import useCategories from "../../hooks/categories/useCategories.ts";
import Spinner from "../other/Spinner.tsx";
import TransactionEditingBlock from "./TransactionEditingBlock.tsx";

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
                        <p className="break-words">{transaction.note || "---"}</p>
                        <p className="">{transaction.label || "---"}</p>
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
                {isEditing && <TransactionEditingBlock transaction={transaction} handleIsEditing={handleIsEditing}
                                                       categories={categories}/>}
            </motion.div>
        </AnimatePresence>
    );
}


