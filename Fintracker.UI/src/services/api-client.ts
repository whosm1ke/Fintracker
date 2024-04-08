import axios, {AxiosRequestConfig} from "axios";

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7295/api/',
})

export default class ApiClient<T> {
    endpoint: string;

    constructor(endpoint: string) {
        this.endpoint = endpoint;
    }

    getAll = (cfg?: AxiosRequestConfig) => {
        return axiosInstance.get<T>(this.endpoint, cfg)
            .then(res => res.data);
    }

    get = (id: string | number, cfg?: AxiosRequestConfig) =>
        axiosInstance.get<T>(this.endpoint + `/${id}`, cfg)
            .then(res => res.data)

    post = (newEntity: T, cfg?: AxiosRequestConfig) =>
        axiosInstance.post<T>(this.endpoint, newEntity, cfg)
            .then(res => res.data)

    postForm = (newEntity: T, cfg?: AxiosRequestConfig) =>
        axiosInstance.postForm<T>(this.endpoint, newEntity, cfg)
            .then(res => res.data)

    put = (updatedEntity: T, cfg?: AxiosRequestConfig) =>
        axiosInstance.put<T>(this.endpoint, updatedEntity, cfg)
            .then(res => res.data)

    delete = (id: string) =>
        axiosInstance.delete<T>(this.endpoint + `/${id}`)
            .then(res => res.data)

}