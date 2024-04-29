import {NavLink, Outlet, useLocation} from "react-router-dom";

export default function WalletSettingsLayoutPage() {
    const loc = useLocation();
    const isGeneralSettings = !loc.pathname.includes('categories');
    return (
        <div className={'container mx-auto p-4'}>
            <div className={'flex flex-row p-2'}>
                <div className={'basis-1/4'}>
                    <nav className={'w-full'}>
                        <ul className={'w-full'}>
                            <li className={'w-full h-full text-center text-lg flex'}>
                                <NavLink to={``}
                                         end
                                         className={({isActive}) => {
                                             return isActive ? "w-full h-full bg-white rounded-tl rounded-bl shadow p-4 " +
                                                 "text-green-400 font-bold" : "w-full p-4"
                                         }}
                                >General Settings</NavLink>
                            </li>
                            <li className={'w-full text-center text-lg flex'}>
                                <NavLink to={`categories`}
                                         className={({isActive}) => {
                                             return isActive ? "w-full h-full bg-white rounded-tl rounded-bl shadow p-4 " +
                                                 "text-green-400 font-bold" : "w-full p-4"
                                         }}
                                >Category Settings</NavLink>
                            </li>
                        </ul>
                    </nav>
                </div>
                <div className={`basis-3/4 bg-white ${isGeneralSettings ? "rounded-tr-xl rounded-br-xl rounded-bl-xl " : "rounded-xl"}`}>
                    <Outlet/>
                </div>
            </div>
        </div>


    )
}