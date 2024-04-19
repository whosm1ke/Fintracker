import { Currency } from "./Currency";

export interface FinancialEntity extends BaseEntity {
    name: string;
    balance: number;
    totalSpent: number;
    currencyId: string;
    currency: Currency;
}
