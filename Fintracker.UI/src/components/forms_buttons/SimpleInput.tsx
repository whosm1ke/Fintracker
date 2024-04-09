import {UseFormRegisterReturn} from "react-hook-form";

type SimpleInputProps = {
    id: string;
    autoComplete: string;
    placeholder: string;
    register: UseFormRegisterReturn;
    error: any;
    showError?: boolean
}
const SimpleInput = ({id, autoComplete, placeholder, register, error, showError}: SimpleInputProps) => {
    return (
        <div>
            <label htmlFor={id} className="text-[1.25rem] sm:text-[1rem] text-gray-500">{placeholder}</label>
            <input id={id} type="text" autoComplete={autoComplete} required
                   className="register-input"
                   placeholder={placeholder} {...register} />
            {showError && error && <p className="text-red-500 text-md italic">{error.message}</p>}
        </div>
    )
}

export default SimpleInput;