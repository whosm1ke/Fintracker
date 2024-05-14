import {Wallet} from "../../entities/Wallet.ts";

interface WalletItemProps {
    item: Wallet
}

export default function WalletItem({item} : WalletItemProps){

    return (
        <div className={'flex w-full justify-between items-center'}>
            <p className={'font-semibold'}>{item.name}</p>
            <p className={'ml-3'}>{item.balance} {item.currency.symbol}</p>
        </div>
    )
}