import useWallets from "../../hooks/wallet/useWallets.ts";
import {useState} from "react";
import {Wallet} from "../../entities/Wallet.ts";
import {Currency} from "../../entities/Currency.ts";
import {Category} from "../../entities/Category.ts";
import CreateBudgetModalBase from "./CreateBudgetModalBase.tsx";

interface CreateBudgetModalGlobalProps {
    userId: string,
    categories: Category[]
}


export default function CreateBudgetModalGlobal ({userId, categories}: CreateBudgetModalGlobalProps) {
    const {data: wallets} = useWallets(userId)
    const [selectedWallet, setSelectedWallet] = useState<Wallet | undefined>()
    const [selectedCurrency, setSelectedCurrency] = useState<Currency | undefined>(undefined)
    const [selectedCategories, setSelectedCategories] = useState<Category[]>([])

    if (wallets === undefined) return null;
    const filteredWallets = wallets.filter(w => w.ownerId === userId)
    const handleSelectedWallet = (wallet: Wallet) => setSelectedWallet(wallet);

    function handleSelectedCurrency(currency: Currency | undefined) {
        setSelectedCurrency(currency);
    }

    const handleToggleCategoryId = (category: Category) => {

        if (selectedCategories.includes(category)) {
            setSelectedCategories(prev => prev.filter(c => c.id !== category.id));
        } else {
            setSelectedCategories(prev => [...prev, category!]);
        }
    };

    const handleSelectAllCategories = () => {
        if (selectedCategories.length === categories.length) {
            setSelectedCategories([]);
        } else {
            setSelectedCategories(categories);
        }
    }


    return (
        <CreateBudgetModalBase userId={userId} walletId={undefined} handleSelectedWallet={handleSelectedWallet}
                               handleSelectedCurrency={handleSelectedCurrency}
                               handleToggleCategoryId={handleToggleCategoryId}
                               handleSelectAllCategories={handleSelectAllCategories}
                               refreshCategories={() => setSelectedCategories([])}
                               categoriesToShow={categories} isActionButtonActive={true} showWallets={true}
                               walletsToShow={filteredWallets}
                               selectedWallet={selectedWallet} selectedCurrency={selectedCurrency}
                               showAddWalletBtn={filteredWallets.length === 0}
                               selectedCategories={selectedCategories}/>
    )
}
