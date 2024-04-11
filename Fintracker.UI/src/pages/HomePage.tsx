import {advantages, textFiller} from "../helpers/textFiller.tsx";
import {motion} from "framer-motion";
import {Link, useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";
import useUserStore from "../stores/userStore.ts";
import useLogout from "../hooks/useLogout.ts";

export default function HomePage() {

    return (
        <>
            <NavBar/>
            <div className={'container w-3/4 mx-auto'}>
                <TextImage title={textFiller[0].title}
                           content={textFiller[0].content}
                           imagePath={textFiller[0].imagePath}/>
                <TextImage title={textFiller[1].title}
                           content={textFiller[1].content}
                           imagePath={textFiller[1].imagePath} imageFirst/>
                <CardContainer/>
            </div>
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

const NavBarHeader = ({motionNav}: { motionNav: boolean }) => (
    <motion.div
        initial="hide"
        animate={motionNav ? "show" : "hide"}
        variants={{
            show: {y: -7},
            hide: {y: 7, transition: {delay: .6}}
        }}
        className={'flex space-x-2 items-center just col-start-1'}
    >
        <img src="../../public/logo.png" alt="logo" className={'w-16 h-16 object-cover'}/>
        <h1 className={'text-2xl sm:text-3xl font-bold'}>Fintracker</h1>
    </motion.div>
);

const NavBarButton = ({motionNav, handleButtonClick}: { motionNav: boolean, handleButtonClick: () => void }) => (
    <motion.nav className="sm:hidden container mx-auto  col-start-4">
        <motion.div className={'container mx-auto h-1 w-full'}>
            <motion.div className={'flex flex-col items-end'}>
                <motion.button
                    initial="hide"
                    animate={motionNav ? "show" : "hide"}
                    variants={{
                        show: {
                            y: -15
                        },
                        hide: {
                            y: 0,
                            x: 0,
                            scale: 1,
                            transition: {
                                delay: 1
                            }
                        }
                    }}
                    onClick={handleButtonClick}
                    className="flex flex-col space-y-1 relative z-10"
                >
                    <motion.span
                        variants={{
                            hide: {
                                rotate: 0
                            },
                            show: {
                                rotate: 45,
                                y: 5,
                                backgroundColor: '#e26031'
                            },
                        }}
                        className="w-6 bg-white h-1 block"
                    ></motion.span>
                    <motion.span
                        variants={{
                            hide: {
                                opacity: 1,
                                scale: 1
                            },
                            show: {
                                opacity: 0,
                                scale: 0,
                                transition: {
                                    duration: 0.1
                                }
                            },
                        }}
                        className="w-6 bg-white h-1 block"
                    ></motion.span>
                    <motion.span
                        variants={{
                            hide: {
                                rotate: 0
                            },
                            show: {
                                rotate: -45,
                                y: -11,
                                backgroundColor: '#e26031'
                            },
                        }}
                        className="w-6 bg-white h-1 block"
                    ></motion.span>
                </motion.button>
            </motion.div>
        </motion.div>
    </motion.nav>
);

interface NavLinkModel {
    to: string,
    text: string,
    show: boolean,
    col: Number
}

const NavBarLinks = ({motionNav, navLinks}: { motionNav: boolean, navLinks: NavLinkModel[] }) => (
    <>
        {navLinks.map((link, i) => link.show &&
            <MobileNavigationLink text={link.text} to={link.to} key={i} col={+link.col} motionNav={motionNav}/>
        )}

        <RenderLoginLogoutLinks isMobile={true} logCol={3} startCol={4} moutionNav={motionNav}/>
    </>
);

const NavBar = () => {
    const [motionNav, toggleMotionNav] = useState(false);
    const [isAnimating, setIsAnimating] = useState(false);

    const handleButtonClick = () => {
        if (!isAnimating) {
            setIsAnimating(true);
            toggleMotionNav(prev => !prev);
        }
    };

    useEffect(() => {
        let timeoutId: number;
        if (motionNav) {
            timeoutId = setTimeout(() => {
                setIsAnimating(false);
            }, 1000);
        }
        return () => {
            if (timeoutId) {
                setIsAnimating(false);
                clearTimeout(timeoutId);
            }
        };
    }, [motionNav]);

    const navLinks = [
        {
            to: '/about',
            text: 'About us',
            show: true,
            col: 1
        },
        {
            to: '/bank',
            text: 'Bank',
            show: true,
            col: 2
        },
    ]

    return (
        <motion.header
            initial={false}
            animate={motionNav ? 'show' : 'hide'}
            variants={{
                show: {height: '10rem', rowGap: '1.5rem'},
                hide: {height: '6rem', transition: {delay: .6}}
            }}
            className=" grid grid-cols-4 grid-rows-2 items-center justify-between p-6 bg-green-400 z-50 overflow-hidden"
        >
            <NavBarHeader motionNav={motionNav}/>
            <NavBarButton motionNav={motionNav} handleButtonClick={handleButtonClick}/>
            <NavBarLinks motionNav={motionNav} navLinks={navLinks}/>

            <div className={'col-start-3 hidden sm:flex justify-around row-start-1 row-span-2 col-span-2 p-8'}>
                {navLinks.map((link, i) =>
                    link.show && <NavigationLink to={link.to} text={link.text} key={i}/>
                )}
                <RenderLoginLogoutLinks isMobile={false} logCol={null} startCol={null} moutionNav={null}/>
            </div>
        </motion.header>
    );
};

interface TextImageBlockProps {
    title: string;
    content: string;
    imagePath: string;
    imageFirst?: boolean
}

const TextImage = ({title, content, imagePath, imageFirst}: TextImageBlockProps) => {
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
                mass: 1,
                stiffness: 100,
                duration: 1
            }}
            className={'w-full mx-auto p-4'}>
            <section className={'p-4 flex flex-col md:flex-row border rounded-xl shadow-lg group'}>
                {imageFirst && <RenderImage imagePath={imagePath}/>}
                <div className={'w-full md:w-1/2 mr-10'}>
                    <header className={'text-3xl font-bold'}>{title}</header>
                    <div className={'mt-10 w-full font-serif'}>
                        <p className={'min-w-full leading-relaxed transform transition-all duration-500 ease-out mb-3 sm:mb-0'}>{content}</p>
                    </div>
                </div>
                {!imageFirst && <RenderImage imagePath={imagePath}/>}
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

const MobileNavigationLink = ({to, text, col, motionNav}: NavigationLinkProps & {
    motionNav: boolean,
    col: number
}) => {
    return (
        <motion.div
            className={`col-start-${col} row-start-2 block sm:opacity-0`}
            initial={{x: '100vw', opacity: 0}}
            animate={motionNav ? {x: 0, opacity: 1, marginLeft: 5, marginRight: 5} : {}}
            exit={motionNav ? {x: '100vw', opacity: 0} : {}}
            transition={{duration: 0.6}}
            variants={{
                show: {display: 'block'},
                hide: {display: 'none'}
            }}
        >
            <div
                className='text-center p-2 border rounded-2xl shadow-md hover:shadow-lg shadow-black bg-lime-400/35 max-h-10 min-w-18 line-clamp-2'>
                <Link to={to}>{text}</Link>
            </div>
        </motion.div>
    )
}


interface NavigationLinkProps {
    to: string;
    text: string;
}

const NavigationLink = ({to, text}: NavigationLinkProps) => {
    const [isHovered, setHovered] = useState(false);

    return (
        <motion.div
            onHoverStart={() => setHovered(true)}
            onHoverEnd={() => setHovered(false)}
            whileHover={{scale: 1.1}}
            whileTap={{scale: 0.9}}
        >
            <Link to={to} className="relative block text-center text-2xl mr-4">
                {text}
                <motion.span
                    initial={{
                        backgroundColor: 'gray',
                        width: '100%',
                        height: '2px',
                        opacity: 0,
                        translateY: 5,
                        scaleX: '50%'
                    }}
                    animate={{
                        opacity: isHovered ? 1 : 0,
                        translateY: isHovered ? -2 : 5,
                        scaleX: isHovered ? '100%' : '50%'
                    }}
                    transition={{duration: .2, stiffness: 500}}
                    className="absolute bottom-0 left-0"
                />
            </Link>
        </motion.div>
    );
};

const RenderLoginLogoutLinks = ({isMobile, logCol, moutionNav, startCol}: {
    isMobile: boolean,
    logCol: number | null,
    startCol: number | null,
    moutionNav: boolean | null
}) => {
    const isUserSignedIn = useUserStore(x => x.isSignedIn());
    const navigate = useNavigate();

    const logIn = isMobile ? <MobileNavigationLink to={'/login'} text={'Log in'} col={logCol ? logCol : -1}
                                                   motionNav={moutionNav ? moutionNav : false}/> :
        <NavigationLink to={'/login'} text={'Log in'}/>
    const logOut = isMobile ? <MobileNavigationLink to={'/'} text={'Log out'} col={logCol ? logCol : -1}
                                                    motionNav={moutionNav ? moutionNav : false}/> :
        <NavigationLink to={'/'} text={'Log out'}/>

    const startLink = isMobile ? <MobileNavigationLink to={'/manager'} text={'Start'} col={startCol ? startCol : -1}
                                                       motionNav={moutionNav ? moutionNav : false}/> :
        <NavigationLink to={'/manager'} text={'Start'}/>

    const handleLogout = () => {
        useLogout();
        navigate('/');
        window.location.reload();
    }
    if (isUserSignedIn) {
        return (
            <>
                <div onClick={() => handleLogout()}>{logOut}</div>
                {startLink}
            </>
        )
    }

    return (
        <>
            {logIn}
        </>
    )
}