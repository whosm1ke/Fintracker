interface TransactionFiltersProps {
    startDate: Date;
    endDate: Date;
    handleDateFilterChange: (date: Date, isStartDate: boolean) => void;
    transPerPage: number;
    handleTransPerDateChangle: (num: number) => void;
}

const TransactionFilters = ({
                                startDate,
                                endDate,
                                handleDateFilterChange,
                                handleTransPerDateChangle,
                                transPerPage
                            }: TransactionFiltersProps) => {

    const startDateFilter = new Date(startDate).toLocaleDateString('en-CA');
    const endDateFilter = new Date(endDate).toLocaleDateString('en-CA');
    return (
        <>
            <div className={''}>
                <label className="text-sm lg:text-lg font-semibold flex flex-col">Start date
                    <input type="date" value={startDateFilter}
                           onChange={e => handleDateFilterChange(e.target.valueAsDate ?? new Date, true)}
                           className="p-2 border-2 border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                </label>
            </div>
            <div className={''}>
                <label className="text-sm lg:text-lg font-semibold flex flex-col">End date
                    <input type="date" value={endDateFilter}
                           onChange={e => handleDateFilterChange(e.target.valueAsDate ?? new Date, false)}
                           className="p-2 border-2 border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                </label>
            </div>
            <div className={''}>
                <label className="text-sm lg:text-lg font-semibold flex flex-col">Transactions per days
                    <input type="number" value={transPerPage}
                           onChange={e => handleTransPerDateChangle(e.target.valueAsNumber)}
                           className="p-2 border-2 border-gray-300 rounded-md focus:outline-none focus:border-blue-500"/>
                </label>
            </div>
        </>
    )
}

export default TransactionFilters;