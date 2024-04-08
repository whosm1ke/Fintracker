import {AxiosRequestConfig} from "axios";

export interface RequestApiClient<TResponse> {
    getById:(id: string) => Promise<ClientWrapper<TResponse>>;
    getByKey:(key: number) => Promise<ClientWrapper<TResponse>>;
    getAll:() => Promise<TResponse>;
    getAllSorted:(config?: AxiosRequestConfig) => Promise<TResponse>;
}

