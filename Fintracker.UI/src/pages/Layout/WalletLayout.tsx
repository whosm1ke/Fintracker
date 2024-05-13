import {NavLink, Outlet, useParams} from "react-router-dom";
import useUserStore from "../../stores/userStore.ts";
import {useGetUser} from "../../hooks/auth/useUser.ts";
// @ts-ignore
import logo from "../../../public/logo.png";
import {useState} from "react";
import {motion} from "framer-motion";
import {FiChevronDown} from "react-icons/fi";
import FlyoutLink from "../../components/other/FlyoutLink.tsx";
import NavigationContent from "../../components/other/NavigationContent.tsx";

export default function WalletLayout() {
    return (
        <div className={'flex flex-col min-h-screen overflow-hidden bg-stone-100'}>
            <header>
                <WalletNavBar/>
            </header>
            <main className={'flex-grow mb-24'}>
                <Outlet/>
            </main>
            <footer className={'block lg:hidden flex-shrink-0 z-[100]'}>
                <Footer/>
            </footer>
        </div>
    )
}

const Footer = () => {
    const {walletId} = useParams();
    return (
        <div className="border-t-2 border-t-gray-300 p-6 mt-6 w-full fixed bottom-0 bg-white z-[210]">
            <div className={'flex justify-around items-center gap-x-2 w-full text-sm'}>
                <div>
                    <NavLink to={`./${walletId}/trans`}
                             className={({isActive}) =>
                                 isActive ? 'text-green-400 font-bold' : 'p-2'
                             }
                    >Transactions</NavLink>
                </div>
                <div>
                    <NavLink to={`./${walletId}/budgets`}
                             className={({isActive}) =>
                                 isActive ? 'text-green-400 font-bold' : 'p-2'
                             }
                    >Budgets</NavLink>
                </div>
                <div>
                    <NavLink to={`./${walletId}/settings`}
                             className={({isActive}) =>
                                 isActive ? 'text-green-400 font-bold' : 'p-2'
                             }
                    >Settings</NavLink>
                </div>
            </div>
        </div>

    )
}

const WalletNavBar = () => {
    const {walletId} = useParams();
    const [userId] = useUserStore(x => [x.getUserId()]);
    const {data} = useGetUser(userId || 'no-user');
    const userName = data?.response?.userName || 'New user';
    const avatar = data?.response?.userDetails?.avatar || logo
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const toggleMenuOpen = () => setIsMenuOpen(p => !p)

    return (
        <motion.nav className={'w-full flex justify-between items-center bg-gray-100 p-3 px-2 sm:px-12 shadow'}>
            <div>
                <NavLink to={'/dashboard'} className={'flex items-center gap-x-3'}>
                    <img src={logo} alt="Logo" className={'w-14 h-14 hidden md:inline'}/>
                    <p className={'text-2xl font-bold'}>Fintracker</p>
                </NavLink>
            </div>
            <div className="hidden lg:block">
                <NavLink
                    to={`./${walletId}/trans`}
                    className={({isActive}) =>
                        isActive ? 'text-green-400 text-xl font-bold border-b-4 rounded-b border-b-green-400 p-6' : 'text-xl p-5'
                    }
                >
                    Transactions
                </NavLink>
                <NavLink
                    to={`./${walletId}/budgets`}
                    className={({isActive}) =>
                        isActive ? 'text-green-400 text-xl font-bold border-b-4 rounded-b border-b-green-400 p-6' : 'text-xl p-5'
                    }
                >
                    Budgets
                </NavLink>
                <NavLink
                    to={`./${walletId}/settings`}
                    className={({isActive}) =>
                        isActive ? 'text-green-400 text-xl font-bold border-b-4 rounded-b border-b-green-400 p-6' : 'text-xl p-5'
                    }
                >
                    Settings
                </NavLink>

            </div>
            <motion.div
                className={'relative z-50'}
                onHoverEnd={toggleMenuOpen}
                onHoverStart={toggleMenuOpen}
                onTouchStart={toggleMenuOpen}
            >
                <FlyoutLink FlyoutContent={NavigationContent} open={isMenuOpen}>
                    <div className={'flex items-center space-x-2 text-neutral-900'}>
                        <img src={avatar} alt="Logo" className={'w-12 h-12 rounded-full'}/>
                        <p className={'text-2xl font-bold hidden md:inline'}>{userName}</p>
                        <motion.span
                            animate={isMenuOpen ? {rotate: 180} : {rotate: 0}}
                        >
                            <FiChevronDown size={'2rem'}/>
                        </motion.span>
                    </div>
                </FlyoutLink>

            </motion.div>
        </motion.nav>
    )
}