import DateFilterBase from "../other/DateFilterBase.tsx";
import useTransactionQueryStore from "../../stores/transactionQueryStore.ts";

export default function TransactionDateFilter(){

    const [
        startDate, endDate, setStartDate, setEndDate
    ] = useTransactionQueryStore(x =>
        [x.query.startDate, x.query.endDate, x.setStartDate, x.setEndDate]);
    return (
        <DateFilterBase startDate={startDate!} setStartDate={setStartDate} endDate={endDate!} setEndDate={setEndDate}/>
    )
}