import { User } from "../../entities/User"
// @ts-ignore
import logo from "../../assets/logo.png"
interface UserItemProps {
    item: User
}

export default function UserItem({item} : UserItemProps){

    return (
        <div className={'w-full flex justify-between items-center'}>
            <img src={item.userDetails?.avatar || logo} alt="Avatar" className="hidden ml-1 xl:inline xl:min-w-10 xl:h-10 rounded-full"/>
            <p className={'font-semibold text-sm md:text-lg xl:ml-3'}>{item.email}</p>
        </div>
    )
}
