import {Category} from "../../entities/Category.ts";
import * as Icons from "react-icons/md";
import {IconType} from "react-icons";

interface CategoryHeadingItemProps {
    item: Category
}

export default function CategoryHeadingItem({item}: CategoryHeadingItemProps) {

    const Icon = (Icons as any)[item.image] as IconType;
    return (
        <div className={'flex items-center h-auto w-full'}>
            <Icon color={item.iconColour} className="" size={'21px'}/>
            <span className="ml-3">{item.name}</span>
        </div>
    )
}