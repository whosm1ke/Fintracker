import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";

interface WalletInfo {
    walletBalance: number;
    walletChangeForPeriod: number;
    walletExpenseForPeriod: number;
    walletIncomeForPeriod: number;
}

interface WalletInfoStore {
    info: WalletInfo;
    setBalance:(balance: number) => void;
    setChangeForPeriod:(total: number) => void;
    setExpenseForPeriod:(expense: number) => void;
    setIncomeForPeriod:(income: number) => void;
}

const useWalletInfoStore = createWithEqualityFn<WalletInfoStore>((set) => ({
    info: {
        walletBalance: 0,
        walletChangeForPeriod: 0,
        walletExpenseForPeriod: 0,
        walletIncomeForPeriod: 0
    },
    setBalance: (num: number) => set(store => ({info: {...store.info, walletBalance: num}})),
    setChangeForPeriod: (num: number) => set(store => ({info: {...store.info, walletChangeForPeriod: num}})),
    setExpenseForPeriod: (num: number) => set(store => ({info: {...store.info, walletExpenseForPeriod: num}})),
    setIncomeForPeriod: (num: number) => set(store => ({info: {...store.info, walletIncomeForPeriod: num}})),
   
}), shallow);

export default useWalletInfoStore;