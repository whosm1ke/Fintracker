interface User extends BaseEntity {
    email: string;
    userDetails?: UserDetails;
    budgets: Budget[];
    memberWallets: Wallet[];
    ownedWallets: Wallet[];
}