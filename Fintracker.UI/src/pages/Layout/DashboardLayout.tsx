import {NavLink, Outlet} from "react-router-dom";
// @ts-ignore
import logo from "../../../public/logo.png";
import {motion} from "framer-motion";
import {useState} from "react";
import {FiChevronDown} from "react-icons/fi";
import useUserStore from "../../stores/userStore";
import { useGetUser } from "../../hooks/auth/useUser";
import FlyoutLink from "../../components/other/FlyoutLink.tsx";
import NavigationContent from "../../components/other/NavigationContent.tsx";

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
        <div className="block md:hidden border-t-2 bg-white border-t-gray-300 p-6 fixed bottom-0 w-full z-[110]">
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
    const avatar = data?.response?.userDetails?.avatar;
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const toggleMenuOpen = () => setIsMenuOpen(p => !p)

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



