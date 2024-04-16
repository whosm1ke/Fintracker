import {useState} from "react";
import {NavLink, useLocation, useNavigate} from "react-router-dom";
import useLogout from "../hooks/useLogout.ts";
import {motion} from "framer-motion";

export interface NavigationLinkProps {
    to: string;
    text: string;
}

const NavigationLink = ({to, text, isLogin}: NavigationLinkProps & { isLogin?: boolean }) => {
    const [isHovered, setHovered] = useState(false);
    const navigate = useNavigate();
    const location = useLocation();
    const isActiveLink = location.pathname === to;
    const handleLogout = () => {
        useLogout();
        window.location.reload();
        navigate('/');
    }
    return (
        <motion.div
            onHoverStart={() => setHovered(true)}
            onHoverEnd={() => setHovered(false)}
            whileHover={{scale: 1.1}}
            whileTap={{scale: 0.9}}
        >
            <NavLink
                className={({isActive}) => {
                    return isActive ? 'relative text-center text-xl p-2 bg-green-300 border-2 border-blue-500 rounded-lg' :
                        'relative text-center text-2xl sm:mx-2 md:mx-4 xl:mx-8'
                }}
                to={to}
                onClick={(e) => {
                    if (isLogin) {
                        
                        e.preventDefault();
                        handleLogout();
                    }
                }}
            >
                {text}
                {!isActiveLink && <motion.span
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
                />}
            </NavLink>
        </motion.div>
    );
};

export default NavigationLink;