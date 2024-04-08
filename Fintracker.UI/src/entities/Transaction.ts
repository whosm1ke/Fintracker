interface Transaction extends BaseEntity {
    walletId: string;
    userId: string;
    amount: number;
    note?: string;
    label?: string;
    createdAt: Date;
    isBankTransaction: boolean;
    category: Category;
    currency: Currency;
    wallet: Wallet;
    user: User;
}