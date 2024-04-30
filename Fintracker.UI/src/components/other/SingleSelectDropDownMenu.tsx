import {useState, useRef, FC, useEffect} from 'react';
import {FaChevronDown} from "react-icons/fa6";

interface DropdownProps<T extends { id: string }> {
    items: T[];
    ItemComponent: FC<{ item: T }>;
    HeadingComponent?: FC<{ item: T }>;
    heading: string;
    onItemSelected: (item: T) => void;
    defaultSelectedItem: T | undefined;
    canBeChanged?: boolean;
}

const SingleSelectDropDownMenu = <T extends { id: string }>({
                                                                items,
                                                                onItemSelected,
                                                                HeadingComponent,
                                                                ItemComponent,
                                                                heading,
                                                                defaultSelectedItem,
                                                                canBeChanged = true,
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
        <div className="relative w-full" ref={dropdownRef}>
            <div onClick={() => canBeChanged && setIsOpen(p => !p)}>
                <button type="button"
                        className="inline-flex justify-between items-center w-full rounded-md border border-gray-300 shadow-sm pr-4 pl-1 py-2 bg-white text-sm ring-0 font-medium text-gray-700 hover:bg-gray-50 hover:outline-none hover:ring-2 hover:ring-offset-2 hover:ring-offset-gray-100 hover:ring-indigo-500"
                        id="options-menu" aria-haspopup="true" aria-expanded="true">
                    {defaultSelectedItem ? (
                        <div className="flex items-center space-x-2 overflow-hidden w-full">
                            <div className="flex-shrink-0 w-full px-2">
                                {HeadingComponent ? <HeadingComponent item={defaultSelectedItem}/> :
                                    <ItemComponent item={defaultSelectedItem}/>}
                            </div>
                        </div>
                    ) : (
                        <span className={'px-2'}>{heading}</span>
                    )}
                    {canBeChanged &&
                        <FaChevronDown size={'1rem'} className={''}/>}
                </button>
            </div>
            {isOpen && items.length > 0 && (
                <div
                    className="origin-top-right absolute right-0 mt-2 w-full rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 overflow-auto max-h-60 z-10">
                    <div className="py-1" role="menu" aria-orientation="vertical" aria-labelledby="options-menu">
                        {items.map(item => (
                            <div key={item.id}
                                 className="flex w-full items-center px-4 py-2 gap-x-2 text-sm text-gray-700 cursor-pointer hover:bg-gray-100 hover:text-gray-900"
                                 role="menuitem" onClick={() => {
                                setIsOpen(p => !p);
                                onItemSelected(item);
                            }}>
                                <input type="radio" className="form-radio h-5 w-5 text-indigo-600"
                                       checked={defaultSelectedItem?.id === item.id} readOnly/>
                                <div className="cursor-pointer hover:bg-gray-100 w-full">
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


export default SingleSelectDropDownMenu;
