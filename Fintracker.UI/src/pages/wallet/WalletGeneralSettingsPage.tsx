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
// @ts-ignore
import logo from "../../assets/logo.png";
import useUserStore from "../../stores/userStore.ts";


export default function WalletGeneralSettingsPage() {
    const currenctUserId = useUserStore(x => x.getUserId());
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
    const [userIdsToDelete, setUserIdsToDelete] = useState<string[]>([]);
    const [selectedName, setSelectedName] = useState("")
    const [selectedStartBalance, setSelectedStartBalance] = useState(walletResponse?.response?.startBalance)
    const [isEmailValid, setIsEmailValid] = useState(false);
    const [isEmailAlreadyAdded, setIsEmailAlreadyAdded] = useState(false);
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
            setUserIdsToDelete(prev => [...prev, user.id])
            setSelectedUsers(prev => prev?.filter(c => c.id !== user.id));
        } else {
            setUserIdsToDelete(prev => prev?.filter(id => id !== user.id))
            setSelectedUsers(prev => [...prev, user!]);
        }
    };

    const handleSelectAllUsers = () => {
        if (selectedUsers.length === wallet.users.length) {
            setSelectedUsers([]);
            setUserIdsToDelete(wallet.users.map(u => u.id))
        } else {
            setSelectedUsers(wallet.users);
            setUserIdsToDelete([])
        }
    }


    const handleEmailChange = (email: string) => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const regResult = emailRegex.test(email);

        if (wallet.users.find(u => u.email === email)) {
            setIsEmailAlreadyAdded(true)
            return;
        }

        setIsEmailAlreadyAdded(false)
        setIsEmailValid(regResult)
    }

    const handleInviteUser = async () => {
        if (!emailInputRef.current) return;

        if (sessionStorage.getItem('inviteEmail') === emailInputRef.current.value) {
            setIsEmailAlreadyAdded(true);
            return;
        }

        await inviteUser.mutateAsync({
            walletId: wallet.id,
            email: emailInputRef.current.value,
            urlCallback: "confirm-invite"
        })

        sessionStorage.setItem('inviteEmail', emailInputRef.current.value);
        emailInputRef.current.value = "";
        setIsEmailValid(false);
    }

    const onSumbit: SubmitHandler<UpdateWalletDTO> = async (model: UpdateWalletDTO) => {
        if (selectedCurrency) {
            model.currencyId = selectedCurrency.id;
            model.currency = selectedCurrency;
            clearErrors("currencyId");

        } else {
            setError("currencyId", {message: 'Currency os required for wallet'})
            return;
        }

        model.userIds = userIdsToDelete;
        model.id = wallet.id;
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
        <div className={'mx-auto px-10 sm:px-12 md:px-24 lg:px-36 py-7 flex flex-col gap-y-10'}>
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
                                    value={selectedName || wallet.name}
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
                                    value={selectedStartBalance || wallet.startBalance}
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
                            {(wallet.users.length !== 0 && currenctUserId === wallet.ownerId) &&
                                <div className={'basis-1/2 flex flex-col'}>
                                    <label>Wallet members</label>
                                    <div
                                        {...register("userIds")}>
                                        <MultiSelectDropDownMenu items={wallet.users} ItemComponent={UserItem}
                                                                 heading={"Members"}
                                                                 onAllItemsSelected={handleSelectAllUsers}
                                                                 onItemSelected={handleToggleUser}
                                                                 selectedItems={selectedUsers}/>
                                        {errors.userIds &&
                                            <p className={'text-red-400 italic'}>{errors.userIds.message}</p>}
                                    </div>
                                </div>}
                        </div>
                        <div className={'mt-10 w-full sm:w-1/2'}>
                            <button
                                className={updateWalletBtnClassname}
                                disabled={disabled}
                                type={'submit'}>Update wallet
                            </button>
                        </div>
                    </form>
                </div>
            </section>
            {currenctUserId === wallet.ownerId && <section className={'flex flex-col gap-y-5'}>
                <h2 className={'font-semibold text-md'}>Wallet members</h2>
                <div className={'flex flex-col gap-y-1'}>
                    <label htmlFor="invite" className={'flex justify-between w-full'}>
                        Invite new member
                        {isEmailAlreadyAdded &&
                            <span className={'text-red-400 italic'}>You have already invited that user</span>}
                    </label>
                    <div className={'flex flex-col sm:flex-row gap-5'}>
                        <input
                            className="shadow appearance-none border rounded sm:w-1/2 py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="invite"
                            type="email"
                            ref={emailInputRef}
                            onChange={e => handleEmailChange(e.target.value)}
                        />
                        <div className={'sm:w-1/2'}>
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
                <div className={'overflow-hidden flex flex-col gap-4'}>
                    <div className={'w-1/2 flex items-center'}>
                        <img src={wallet.owner.userDetails?.avatar || logo} alt="Avatar"
                             className="w-12 h-12 rounded-lg"/>
                        <div className={'flex flex-col'}>
                            <p className={'text-[17px] ml-3'}>{wallet.owner.userName}
                                <span
                                    className={'hidden sm:inline text-sm text-white bg-red-300 ml-3 px-1 py-1 rounded'}>owner</span>
                            </p>
                            <p className={'hidden sm:block text-md ml-3'}>{wallet.owner.email}</p>
                        </div>
                    </div>
                    {wallet.users.map(u =>
                        <div className={'w-1/2 flex items-center'}>
                            <img src={u.userDetails?.avatar || logo} alt="Avatar"
                                 className="w-12 h-12 rounded-lg"/>
                            <div className={'flex flex-col'}>
                                <p className={'text-[17px] ml-3'}>{u.userName}
                                    <span
                                        className={'hidden sm:inline text-sm text-white bg-orange-300 ml-3 px-1 py-1 rounded'}>member</span>
                                </p>
                                <p className={'hidden sm:block text-md ml-3'}>{u.email}</p>
                            </div>
                        </div>
                    )}
                </div>
            </section>}
        </div>
    )
}