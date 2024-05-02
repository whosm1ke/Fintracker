import { CategoryType } from "./CategoryType";
import {RegisterOptions} from "react-hook-form";

export interface Category extends BaseEntity {
    name: string;
    type: CategoryType;
    image: string;
    iconColour: string;
    transactionCount: number;
    budgetCount: number;
    isSystemCategory: boolean
}

export const nameRegisterOptionsForCategory: RegisterOptions = {
    minLength: {value: 1, message: "Minimum length for category name is 1"},
    maxLength: {value: 35, message: "Maximum name for category is 35"},
    required: "Name for category is required",
}