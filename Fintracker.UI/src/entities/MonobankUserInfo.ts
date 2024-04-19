export interface MonobankUserInfo {
    name: string;
    accounts: Account[];
}

export interface Account {
    maskedPan: string[];
    iban: string;
    id: string;
    balance: number;
}

export interface MonobankConfiguration {
    accountId: string;
    from: number;
    to?: number;
}