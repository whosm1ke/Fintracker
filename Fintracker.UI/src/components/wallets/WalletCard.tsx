import {Link} from "react-router-dom";
import {IoWalletSharp} from "react-icons/io5";

interface WalletCardProps {
    name: string,
    balance: number,
    isBanking: boolean,
    currencySymbol: string,
    walletId: string
}

const WalletCard = ({name, balance, isBanking, currencySymbol, walletId}: WalletCardProps) => {

    const isPositiveBalance = balance > 0;
    const formatedBalance: string = Math.abs(balance).toLocaleString();
    const balanceText: string = isPositiveBalance ? `+ ${formatedBalance} ${currencySymbol}` :
        `- ${formatedBalance} ${currencySymbol}`;

    return (
        <Link to={`/wallet/${walletId}/trans`}
              className={'flex flex-col sm:flex-row space-y-3 sm:space-y-0 space-x-0 sm:space-x-3 ' +
                  'p-4 bg-slate-100 rounded-lg shadow w-full'}>
    <span className={'self-center'}>
        <IoWalletSharp color={isBanking ? 'orange' : 'green'} size={'2rem'}/>
    </span>
            <div className={'px-4 py-2'}>
                <h4 className={'text-lg'}>{name}</h4>
                <span>{isBanking ? 'Monobank' : 'Cash'}</span>
                <p className={isPositiveBalance ? "text-green-400 text-xl" : 'text-red-500 text-xl'}>{balanceText}</p>
            </div>
        </Link>

    )
}

export default WalletCard;