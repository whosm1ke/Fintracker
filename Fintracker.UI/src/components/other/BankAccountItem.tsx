import {Account} from "../../entities/MonobankUserInfo.ts";
import currencies from "../../data/currencies.ts";

interface BankAccountItemProps {
    item: Account
}

export default function BankAccountItem({item} : BankAccountItemProps){
    const currency = currencies.find(c => c.code === item.currencyCode) || currencies.find(c => c.symbol === 'UAH')!
    return (
        <div className={'flex w-full justify-between items-center'}>
            <p>{item.type}</p>
            <p>{item.maskedPan}</p>
            <p>({item.balance / 100} {currency.symbol})</p>
        </div>
    )
}