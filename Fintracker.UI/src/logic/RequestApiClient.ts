import {AxiosRequestConfig} from "axios";
import ClientWrapper from "../serverResponses/responses";

export interface RequestApiClient<TResponse> {
    getById:(id: string) => Promise<ClientWrapper<TResponse>>;
    getByKey:(key: number) => Promise<ClientWrapper<TResponse>>;
    getAll:(config?: AxiosRequestConfig) => Promise<TResponse>;
}

