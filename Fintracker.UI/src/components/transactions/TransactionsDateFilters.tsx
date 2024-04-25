import {FaChevronLeft, FaChevronRight} from "react-icons/fa6";
import {useEffect, useRef, useState} from "react";

interface TransactionsDateFiltersProps {
    startDate: string;
    endDate: string;
    handleDateFilterChange: (date: Date, isStartDate: boolean) => void;
}


const TransactionsDateFilters = ({
                                startDate,
                                endDate,
                                handleDateFilterChange
                            }: TransactionsDateFiltersProps) => {

    const [isDateFilterOpen, setIsDateFilterOpen] = useState(false);
    const [currentStep, setCurrentStep] = useState('This week');
    const closeDateFilter = () => setIsDateFilterOpen(false);
    const openDateFilter = () => setIsDateFilterOpen(true);
    const startDateToShow = new Date(startDate).toDateString();
    const endDateToShow = new Date(endDate).toDateString();

    const startDateAsDefaultValue = new Date(startDate).toLocaleDateString('en-CA');
    const endDateAsDefaultValue = new Date(endDate).toLocaleDateString('en-CA');
    const menuRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
                closeDateFilter();
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);

    const setDates = (period: string) => {
        let newStartDate = new Date();
        newStartDate.setHours(0, 0, 0, 0)
        let newEndDate = new Date();
        newEndDate.setHours(0, 0, 0, 0)

        switch (period) {
            case 'This week':
                newStartDate.setDate(newStartDate.getDate() - newStartDate.getDay() + 1);
                newEndDate.setDate(newStartDate.getDate() + 6);
                setCurrentStep('This week')
                break;
            case 'Previous week':
                newStartDate.setDate(newStartDate.getDate() - newStartDate.getDay() - 6);
                newEndDate.setDate(newStartDate.getDate() + 6);
                setCurrentStep('Previous week')
                break;
            case 'This month':
                newStartDate.setDate(1);
                newEndDate = new Date(newEndDate.getFullYear(), newEndDate.getMonth() + 1, 0);
                setCurrentStep('This month')
                break;
            case 'Previous month':
                newStartDate = new Date(newStartDate.getFullYear(), newStartDate.getMonth() - 1, 1);
                newEndDate = new Date(newEndDate.getFullYear(), newEndDate.getMonth(), 0);
                setCurrentStep('Previous month')
                break;
            case 'This year':
                newStartDate = new Date(newStartDate.getFullYear(), 0, 1);
                newEndDate = new Date(newEndDate.getFullYear(), 11, 31);
                setCurrentStep('This year')
                break;
            case 'Previous year':
                newStartDate = new Date(newStartDate.getFullYear() - 1, 0, 1);
                newEndDate = new Date(newEndDate.getFullYear() - 1, 11, 31);
                setCurrentStep('Previous year')
                break;
            case 'All history':
                newStartDate = new Date(0); // Set to Unix Epoch
                setCurrentStep('All history')
                break;
        }
        handleDateFilterChange(newStartDate, true);
        handleDateFilterChange(newEndDate, false);

        closeDateFilter()
    }

    const shiftDates = (direction: 'left' | 'right') => {
        let newStartDate = new Date(startDate);
        newStartDate.setHours(0, 0, 0, 0)
        let newEndDate = new Date(endDate);
        newEndDate.setHours(0, 0, 0, 0)

        switch (currentStep) {
            case 'This week':
            case 'Previous week':
                newStartDate.setDate(newStartDate.getDate() + (direction === 'left' ? -7 : 7));
                newEndDate.setDate(newEndDate.getDate() + (direction === 'left' ? -7 : 7));
                break;
            case 'This month':
            case 'Previous month':
                if (direction === 'left') {
                    newStartDate.setMonth(newStartDate.getMonth() - 1, 1);
                    newEndDate.setMonth(newEndDate.getMonth(), 0);
                } else {
                    newStartDate.setMonth(newStartDate.getMonth() + 1, 1);
                    newEndDate.setMonth(newEndDate.getMonth() + 2, 0);
                }
                break;
            case 'This year':
            case 'Previous year':
                if (direction === 'left') {
                    newStartDate.setFullYear(newStartDate.getFullYear() - 1, 0, 1);
                    newEndDate.setFullYear(newEndDate.getFullYear(), 0, 0);
                } else {
                    newStartDate.setFullYear(newStartDate.getFullYear() + 1, 0, 1);
                    newEndDate.setFullYear(newEndDate.getFullYear() + 2, 0, 0);
                }
                break;
            case 'All history':
                // No shift for 'All history'
                break;
        }
        handleDateFilterChange(newStartDate, true);
        handleDateFilterChange(newEndDate, false);

        closeDateFilter()
    }

    const buttonsLabels: string[] = [
        'This week', 'Previous week', 'This month', 'Previous month', 'This year', 'Previous year', 'All history'
    ]
    return (
        <>
            <div
                className={'flex  justify-between items-center gap-5 ' + `${isDateFilterOpen ? 'pointer-events-none' : 'pointer-events-auto'}`}>
                <button
                    onClick={() => shiftDates('left')}
                    className={'hidden sm:block w-full md:w-auto px-4 py-2 bg-stone-200 rounded shadow'}>
                    <FaChevronLeft size={'1.4rem'}/>
                </button>
                <div
                    onClick={openDateFilter}
                    className={'w-full md:w-auto flex justify-center md:justify-between gap-x-5 bg-stone-200 rounded shadow px-4 py-2'}>
                    <p>{startDateToShow}</p>
                    -
                    <p>{endDateToShow}</p>
                </div>
                <button
                    onClick={() => shiftDates('right')}
                    className={'hidden sm:block w-full md:w-auto px-4 py-2 bg-stone-200 rounded shadow'}>
                    <FaChevronRight size={'1.4rem'}/>
                </button>
            </div>
            {isDateFilterOpen &&
                <div ref={menuRef}
                     className={'relative inset-0 flex w-full justify-center items-center visible bg-black/20 z-10'}>
                    <div
                        className={'absolute top-1 w-full mt-1 bg-stone-200 px-4 py-2 rounded-lg shadow-lg'}>
                        <div className={'grid grid-cols-1 sm:grid-cols-2 gap-3 w-full'}>
                            <label className="text-sm flex font-bold flex-col">Start date
                                <input type="date" value={startDateAsDefaultValue}
                                       onChange={e => handleDateFilterChange(e.target.valueAsDate || new Date(), true)}
                                       className="px-2 py-1 text-[18px] border-2 font-[100] border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                            </label>
                            <label className="text-sm font-bold flex flex-col">End date
                                <input type="date" value={endDateAsDefaultValue}
                                       onChange={e => handleDateFilterChange(e.target.valueAsDate || new Date(), false)}
                                       className="px-2 py-1 text-[18px] font-[100] border-2 border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                            </label>
                        </div>
                        <div className={'grid grid-cols-1 sm:grid-cols-2 mt-2 gap-2'}>
                            {buttonsLabels.map(b =>
                                <button key={b}
                                        onClick={(e) => setDates(e.currentTarget.innerText)}
                                        className="w-full bg-blue-500 text-white rounded-md px-2 py-1 hover:bg-blue-700 transition-colors duration-300">{b}
                                </button>
                            )}
                        </div>
                    </div>
                </div>
            }
        </>
    )
}

//@ts-ignore
export default TransactionsDateFilters;