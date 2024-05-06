import {NavLink, Outlet} from "react-router-dom";
import useUserStore from "../../stores/userStore.ts";
import {useGetUser} from "../../hooks/auth/useUser.ts";
// @ts-ignore
import logo from "../../assets/logo.png";
import {useState} from "react";
import {motion} from "framer-motion";
import FlyoutLink from "../../components/other/FlyoutLink.tsx";
import NavigationContent from "../../components/other/NavigationContent.tsx";
import {FiChevronDown} from "react-icons/fi";

export default function GlobalSettings() {



    return (
        <div className={'bg-stone-100 min-h-screen'}>
            <Header/>
            <div className={'container mx-auto p-4'}>
                <div className={'flex flex-col sm:flex-row p-2'}>
                    <div className={'basis-1/4'}>
                        <nav className={'w-full'}>
                            <ul className={'w-full'}>
                                <li className={'w-full h-full text-center text-md sm:text-lg flex'}>
                                    <NavLink to={``}
                                             end
                                             className={({isActive}) => {
                                                 return isActive ? "w-full h-full bg-white rounded-tl rounded-bl shadow p-4 " +
                                                     "text-green-400 font-bold" : "w-full p-4"
                                             }}
                                    >Account</NavLink>
                                </li>
                                <li className={'w-full text-center text-md sm:text-lg flex'}>
                                    <NavLink to={`all-categories?isOwner=true`}
                                             className={({isActive}) => {
                                                 return isActive ? "w-full h-full bg-white rounded-tl rounded-bl shadow p-4 " +
                                                     "text-green-400 font-bold" : "w-full p-4"
                                             }}
                                    >All Categories</NavLink>
                                </li>
                            </ul>
                        </nav>
                    </div>
                    <div
                        className={`basis-3/4 bg-white ${true ? "rounded-tr-xl rounded-br-xl rounded-bl-xl " : "rounded-xl"}`}>
                        <Outlet/>
                    </div>
                </div>
            </div>

        </div>

    )
}

const Header = () => {
    const [userId] = useUserStore(x => [x.getUserId()]);
    const {data} = useGetUser(userId || 'no-user');
    const userName = data?.response?.userName || 'New user';
    const avatar = data?.response?.userDetails?.avatar || logo
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
            <motion.div
                className={'relative z-50'}
                onHoverEnd={toggleMenuOpen}
                onHoverStart={toggleMenuOpen}
            >
                <FlyoutLink FlyoutContent={NavigationContent} open={isMenuOpen}>
                    <div className={'flex items-center space-x-2 text-neutral-900'}>
                        <img src={avatar || logo} alt="Logo" className={'w-12 h-12 rounded-full'}/>
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