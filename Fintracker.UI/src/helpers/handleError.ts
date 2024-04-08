import axios from "axios";

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
