import {motion} from "framer-motion";

interface ActionButtonProps {
    text: string;
    onModalOpen: () => void;
    isActive?: boolean;
}

export const ActionButton = ({text, onModalOpen, isActive = true}: ActionButtonProps) => {
    return (
        <>
            <motion.button
                onClick={() => {
                    if (isActive)
                        onModalOpen()
                }}
                whileHover={isActive ? {scale: 1.1} : {}}
                whileTap={isActive ? {scale: 0.9} : {}}
                className={`text-[18px] lg:text-xl ${isActive ? 'bg-green-400 text-white' : 'bg-gray-400/40 text-gray-400 cursor-default'} 
                px-5 py-2 rounded-lg shadow-lg`}>{text}
            </motion.button>
        </>
    )
}