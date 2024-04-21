import { Category } from "./Category.ts";
import { Currency } from "./Currency.ts";
import { User } from "./User.ts";
import {Wallet} from "./Wallet.ts";
import {RegisterOptions} from "react-hook-form";

export interface Transaction extends BaseEntity {
    walletId: string;
    userId: string;
    currencyId?: string;
    categoryId?: string;
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

export const amountRegisterForTransaction: RegisterOptions = {
    max: {value: 100_000_000_000, message: "Maximum amount for transaction is 100 billions"},
    min: {value: 0.01, message: "Minimum amount for transaction is 0.01"},
    required: "Name for wallet is required",
}

export const labelRegisterForTransaction: RegisterOptions = {
    maxLength: {value: 15, message: "Maximum label length is 15 characters"},
    minLength: {value: 3, message: "Minimum label length is 3 characters"},
    required: false,
}

export const noteRegisterForTransaction: RegisterOptions = {
    maxLength: {value: 80, message: "Maximum note length is 80 characters"},
    minLength: {value: 3, message: "Minimum note length is 3 characters"},
    required: false,
}

export const dateRegisterForTransaction: RegisterOptions = {
    required: "Date for transaction is required",
    validate: (_value, formValues) => {
        const today = new Date();
        today.setHours(0, 0, 0, 0); // Set the time to the start of the day
        const formDate = new Date(formValues.date);
        formDate.setHours(0, 0, 0, 0); // Also set the time of formDate to the start of the day
        if(formDate > today)
            return "Date can not be in future";
        return true;
    }
}


