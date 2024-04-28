import DateFilterBase from "../other/DateFilterBase.tsx";
import useWalletInfoStore from "../../stores/walletStore.ts";

export default function WalletsDateFilter(){

    const [
        startDate, endDate, setStartDate, setEndDate
    ] = useWalletInfoStore(x =>
        [x.filters.startDate, x.filters.endDate, x.setStartDate, x.setEndDate]);
    return (
        <DateFilterBase startDate={startDate!} setStartDate={setStartDate} endDate={endDate!} setEndDate={setEndDate}/>
    )
}