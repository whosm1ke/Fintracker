//API RESPONSES
interface BaseResponse {
    when: Date;
    reason: string;
    message: string;
    details: ExceptionDetails[];
    statusCode: number;
}

interface NotFoundResponse extends BaseResponse {
    type: string;
}

interface UnauthorizedResponse extends BaseResponse {
}

interface ExceptionDetails {
    errorMessage: string;
    propertyName?: string;
}

// COMMAND RESPONSES

interface BaseCommandResponse {
    id: string;
    success: boolean;
    message: string;
}

interface CreateCommandResponse<T> extends BaseCommandResponse{
    createdObject? : T;
}

interface UpdateCommandResponse<T> extends BaseCommandResponse {
    old: T;
    new?: T;
}

interface DeleteCommandResponse<T> extends BaseCommandResponse {
    deletedObj? : T;
}

//CLIENT RESPONSE WRAPPER

interface ClientWrapper<TData> {
    response?: TData;
    error?: BaseResponse | NotFoundResponse | UnauthorizedResponse | Error;
    hasError: boolean;
}