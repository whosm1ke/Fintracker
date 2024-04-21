﻿import {Link} from "react-router-dom";
import {IoWalletSharp} from "react-icons/io5";
import useDeleteWallet from "../../hooks/wallet/useDeleteWallet.ts";
import { motion, useMotionValue, PanInfo} from 'framer-motion';
import {useState} from "react";

interface WalletCardProps {
    name: string,
    balance: number,
    isBanking: boolean,
    currencySymbol: string,
    walletId: string
}


const WalletCard = ({name, balance, isBanking, currencySymbol, walletId}: WalletCardProps) => {
    const isPositiveBalance = balance > 0;
    const formatedBalance: string = Math.abs(balance).toLocaleString();
    const balanceText: string = isPositiveBalance ? `+ ${formatedBalance} ${currencySymbol}` :
        `- ${formatedBalance} ${currencySymbol}`;
    const deleteWalletMutation = useDeleteWallet(walletId);
    const [showDeleteButton, setShowDeleteButton] = useState(false);

    const handleDeleteWallet = async () => {
        await deleteWalletMutation.mutateAsync(walletId);
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
            style={{ x }}
        >
           
            <Link to={`/wallet/${walletId}/trans`}
                  className={'flex flex-col sm:flex-row space-y-3 sm:space-y-0 space-x-0 sm:space-x-3 ' +
                      'p-4 bg-slate-100 rounded-lg shadow w-full'}>
                <span className={'self-center'}>
                    <IoWalletSharp color={isBanking ? 'orange' : 'green'} size={'2rem'}/>
                </span>
                <div className={'px-4 py-2'}>
                    <h4 className={'text-lg'}>{name}</h4>
                    <span>{isBanking ? 'Monobank' : 'Cash'}</span>
                    <p className={isPositiveBalance ? "text-green-400 text-xl" : 'text-red-500 text-xl'}>{balanceText}</p>
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


export default WalletCard;