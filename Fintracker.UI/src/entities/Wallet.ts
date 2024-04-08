interface Wallet extends FinancialEntity {
    ownerId: string,
    isBanking: boolean;
    bankAccountId: string;
    owner: User;
    budgets: Budget[];
    transactions: Transaction[];
    users: User[];
}