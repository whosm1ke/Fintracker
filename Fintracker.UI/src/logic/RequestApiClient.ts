import {AxiosRequestConfig} from "axios";

export interface RequestApiClient<TResponse> {
    getById:(id: string) => Promise<ClientWrapper<TResponse>>;
    getByKey:(key: number) => Promise<ClientWrapper<TResponse>>;
    getAll:(config?: AxiosRequestConfig) => Promise<TResponse>;
}

