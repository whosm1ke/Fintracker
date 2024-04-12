import {bank} from "../helpers/textFiller.tsx";
import {ReactElement, useState} from "react";
import {motion} from "framer-motion";
import {FiChevronDown} from "react-icons/fi";

export default function BankPage() {


    return (
        <main className={'flex-grow'}>
            <div className={'container mx-auto px-4 py-10'}>
                <section className={'w-1/2 mx-auto'}>
                    <header className={'mb-12 text-3xl font-bold text-center'}>
                        How we approach security
                    </header>


                    <div className={'flex flex-col gap-y-5'}>
                        {bank.map((b, i) =>
                            <Card key={i} title={b.title} content={b.content} icon={b.icon}/>
                        )}
                    </div>
                </section>
            </div>
        </main>
    )
};

const Card = ({title, content, icon}: { title: string, content: string, icon: ReactElement }) => {

    const [isOpen, setIsOpen] = useState(false);

    function handleOpen() {
        setIsOpen(!isOpen);
    }

    return (
        
            <motion.div
                layout
                transition={{layout: {duration: 0.5},}}
                className={'p-5 bg-white/70 rounded-2xl shadow-xl overflow-hidden -z-0'}>
                <motion.h1
                    layout
                    className={'flex items-center justify-between'}>
                    <div className={'flex items-center gap-x-4'}>
                        <p>{icon}</p>
                        <p className={'text-xl sm:text-3xl font-[400]'}>{title}</p>
                    </div>
                    <motion.button
                        initial={{
                            rotate: 0
                        }}
                        animate={isOpen ? {rotate: 180} : {rotate: 0}}
                        onClick={handleOpen}>
                        <FiChevronDown size={'2rem'}/>
                    </motion.button>
                </motion.h1>
                {isOpen && <motion.div
                    layout
                    className={'mt-4 text-sm sm:text-xl'}>
                    {content}
                </motion.div>}
            </motion.div>
    )
}



