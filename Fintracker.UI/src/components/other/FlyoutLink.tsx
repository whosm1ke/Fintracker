import {ReactNode} from "react";
import {AnimatePresence, motion} from "framer-motion";

interface FlyoutLinkProps {
    children: ReactNode,
    FlyoutContent: any,
    open: boolean;
}

export default function FlyoutLink ({children, FlyoutContent, open}: FlyoutLinkProps) {

    const showFlyout = FlyoutContent && open;
    return (
        <div
            className="relative w-fit h-fit bg-gray-100"
        >
            <span className="relative text-white cursor-pointer">
                {children}
            </span>
            <AnimatePresence>
                {showFlyout && (
                    <motion.div
                        initial={{opacity: 0, y: 50}}
                        animate={{opacity: 1, y: 15}}
                        exit={{opacity: 0, y: 50}}
                        style={{translateX: "-50%"}}
                        transition={{duration: 0.3, ease: "easeOut"}}
                        className="absolute left-1/2 top-12 bg-white text-black"
                    >
                        <div className="absolute -top-6 left-0 right-0 h-6 bg-transparent"/>
                        <div
                            className="absolute left-1/2 top-0 h-4 w-4 -translate-x-1/2 -translate-y-1/2 rotate-45 bg-white"/>
                        <FlyoutContent/>
                    </motion.div>
                )}
            </AnimatePresence>
        </div>
    );
};