import {RegisterOptions} from "react-hook-form";

export interface Wallet extends FinancialEntity {
    ownerId: string,
    isBanking: boolean;
    bankAccountId: string;
    owner: User;
    budgets: Budget[];
    transactions: Transaction[];
    users: User[];
}

export const nameRegisterOptions: RegisterOptions = {
    maxLength: {value: 50, message: "Maximum name for wallet is 50"},
    minLength: {value: 3, message: "Minimum length for wallet name is 3"},
    required: "Name for wallet is required",
}

export const balanceRegisterOptions: RegisterOptions = {
    valueAsNumber: true,
    max: {value: 100_000_000_000, message: "Maximum balance for wallet is 100 billions"},
    required: 'Balance is required'
}