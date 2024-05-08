import {UseFormRegisterReturn} from "react-hook-form";
import {useState} from "react";
import TogglePassword from "./TogglePassword";

type PasswordInputProps = {
    id: string;
    placeholder: string;
    register: UseFormRegisterReturn;
    error: any;
    showError?: boolean
}
const PasswordInput = ({id, placeholder, register, error, showError}: PasswordInputProps) => {

    const [showPassword, togglePassword] = useState(false);

    return (
        <div>
            <label htmlFor={id} className="text-[1.25rem] sm:text-[1rem] text-gray-500">{placeholder}</label>
            <div className="relative">
                <input id={id} type={showPassword ? 'text' : 'password'}
                       required
                       className="register-password-input"
                       placeholder={placeholder} {...register} />
                <TogglePassword showPassword={showPassword}
                                toggleShowPassword={() => togglePassword(prev => !prev)}
                />
            </div>
            {showError && error &&
                <p className="text-red-500 text-md italic">{error.message}</p>}
        </div>
    )
}



export default PasswordInput;