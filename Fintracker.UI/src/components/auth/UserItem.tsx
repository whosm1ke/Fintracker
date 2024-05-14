import { User } from "../../entities/User"
interface UserItemProps {
    item: User
}

export default function UserItem({item} : UserItemProps){

    return (
        <div className={'w-full flex items-center'}>
            <img src={item.userDetails?.avatar} alt="Avatar" className="hidden ml-1 xl:inline xl:min-w-10 xl:h-10 rounded-full"/>
            <p className={'font-semibold text-sm md:text-lg xl:ml-3'}>{item.email}</p>
        </div>
    )
}
