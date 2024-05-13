import {useState} from "react";
import {Account, ExtendedMonobankConfiguration, MonobankUserInfo} from "../../entities/MonobankUserInfo";
import {HiX} from "react-icons/hi";
import {SubmitHandler, useForm} from "react-hook-form";
import {ActionButton} from "../other/ActionButton";
import {useMonoUserInfo} from "../../hooks/wallet/useMonoUserInfo";
import useCreateMonoWallet, {MonoWalletToken} from "../../hooks/wallet/useCreateMonoWallet";
import SingleSelectDropDownMenu from "../other/SingleSelectDropDownMenu.tsx";
import BankAccountItem from "../other/BankAccountItem.tsx";
import currencies from "../../data/currencies.ts";
import {dateToString} from "../../helpers/globalHelper.ts";

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
    const [isLoading, setIsLoading] = useState(false);

    const xTokenMutation = useMonoUserInfo();
    const onSubmit: SubmitHandler<MonoWalletToken> = async (model: MonoWalletToken) => {
        setIsLoading(true);
        const mutationResponse = await xTokenMutation.mutateAsync(model);
        if (mutationResponse.hasError) {
            setError("xToken", {message: "Invalid token"})
            setIsLoading(false);
        } else {
            if (mutationResponse.response) {
                handleMonouserInfo(mutationResponse.response);
                handleNextStep();
                setIsLoading(false);
            }
        }
    };
    

    return (
        <div className={`absolute inset-0  flex justify-center items-center visible bg-black/20 z-50`}>
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
                        className={isLoading ? "inactive-create-cash-wallet-button" : "create-cash-wallet-button"}
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
    const {handleSubmit, register, reset, clearErrors, setError, formState: {errors}} = useForm<ExtendedMonobankConfiguration>({
        mode: 'onSubmit'
    });

    const monobankMutation = useCreateMonoWallet();
    const [selectedAcc, setSelectedAcc] = useState<Account>(userInfo.accounts[0])
    const [isLoading, setIsLoading] = useState(false);
    const handleSelectedAccount = (acc: Account) => setSelectedAcc(acc);
    const onSubmit: SubmitHandler<ExtendedMonobankConfiguration> = async (model: ExtendedMonobankConfiguration) => {
        const fromUnix = new Date(model.from).getTime() / 1000;
        const toUnix = model.to ? new Date(model.to).getTime() / 1000 : undefined;
        model.accountId = selectedAcc.id;
        model.currency = currencies.find(c => c.code === selectedAcc.currencyCode) || currencies.find(c => c.symbol === "UAN")!
        const unixModel = {
            ...model,
            from: fromUnix,
            to: toUnix,
        };
        setIsLoading(true)
        const monobankResponse = await monobankMutation.mutateAsync(unixModel);

        if (monobankResponse.hasError) {
            // @ts-ignore
            setError("root", {message: monobankResponse.error?.details[0].errorMessage})
            setIsLoading(false);
        } else {
            handleOpenModal()
            setIsLoading(false);
        }
    };

    return (
        <div className={`absolute inset-0  flex justify-center items-center visible bg-black/20 z-50`}>
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
                        <div {...register("accountId")}>
                            <SingleSelectDropDownMenu items={userInfo.accounts} ItemComponent={BankAccountItem}
                                                      heading={"Accounts"}
                                                      onItemSelected={handleSelectedAccount}
                                                      defaultSelectedItem={selectedAcc}/>
                            {errors.accountId && <p className={'text-red-400 italic'}>{errors.accountId.message}</p>}
                        </div>
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
                            defaultValue={dateToString(new Date())} // Set default value to today's date
                            {...register("to")}
                        />
                    </div>
                    <button
                        className={isLoading ? "inactive-create-cash-wallet-button" : "create-cash-wallet-button"}
                        type="submit"
                    >
                        Connect wallet
                    </button>
                </form>
            </div>
        </div>
    );
};

export default CreateMonobankWalletModal;