import {RegisterOptions} from "react-hook-form";
import {Budget} from "./Budget.ts";
import { Transaction } from "./Transaction.ts";
import { FinancialEntity } from "./FinancialEntity.ts";
import { User } from "./User.ts";

export interface Wallet extends FinancialEntity {
    ownerId: string,
    isBanking: boolean;
    bankAccountId: string;
    owner: User;
    budgets: Budget[];
    transactions: Transaction[];
    users: User[];
}

export const nameRegisterOptionsForWallet: RegisterOptions = {
    maxLength: {value: 50, message: "Maximum name for wallet is 50"},
    minLength: {value: 3, message: "Minimum length for wallet name is 3"},
    required: "Name for wallet is required",
}

export const balanceRegisterOptionsForWallet: RegisterOptions = {
    valueAsNumber: true,
    max: {value: 100_000_000_000, message: "Maximum start balance for wallet is 100 billions"},
    required: 'Balance is required'
}