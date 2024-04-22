import { motion } from "framer-motion";

const Spinner = () => {
    return (
        <div className="fixed top-0 left-0 w-screen h-screen flex items-center justify-center pointer-events-none">
            <motion.div
                className="w-12 h-12 bg-blue-500 rounded-full"
                animate={{ scale: [1, 1.5, 1], opacity: [1, 0.5, 1] }}
                transition={{ duration: 0.5, repeat: Infinity }}
            />
        </div>
    );
};

export default Spinner;