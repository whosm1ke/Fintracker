import {RegisterResponse, RegisterSchema} from "../models/RegisterSchema.ts";
import {LoginResponse, LoginSchema} from "../models/LoginSchema.ts";
import {Wallet} from "../entities/Wallet.ts";
import {MonoWalletToken} from "../hooks/wallet/useCreateMonoWallet.ts";
import { MonobankConfiguration, MonobankUserInfo } from "../entities/MonobankUserInfo.ts";
import { ConvertCurrency } from "../entities/Currency.ts";

export interface CommandApiClient<TRequest> {
    create: (newEntity: TRequest) => Promise<ClientWrapper<CreateCommandResponse<TRequest>>>;
    update: (updatedEntity: TRequest) => Promise<ClientWrapper<UpdateCommandResponse<TRequest>>>;
    delete: (id: number) => Promise<ClientWrapper<DeleteCommandResponse<TRequest>>>;
}

export interface AuthApiClient {
    register: (registerModel: RegisterSchema) => Promise<ClientWrapper<RegisterResponse>>;
    login: (loginModel: LoginSchema) => Promise<ClientWrapper<LoginResponse>>;
    logout: () => Promise<null>;
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

