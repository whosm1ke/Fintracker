import {useLocation} from "react-router-dom";
import useConfirmResetPassword from "../../hooks/auth/useConfirmResetPassword.ts";
import {useState} from "react";
import Title from "./Title.tsx";
import PasswordInput from "./PasswordInput.tsx";
import {SubmitHandler, useForm} from "react-hook-form";
import {handleServerErrorResponse} from "../../helpers/handleError.ts";
import {isAxiosError} from "axios";

export default function ResetPassword() {
    const loc = useLocation();
    const urlQueryParams = new URLSearchParams(loc.search);
    const token = urlQueryParams.get('token');
    const userId = urlQueryParams.get('userId');
    const passMutation = useConfirmResetPassword();

    const [canBeClosed, setCanBeClosed] = useState(false)
    const {register, formState: {errors}, setError, handleSubmit} = useForm<{
        password: string,
        confirmPassword: string
    }>()
    const handleResetPasswordSubmit: SubmitHandler<{ password: string, confirmPassword: string }> = async (model) => {
        await passMutation.mutateAsync({
            token: token!,
            password: model.password,
            userId: userId!
        }, {
            onError: (error) => {
                console.log("error: ", error)
                // @ts-ignore
                if (isAxiosError(error))
                    handleServerErrorResponse(error.response?.data, setError);
            },
            onSuccess: _data => {
                setCanBeClosed(true)
            }
        });

    }
    return (
        <section
            className="flex flex-col min-h-screen bg-gray-50 p-4">
            <Title/>
            {!canBeClosed && <div className="max-w-md w-2/3 mx-auto my-auto order-1 sm:order-2">
                <form onSubmit={handleSubmit(handleResetPasswordSubmit)}>
                    <div className={'flex flex-col gap-5'}>
                        <PasswordInput id={'password'} placeholder={'Password'} register={register('password')}
                                       error={errors.password} showError={true}/>
                        <button type="submit"
                                className="submit-register-button">
                            Confirm
                        </button>
                    </div>
                </form>
            </div>}
            {canBeClosed &&
                <div className="max-w-md w-2/3 mx-auto my-auto order-1 sm:order-2">
                    <div className={'text-green-400 text-xl text-center'}>
                        This page can be closed now
                    </div>
                </div>
            }
        </section>
    )
}