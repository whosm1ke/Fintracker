import {FaChevronLeft, FaChevronRight} from "react-icons/fa6";
import {useEffect, useRef, useState} from "react";
import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";
import {formatDate} from "../../helpers/globalHelper.ts";

// interface TransactionsDateFiltersProps {}


const TransactionsDateFilters = () => {

    const [
        startDate, endDate, setStartDate, setEndDate
    ] = useTransactionQueryStore(x =>
        [x.query.startDate, x.query.endDate, x.setStartDate, x.setEndDate]);

    const [isDateFilterOpen, setIsDateFilterOpen] = useState(false);
    const [currentStep, setCurrentStep] = useState('This week');
    const closeDateFilter = () => setIsDateFilterOpen(false);
    const openDateFilter = () => setIsDateFilterOpen(true);
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

    const handleDateFilterChange = (date: Date, isStartDate: boolean) => {
        if (isStartDate) setStartDate(formatDate(date))
        if (!isStartDate) setEndDate(formatDate(date))
    }


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
        let newStartDate = new Date(startDate!);
        newStartDate.setHours(0, 0, 0, 0)
        let newEndDate = new Date(endDate!);
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
    
    const isAllHistoryShowing = currentStep !== "All history"
    return (
        <div className={''}>
            <div
                className={'flex justify-between items-center gap-x-3 sm:gap-x-5 text-sm md:text-lg ' + `${isDateFilterOpen ? 'pointer-events-none' : 'pointer-events-auto'}`}>
                <div className={'bg-stone-200 rounded shadow px-4 py-2'}>
                    <button onClick={() => shiftDates('left')}
                            className={'text-center align-middle'}>
                        <FaChevronLeft size={'1rem'}/>
                    </button>
                </div>
                <div onClick={openDateFilter}
                     className={'min-w-64 md:w-96 lg:w-128 xl:w-160 flex justify-center bg-stone-200 rounded shadow px-4 py-2'}>
                    {isAllHistoryShowing ?
                        <div className={'flex justify-around w-full'}>
                            <p className={''}>{new Date(startDate!).toDateString()}</p>
                            <p className={''}>{" - "}</p>
                            <p className={''}>{new Date(endDate!).toDateString()}</p>
                        </div>
                        :
                        <p className={''}>All history</p>
                    }
                </div>
                <div className={'bg-stone-200 rounded shadow px-4 py-2'}>
                    <button onClick={() => shiftDates('right')}
                            className={'text-center align-middle'}>
                        <FaChevronRight size={'1rem'}/>
                    </button>
                </div>
            </div>
            {isDateFilterOpen &&
                <div ref={menuRef}
                     className={'relative inset-0 flex w-full justify-center items-center visible bg-black/20 z-10'}>
                    <div className={'absolute top-1 w-full mt-1 bg-stone-200 px-4 py-2 rounded-lg shadow-lg'}>
                        <div className={'grid grid-cols-1 sm:grid-cols-2 gap-3 w-full'}>
                            <label className="text-sm flex font-bold flex-col">Start date
                                <input type="date" value={isAllHistoryShowing ? startDate : ""}
                                       onChange={e => handleDateFilterChange(e.target.valueAsDate || new Date(), true)}
                                       className="px-2 py-1 text-[18px] border-2 font-[100] border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                            </label>
                            <label className="text-sm font-bold flex flex-col">End date
                                <input type="date" value={isAllHistoryShowing ? endDate : ""}
                                       onChange={e => handleDateFilterChange(e.target.valueAsDate || new Date(), false)}
                                       className="px-2 py-1 text-[18px] font-[100] border-2 border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                            </label>
                        </div>
                        <div className={'grid grid-cols-1 sm:grid-cols-2 mt-2 gap-2'}>
                            {buttonsLabels.map(b =>
                                <button key={b} onClick={(e) => setDates(e.currentTarget.innerText)}
                                        className="w-full bg-blue-500 text-white rounded-md px-2 py-1 hover:bg-blue-700 transition-colors duration-300">{b}</button>
                            )}
                        </div>
                    </div>
                </div>
            }
        </div>

    )
}

//@ts-ignore
export default TransactionsDateFilters;