import {NavLink, useLocation, useNavigate} from "react-router-dom";
import useLogoutMutation from "../auth/useLogoutMutation.ts";
import {motion} from "framer-motion";
import { NavigationLinkProps } from "../../components/other/NavigationLink.tsx";

const MobileNavigationLink = ({to, text, motionNav, isLogin}: NavigationLinkProps & {
    motionNav: boolean,
    isLogin?: boolean
}) => {
    const navigate = useNavigate();
    const location = useLocation();
    const isActiveLink = location.pathname === to;

    const logout = useLogoutMutation();

    const handleLogout = async () => {
        await logout.mutateAsync(null);
        navigate('/');
        window.location.reload();
    }

    return (
        <motion.div
            className={`w-full`}
            initial={{x: '100vw', opacity: 0, display: 'none'}}
            animate={motionNav ? {x: 0, opacity: 1, marginLeft: 5, marginRight: 5, display: 'block'} : {}}
            exit={motionNav ? {x: '100vw', opacity: 0} : {}}
            transition={{duration: 0.6}}
            variants={{
                show: {display: 'block'},
                hide: {display: 'none'}
            }}
        >
            <div
                className={isActiveLink ? 'text-center p-2 border rounded-2xl shadow-md shadow-gray-900 bg-lime-500/85 h-10 line-clamp-2' :
                    'text-center p-2 border rounded-2xl shadow-md hover:shadow-lg shadow-gray-700 bg-lime-400/35 h-10 line-clamp-2'}>
                <NavLink

                    to={to}
                    onClick={(e) => {
                        if (isLogin) {
                            e.preventDefault();
                            handleLogout();
                        }
                    }}
                >{text}
                </NavLink>
            </div>
        </motion.div>
    )
}

export default MobileNavigationLink;