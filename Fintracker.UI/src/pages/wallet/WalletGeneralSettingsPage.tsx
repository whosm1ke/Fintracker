import {useParams} from "react-router-dom";
import useWallet from "../../hooks/wallet/useWallet.ts";
import Spinner from "../../components/other/Spinner.tsx";
import {SubmitHandler, useForm} from "react-hook-form";
import {balanceRegisterOptionsForWallet, nameRegisterOptionsForWallet} from "../../entities/Wallet.ts";
import {useEffect, useRef, useState} from "react";
import {Currency} from "../../entities/Currency.ts";
import SingleSelectDropDownMenu from "../../components/other/SingleSelectDropDownMenu.tsx";
import currencies from "../../data/currencies.ts";
import CurrencyItem from "../../components/currencies/CurrencyItem.tsx";
import useUpdateWallet, {UpdateWalletDTO} from "../../hooks/wallet/useUpdateWallet.ts";
import {User} from "../../entities/User.ts";
import MultiSelectDropDownMenu from "../../components/other/MultiSelectDropDownMenu.tsx";
import UserItem from "../../components/auth/UserItem.tsx";
import useInviteUser from "../../hooks/auth/useInviteUser.ts";


export default function WalletGeneralSettingsPage() {
    const {walletId} = useParams();
    const {data: walletResponse} = useWallet(walletId!);
    const {
        register,
        handleSubmit,
        formState: {errors},
        setError,
        clearErrors,
    } = useForm<UpdateWalletDTO>();
    const [selectedCurrency, setSelectedCurrency] = useState<Currency | undefined>(undefined);
    const [selectedUsers, setSelectedUsers] = useState<User[]>([]);
    const [selectedName, setSelectedName] = useState("")
    const [selectedStartBalance, setSelectedStartBalance] = useState(walletResponse?.response?.startBalance)
    const [isEmailValid, setIsEmailValid] = useState(false);
    const inviteBtnRef = useRef<HTMLButtonElement>(null);
    const emailInputRef = useRef<HTMLInputElement>(null);
    const walletUpdateMutation = useUpdateWallet();
    const inviteUser = useInviteUser();
    
    useEffect(() => {
        if (walletResponse && walletResponse.response) {
            const wallet = walletResponse.response;
            setSelectedName(wallet.name);
            setSelectedStartBalance(wallet.startBalance)
            setSelectedCurrency(wallet.currency)
            setSelectedUsers(wallet.users)
        }
    }, [walletResponse]);

    if (!walletResponse || !walletResponse.response) return <Spinner/>
    const wallet = walletResponse.response;

    const handleNewCurrencySelected = (curr: Currency) => {
        setSelectedCurrency(curr);
    }
    const handleWalletNameChange = (name: string) => {
        setSelectedName(name)
    }
    const handleWalletStartBalanceChange = (startBalance: number) => {
        setSelectedStartBalance(startBalance)
    }

    const handleToggleUser = (user: User) => {

        if (selectedUsers?.includes(user)) {
            setSelectedUsers(prev => prev?.filter(c => c.id !== user.id));
        } else {
            setSelectedUsers(prev => [...prev, user!]);
        }
    };

    const handleSelectAllUsers = () => {
        if (selectedUsers.length === wallet.users.length) {
            setSelectedUsers([]);
        } else {
            setSelectedUsers(wallet.users);
        }
    }

    const handleEmailChange = (email: string) => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const regResult = emailRegex.test(email);

        setIsEmailValid(regResult)
    }

    const handleInviteUser = async () => {
        if (!emailInputRef.current) return;
        await inviteUser.mutateAsync({
            walletId: wallet.id,
            email: emailInputRef.current.value,
            urlCallback: "confirm-invite"
        })
        console.log(emailInputRef.current.value)
    }

    const onSumbit: SubmitHandler<UpdateWalletDTO> = async (model: UpdateWalletDTO) => {
        if (selectedCurrency) {
            model.currencyId = selectedCurrency.id;
            clearErrors("currencyId");

        } else {
            setError("currencyId", {message: 'Currency os required for wallet'})
            return;
        }

        console.log(model)


        await walletUpdateMutation.mutateAsync(model)
    }

    const disabled = selectedCurrency?.id === wallet.currency.id &&
        selectedName === wallet.name &&
        selectedStartBalance === wallet.startBalance &&
        selectedUsers.length === wallet.users.length;

    const updateWalletBtnClassname = disabled ? 'bg-gray-300 p-2 text-center w-full rounded-md text-sm font-semibold text-gray-500' :
        'bg-green-400 p-2 text-center w-full rounded-md text-sm font-semibold text-white shadow-md shadow-green-600';

    const inviteUserBtnClassname = !isEmailValid ? 'bg-gray-300 p-2 text-center w-full rounded-md text-sm font-semibold text-gray-500' :
        'bg-green-400 p-2 text-center w-full rounded-md text-sm font-semibold text-white shadow-md shadow-green-600';

    return (
        <div className={'mx-auto px-10 py-7 flex flex-col gap-y-10'}>
            <section className={'flex flex-col gap-y-10'}>
                <h2 className={'text-lg font-semibold'}>
                    General information
                </h2>
                <div className={''}>
                    <form onSubmit={handleSubmit(onSumbit)} className={''}>
                        <div className={'grid grid-cols-1 sm:grid-cols-2 gap-4'}>
                            <div className={'flex flex-col'}>
                                <label htmlFor="name">Wallet name</label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="name"
                                    type="text"
                                    defaultValue={selectedName}
                                    {...register("name", nameRegisterOptionsForWallet)}
                                    onChange={e => handleWalletNameChange(e.target.value)}
                                />
                                {errors.name && <p className={'text-red-400 italic'}>{errors.name.message}</p>}
                            </div>
                            <div className={'flex flex-col'}>
                                <label htmlFor="startBalance">Start balance</label>
                                <input
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="startBalance"
                                    type="number"
                                    step={0.01}
                                    defaultValue={selectedStartBalance}
                                    {...register("startBalance", balanceRegisterOptionsForWallet)}
                                    onChange={e => handleWalletStartBalanceChange(e.target.valueAsNumber)}
                                />
                                {errors.startBalance &&
                                    <p className={'text-red-400 italic'}>{errors.startBalance.message}</p>}
                            </div>
                            <div className={'flex flex-col'}>
                                <label>Wallet currency</label>
                                <div
                                    {...register("currencyId")}>
                                    <SingleSelectDropDownMenu items={currencies} ItemComponent={CurrencyItem}
                                                              heading={"Currency"}
                                                              onItemSelected={handleNewCurrencySelected}
                                                              defaultSelectedItem={selectedCurrency}/>
                                </div>
                                {errors.currencyId &&
                                    <p className={'text-red-400 italic'}>{errors.currencyId.message}</p>}
                            </div>
                            {wallet.users.length !== 0 && <div className={'flex flex-col'}>
                                <label>Wallet members</label>
                                <div
                                    {...register("userIds")}>
                                    <MultiSelectDropDownMenu items={wallet.users} ItemComponent={UserItem}
                                                             heading={"Members"}
                                                             onAllItemsSelected={handleSelectAllUsers}
                                                             onItemSelected={handleToggleUser}
                                                             selectedItems={selectedUsers}/>
                                </div>
                                {errors.userIds && <p className={'text-red-400 italic'}>{errors.userIds.message}</p>}
                                <div className="flex gap-4">
                                    <label className="block text-sm font-bold" htmlFor="deleteTrans">
                                        Delete members transactions?
                                    </label>
                                    <div className="relative">
                                        <input
                                            className="h-6 w-6 border-blue-500 rounded-full cursor-pointer checked:border-transparent 
                                        checked:bg-blue-500"
                                            id="deleteTrans"
                                            type="checkbox"
                                            defaultChecked={false}
                                            {...register("deleteUserTransaction")}
                                        />
                                    </div>
                                </div>
                            </div>}
                        </div>
                        <div className={'mt-10 w-1/2'}>
                            <button
                                className={updateWalletBtnClassname}
                                disabled={disabled}
                                type={'submit'}>Update wallet
                            </button>
                        </div>
                    </form>
                </div>
            </section>
            <section className={'flex flex-col gap-y-5'}>
                <h2 className={'font-semibold text-md'}>Wallet members</h2>
                <div className={'flex flex-col gap-y-1'}>
                    <label htmlFor="invite">
                        Invite new member
                    </label>
                    <div className={'flex gap-5'}>
                        <input
                            className="shadow appearance-none border rounded w-1/2 py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="invite"
                            type="email"
                            ref={emailInputRef}
                            onChange={e => handleEmailChange(e.target.value)}
                        />
                        <div className={'w-1/2'}>
                            <button
                                className={inviteUserBtnClassname}
                                disabled={!isEmailValid}
                                ref={inviteBtnRef}
                                type={'submit'}
                                onClick={handleInviteUser}
                            >Invite
                            </button>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    )
}