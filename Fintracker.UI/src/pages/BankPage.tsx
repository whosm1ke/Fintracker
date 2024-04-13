import {security, bankConnect} from "../helpers/textFiller.tsx";
import {ReactElement, useState} from "react";
import {motion} from "framer-motion";
import {FiChevronDown} from "react-icons/fi";

export default function BankPage() {
    return (
        <main className={'flex-grow'}>
            <div className={'container mx-auto px-4 py-10'}>
                <div className={'grid grid-cols-1 md:grid-cols-2 gap-10'}>
                    <section className={'col'}>
                        <header className={'mb-12 text-3xl font-bold text-center'}>
                            How we approach security
                        </header>
                        <motion.div layout className={'flex flex-col gap-y-5'}>
                            {security.map((b, i) => (
                                <Card key={i} title={b.title} content={b.content} icon={b.icon}/>
                            ))}
                        </motion.div>
                    </section>
                    <section className={'col'}>
                        <header className={'mb-12 text-3xl font-bold text-center'}>
                            How to connect monobank
                        </header>
                        <motion.div layout className={'flex flex-col gap-y-5'}>
                            {bankConnect.map((b, i) => (
                                <Card key={i} title={b.title} content={b.content} icon={b.icon}/>
                            ))}
                        </motion.div>
                    </section>
                </div>
            </div>
        </main>
    );
};


interface CardProps {
    title: string;
    content: string;
    icon: ReactElement;
}

const Card = ({title, content, icon}: CardProps) => {
    const [isOpen, setIsOpen] = useState(false);

    function handleOpen() {
        setIsOpen(!isOpen);
    }

    return (
        <motion.div
            layout
            className={'border-2 rounded-2xl flex flex-col justify-center shadow-md overflow-hidden p-4'}
        >
            <motion.h1 layout className={'flex items-center justify-between'}>
                <motion.div 
                    className={'flex items-center gap-x-5'}
                >
                    <p>{icon}</p>
                    <p className={'text-xl sm:text-2xl font-medium'}>
                        {title}
                    </p>
                </motion.div>
                <motion.button
                    initial={{rotate: 0}}
                    animate={isOpen ? {rotate: 180} : {rotate: 0}}
                    onClick={handleOpen}
                    className={'focus:outline-none'}
                >
                    <FiChevronDown size={'2rem'}/>
                </motion.button>
            </motion.h1>
            <motion.div
                initial={{height: 0, opacity: 0, pointerEvents: 'none'}}
                animate={isOpen ? {height: 'auto', opacity: 1, marginTop: '1.25rem', pointerEvents: 'auto'} : {}}
                transition={{duration: 0.4}}
                layout
                className={'text-base sm:text-lg'}
            >
                {content}
            </motion.div>
        </motion.div>
    );
};




