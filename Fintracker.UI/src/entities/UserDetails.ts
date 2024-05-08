import { Language } from "./Language";
import {RegisterOptions} from "react-hook-form";

export interface UserDetails {
    sex?: string,
    dateOfBirth?: string,
    avatar?: string,
    language: Language
}


export const bDayRegisterOptionsForUserDetails: RegisterOptions = {
    validate: (_value, formValues) => {
        const today = new Date();
        today.setHours(0, 0, 0, 0); // Set the time to the start of the day
        const formDate = new Date(formValues.userDetails.dateOfBirth);
        formDate.setHours(0, 0, 0, 0); // Also set the time of formDate to the start of the day
        if(formDate > today)
            return "Date can not be in future";
        return true;
    }
}
