import axios, {AxiosRequestConfig, CancelTokenSource} from "axios";
import {AuthApiClient, CommandApiClient} from "../logic/CommandApiClient.ts";
import {RequestApiClient} from "../logic/RequestApiClient.ts";
import {handleError} from "../helpers/handleError.ts";
import {RegisterResponse, RegisterSchema} from "../models/RegisterSchema.ts";
import {LoginResponse, LoginSchema} from "../models/LoginSchema.ts";
import axiosInstance from "../logic/axiosInstance.ts";


export default class ApiClient<TRequest, TResponse> implements CommandApiClient<TRequest>, RequestApiClient<TResponse>, AuthApiClient {

    constructor(private endpoint: string) {
    }

    private cancelToken: CancelTokenSource | undefined;

    private cancelCurrentRequest() {
        if (this.cancelToken) {
            this.cancelToken.cancel('Request canceled');
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

    async logout(): Promise<void> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        await axiosInstance.post(this.endpoint, {
            cancelToken: this.cancelToken.token
        });

    }

    async create(newEntity: TRequest): Promise<ClientWrapper<CreateCommandResponse<TRequest>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.post<CreateCommandResponse<TRequest>>(this.endpoint, newEntity, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async update(updatedEntity: TRequest): Promise<ClientWrapper<UpdateCommandResponse<TRequest>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.put<UpdateCommandResponse<TRequest>>(this.endpoint, updatedEntity, {
                cancelToken: this.cancelToken.token
            });

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async delete(id: number): Promise<ClientWrapper<DeleteCommandResponse<TRequest>>> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        try {
            const data = await axiosInstance.delete<DeleteCommandResponse<TRequest>>(this.endpoint + `/${id}`, {
                cancelToken: this.cancelToken.token
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

    async getAll(): Promise<TResponse> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        return await axiosInstance.get<TResponse>(this.endpoint, {
            cancelToken: this.cancelToken.token
        })
            .then(res => res.data);
    }

    async getAllSorted(config?: AxiosRequestConfig): Promise<TResponse> {
        this.cancelCurrentRequest();
        this.cancelToken = axios.CancelToken.source();
        if (config)
            config.cancelToken = this.cancelToken.token;
        return await axiosInstance.get<TResponse>(this.endpoint, config || {
            cancelToken: this.cancelToken.token
        })
            .then(res => res.data);
    }
}