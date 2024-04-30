//API RESPONSES
export class BaseResponse extends Error {
    when: Date;
    reason: string;
    details: ExceptionDetails[];
    statusCode: number;

    constructor(message: string, when: Date, reason: string,
                details: ExceptionDetails[], statusCode: number) {
        super(message);
        this.name = this.constructor.name;
        this.when = when;
        this.reason = reason;
        this.details = details;
        this.statusCode = statusCode;
    }
}

export class NotFoundResponse extends BaseResponse {
    type: string;

    constructor(message: string, when: Date, reason: string,
                details: ExceptionDetails[], statusCode: number, type: string) {
        super(message, when, reason, details, statusCode);
        this.type = type;
    }
}

export class UnauthorizedResponse extends BaseResponse {
}


export interface ExceptionDetails {
    errorMessage: string;
    propertyName?: string;
}

// COMMAND RESPONSES

export interface BaseCommandResponse {
    id: string;
    success: boolean;
    message: string;
}

export interface CreateCommandResponse<T> extends BaseCommandResponse {
    createdObject?: T;
}

export interface UpdateCommandResponse<T> extends BaseCommandResponse {
    old: T;
    new?: T;
}

export interface DeleteCommandResponse<T> extends BaseCommandResponse {
    deletedObj?: T;
}

//CLIENT RESPONSE WRAPPER

export default interface ClientWrapper<TData> {
    response?: TData;
    error?: BaseResponse | NotFoundResponse | UnauthorizedResponse | Error;
    hasError: boolean;
}