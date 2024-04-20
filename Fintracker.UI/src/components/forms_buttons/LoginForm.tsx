import {SubmitHandler, useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import SimpleInput from "./SimpleInput.tsx";
import PasswordInput from "./PasswordInput.tsx";
import SubTitle from "./SubTitle.tsx";
import Title from "./Title.tsx";
import {loginSchema, LoginSchema} from "../../models/LoginSchema.ts";
import {useNavigate} from "react-router-dom";
import useLogin from "../../hooks/auth/useLogin.ts";


export default function LoginForm() {


    const {register, handleSubmit, setError, formState: {errors}} = useForm<LoginSchema>({
        resolver: zodResolver(loginSchema),
        mode: 'onBlur'
    });
    const navigate = useNavigate();
    const loginMutation = useLogin(setError);
    const onSubmit: SubmitHandler<LoginSchema> = (model: LoginSchema) => {
        loginMutation.mutate(model, {
            onSuccess: () => {
                navigate('/dashboard', {replace: true});
            }
        });
    };
    return (

        <section
            className="flex flex-col min-h-screen bg-gray-50 p-4">
            <Title/>
            <div className="max-w-md w-2/3 mx-auto my-auto order-1 sm:order-2">
                <SubTitle h1={'Sign in'} h4={'Don`t have an account?'} linkTo={'registration'}
                          linkText={'Sign up'}/>
                <form className="mt-8 space-y-6 border-2 p-6 sm:p-16 w-auto rounded bg-white shadow-xl"
                      onSubmit={handleSubmit(onSubmit)} method={'post'}>
                    <div className="flex flex-col gap-y-6 rounded-md shadow-sm -space-y-px">
                        <SimpleInput id={'email'} placeholder={'Email'} showError={false}
                                     register={register('email')} error={errors.email} autoComplete={"email"}/>
                        <PasswordInput id={'password'} placeholder={'Password'} register={register('password')}
                                       error={errors.password} showError={false}/>
                    </div>
                    {errors && <p className="text-red-500 text-md italic">{errors.root?.message}</p>}
                    <div>
                        <button type="submit"
                                className="submit-register-button">
                            Login
                        </button>
                    </div>
                </form>
            </div>
        </section>
    )
}

