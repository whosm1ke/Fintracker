import {motion, Variants} from "framer-motion";
import {about} from "../../helpers/textFiller.tsx";
import {RefObject, useEffect, useRef, useState} from "react";

const container: Variants = {
    hidden: {opacity: 0},
    show: {
        opacity: 1,
        transition: {
            staggerChildren: 0.5,
        }
    }
}


export default function AboutPage() {
    const arrowContainerRef = useRef<HTMLDivElement>(null);
    return (
        <motion.div className="container mx-auto px-4"
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
                    <Arrow dir={'ltr'} delayFromPrevious={1} parentRef={arrowContainerRef}/>
                </div>

                <div className="relative col-start-1 row-start-3 order-5 text-center">
                    <Arrow dir={'rtl'} delayFromPrevious={2} parentRef={arrowContainerRef}/>

                </div>
                <div className="col-start-2 row-start-3 order-4 text-center z-10">
                    <Card title={about[2].title} content={about[2].content} fromLeft={false}/>
                </div>

                <div className="col-start-1 row-start-4 order-6 text-center z-10">
                    <Card title={about[3].title} content={about[3].content} fromLeft/>
                </div>
                <div className="relative col-start-2 row-start-4 order-7 text-center">
                    <Arrow dir={'ltr'} delayFromPrevious={3} parentRef={arrowContainerRef}/>
                </div>

                <div className="relative col-start-1 row-start-5 order-9 text-center">
                    <Arrow dir={'rtl'} delayFromPrevious={4} parentRef={arrowContainerRef}/>
                </div>
                <div className="col-start-2 row-start-5 order-8 text-center z-10">
                    <Card title={about[4].title} content={about[4].content} fromLeft={false}/>
                </div>

                <div className="col-start-1 row-start-6 order-10 text-center z-10">
                    <Card title={about[5].title} content={about[5].content} fromLeft/>
                </div>
                <div className="relative col-start-2 row-start-6 order-11 text-center">
                    <Arrow dir={'ltr'} delayFromPrevious={5} parentRef={arrowContainerRef}/>
                </div>
                <div className="col-start-2 row-start-7 order-12 text-center z-10">
                    <Card title={about[6].title} content={about[6].content} fromLeft={false}/>
                </div>
            </div>
        </motion.div>
    );
}

interface CardProps {
    title: string;
    content: string;
    fromLeft: boolean;
    duration?: number;
}

const Card = ({title, content, fromLeft = false, duration = 0.8}: CardProps) => {
    const item: Variants = {
        hidden: {x: fromLeft ? '-200vw' : '200vw', scale: 1.05, rotateZ: fromLeft ? -15 : 15},
        show: {
            scale: 1,
            rotateZ: 0,
            x: 0,
            transition: {
                duration: duration
            }
        }
    }
    return (
        <motion.div
            variants={item}
            whileHover={{scale: 1.05, rotateZ: 2}}
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

const Arrow = ({dir, parentRef, delayFromPrevious}: {
    dir: 'ltr' | 'rtl',
    delayFromPrevious: number,
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


    const svgVariants: Variants = {
        hidden: {
            scaleX: dir === 'rtl' ? -1 : 1
        },
        visible: custom => {
            return {
                transition: {
                    delayChildren: custom * 0.5 + 0.7,
                    staggerChildren: 0.3
                }
            }
        }
    }

    const lineVariants: Variants = {
        hidden: {
            pathLength: 0,
            fillOpacity: 0
        },
        visible: {
            fillOpacity: 1,
            pathLength: 1,
            transition: {
                duration: 0.5
            }
        }
    }


    return (

        <motion.svg
            custom={delayFromPrevious}
            variants={svgVariants}
            animate={'visible'}
            initial={'hidden'}
            className={`absolute top-0 ${dir === 'rtl' ? 'left-6' : '-left-6'} z-0 h-[150%] w-full`}
        >
            <motion.line
                variants={lineVariants}
                id="svg_7"
                y2={halfGabarites.height}
                x2={halfGabarites.width}
                y1={halfGabarites.height}
                x1="0"
                strokeWidth="3" stroke="#000000" fill="none"/>
            <motion.line
                variants={lineVariants}
                id="svg_8"
                y2={halfGabarites.height * 2 - 10}
                x2={halfGabarites.width - 2}
                y1={halfGabarites.height}
                x1={halfGabarites.width - 2}
                strokeWidth="3"
                stroke="#000000"
                fill="none"
            />
            <motion.path
                variants={lineVariants}
                stroke="#000000"
                id="svg_11"
                d={`M${halfGabarites.width - 25},${halfGabarites.height * 2 - 10}
            l48,0
            l-24,33
            l-25,-33
            z`}
                strokeWidth="3"
                fill="#396849"
            />
        </motion.svg>
    );
};

