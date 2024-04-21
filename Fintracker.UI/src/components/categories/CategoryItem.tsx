import {IconType} from "react-icons";
import {Category} from "../../entities/Category"
import * as Icons from 'react-icons/md'

interface CategoryItemProps {
    item: Category
}

export default function CategoryItem({item}: CategoryItemProps) {
    const Icon = (Icons as any)[item.image] as IconType;
    return (
        <div className={'flex items-center'}>
            <Icon color={item.iconColour} className="" size={'2rem'}/>
            <span className="ml-3">{item.name}</span>
        </div>
    )
}