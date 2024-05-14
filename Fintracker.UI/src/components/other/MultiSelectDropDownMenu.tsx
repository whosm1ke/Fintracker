import React, {useState, useEffect, useRef} from 'react';
import {FaChevronDown} from "react-icons/fa6";

interface DropdownProps<T extends { id: string }> {
    items: T[];
    ItemComponent: React.FC<{ item: T }>;
    heading: string;
    onItemSelected: (item: T) => void;
    onAllItemsSelected: () => void;
    selectedItems: T[];
}

const MultiSelectDropDownMenu = <T extends { id: string }>({
                                                               items,
                                                               onItemSelected,
                                                               selectedItems,
                                                               onAllItemsSelected,
                                                               ItemComponent,
                                                               heading
                                                           }: DropdownProps<T>) => {
    const [isOpen, setIsOpen] = useState(false);
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
            <div onClick={(e) => {
                e.stopPropagation();
                setIsOpen(p => !p)
            }}>
                <button type="button"
                        className="inline-flex justify-between w-full rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-sm ring-0 font-medium text-gray-700 hover:bg-gray-50 hover:outline-none hover:ring-2 hover:ring-offset-2 hover:ring-offset-gray-100 hover:ring-indigo-500"
                        id="options-menu" aria-haspopup="true" aria-expanded="true">
                    <div>
                        <span>{heading}</span>
                        <span
                            className="ml-2 bg-indigo-200 text-indigo-700 rounded-full px-2 py-1 text-xs font-bold">{selectedItems.length}</span>
                    </div>
                    <FaChevronDown size={'1rem'}/>
                </button>
            </div>
            {isOpen && (
                <div
                    className="origin-top-right absolute right-0 mt-2 w-full rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 overflow-auto max-h-60 z-10">
                    <div className="py-1" role="menu" aria-orientation="vertical" aria-labelledby="options-menu">
                        <div
                            className="flex items-center px-4 py-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100 hover:text-gray-900"
                            role="menuitem" onClick={onAllItemsSelected}>
                            <input type="checkbox" className="min-h-5 min-w-5 text-indigo-600"
                                   checked={selectedItems.length === items.length} readOnly/>
                            <span className="ml-3">Select all</span>
                        </div>
                        {items.map(item => (
                            <div key={item.id}
                                 className="w-full flex items-center gap-x-2 px-4 py-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100 hover:text-gray-900"
                                 role="menuitem" onClick={() => onItemSelected(item)}>

                                <input type="checkbox" className="min-h-5 min-w-5 text-indigo-600"
                                       checked={selectedItems.some(selectedItem => selectedItem.id === item.id)} readOnly/>
                                <div className="w-full cursor-pointer hover:bg-gray-100">
                                    <ItemComponent item={item}/>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
};

export default MultiSelectDropDownMenu;
