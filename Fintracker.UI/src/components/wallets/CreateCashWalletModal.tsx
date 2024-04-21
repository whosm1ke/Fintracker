import {SubmitHandler, useForm } from "react-hook-form";
import {Wallet, balanceRegisterOptionsForWallet, nameRegisterOptionsForWallet } from "../../entities/Wallet";
import { useState } from "react";
import { Currency } from "../../entities/Currency";
import useCreateWallet from "../../hooks/wallet/useCreateWallet.ts";
import {ActionButton} from "../other/ActionButton.tsx";
import {HiX} from "react-icons/hi";
import currencies from "../../data/currencies.ts";

interface CashWalletModalProps {
    userId: string,
}
const CreateCashWalletModal = ({userId}: CashWalletModalProps) => {

    const {register, handleSubmit, clearErrors, reset, formState: {errors}} = useForm<Wallet>();
    const walletMutation = useCreateWallet();
    const [isOpen, setIsOpen] = useState(false);
    const [selectedCurrency, setSelectedCurrency] = useState<Currency>(currencies[0])

    function handleOpenModal() {
        setIsOpen(p => !p);
    }

    function handleSelectedCurrency(currency: Currency | undefined) {
        if (currency === undefined)
            currency = currencies[0];
        setSelectedCurrency(currency);
    }

    const onSubmit: SubmitHandler<Wallet> = async (model: Wallet) => {
        model.ownerId = userId;
        model.currency = selectedCurrency;
        await walletMutation.mutateAsync(model, {
            onSuccess: () => {
                reset();
                clearErrors("root");
                handleOpenModal();
            }
        });
    };

    return (
        <>
            <ActionButton text={"Add new wallet"} onModalOpen={handleOpenModal}/>
            <div className={`absolute inset-0  flex justify-center items-center
                        ${isOpen ? 'visible bg-black/20' : 'invisible'}`}>
                <div className="bg-white p-4 rounded-md shadow-lg max-w-md mx-auto mt-4">
                    <h2 className="text-2xl font-bold mb-4 flex justify-between">Add Wallet
                        <HiX size={'2rem'} color={'red'} onClick={handleOpenModal}/>
                    </h2>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Name">
                                Name
                            </label>
                            <input
                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="Name"
                                type="text"
                                {...register("name", nameRegisterOptionsForWallet)}
                            />
                            {errors.name && <p className={'text-red-400 italic'}>{errors.name.message}</p>}
                        </div>
                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Balance">
                                Balance
                            </label>
                            <input
                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="Balance"
                                min={1}
                                type="number"
                                {...register("balance", balanceRegisterOptionsForWallet)}
                            />
                            {errors.balance && <p className={'text-red-400 italic'}>{errors.balance.message}</p>}
                        </div>
                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="CurrencyId">
                                Currency
                            </label>
                            <select
                                className="shadow border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="CurrencyId"
                                {...register("currencyId", {
                                    required: "Currency is required for wallet",
                                })}
                                onChange={(e) => handleSelectedCurrency(currencies.find(currency => currency.id === e.target.value))}
                            >
                                {currencies.map((currency) => (
                                    <option key={currency.id} value={currency.id}>
                                        {currency.name} ({currency.symbol})
                                    </option>
                                ))}
                            </select>
                            {errors.currencyId && <p className={'text-red-400 italic'}>{errors.currencyId.message}</p>}
                        </div>
                        <div className="flex items-center justify-between">
                            <button
                                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                                type="submit"
                            >
                                Create Wallet
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
}

export default CreateCashWalletModal;