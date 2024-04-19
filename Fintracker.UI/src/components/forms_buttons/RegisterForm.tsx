import {SubmitHandler, useForm,} from "react-hook-form";
import {zodResolver} from '@hookform/resolvers/zod'
import PasswordInput from "./PasswordInput.tsx";
import SimpleInput from "./SimpleInput.tsx";
import useRegister from "../../hooks/useRegister.ts";
import Title from "./Title.tsx";
import SubTitle from "./SubTitle.tsx";
import {registerSchema, RegisterSchema} from "../../models/RegisterSchema.ts";
import {useNavigate} from "react-router-dom";


export default function RegisterForm() {


    const {register, handleSubmit, setError, formState: {errors}} = useForm<RegisterSchema>({
        resolver: zodResolver(registerSchema),
        mode: 'onBlur'
    });

    const registerMutation = useRegister(setError);
    const navigate = useNavigate();
    const onSubmit: SubmitHandler<RegisterSchema> = async (model: RegisterSchema) => {
        await registerMutation.mutateAsync(model, {
            onSuccess: () => {
                navigate('/', {replace: true});
            }
        });
    };

    return (
            <section
                className="flex flex-col min-h-screen bg-gray-50 p-4">
                <Title/>
                <div className="max-w-md w-2/3 mx-auto my-auto order-1 sm:order-2">
                    <SubTitle h1={'Sign up'} h4={'Already registered?'} linkTo={'login'} linkText={'Sign in'}/>
                    <form className="mt-8 space-y-6 border-2 p-8 rounded bg-white shadow-xl sm:p-12"
                          onSubmit={handleSubmit(onSubmit)} method={'post'}>
                        <div className="flex flex-col gap-y-6 rounded-md shadow-sm -space-y-px">
                            <SimpleInput id={'userName'} autoComplete={'userName'} placeholder={'Username'}
                                         register={register('userName')} error={errors.userName} showError={true}/>
                            <SimpleInput id={'email'} autoComplete={'email'} placeholder={'Email'}
                                         register={register('email')} error={errors.email} showError={true}/>
                            <PasswordInput id={'password'} placeholder={'Password'} register={register('password')}
                                           error={errors.password} showError={true}/>
                            <PasswordInput id={'confirmPassword'} placeholder={'Confirm password'}
                                           register={register('confirmPassword')} error={errors.confirmPassword}
                                           showError={true}/>
                        </div>
                        <div>
                            <button type="submit"
                                    className="submit-register-button">
                                Register
                            </button>
                        </div>
                    </form>
                </div>
            </section>
    )
}


