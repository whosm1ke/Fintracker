import useUserStore from "../stores/userStore.ts";
import {useGetUser} from "../hooks/auth/useUser.ts";
import {ChangeEvent, useEffect, useState} from "react";
import currencies from "../data/currencies.ts";
import SingleSelectDropDownMenu from "../components/other/SingleSelectDropDownMenu.tsx";
import CurrencyItem from "../components/currencies/CurrencyItem.tsx";
import {Language} from "../entities/Language.ts";
import {SubmitHandler, useForm} from "react-hook-form";
import {User} from "../entities/User.ts";
import {bDayRegisterOptionsForUserDetails} from "../entities/UserDetails.ts";
import useUpdateUser from "../hooks/auth/useUpdateUser.ts";
import {dateToString} from "../helpers/globalHelper.ts";
import {Currency} from "../entities/Currency.ts";
import useUpdateUserUsername from "../hooks/auth/useUpdateUserUsername.ts";
import useUpdateUserEmail from "../hooks/auth/useUpdateUserEmail.ts";
import useUpdateUserPassword from "../hooks/auth/useUpdateUserPassword.ts";

export default function AccountSettings() {

    const userId = useUserStore(x => x.getUserId());
    const {data: userResponse} = useGetUser(userId!);
    const [userToUpdate, setUserToUpdate] = useState({
        email: "",
        userName: "",
        avatar: "",
        sex: "",
        dateOfBirth: dateToString(new Date(userResponse?.response?.userDetails?.dateOfBirth!)) || dateToString(new Date()),
        language: Language.English,
        globalCurrency: userResponse?.response?.globalCurrency,
    })
    const [fileList, setFileList] = useState<FileList | null>(null);
    const [avatarURL, setAvatarURL] = useState("");
    const [isAvatarChanged, setIsAvatarChanged] = useState(false);
    const {
        register,
        handleSubmit,
        formState: {errors},
        setError,
        setValue,
        clearErrors,
    } = useForm<User>();

    useEffect(() => {
        if (userResponse?.response) {
            handleSexChange(userResponse.response.userDetails?.sex || "Other")
            handleBDayChange(dateToString(new Date(userResponse?.response?.userDetails?.dateOfBirth!)) || dateToString(new Date()))
            handleGlobalCurrencyChange(userResponse.response.globalCurrency)
            handleLanguageChange(userResponse.response.userDetails?.language || Language.English)
            handleUserNameChange(userResponse.response.userName || "");
            handleEmailChange(userResponse.response.email || "");
            setUserToUpdate(p => ({...p, avatar: getImageFromURL(userResponse.response?.userDetails?.avatar)}));
            setValue("email", userResponse.response.email)
            setValue("userName", userResponse.response.userName)
        }
    }, [userResponse])


    useEffect(() => {
        const isEmailMatchingRegex = (email: string) => {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return emailRegex.test(email);

        }
        setIsEmailChanged(userResponse?.response?.email !== userToUpdate.email && isEmailMatchingRegex(userToUpdate.email!))
    }, [userToUpdate.email])

    const [isEmailChanged, setIsEmailChanged] = useState(false);
    const userUpdateMutation = useUpdateUser();
    const userUpdateUsernameMutation = useUpdateUserUsername();
    const userUpdateEmailMutation = useUpdateUserEmail();
    const userUpdatePasswordMutation = useUpdateUserPassword();
    if (!userResponse || !userResponse.response) return null;
    const user = userResponse.response;


    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            setIsAvatarChanged(userToUpdate.avatar !== file.name)
            const objectURL = URL.createObjectURL(file);
            setAvatarURL(objectURL);
            setFileList(event.target.files);
        }
    }
    const handleSexChange = (sex: string) => {
        setUserToUpdate(p => ({...p, sex: sex}))
    }

    const handleUserNameChange = (userName: string) => {
        setUserToUpdate(p => ({...p, userName: userName}))
    }

    const handleEmailChange = (email: string) => {
        setUserToUpdate(p => ({...p, email: email}))
    }

    const getImageFromURL = (url: string | undefined) => {
        if (!url) return "logo.png";

        const parts = url.split('/');
        return parts[parts.length - 1];
    }

    const handleBDayChange = (bDay: string) => {
        setUserToUpdate(p => ({...p, dateOfBirth: bDay}))
    }

    const handleGlobalCurrencyChange = (currency: Currency) => {
        setUserToUpdate(p => ({...p, globalCurrency: currency}))
    }

    const handleLanguageChange = (lang: Language) => {
        setUserToUpdate(p => ({...p, language: lang}))
    }
    const onSubmit: SubmitHandler<User> = async (_model: User) => {

        if (!userToUpdate.globalCurrency) {
            setError("globalCurrency", {message: "Currency is not selected"});
            return;
        } else {
            clearErrors("globalCurrency");
        }
        const formData = new FormData();
        formData.append('Avatar', fileList ? fileList[0] : user.userDetails.avatar!);
        formData.append('Id', user.id);
        formData.append('CurrencyId', userToUpdate.globalCurrency.id);
        formData.append('Currency', JSON.stringify(userToUpdate.globalCurrency));
        formData.append('UserDetails.Sex', userToUpdate.sex || "Other");
        formData.append('UserDetails.DateOfBirth', userToUpdate.dateOfBirth!);
        formData.append('UserDetails.Language', userToUpdate.language?.toString() || Language.English.toString());
        const updateResult = await userUpdateMutation.mutateAsync(formData);

        if (!updateResult.hasError) {
            clearErrors();

        }

    }

    const onAuthChangeSubmit: SubmitHandler<User> = async (model) => {
        console.log("model: ", model);
        if (model.userName !== user.userName) {
            const changeRes = await userUpdateUsernameMutation.mutateAsync(({
                ...user,
                userName: model.userName
            }));

            if (changeRes.hasError) {
                setError("userName", {message: "This username is already used"})
            } else {
                clearErrors();

            }
        }
        if (model.email !== user.email) {
            await userUpdateEmailMutation.mutateAsync({
                newEmail: model.email,
                urlCallback: 'email-reset'
            })
            setIsEmailChanged(false);
        }
    }
    
    const handleChangePassword = async () => {
        await userUpdatePasswordMutation.mutateAsync({
            urlCallback: 'password-reset'
        })
    }
    
    const isDataSameAsPrevious = user.globalCurrency.id === userToUpdate.globalCurrency?.id &&
        dateToString(new Date(user.userDetails?.dateOfBirth || new Date())) === userToUpdate.dateOfBirth &&
        user.userDetails?.sex === userToUpdate.sex &&
        user.userDetails?.language === userToUpdate.language &&
        getImageFromURL(user.userDetails?.avatar) === userToUpdate.avatar;

    const isUsernameChanged = user.userName !== userToUpdate.userName;
console.log("user.userDetails?.avatar: ", user.userDetails?.avatar)
    return (
        <div className={'px-16 py-6 flex flex-col gap-10'}>
            <header className={'font-semibold text-lg'}>
                Account setting
            </header>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className={'flex flex-col gap-y-5'}>
                    <div className={'flex flex-col gap-y-2'}>
                        <p className={'text-sm text-gray-500'}>Profile avatar</p>
                        <div className="flex flex-col sm:flex-row gap-x-5 gap-y-3 items-start sm:items-center">
                            <img src={avatarURL || user.userDetails?.avatar} alt="avatar"
                                 className="w-20 h-20 rounded-full"/>
                            <div
                                className="bg-green-500 text-white cursor-pointer px-3 py-2 rounded transition-colors duration-300 hover:bg-green-400">
                                <input type="file" id="file" accept=".jpg,.jpeg,.png,.gif"
                                       multiple={false}
                                       className="hidden"
                                       onChange={handleFileChange}
                                />
                                <label htmlFor="file">
                                    Choose avatar
                                </label>
                            </div>
                        </div>
                    </div>
                    <div className={'grid grid-cols-1 md:grid-cols-2 gap-4'}>
                        <div className="mb-4">
                            <label className="block text-gray-400 text-sm mb-2" htmlFor="sex">
                                Sex
                            </label>
                            <select
                                value={userToUpdate.sex}
                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="sex"
                                {...register("userDetails.sex")}
                                onChange={e => handleSexChange(e.target.value)}
                            >
                                <option value="Male">Male</option>
                                <option value="Female">Female</option>
                                <option value="Other">Other</option>
                            </select>
                        </div>
                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="bday">
                                Birthday
                            </label>
                            <input
                                {...register("userDetails.dateOfBirth", bDayRegisterOptionsForUserDetails)}
                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="bday"
                                type="date"
                                value={userToUpdate.dateOfBirth}
                                onChange={e => handleBDayChange(e.target.value)}
                            />
                            {errors.userDetails?.dateOfBirth &&
                                <p className={'text-red-400 italic'}>{errors.userDetails.dateOfBirth.message}</p>}
                        </div>
                    </div>
                    <div className={'flex flex-col gap-y-4'}>
                        <h2 className={'font-semibold'}>Language settings</h2>
                        <div className={'grid grid-cols-1 md:grid-cols-2 gap-4'}>
                            <div className="mb-4"
                            >
                                <label className="block text-gray-400 text-sm mb-2" htmlFor="email">
                                    Currency
                                </label>
                                <SingleSelectDropDownMenu items={currencies} ItemComponent={CurrencyItem}
                                                          heading={"Currency"}
                                                          onItemSelected={handleGlobalCurrencyChange}
                                                          defaultSelectedItem={userToUpdate.globalCurrency}/>
                            </div>
                            <div className="mb-4">
                                <label className="block text-gray-400 text-sm mb-2" htmlFor="lang">
                                    Language
                                </label>
                                <select
                                    {...register("userDetails.language", {required: "Language is required"})}
                                    value={userToUpdate.language}
                                    onChange={e => handleLanguageChange(+e.target.value)}
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="lang">
                                    <option value={Language.Ukrainian}>Ukrainian</option>
                                    <option value={Language.English}>English</option>
                                    <option value={Language.Deutch}>Germany</option>
                                </select>
                                {errors.userDetails?.language &&
                                    <p className={'text-red-400 italic'}>{errors.userDetails?.message}</p>}
                            </div>
                            <button
                                className={!isDataSameAsPrevious || isAvatarChanged ? 'bg-green-400 rounded-sm px-4 py-2 text-white' :
                                    'bg-gray-400 rounded-sm px-4 py-2 text-gray-700 pointer-events-none'}>Update
                                settings
                            </button>
                        </div>
                    </div>
                </div>
            </form>
            <form onSubmit={handleSubmit(onAuthChangeSubmit)} className={'grid grid-cols-1 sm:grid-cols-2 gap-4'}>
                <div className="mb-4">
                    <label className="block text-gray-400 text-sm mb-2" htmlFor="username">
                        Username
                    </label>
                    <input
                        {...register("userName", {required: "Username is required"})}
                        className="mb-4 shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="username"
                        type="text"
                        onChange={(e) => handleUserNameChange(e.target.value)}
                        value={userToUpdate.userName}
                    />
                    {errors.userName && <p className={'text-red-400 italic'}>{errors.userName.message}</p>}

                </div>
                <div className="mb-4">
                    <label className="block text-gray-400 text-sm mb-2" htmlFor="email">
                        Email
                    </label>
                    <input
                        {...register("email", {required: "Email is required"})}
                        className="mb-4 shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="email"
                        type="email"
                        value={userToUpdate.email}
                        onChange={(e) => handleEmailChange(e.target.value)}
                    />
                    {isEmailChanged &&
                        <p className={'bg-yellow-200 px-4 py-2 rounded'}>We will sent you an email to confirm
                            changing</p>}
                </div>
                <button type={'submit'}
                        className={isUsernameChanged || isEmailChanged ? 'bg-green-400 rounded-sm px-4 py-2 text-white' :
                            'bg-gray-400 rounded-sm px-4 py-2 text-gray-700 pointer-events-none'}>Change
                </button>
                <button
                    type={'button'}
                    onClick={handleChangePassword}
                    className={'bg-green-400 rounded-sm px-4 py-2 text-white'}>Change password
                </button>
            </form>
        </div>
    )
}
