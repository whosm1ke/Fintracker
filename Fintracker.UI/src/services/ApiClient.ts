import {AxiosRequestConfig} from "axios";
import {AuthApiClient, CommandApiClient} from "../logic/CommandApiClient.ts";
import {RequestApiClient} from "../logic/RequestApiClient.ts";
import {handleError} from "../helpers/handleError.ts";
import {RegisterResponse, RegisterSchema} from "../models/RegisterSchema.ts";
import {LoginResponse, LoginSchema} from "../models/LoginSchema.ts";
import {axiosInstance} from "../logic/axiosInstance.ts";


export default class ApiClient<TRequest, TResponse> implements CommandApiClient<TRequest>, RequestApiClient<TResponse>, AuthApiClient {

    constructor(private endpoint: string) {
    }

    
    async register(registerModel: RegisterSchema): Promise<ClientWrapper<RegisterResponse>> {
        try {
            const data = await  axiosInstance.post<RegisterResponse>(this.endpoint, registerModel);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async login(loginModel: LoginSchema): Promise<ClientWrapper<LoginResponse>> {
        try {
            const data = await axiosInstance.post<LoginResponse>(this.endpoint, loginModel);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async logout(): Promise<void> {
        await axiosInstance.post(this.endpoint);

    }

    async create(newEntity: TRequest): Promise<ClientWrapper<CreateCommandResponse<TRequest>>> {
        try {
            const data = await axiosInstance.post<CreateCommandResponse<TRequest>>(this.endpoint, newEntity);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async update(updatedEntity: TRequest): Promise<ClientWrapper<UpdateCommandResponse<TRequest>>> {
        try {
            const data = await axiosInstance.put<UpdateCommandResponse<TRequest>>(this.endpoint, updatedEntity);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async delete(id: number): Promise<ClientWrapper<DeleteCommandResponse<TRequest>>> {
        try {
            const data = await axiosInstance.delete<DeleteCommandResponse<TRequest>>(this.endpoint + `/${id}`);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async getById(id: string): Promise<ClientWrapper<TResponse>> {
        try {
            const data = await axiosInstance.get<TResponse>(this.endpoint + `/${id}`);
            return {response: data.data, hasError: false};
        } catch (error) {
            console.log(error)
            return {hasError: true, error: handleError(error)};
        }
    }

    async getByKey(key: number): Promise<ClientWrapper<TResponse>> {
        try {
            const data = await axiosInstance.get<TResponse>(this.endpoint + `/${key}`);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async getAll(): Promise<TResponse> {
        return await axiosInstance.get<TResponse>(this.endpoint)
            .then(res => res.data);
    }

    async getAllSorted(config?: AxiosRequestConfig): Promise<TResponse> {
        return await axiosInstance.get<TResponse>(this.endpoint, config)
            .then(res => res.data);
    }
}