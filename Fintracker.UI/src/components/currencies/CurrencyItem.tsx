import {Currency} from "../../entities/Currency.ts";

interface CurrencyItemProps {
    item: Currency
}

export default function CurrencyItem({item} : CurrencyItemProps){
  
    return (
        <div className={'flex w-full justify-between items-center'}>
            <p>{item.name}</p>
            <p>({item.symbol})</p>
        </div>
    )
}

