// @ts-ignore
import logo from '../../src/assets/logo.png'
import useWallets from "../hooks/useWallets.ts";
import useUserStore from "../stores/userStore.ts";
import {IoWalletSharp} from "react-icons/io5";
import {NavLink} from "react-router-dom";
import {useState} from "react";
import {motion} from 'framer-motion';
import currencies from "../data/currencies.ts";
import {SubmitHandler, useForm} from "react-hook-form";
import useCreateCashWallet from "../hooks/useCreateWallet.ts";
import {HiX} from "react-icons/hi";
import {balanceRegisterOptions, nameRegisterOptions, Wallet} from "../entities/Wallet.ts";
import UseCreateMonoWallet, {MonoWalletToken} from "../hooks/useCreateMonoWallet.ts";
import {useMonoUserInfo} from "../hooks/useMonoUserInfo.ts";


function calculateTotalBalance(wallets: Wallet[]): number {
    let totalBalance = 0;
    wallets.forEach(wallet => {
        totalBalance += wallet.balance;
    });
    return totalBalance;
}

export default function FintrackerPage() {
    const userId = useUserStore(x => x.getUserId());
    const {data: wallets} = useWallets(userId || 'no-user');
    const totalBalance = calculateTotalBalance(wallets || []);
    return (
        <div className={'container mx-auto p-4 overflow-hidden'}>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Wallets</h2>
                    <CreateCashWalletModal userId={userId!}/>
                    <CreateMonobankWalletModal/>
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                    {wallets?.map((wallet, i) =>
                        <WalletCard name={wallet.name} balance={wallet.balance} isBanking={wallet.isBanking}
                                    currencySymbol={wallet.currency.symbol} walletId={wallet.id}
                                    key={wallet.id ?? i}/>)}
                </div>
            </section>
            <section className={'space-y-5 mt-10'}>
                <div className={'flex flex-col gap-y-5 items-start sm:flex-row sm:items-center gap-x-10'}>
                    <h2 className={'text-2xl font-[500]'}>Overview</h2>
                </div>
                <div className={'grid gap-x-10 gap-y-5 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4'}>
                </div>
            </section>
        </div>
    )
}

interface WalletCardProps {
    name: string,
    balance: number,
    isBanking: boolean,
    currencySymbol: string,
    walletId: string
}

const WalletCard = ({name, balance, isBanking, currencySymbol, walletId}: WalletCardProps) => {

    const isPositiveBalance = balance > 0;
    const formatedBalance: string = Math.abs(balance).toLocaleString();
    const balanceText: string = isPositiveBalance ? `+ ${formatedBalance} ${currencySymbol}` :
        `- ${formatedBalance} ${currencySymbol}`;

    return (
        <NavLink to={`/wallet/${walletId}`}
                 className={'flex flex-col sm:flex-row space-y-3 sm:space-y-0 space-x-0 sm:space-x-3 ' +
                     'p-4 bg-slate-100 rounded-lg shadow w-full'}>
    <span className={'self-center'}>
        <IoWalletSharp color={isBanking ? 'orange' : 'green'} size={'2rem'}/>
    </span>
            <div className={'px-4 py-2'}>
                <h4 className={'text-lg'}>{name}</h4>
                <span>{isBanking ? 'Monobank' : 'Cash'}</span>
                <p className={isPositiveBalance ? "text-green-400 text-xl" : 'text-red-500 text-xl'}>{balanceText}</p>
            </div>
        </NavLink>

    )
}

interface ActionButtonProps {
    text: string,
    onModalOpen: () => void
}

const ActionButton = ({text, onModalOpen}: ActionButtonProps) => {
    return (
        <>
            <motion.button
                onClick={() => onModalOpen()}
                whileHover={{scale: 1.1}}
                whileTap={{scale: 0.9}}
                className={'text-xl bg-green-400 px-4 py-2 text-white rounded-lg shadow-lg'}>{text}
            </motion.button>
        </>
    )
}

interface CashWalletModalProps {
    userId: string,
}

const CreateCashWalletModal = ({userId}: CashWalletModalProps) => {

    const {register, handleSubmit, clearErrors, reset, formState: {errors}} = useForm<Wallet>();
    const walletMutation = useCreateCashWallet();
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
                                {...register("name", nameRegisterOptions)}
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
                                {...register("balance", balanceRegisterOptions)}
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

const CreateMonobankWalletModal = () => {


    const [monouserInfo, setMonouserInfo] = useState<MonobankUserInfo>({} as MonobankUserInfo);
    const [step, setStep] = useState(0);

    function handleNextStep() {
        setStep(p => p + 1);
    }

    function resetStep() {
        setStep(0);
    }

    function handleMonouserInfo(user: MonobankUserInfo) {
        setMonouserInfo(user);
    }

    return (
        <>
            <ActionButton text={"Add new monobank wallet"} onModalOpen={handleNextStep}/>
            {step === 1 && <MonobankModalStep1 handleOpenModal={resetStep}
                                               handleMonouserInfo={handleMonouserInfo}
                                               handleNextStep={handleNextStep}/>}
            {step === 2 && <MonobankModalStep2 userInfo={monouserInfo}
                                               handleOpenModal={resetStep}/>}
        </>
    );
}

interface MonobankModalStep1Props {
    handleOpenModal: () => void;
    handleNextStep: () => void;
    handleMonouserInfo: (user: MonobankUserInfo) => void
}

const MonobankModalStep1 = ({handleOpenModal, handleNextStep, handleMonouserInfo}: MonobankModalStep1Props) => {
    const {handleSubmit, register, setError, reset, clearErrors, formState: {errors}} = useForm<MonoWalletToken>({
        mode: 'onSubmit'
    });

    const xTokenMutation = useMonoUserInfo();
    const onSubmit: SubmitHandler<MonoWalletToken> = async (model: MonoWalletToken) => {
        const mutationResponse = await xTokenMutation.mutateAsync(model);

        if (mutationResponse.hasError) {
            setError("xToken", {message: "Invalid token"})
        } else {
            if (mutationResponse.response) {
                handleMonouserInfo(mutationResponse.response);
                handleNextStep();
            }
        }
    };

    return (
        <div className={`absolute inset-0  flex justify-center items-center visible bg-black/20`}>
            <div className="bg-white p-4 rounded-md shadow-lg max-w-md w-1/2 mx-auto mt-4">
                <h2 className="text-2xl font-bold mb-4 flex justify-between">Add monobank
                    <HiX size={'2rem'} color={'red'} onClick={() => {
                        reset();
                        clearErrors("root")
                        handleOpenModal()
                    }}/>
                </h2>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-4">
                        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="xToken">
                            Your monobank token
                        </label>
                        <input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="xToken"
                            type="text"
                            {...register("xToken", {required: 'Token is required'})}
                        />
                        {errors.xToken && <p className={'text-red-400 italic'}>{errors.xToken.message}</p>}
                    </div>
                    <button
                        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                        type="submit"
                    >
                        Send token
                    </button>
                </form>
            </div>
        </div>
    )
}

interface MonobankModalStep2Props {
    userInfo: MonobankUserInfo;
    handleOpenModal: () => void;
}

const MonobankModalStep2 = ({userInfo, handleOpenModal}: MonobankModalStep2Props) => {
    const {handleSubmit, register, reset, clearErrors, setError, formState: {errors}} = useForm<MonobankConfiguration>({
        mode: 'onSubmit'
    });

    const monobankMutation = UseCreateMonoWallet();
    const onSubmit: SubmitHandler<MonobankConfiguration> = async (model: MonobankConfiguration) => {
        const fromUnix = new Date(model.from).getTime() / 1000;
        const toUnix = model.to ? new Date(model.to).getTime() / 1000 : undefined;

        const unixModel = {
            ...model,
            from: fromUnix,
            to: toUnix,
        };

        const monobankResponse = await monobankMutation.mutateAsync(unixModel);

        if (monobankResponse.hasError) {
            setError("root", {message: 'Something went wrong'})
        } else {
            handleOpenModal()
            window.location.reload();
        }
    };

    return (
        <div className={`absolute inset-0  flex justify-center items-center visible bg-black/20`}>
            <div className="bg-white p-4 rounded-md shadow-lg max-w-md w-1/2 mx-auto mt-4">
                <h2 className="text-2xl font-bold mb-4 flex justify-between">{userInfo.name}
                    <HiX size={'2rem'} color={'red'} onClick={() => {
                        reset();
                        clearErrors("root")
                        handleOpenModal()
                    }}/>
                </h2>
                {errors.root && <p className={'text-red-400 italic'}>{errors.root.message}</p>}
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-4">
                        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="accounntId">
                            Your accounts
                        </label>
                        <select
                            className="shadow border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="accounntId"
                            {...register("accountId", {
                                required: "Select the account",
                            })}
                        >
                            {userInfo.accounts.map((acc) => (
                                <option key={acc.id} value={acc.id}>
                                    {`${acc.maskedPan} \t\t (${acc.balance / 100})`}
                                </option>
                            ))}

                        </select>
                    </div>
                    <div className="mb-4">
                        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="startDate">
                            Start Date
                        </label>
                        <input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="startDate"
                            type="date"
                            {...register("from", {required: 'Start date is required'})}
                        />
                        {errors.from && <p className={'text-red-400 italic'}>{errors.from.message}</p>}
                    </div>

                    <div className="mb-4">
                        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="endDate">
                            End Date
                        </label>
                        <input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="endDate"
                            type="date"
                            defaultValue={new Date().toLocaleDateString()} // Set default value to today's date
                            {...register("to")}
                        />
                    </div>
                    <button
                        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                        type="submit"
                    >
                        Connect wallet
                    </button>
                </form>
            </div>
        </div>
    );
};

interface OverviewCardProps {
    text: string;
    balance: number;
    currencySymbol: string
}

const OverviewCard = ({balance, text, currencySymbol}: OverviewCardProps) => {
    const isPositiveBalance = balance > 0;
    const formatedBalance: string = Math.abs(balance).toLocaleString();
    const balanceText: string = isPositiveBalance ? `+ ${formatedBalance} ${currencySymbol}` :
        `- ${formatedBalance} ${currencySymbol}`;


    return (
        <div className={'flex flex-col gap-y-3'}>
            <p className={'text-neutral-900 text-lg'}>{text}</p>
            <p className={`text-lg ${isPositiveBalance ? "text-green-400" : 'text-red-500'}`}>{balanceText}</p>
        </div>
    )
}

