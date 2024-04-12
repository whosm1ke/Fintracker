import {advantages, textFiller} from "../helpers/textFiller.tsx";
import {AnimatePresence, motion} from "framer-motion";
import {useEffect, useState} from "react";
import {FiChevronDown} from "react-icons/fi";

export default function HomePage() {

    return (
        <>
            <main className={'container w-3/4 mx-auto'}>
                <TextImage title={textFiller[0].title}
                           content={textFiller[0].content}
                           imagePath={textFiller[0].imagePath}/>
                <TextImage title={textFiller[1].title}
                           content={textFiller[1].content}
                           imagePath={textFiller[1].imagePath} imageFirst={window.innerWidth > 500}/>
                <CardContainer/>
            </main>
          
        </>
    )
}


const Card = ({title, description}: { title: string, description: string }) => {
    return (
        <motion.div
            className="bg-white p-4 border rounded-xl shadow-lg"
            initial={{opacity: 0, y: -100}}
            animate={{opacity: 1, y: 0}}
            transition={{duration: 0.5}}
            whileHover={{scale: 1.2}}
        >
            <h2 className="text-xl font-bold">{title}</h2>
            <p>{description}</p>
        </motion.div>
    );
}

const CardContainer = () => {
    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4 p-4">
            {advantages.map((advantage, index) => (
                <Card key={index} title={advantage.title} description={advantage.description}/>
            ))}
        </div>
    );
}


interface TextImageBlockProps {
    title: string;
    content: string;
    imagePath: string;
    imageFirst?: boolean
}

const TextImage = ({title, content, imagePath, imageFirst}: TextImageBlockProps) => {
    const [isCollapsed, setIsCollapsed] = useState(window.innerWidth < 500);

    useEffect(() => {
        const handleResize = () => {
            setIsCollapsed(window.innerWidth < 500);
        };

        window.addEventListener('resize', handleResize);

        return () => {
            window.removeEventListener('resize', handleResize);
        };
    }, []);

    return (
        <motion.div
            initial={{
                opacity: 0,
                scale: .5
            }}
            animate={{
                opacity: 1,
                scale: 1
            }}
            transition={{
                type: 'spring',
                duration: 1.5,
                when: 'beforeChildren'
            }}
            className={'w-full mx-auto p-4'}>
            <section className={'p-4 flex flex-col md:flex-row border rounded-xl shadow-lg group'}>
                {imageFirst && !isCollapsed && <RenderImage imagePath={imagePath}/>}
                <div className={'w-full md:w-1/2 mr-10'}>
                    <header className={'flex justify-center items-center sm:block'}
                            >
                        <p className={'text-xl sm:text-3xl font-bold '}>{title}</p>
                        {window.innerWidth < 500 &&
                            <motion.span className={'text-3xl'}
                                         onClick={() => setIsCollapsed(!isCollapsed)}
                                         animate={isCollapsed ? {rotate: 0} : {rotate: 180}}
                            >
                                <FiChevronDown size={'3rem'}/>
                            </motion.span>}
                    </header>
                    <AnimatePresence>
                        {!isCollapsed && (
                            <motion.div
                                initial={{opacity: 0, height: 0, marginTop: 0}}
                                animate={{opacity: 1, height: 'auto', marginTop: '2.5rem'}}
                                exit={{opacity: 0, height: 0, marginTop: 0}}
                                transition={{
                                    duration: 1,
                                    type: 'tween'
                                }}
                                className={'mt-10 w-full font-serif'}>
                                <p className={'min-w-full leading-relaxed transform transition-all duration-500 ease-out mb-3 sm:mb-0'}>{content}</p>
                            </motion.div>
                        )}
                    </AnimatePresence>
                </div>
                {!imageFirst && !isCollapsed && <RenderImage imagePath={imagePath}/>}
            </section>
        </motion.div>
    )
}

const RenderImage = ({imagePath}: { imagePath: string }) => {
    return (
        <div className={'w-full md:w-1/2 flex justify-center items-center'}>
            <img src={imagePath} alt="Wallet"
                 className={'max-w-[70%] max-h-[70%] object-fill transform transition-transform duration-500 group-hover:scale-110'}/>
        </div>
    )
}

