import {motion} from "framer-motion";

interface ActionButtonProps {
    text: string,
    onModalOpen: () => void
}

export const ActionButton = ({text, onModalOpen}: ActionButtonProps) => {
    return (
        <>
            <motion.button
                onClick={() => onModalOpen()}
                whileHover={{scale: 1.1}}
                whileTap={{scale: 0.9}}
                className={'text-xl bg-green-400 px-4 py-2 text-white rounded-lg shadow-lg'}>{text}
            </motion.button>
        </>
    )
}