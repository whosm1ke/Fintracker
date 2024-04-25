﻿import { CategoryType } from "./CategoryType";

export interface Category extends BaseEntity {
    name: string;
    type: CategoryType;
    image: string;
    iconColour: string;
}