import axios, {AxiosRequestConfig, CancelTokenSource} from "axios";
import {AuthApiClient, CommandApiClient, ConvertClient, MonobankClient} from "../logic/CommandApiClient.ts";
import {RequestApiClient} from "../logic/RequestApiClient.ts";
import {handleError} from "../helpers/handleError.ts";
import {RegisterResponse, RegisterSchema} from "../models/RegisterSchema.ts";
import {LoginResponse, LoginSchema} from "../models/LoginSchema.ts";
import axiosInstance, {axiosInstanceCurrencyConverter} from "../logic/axiosInstance.ts";
import {Wallet} from "../entities/Wallet.ts";
import {MonoWalletToken} from "../hooks/wallet/useCreateMonoWallet.ts";
import {ConvertCurrency} from "../entities/Currency.ts";
import {MonobankConfiguration, MonobankUserInfo} from "../entities/MonobankUserInfo.ts";
import AcceptInvite from "../models/AcceptInvite.ts";
import ClientWrapper, {
    BaseCommandResponse,
    CreateCommandResponse,
    DeleteCommandResponse,
    UpdateCommandResponse
} from "../serverResponses/responses.ts";
import ResetPasswordModel from "../models/ResetPasswordModel.ts";
import ResetEmailModel from "../models/ResetEmailModel.ts";


export default class ApiClient<TResponse, TModel = undefined> implements CommandApiClient<TModel, TResponse>,
    RequestApiClient<TResponse>,
    AuthApiClient,
    MonobankClient,
    ConvertClient {

    constructor(private endpoint: string) {
    }


    private cancelToken: CancelTokenSource | undefined;

    private cancelCurrentRequest() {
        if (this.cancelToken) {
            this.cancelToken.cancel('Request canceled');
        }
    }

    async convert(from: string, to: string, amount: number): Promise<ConvertCurrency | null> {

        try {
            const data = await axiosInstanceCurrencyConverter.get<ConvertCurrency>(this.endpoint, {
                params: {
                    from: from,
                    to: to,
                    amount: amount,
                },
            })

            return data.data;
        } catch (error) {
            console.log(error)
            return null;
        }
    }

    async convertAll(data: { from: string[]; to: string; amount: number[]; }): Promise<ConvertCurrency[]> {


        try {
            const promises = data.from.map((fromCurrency, index) => {
                return this.convert(fromCurrency, data.to, data.amount[index] | 1);
            });

            const results = await Promise.all(promises);
            return results.filter(result => result !== null) as ConvertCurrency[];
        } catch (error) {
            console.log("error: ", error);
            return [];
        }
    }


    async register(registerModel: RegisterSchema): Promise<ClientWrapper<RegisterResponse>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<RegisterResponse>(this.endpoint, registerModel, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async login(loginModel: LoginSchema): Promise<ClientWrapper<LoginResponse>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<LoginResponse>(this.endpoint, loginModel, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async logout(): Promise<null> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        await axiosInstance.post(this.endpoint, {
            cancelToken: this.cancelToken.token
        });
        return null;

    }

    async create(newEntity: TModel): Promise<ClientWrapper<CreateCommandResponse<TResponse>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<CreateCommandResponse<TResponse>>(this.endpoint, newEntity, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }


    async acceptInvite(model: AcceptInvite): Promise<BaseCommandResponse> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<BaseCommandResponse>(this.endpoint, model, {
                cancelToken: this.cancelToken.token
            });

            return data.data;
        } catch (error) {
            throw error;
        }
    }

    async resetEmail(model: ResetEmailModel): Promise<ClientWrapper<any>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<BaseCommandResponse>(this.endpoint, model, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            throw error;
        }
    }

    async resetPassword(model: ResetPasswordModel): Promise<ClientWrapper<any>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<BaseCommandResponse>(this.endpoint, model, {
                cancelToken: this.cancelToken.token
            });
            return {response: data.data, hasError: false};
        } catch (error) {
            throw error;
        }
    }

    async update(updatedEntity: TModel): Promise<ClientWrapper<UpdateCommandResponse<TResponse>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.put<UpdateCommandResponse<TResponse>>(this.endpoint, updatedEntity, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async updateForm(updatedEntity: FormData): Promise<ClientWrapper<UpdateCommandResponse<TResponse>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.putForm<UpdateCommandResponse<TResponse>>(this.endpoint, updatedEntity, {
                cancelToken: this.cancelToken.token,
                headers: {'Content-Type': 'multipart/form-data'}
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async deleteWithModel(id: string, model: TModel): Promise<ClientWrapper<DeleteCommandResponse<TResponse>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.delete<DeleteCommandResponse<TResponse>>(this.endpoint + `/${id}`, {
                cancelToken: this.cancelToken.token,
                data: model
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async delete(id: string, otherParams?: Record<string, any>): Promise<ClientWrapper<DeleteCommandResponse<TResponse>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        const url = new URL(this.endpoint + `/${id}`,axiosInstance.defaults.baseURL);
        if (otherParams) {
            Object.entries(otherParams).forEach(([key, value]) => {
                url.searchParams.append(key, value);
            });
        }

        try {
            const data = await axiosInstance.delete<DeleteCommandResponse<TResponse>>(url.toString(), {
                cancelToken: this.cancelToken.token,
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async getById(id: string): Promise<ClientWrapper<TResponse>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.get<TResponse>(this.endpoint + `/${id}`, {
                cancelToken: this.cancelToken.token
            });
            return {response: data.data, hasError: false};
        } catch (error) {
            console.log(error)
            return {hasError: true, error: handleError(error)};
        }
    }


    async getByKey(key: number): Promise<ClientWrapper<TResponse>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.get<TResponse>(this.endpoint + `/${key}`, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async getAll(config?: AxiosRequestConfig): Promise<TResponse> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        if (config)
            config.cancelToken = this.cancelToken.token;
        return await axiosInstance.get<TResponse>(this.endpoint, config || {
            cancelToken: this.cancelToken.token
        })
            .then(res => res.data);
    }


    async get(cfg: AxiosRequestConfig): Promise<TResponse> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        cfg.cancelToken = this.cancelToken.token
        return await axiosInstance.get<TResponse>(this.endpoint, cfg)
            .then(res => res.data);
    }

    async getMonobankUserInfo(xToken: MonoWalletToken): Promise<ClientWrapper<MonobankUserInfo>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.get<MonobankUserInfo>(this.endpoint, {
                headers: {
                    xToken: xToken.xToken
                },
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async addInitialMonobankTransaction(config: MonobankConfiguration): Promise<ClientWrapper<CreateCommandResponse<Wallet>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<CreateCommandResponse<Wallet>>(this.endpoint, config, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false}
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async addNewMonobankTransaction(accId: string): Promise<void> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            await axiosInstance.post(this.endpoint, accId, {
                cancelToken: this.cancelToken.token
            });

        } catch (error) {
            console.log(error)
        }
    }
}