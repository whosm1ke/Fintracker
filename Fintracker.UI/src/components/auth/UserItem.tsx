import { User } from "../../entities/User"
// @ts-ignore
import logo from "../../assets/logo.png"
interface UserItemProps {
    item: User
}

export default function UserItem({item} : UserItemProps){

    return (
        <div className={'w-full flex justify-between items-center'}>
            <img src={item.userDetails?.avatar || logo} alt="Avatar" className="w-9 h-9 object-cover aspect-auto rounded-full"/>
            <p className={'font-semibold text-lg ml-4'}>{item.email}</p>
        </div>
    )
}
