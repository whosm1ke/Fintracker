import {useEffect, useRef, useState} from "react";
import * as Icons from "react-icons/md";
import {IconType} from "react-icons";
import { Category } from "../../entities/Category";

interface CategoriesDropDownListProps {
    categories: Category[];
    handleToggleCategoryId: (categoryId: string) => void;
    selectedCategoryIds: string[];
    handleSelectAllCategories: () => void
}

const CategoriesDropDownList = ({
                                    categories,
                                    handleToggleCategoryId,
                                    selectedCategoryIds,
                                    handleSelectAllCategories,
                                }: CategoriesDropDownListProps) => {
    const [isOpen, setIsOpen] = useState(false)
    const CategoryItem = (category: Category) => {
        const Icon = (Icons as any)[category.image] as IconType;

        return (
            <div
                className="flex items-center px-4 py-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100 hover:text-gray-900"
                role="menuitem" onClick={() => handleToggleCategoryId(category.id)}>
                <input type="checkbox" className="form-checkbox h-5 w-5 text-indigo-600"
                       checked={selectedCategoryIds.includes(category.id)} readOnly/>
                <Icon color={category.iconColour} className="ml-3" size={'2rem'}/>
                <span className="ml-3">{category.name}</span>
            </div>
        )
    }


    const dropdownRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
                setIsOpen(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);

    return (
        <div className="relative" ref={dropdownRef}>
            <div
                onClick={() => setIsOpen(p => !p)}>
                <button type="button"
                        className="inline-flex justify-between w-full rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-sm ring-0 font-medium text-gray-700 hover:bg-gray-50 hover:outline-none hover:ring-2 hover:ring-offset-2 hover:ring-offset-gray-100 hover:ring-indigo-500"
                        id="options-menu" aria-haspopup="true" aria-expanded="true">
                    <div>
                        <span>Categories</span>
                        <span
                            className="ml-2 bg-indigo-200 text-indigo-700 rounded-full px-2 py-1 text-xs font-bold">{selectedCategoryIds.length}</span>
                    </div>
                    <svg className="-mr-1 ml-2 h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"
                         fill="currentColor" aria-hidden="true">
                        <path fillRule="evenodd"
                              d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                              clipRule="evenodd"/>
                    </svg>
                </button>
            </div>

            {isOpen && <div
                className="origin-top-right absolute right-0 mt-2 w-full rounded-md shadow-lg bg-white
                 ring-1 ring-black ring-opacity-5 overflow-auto max-h-60 z-10">
                <div className="py-1" role="menu" aria-orientation="vertical" aria-labelledby="options-menu">
                    <div
                        className="flex items-center px-4 py-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100 hover:text-gray-900"
                        role="menuitem" onClick={handleSelectAllCategories}>
                        <input type="checkbox" className="form-checkbox h-5 w-5 text-indigo-600"
                               checked={selectedCategoryIds.length === categories.length} readOnly/>
                        <span className="ml-3">Select all</span>
                    </div>
                    {categories.map(category => (
                        <CategoryItem key={category.id} name={category.name} type={category.type} image={category.image}
                                      iconColour={category.iconColour} id={category.id}/>
                    ))}
                </div>
            </div>}
        </div>
    );
}

export default CategoriesDropDownList;