import {NavLink, useNavigate} from "react-router-dom";
import useLogoutMutation from "../../hooks/auth/useLogoutMutation.ts";

export default function NavigationContent() {

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