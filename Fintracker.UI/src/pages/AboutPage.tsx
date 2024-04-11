import {motion, Variants} from "framer-motion";
import {about} from "../helpers/textFiller.ts";
import {RefObject, useEffect, useRef, useState} from "react";

const container: Variants = {
    hidden: {opacity: 0},
    show: {
        opacity: 1,
        transition: {
            staggerChildren: 0.2
        }
    }
}

let item = {
    hidden: {x: '-100vw'},
    show: {x: 0}
}



export default function AboutPage() {
    const arrowContainerRef = useRef<HTMLDivElement>(null);
    return (
        <motion.main className="flex-grow container mx-auto px-4"
                     variants={container}
                     initial="hidden"
                     animate="show">
            <div className={'flex flex-col sm:grid sm:grid-cols-2 sm:grid-rows-6 gap-8 mt-10'}>
                <div className="col-start-1 sm:col-span-2 order-1 row-start-1  text-center">
                    <Card title={about[0].title} content={about[0].content} fromLeft={false}/>
                </div>

                <div className="col-start-1 row-start-2 order-2 text-center z-10">
                    <Card title={about[1].title} content={about[1].content} fromLeft/>
                </div>
                <div className="relative col-start-2 row-start-2 order-3" ref={arrowContainerRef}>
                    <Arrow fromLeft={false} dir={'ltr'} parentRef={arrowContainerRef}/>
                </div>

                <div className="relative col-start-1 row-start-3 order-5 text-center">
                    <Arrow fromLeft={true} dir={'rtl'} parentRef={arrowContainerRef}/>

                </div>
                <div className="col-start-2 row-start-3 order-4 text-center z-10">
                    <Card title={about[2].title} content={about[2].content} fromLeft={false}/>
                </div>

                <div className="col-start-1 row-start-4 order-6 text-center z-10">
                    <Card title={about[3].title} content={about[3].content} fromLeft/>
                </div>
                <div className="relative col-start-2 row-start-4 order-7 text-center">
                    <Arrow fromLeft={false} dir={'ltr'} parentRef={arrowContainerRef}/>
                </div>

                <div className="relative col-start-1 row-start-5 order-9 text-center">
                    <Arrow fromLeft={true} dir={'rtl'} parentRef={arrowContainerRef}/>
                </div>
                <div className="col-start-2 row-start-5 order-8 text-center z-10">
                    <Card title={about[4].title} content={about[4].content} fromLeft={false}/>
                </div>

                <div className="col-start-1 row-start-6 order-10 text-center z-10">
                    <Card title={about[5].title} content={about[5].content} fromLeft/>
                </div>
                <div className="relative col-start-2 row-start-6 order-11 text-center">
                    <Arrow fromLeft={false} dir={'ltr'} parentRef={arrowContainerRef}/>
                </div>
                <div className="col-start-2 row-start-7 order-12 text-center z-10">
                    <Card title={about[6].title} content={about[6].content} fromLeft={false}/>
                </div>
            </div>
        </motion.main>
    );
}

interface CardProps {
    title: string;
    content: string;
    fromLeft: boolean;
    duration?: number;
}

const Card = ({title, content, fromLeft = false, duration = 0.5}: CardProps) => {
    item.hidden.x = fromLeft ? '-100vw' : '100vw';
    return (
        <motion.div
            variants={item}
            whileHover={{scale: 1.05, rotateZ: 2}}
            transition={{duration: duration}}
        >
            <div
                className="bg-blue-300 p-4 rounded-lg shadow-lg hover:shadow-2xl transition-shadow duration-500">
                <h3 className="text-xl font-bold mb-2">{title}</h3>
                <p>{content}</p>
            </div>
        </motion.div>
    )
}

function getHalfWidth(parentRef: RefObject<HTMLElement>): number {
    if (!parentRef.current) {
        return 0;
    }

    return parentRef.current.offsetWidth / 2;
}

function getHalfHeight(parentRef: RefObject<HTMLElement>): number {
    if (!parentRef.current) {
        return 0;
    }

    return parentRef.current.offsetHeight / 2;
}

const Arrow = ({fromLeft = false, dir, parentRef}: {
    fromLeft: boolean,
    dir: 'ltr' | 'rtl',
    parentRef: RefObject<HTMLElement>
}) => {
    const [halfGabarites, setHalfWidth] = useState({
        width: 0,
        height: 0
    });

    useEffect(() => {
        const updateHalfWidth = () => {
            const newHalfWidth = getHalfWidth(parentRef);
            const newHalfHeight = getHalfHeight(parentRef);
            setHalfWidth({height: newHalfHeight, width: newHalfWidth});
        };

        updateHalfWidth();
        window.addEventListener('resize', updateHalfWidth);

        return () => {
            window.removeEventListener('resize', updateHalfWidth);
        };
    }, [window.innerWidth]);

    console.log(halfGabarites);
    item.hidden.x = fromLeft ? '-100vw' : '100vw';
    

    const groupVariants: Variants = {
        hidden: {opacity: 0},
        show: {opacity: 1}
    }

    const svgVariants = {
        hidden: { scale: 0, x: 100, opacity: 0 },
        show: (custom: number) => ({
            scale: 1,
            x: 0,
            opacity: 1,
            transition: {
                delay: custom * 0.5
            }
        })
    }


    return (
        <motion.svg
            initial={{opacity: 0, scaleX: dir === 'rtl' ? -1 : 1}}
            animate={{opacity: 1}}
            className={`absolute top-0 ${dir === 'rtl' ? 'left-6' : '-left-6'} z-0 h-[150%] w-full`}
        >
            <motion.g id="svg_10" variants={groupVariants}>
                <motion.line
                    custom={1}
                    variants={svgVariants}
                    id="svg_7" y2={halfGabarites.height} x2={halfGabarites.width} y1={halfGabarites.height} x1="0"
                    strokeWidth="3" stroke="#000000" fill="none"/>
                <motion.line
                    custom={2} 
                    variants={svgVariants}
                    id="svg_8"
                    y2={halfGabarites.height * 2}
                    x2={halfGabarites.width - 2}
                    y1={halfGabarites.height}
                    x1={halfGabarites.width - 2}
                    strokeWidth="3"
                    stroke="#000000"
                    fill="none"
                />
                <motion.path
                    custom={3}
                    variants={svgVariants}
                    stroke="#000000"
                    id="svg_11"
                    d={`M${halfGabarites.width - 25},${halfGabarites.height * 2 - 10}
            l48,0
            l-24,32
            z`}
                    strokeWidth="3"
                    fill="#396849"
                />
            </motion.g>
        </motion.svg>
    );
};

