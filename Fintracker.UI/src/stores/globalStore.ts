import {Currency} from "../entities/Currency";
import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";

interface GlobalStore {
    walletCurrency: Currency;
    setWalletCurrency: (c: Currency) => void;
}

const useGlobalStore = createWithEqualityFn<GlobalStore>(set => ({
    walletCurrency: {
        code: 980,
        id: 'ff659ed4-aee0-4a54-cee0-04095560520d',
        symbol: "UAH",
        name: "Hryvnia"
    },
    setWalletCurrency: (c: Currency) => set(({walletCurrency: c})),
}), shallow);


export default useGlobalStore;