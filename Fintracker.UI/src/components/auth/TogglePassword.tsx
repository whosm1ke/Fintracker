import {FaEye, FaEyeSlash} from "react-icons/fa";

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

export default TogglePassword;