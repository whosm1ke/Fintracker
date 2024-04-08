interface Budget extends FinancialEntity {
    categories: Category[];
    startDate: Date;
    endDate: Date;
    userId: string;
    isPublic: boolean;
    user: User;
    wallet: Wallet;
}