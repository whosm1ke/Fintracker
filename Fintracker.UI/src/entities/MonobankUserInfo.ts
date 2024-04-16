interface MonobankUserInfo {
    name: string;
    accounts: Account[];
}

interface Account {
    maskedPan: string[];
    iban: string;
    id: string;
    balance: number;
}

interface MonobankConfiguration {
    accountId: string;
    from: number;
    to?: number;
}