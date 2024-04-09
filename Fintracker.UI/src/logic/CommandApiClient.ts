import {RegisterResponse, RegisterSchema} from "../models/RegisterSchema.ts";
import {LoginResponse, LoginSchema} from "../models/LoginSchema.ts";

export interface CommandApiClient<TRequest> {
    create: (newEntity: TRequest) => Promise<ClientWrapper<CreateCommandResponse<TRequest>>>;
    update: (updatedEntity: TRequest) => Promise<ClientWrapper<UpdateCommandResponse<TRequest>>>;
    delete: (id: number) => Promise<ClientWrapper<DeleteCommandResponse<TRequest>>>;
}

export interface AuthApiClient {
    register: (registerModel: RegisterSchema) => Promise<ClientWrapper<RegisterResponse>>;
    login: (loginModel: LoginSchema) => Promise<ClientWrapper<LoginResponse>>;
    logout: () => Promise<void>;
}

