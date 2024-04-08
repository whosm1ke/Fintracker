interface FinancialEntity extends BaseEntity {
    name: string;
    balance: number;
    totalSpent: number;
    currencyId: string;
    currency: Currency;
}
