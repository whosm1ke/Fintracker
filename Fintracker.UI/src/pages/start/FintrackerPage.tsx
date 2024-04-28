import useWallets from "../../hooks/wallet/useWallets";
import useUserStore from "../../stores/userStore";
import Spinner from "../../components/other/Spinner.tsx";
import CreateCashWalletModal from "../../components/wallets/CreateCashWalletModal.tsx";
import WalletCard from "../../components/wallets/WalletCard.tsx";
import CreateMonobankWalletModal from "../../components/wallets/CreateMonobankWalletModal.tsx";
import {useLocation} from "react-router-dom";
import {motion} from "framer-motion";
import WalletsDateFilter from "../../components/wallets/WalletsDateFilter.tsx";
import WalletOverviewList from "../../components/wallets/WalletOverviewList.tsx";
import {useGetUser} from "../../hooks/auth/useUser.ts";
import WalletsOtherFilters from "../../components/wallets/WalletsOtherFilters.tsx";


export default function FintrackerPage() {
    const userId = useUserStore(x => x.getUserId());
    const {data: wallets} = useWallets(userId || 'no-user');
    const location = useLocation();
    const {data: user} = useGetUser(userId!);
    const shouldBounce = location.state !== null
    
    
    if (!wallets || !user || !user.response) return <Spinner/>

    return (
        <div className={'container mx-auto p-4'}>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-5 items-start sm:flex-row sm:items-center'}>
                    <h2 className={'text-2xl font-[500]'}>Wallets</h2>
                    <motion.div
                        animate={shouldBounce ? {scale: [1, 1.5, 1]} : {}}
                    >
                        <CreateCashWalletModal userId={userId!}/>
                    </motion.div>
                    
                    <CreateMonobankWalletModal/>
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                    {wallets.map((wallet, i) =>
                        <WalletCard wallet={wallet} key={wallet.id ?? i}/>)}
                </div>
            </section>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start justify-between sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Overview</h2>
                    <WalletsDateFilter/>
                </div>
                <div className={'flex flex-col gap-y-5'}>
                    <WalletsOtherFilters wallets={wallets}/>
                    <WalletOverviewList globalCurrency={user.response.globalCurrency}/>
                </div>
            </section>
        </div>
    )
}

//