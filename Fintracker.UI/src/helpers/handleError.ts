import axios from "axios";
import {FieldValues, Path, UseFormSetError} from "react-hook-form";
import { BaseResponse, UnauthorizedResponse, NotFoundResponse } from "../serverResponses/responses";

export const handleError = (error: any): BaseResponse | NotFoundResponse | UnauthorizedResponse | Error => {
    if (axios.isAxiosError(error)) {
        const axiosErrorResponse = error.response;
        if (!axiosErrorResponse)
            return error;
        else if (axiosErrorResponse.data)
            return axiosErrorResponse.data
        else
            return {
                message: error.message,
                name: error.code ?? "Internal server error",
                statusCode: axiosErrorResponse.status
            }
    }

    return error;
}

export const handleServerErrorResponse = <T extends FieldValues>(error: any,setError: UseFormSetError<T>) => {
    if (error && 'details' in error) {
        const serverErrors = error.details;
        serverErrors.forEach((errorDetail: any) => {
            if (errorDetail.propertyName) {
                setError(errorDetail.propertyName as unknown as Path<T>, {
                    type: 'server',
                    message: errorDetail.errorMessage,
                });

            }
        });
    } else {
        console.log(error);
    }
}
