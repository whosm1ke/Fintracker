import axios, {AxiosRequestConfig} from "axios";
import {CommandApiClient} from "../logic/CommandApiClient.ts";
import {RequestApiClient} from "../logic/RequestApiClient.ts";
import {handleError} from "../helpers/handleError.ts";

const axiosInstanse = axios.create({
    baseURL: 'https://localhost:7295/api/',
});
export default class ApiClient<TRequest, TResponse> implements CommandApiClient<TRequest>, RequestApiClient<TResponse> {

    constructor(private endpoint: string) {
    }

    async create(newEntity: TRequest): Promise<ClientWrapper<CreateCommandResponse<TRequest>>> {
        try {
            const data = await axiosInstanse.post<CreateCommandResponse<TRequest>>(this.endpoint, newEntity);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async update(updatedEntity: TRequest): Promise<ClientWrapper<UpdateCommandResponse<TRequest>>> {
        try {
            const data = await axiosInstanse.put<UpdateCommandResponse<TRequest>>(this.endpoint, updatedEntity);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async delete(id: number): Promise<ClientWrapper<DeleteCommandResponse<TRequest>>> {
        try {
            const data = await axiosInstanse.delete<DeleteCommandResponse<TRequest>>(this.endpoint + `/${id}`);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async getById(id: string): Promise<ClientWrapper<TResponse>> {
        try {
            const data = await axiosInstanse.get<TResponse>(this.endpoint + `/${id}`);
            return {response: data.data, hasError: false};
        } catch (error) {
            console.log(error)
            return {hasError: true, error: handleError(error)};
        }
    }

    async getByKey(key: number): Promise<ClientWrapper<TResponse>> {
        try {
            const data = await axiosInstanse.get<TResponse>(this.endpoint + `/${key}`);

            return {response: data.data, hasError: false};
        } catch (error) {
            return {hasError: true, error: handleError(error)};
        }
    }

    async getAll(): Promise<TResponse> {
        return await axiosInstanse.get<TResponse>(this.endpoint)
            .then(res => res.data);
    }

    async getAllSorted(config?: AxiosRequestConfig): Promise<TResponse> {
        return await axiosInstanse.get<TResponse>(this.endpoint, config)
            .then(res => res.data);
    }
}