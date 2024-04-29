import { Currency } from "./Currency";

export interface FinancialEntity extends BaseEntity {
    name: string;
    balance: number;
    startBalance: number
    totalSpent: number;
    currencyId: string;
    currency: Currency;
}
