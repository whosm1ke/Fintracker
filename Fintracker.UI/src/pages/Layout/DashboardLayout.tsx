import {NavLink, Outlet, useNavigate} from "react-router-dom";
// @ts-ignore
import logo from "../../assets/logo.png";
import {AnimatePresence, motion} from "framer-motion";
import {ReactNode, useState} from "react";
import {FiChevronDown} from "react-icons/fi";
import useUserStore from "../../stores/userStore";
import { useGetUser } from "../../hooks/auth/useUser";
import useLogoutMutation from "../../hooks/auth/useLogoutMutation";

export default function DashboardLayout() {

    return (
        <div className={'relative flex flex-col min-h-screen overflow-hidden bg-stone-100'}>
            <header>
                <DashboardNavBar/>
            </header>
            <main className={'flex-grow mb-24'}>
                <Outlet/>
            </main>
            <footer>
                <Footer/>
            </footer>
        </div>
    )
}

const Footer = () => {
    return (
        <div className="block md:hidden border-t-2 bg-white border-t-gray-300 p-6 fixed bottom-0 w-full">
            <div className={'flex justify-around items-center gap-x-4'}>
                <div>
                    <NavLink to={'/dashboard'}
                             end
                             className={({isActive}) =>
                                 isActive ? 'text-green-400 text-xl font-bold' : 'text-xl p-5'
                             }
                    >Dashboard</NavLink>
                </div>
                <div>
                    <NavLink to={'/dashboard/budgets'}
                             className={({isActive}) =>
                                 isActive ? 'text-green-400 text-xl font-bold' : 'text-xl p-5'
                             }
                    >Budgets</NavLink>
                </div>
            </div>
        </div>
    )
}


const DashboardNavBar = () => {
    const [userId] = useUserStore(x => [x.getUserId()]);
    const {data} = useGetUser(userId || 'no-user');
    const userName = data?.response?.userName || 'New user';
    const avatar = data?.response?.userDetails?.avatar || logo
    const [isMenuOpen, toggleMenuOpen] = useState(false);

    return (
        <motion.nav className={'w-full flex justify-between items-center bg-gray-100 p-3 px-12 shadow'}>
            <div>
                <NavLink to={'/dashboard'} className={'flex items-center gap-x-3'}>
                    <img src={logo} alt="Logo" className={'w-14 h-14 hidden md:inline'}/>
                    <p className={'text-2xl font-bold'}>Fintracker</p>
                </NavLink>
            </div>
            <div className="hidden md:block">
                <NavLink
                    end
                    to="/dashboard"
                    className={({isActive}) =>
                        isActive ? 'text-green-400 text-xl font-bold border-b-4 rounded-b border-b-green-400 p-6' : 'text-xl p-5'
                    }
                >
                    Dashboard
                </NavLink>
                <NavLink
                    to="/dashboard/budgets"
                    className={({isActive}) =>
                        isActive ? 'text-green-400 text-xl font-bold border-b-4 rounded-b border-b-green-400 p-6' : 'text-xl p-5'
                    }
                >
                    Budgets
                </NavLink>
            </div>
            <motion.div
                className={'relative'}
            >
                <FlyoutLink FlyoutContent={NavigationContent} open={isMenuOpen}>
                    <div className={'flex items-center space-x-2 text-neutral-900'}>
                        <img src={avatar || logo} alt="Logo" className={'w-12 h-12 rounded-full'}/>
                        <p className={'text-2xl font-bold hidden md:inline'}>{userName}</p>
                        <motion.span
                            onClick={() => toggleMenuOpen(p => !p)}
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

interface FlyoutLinkProps {
    children: ReactNode,
    FlyoutContent: any,
    open: boolean;
}

const FlyoutLink = ({children, FlyoutContent, open}: FlyoutLinkProps) => {

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

const NavigationContent = () => {

    const navigate = useNavigate();
    const logout = useLogoutMutation();
    const handleLogout = async () => {
        await logout.mutateAsync(null);
        navigate('/');
        window.location.reload();
    }

    return (
        <div className="w-64 bg-white rounded-lg shadow-md">
            <ul className="list-none p-4 text-xl">
                <li className="mb-3">
                    <NavLink to="/settings"
                             className="text-gray-600 hover:text-green-500 block px-4 py-2 rounded-md">Settings</NavLink>
                </li>
                <li className="mb-3">
                    <NavLink to="/support"
                             className="text-gray-600 hover:text-green-500 block px-4 py-2 rounded-md">Support</NavLink>
                </li>
                <li className="mb-3">
                    <NavLink to="/"
                             className="text-gray-600 hover:text-green-500 block px-4 py-2 rounded-md">Home</NavLink>
                </li>
                <li className="mb-3">
                    <span
                        onClick={handleLogout}
                        className="text-gray-600 hover:text-green-500 block px-4 py-2 rounded-md cursor-pointer">Logout</span>
                </li>
            </ul>
        </div>

    );
};