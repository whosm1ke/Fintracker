import {Link} from "react-router-dom";
import {motion, PanInfo, useMotionValue} from "framer-motion";
import {useState} from "react";
import useDeleteBudget from "../../hooks/budgets/useDeleteBudget.ts";

interface BudgetCardProps {
    budgetId: string,
    name: string
    walletName: string;
    balance: number,
    totalSpent: number,
    currencySymbol: string,
    startDate: string,
    endDate: string,
    isPublic: boolean
}

export const BudgetCard = ({
                               isPublic,
                               budgetId,
                               name,
                               balance,
                               walletName,
                               currencySymbol,
                               endDate,
                               startDate,
                               totalSpent = 0,
                           }: BudgetCardProps) => {


    const start = new Date(startDate).toLocaleDateString()
    const end = new Date(endDate).toLocaleDateString()
    const spentPercentage = (totalSpent * 100) / (totalSpent + balance);
    const spentBgColor = spentPercentage < 33 ? "bg-green-400" : spentPercentage < 66 ? "bg-yellow-200" : "bg-red-400"
    const cardBgColor = isPublic ? "bg-green-100" : "bg-orange-100"
    const [showDeleteButton, setShowDeleteButton] = useState(false);
    const deleteBudgetMutation = useDeleteBudget(budgetId);
    const handleDeleteWallet = async () => {
        await deleteBudgetMutation.mutateAsync({
            id: budgetId
        });
    }
    const toggleDeleteButton = () => setShowDeleteButton(p => !p);

    const x = useMotionValue(0);

    const handleDragEnd = async (info: PanInfo) => {
        if (info.offset.x > 250) {
            await handleDeleteWallet();
        }
    };

    // @ts-ignore
    const isMobile = navigator.userAgentData.mobile;
    return (
        <motion.div
            drag={isMobile ? "x" : false}
            dragConstraints={{left: 0, right: 0}}
            dragElastic={1}
            onDragEnd={async (_e, info) => await handleDragEnd(info)}
            onMouseEnter={toggleDeleteButton}
            onMouseLeave={toggleDeleteButton}
            className={'relative flex'}
            style={{x}}
        >
            <Link
                to={`${budgetId}`}
                className={`${cardBgColor} p-4 rounded-lg w-full shadow-md`}>
                <h2 className="flex justify-between font-bold">
                    <p className={'text-lg text-black'}>{name}</p>
                    <p className={isPublic ? "text-green-600" : "text-red-600"}>{isPublic ? "PUBLIC" : "PRIVATE"}</p>
                </h2>
                <p className="text-sm text-gray-500">{walletName}</p>
                <p className="text-lg font-bold text-black">{balance} {currencySymbol} left</p>
                <p className="text-sm text-gray-500">of {balance + totalSpent} {currencySymbol}</p>
                <div className="h-7 w-full bg-gray-200 mt-2 rounded">
                    <div style={{width: `${spentPercentage}%`, maxWidth: '100%'}}
                         className={`h-full ${spentBgColor} rounded`}></div>
                </div>
                <div className="flex justify-between text-sm text-gray-500 mt-2">
                    <span>{start}</span>
                    <span>{end}</span>
                </div>
            </Link>
            <motion.div
                onClick={async (e) => {
                    e.stopPropagation()
                    await handleDeleteWallet()
                }}
                initial={{scale: 0}}
                animate={showDeleteButton ? {scale: 1} : {}}
                exit={{scale: 0}}
                className={'absolute top-0 bg-red-400 right-0 w-8 h-8 rounded-full p-1 cursor-pointer z-30 flex items-center justify-center'}
            >
                <motion.span
                    initial={{rotate: 45}}
                    className={'w-3 bg-black h-0.5 block absolute'}></motion.span>
                <motion.span
                    initial={{rotate: -45}}
                    className={'w-3 bg-black h-0.5 block absolute'}></motion.span>
            </motion.div>
        </motion.div>
    )
}