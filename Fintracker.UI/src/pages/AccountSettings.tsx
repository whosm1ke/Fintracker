import useUserStore from "../stores/userStore.ts";
import {useGetUser} from "../hooks/auth/useUser.ts";
import Spinner from "../components/other/Spinner.tsx";
// @ts-ignore
import logo from '../assets/logo.png'
import {ChangeEvent, useLayoutEffect, useState} from "react";
import currencies from "../data/currencies.ts";
import SingleSelectDropDownMenu from "../components/other/SingleSelectDropDownMenu.tsx";
import CurrencyItem from "../components/currencies/CurrencyItem.tsx";
import {Language} from "../entities/Language.ts";
import {SubmitHandler, useForm} from "react-hook-form";
import {User} from "../entities/User.ts";
import {bDayRegisterOptionsForUserDetails} from "../entities/UserDetails.ts";
import useUpdateUser from "../hooks/auth/useUpdateUser.ts";
import {dateToString} from "../helpers/globalHelper.ts";

export default function AccountSettings() {

    const userId = useUserStore(x => x.getUserId());
    const {data: userResponse} = useGetUser(userId!);
    const userGlobalCurrency = currencies.find(c => c.symbol === userResponse?.response?.globalCurrency);
    const [userToUpdate, setUserToUpdate] = useState({
        sex: userResponse?.response?.userDetails?.sex || 'Other',
        birthDay: userResponse?.response?.userDetails?.dateOfBirth,
        globalCurrency: userGlobalCurrency,
        lang: userResponse?.response?.userDetails?.language || Language.English,
        avatar: userResponse?.response?.userDetails?.avatar,
    })
    const [fileList, setFileList] = useState<FileList | null>(null);
    const [avatarURL, setAvatarURL] = useState("");
    const {
        register,
        handleSubmit,
        formState: {errors},
        setError,
        clearErrors,
    } = useForm<User>();
    


    useLayoutEffect(() => {
        if (userResponse) {
            const userGlobalCurrency = currencies.find(c => c.symbol === userResponse.response?.globalCurrency);
            setUserToUpdate({
                sex: userResponse.response?.userDetails?.sex || 'Other',
                birthDay: userResponse.response?.userDetails?.dateOfBirth,
                globalCurrency: userGlobalCurrency,
                lang: userResponse.response?.userDetails?.language || Language.English,
                avatar: userResponse.response?.userDetails?.avatar,
            })
        }
    }, [userResponse])
    const userUpdateMutation = useUpdateUser();

    if (!userResponse || !userResponse.response) return <Spinner/>
    const user = userResponse.response;

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            const objectURL = URL.createObjectURL(file);
            setAvatarURL(objectURL);
            setFileList(event.target.files);
        }


    }
    const onSubmit: SubmitHandler<User> = async (model: User) => {

        if (!userToUpdate.globalCurrency) {
            setError("globalCurrency", {message: "Currency is not selected"});
            return;
        } else {
            clearErrors("globalCurrency");
        }
        model.globalCurrency = userToUpdate.globalCurrency.symbol
        const formData = new FormData();
        formData.append('Avatar', fileList ? fileList[0] : "");
        formData.append('Id', user.id);
        formData.append('GlobalCurrency', model.globalCurrency);
        formData.append('UserDetails.Sex', model.userDetails?.sex || "Other");
        formData.append('UserDetails.DateOfBirth', model.userDetails?.dateOfBirth!);
        formData.append('UserDetails.Language', model.userDetails?.language?.toString() || Language.English.toString());
        console.log("model: ", model)
        const updateResult = await userUpdateMutation.mutateAsync(formData);

        if (!updateResult.hasError) {
            clearErrors();
        }

    }
    

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
                            <img src={avatarURL || user.userDetails?.avatar || logo} alt="avatar"
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
                    <div className={'grid grid-cols-1 sm:grid-cols-2 gap-4'}>

                        <div className="mb-4">
                            <label className="block text-gray-400 text-sm mb-2" htmlFor="sex">
                                Sex
                            </label>
                            <select
                                value={user.userDetails?.sex || userToUpdate.sex}
                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="sex"
                                {...register("userDetails.sex")}
                                onChange={e => setUserToUpdate(p => ({...p, sex: e.target.value}))}
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
                                onChange={e => setUserToUpdate(p => ({
                                    ...p,
                                    birthDay: dateToString(e.target.valueAsDate!)
                                }))}
                                defaultValue={user.userDetails?.dateOfBirth || dateToString(new Date())}
                            />
                            {errors.userDetails?.dateOfBirth &&
                                <p className={'text-red-400 italic'}>{errors.userDetails.dateOfBirth.message}</p>}
                        </div>
                    </div>
                    <div className={'flex flex-col gap-y-4'}>
                        <h2 className={'font-semibold'}>Language settings</h2>
                        <div className={'grid grid-cols-1 sm:grid-cols-2 gap-4'}>
                            <div className="mb-4"
                            >
                                <label className="block text-gray-400 text-sm mb-2" htmlFor="email">
                                    Currency
                                </label>
                                <SingleSelectDropDownMenu items={currencies} ItemComponent={CurrencyItem}
                                                          heading={"Currency"}
                                                          onItemSelected={(c) => setUserToUpdate(p => ({
                                                              ...p,
                                                              globalCurrency: c
                                                          }))}
                                                          defaultSelectedItem={userToUpdate.globalCurrency}/>
                            </div>
                            <div className="mb-4">
                                <label className="block text-gray-400 text-sm mb-2" htmlFor="lang">
                                    Language
                                </label>
                                <select
                                    {...register("userDetails.language", {required: "Language is required"})}
                                    value={user.userDetails?.language || userToUpdate.lang}
                                    onChange={e => setUserToUpdate(p => ({...p, lang: +e.target.value}))}
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    id="lang">
                                    <option value={Language.Ukrainian}>Ukrainian</option>
                                    <option value={Language.English}>English</option>
                                    <option value={Language.Deutch}>Germany</option>
                                </select>
                                {errors.userDetails?.language &&
                                    <p className={'text-red-400 italic'}>{errors.userDetails?.message}</p>}
                            </div>
                        </div>
                    </div>
                    <button className={'bg-green-400 rounded-sm px-4 py-2 text-white'}>Update settings</button>
                </div>
            </form>
        </div>
    )
}


// <div className="mb-4">
//     <label className="block text-gray-400 text-sm mb-2" htmlFor="username">
//         Username
//     </label>
//     <input
//         {...register("userName", {required: "Username is required"})}
//         className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
//         id="username"
//         type="text"
//         onChange={() =>  console.log("getValues(\"userName\"): ", getValues("userName"))}
//         defaultValue={user.userName}
//     />
// </div>
// <div className="mb-4">
//     <label className="block text-gray-400 text-sm mb-2" htmlFor="email">
//         Email
//     </label>
//     <input
//         className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
//         id="email"
//         type="email"
//         defaultValue={user.email}
//     />
// </div>