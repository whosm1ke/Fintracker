import {RegisterResponse, RegisterSchema} from "../models/RegisterSchema.ts";
import {LoginResponse, LoginSchema} from "../models/LoginSchema.ts";
import {Wallet} from "../entities/Wallet.ts";
import {MonoWalletToken} from "../hooks/wallet/useCreateMonoWallet.ts";
import {MonobankConfiguration, MonobankUserInfo} from "../entities/MonobankUserInfo.ts";
import {ConvertCurrency} from "../entities/Currency.ts";
import AcceptInvite from "../models/AcceptInvite.ts";
import ClientWrapper, {
    BaseCommandResponse,
    CreateCommandResponse,
    DeleteCommandResponse,
    UpdateCommandResponse
} from "../serverResponses/responses.ts";

export interface CommandApiClient<TModel, TResponse> {
    create: (newEntity: TModel) => Promise<ClientWrapper<CreateCommandResponse<TResponse>>>;
    update: (updatedEntity: TModel) => Promise<ClientWrapper<UpdateCommandResponse<TResponse>>>;
    updateForm: (updatedEntity: FormData) => Promise<ClientWrapper<UpdateCommandResponse<TResponse>>>;
    delete: (id: string) => Promise<ClientWrapper<DeleteCommandResponse<TResponse>>>;
    deleteWithModel: (id: string, model: TModel) => Promise<ClientWrapper<DeleteCommandResponse<TResponse>>>;
}

export interface AuthApiClient {
    register: (registerModel: RegisterSchema) => Promise<ClientWrapper<RegisterResponse>>;
    login: (loginModel: LoginSchema) => Promise<ClientWrapper<LoginResponse>>;
    logout: () => Promise<null>;
    acceptInvite: (model: AcceptInvite) => Promise<BaseCommandResponse>;
}

export interface MonobankClient {
    getMonobankUserInfo: (xToken: MonoWalletToken) => Promise<ClientWrapper<MonobankUserInfo>>;
    addInitialMonobankTransaction: (config: MonobankConfiguration) => Promise<ClientWrapper<CreateCommandResponse<Wallet>>>
    addNewMonobankTransaction: (accId: string) => Promise<void>
}

type ConvertListProps = {
    from: string[];
    to: string;
    amount: number[];
}

export interface ConvertClient {
    convert: (from: string, to: string, amount: number) => Promise<ConvertCurrency | null>;
    convertAll: (data: ConvertListProps) => Promise<ConvertCurrency[] | null>;
}

