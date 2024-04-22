import {Currency} from "./Currency.ts";

export interface MonobankUserInfo {
    name: string;
    accounts: Account[];
}

export interface Account {
    maskedPan: string[];
    iban: string;
    type: string;
    id: string;
    balance: number;
    currencyCode: number
}

export interface MonobankConfiguration {
    accountId: string;
    from: number;
    to?: number;
}

export interface ExtendedMonobankConfiguration extends MonobankConfiguration {
    currency: Currency
}