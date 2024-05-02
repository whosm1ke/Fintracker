import Spinner from "../other/Spinner.tsx";
import { useState} from "react";
import {Currency} from "../../entities/Currency.ts";
import {Category} from "../../entities/Category.ts";
import {Wallet} from "../../entities/Wallet.ts";
import CreateBudgetModalBase from "./CreateBudgetModalBase.tsx";
import useWallet from "../../hooks/wallet/useWallet.ts";


interface CreateBudgetModalWalletPersonalProps {
    userId: string,
    walletId: string;
    categories: Category[]
}


const CreateBudgetModalWalletPersonal = ({userId, walletId, categories}: CreateBudgetModalWalletPersonalProps) => {
    const {data: wallet} = useWallet(walletId);
    const [selectedCurrency, setSelectedCurrency] = useState<Currency | undefined>(undefined)
    const [selectedCategories, setSelectedCategories] = useState<Category[]>([])


    if (wallet?.response === undefined) return <Spinner/>

    console.log("categories: ", categories)

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
        <CreateBudgetModalBase userId={userId} walletId={walletId} handleSelectedWallet={() => {
        }}
                               handleSelectedCurrency={handleSelectedCurrency}
                               handleToggleCategoryId={handleToggleCategoryId}
                               handleSelectAllCategories={handleSelectAllCategories}
                               refreshCategories={() => setSelectedCategories([])}
                               categoriesToShow={categories}
                               isActionButtonActive={wallet.response.ownerId === userId} showWallets={false}
                               walletsToShow={[]}
                               selectedWallet={{} as Wallet} selectedCurrency={selectedCurrency}
                               showAddWalletBtn={false}
                               selectedCategories={selectedCategories}/>
    )
}


export default CreateBudgetModalWalletPersonal;




