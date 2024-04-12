import {motion} from "framer-motion";

export default function Motion(){


    return  <motion.div
        className={'w-32 h-32 bg-amber-500 text-center'}
        animate={{
            scale: [1, 2, 2, 1, 1],
            rotate: [0, 0, 270, 270, 0],
            borderRadius: ["20%", "20%", "50%", "50%", "20%"],
        }}
    >asdasd</motion.div>
}