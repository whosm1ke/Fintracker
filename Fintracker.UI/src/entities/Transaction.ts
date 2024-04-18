import {Wallet} from "./Wallet.ts";

export interface Transaction extends BaseEntity {
    walletId: string;
    userId: string;
    amount: number;
    note?: string;
    label?: string;
    date: Date;
    isBankTransaction: boolean;
    category: Category;
    currency: Currency;
    wallet: Wallet;
    user: User;
}

export interface GroupedTransactionByDate {
    date: Date;
    transactions: Transaction[];
}