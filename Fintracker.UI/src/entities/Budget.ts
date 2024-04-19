import { Category } from "./Category.ts";
import { FinancialEntity } from "./FinancialEntity.ts";
import { User } from "./User.ts";
import {Wallet} from "./Wallet.ts";
import {RegisterOptions} from "react-hook-form";

export interface Budget extends FinancialEntity {
    categories: Category[];
    startDate: Date;
    endDate: Date;
    userId: string;
    isPublic: boolean;
    user: User;
    wallet: Wallet;
    walletId: string;
}

export const nameRegisterOptionsForBudget: RegisterOptions = {
    maxLength: {value: 50, message: "Maximum name for budget is 50"},
    minLength: {value: 3, message: "Minimum length for budget name is 3"},
    required: "Name for budget is required",
}

export const balanceRegisterOptionsForBudget: RegisterOptions = {
    valueAsNumber: true,
    max: {value: 100_000_000_000, message: "Maximum balance for budget is 100 billions"},
    required: 'Balance is required'
}