import {UseFormRegisterReturn} from "react-hook-form";
import {useState} from "react";
import {FaEye, FaEyeSlash} from "react-icons/fa";

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
            <label htmlFor={id} className="text-[13px] text-gray-500">{placeholder}</label>
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

type TogglePasswordProps = {
    showPassword: boolean;
    toggleShowPassword: () => void;
}
const TogglePassword = ({showPassword, toggleShowPassword}: TogglePasswordProps) => {
    return (
        <div onClick={toggleShowPassword}
             className="absolute right-3 top-1/2 transform -translate-y-1/2 cursor-pointer">
            {showPassword ? <FaEye/> : <FaEyeSlash/>}
        </div>
    );
}

export default PasswordInput;