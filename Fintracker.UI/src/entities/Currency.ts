interface Currency extends BaseEntity {
    name: string;
    symbol: string;
    code: number;
}

interface ConvertCurrency {
    from: string;
    to: string;
    amount: number;
    value: number
}