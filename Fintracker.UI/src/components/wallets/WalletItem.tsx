import {Wallet} from "../../entities/Wallet.ts";

interface WalletItemProps {
    item: Wallet
}

export default function WalletItem({item} : WalletItemProps){

    return (
        <div className={'flex justify-between items-center'}>
            <p>{item.name}</p>
            <p>({item.balance})</p>
        </div>
    )
}