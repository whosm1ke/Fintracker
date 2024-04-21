// @ts-ignore

import useWallets from "../../hooks/wallet/useWallets";
import useUserStore from "../../stores/userStore";
import Spinner from "../../components/other/Spinner.tsx";
import CreateCashWalletModal from "../../components/wallets/CreateCashWalletModal.tsx";
import WalletCard from "../../components/wallets/WalletCard.tsx";
import CreateMonobankWalletModal from "../../components/wallets/CreateMonobankWalletModal.tsx";




export default function FintrackerPage() {
    const userId = useUserStore(x => x.getUserId());
    const {data: wallets} = useWallets(userId || 'no-user');
    
    if(wallets === undefined) return <Spinner/>
    
    return (
        <div className={'container mx-auto p-4 overflow-hidden'}>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Wallets</h2>
                    <CreateCashWalletModal userId={userId!}/>
                    <CreateMonobankWalletModal/>
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                    {wallets?.map((wallet, i) =>
                        <WalletCard name={wallet.name} balance={wallet.balance} isBanking={wallet.isBanking}
                                    currencySymbol={wallet.currency.symbol} walletId={wallet.id}
                                    key={wallet.id ?? i}/>)}
                </div>
            </section>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Overview</h2>
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                </div>
            </section>
        </div>
    )
}

