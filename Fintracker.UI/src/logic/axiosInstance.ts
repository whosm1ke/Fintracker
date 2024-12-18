import axios from "axios";

const axiosInstance = axios.create({
// @ts-ignore
    baseURL: import.meta.env.VITE_APP_API_ENDPOINT
});
axiosInstance.interceptors.request.use(cfg => {
    const token = sessionStorage.getItem('userToken')
    cfg.headers.Authorization = `Bearer ${token || ''}`;
    return cfg;
}, error => {
    return Promise.reject(error);
})

export const axiosInstanceCurrencyConverter = axios.create({
    baseURL: 'https://api.currencybeacon.com/v1/',
    params: {
        // @ts-ignore
        api_key: import.meta.env.VITE_APP_CURRENCY_API_KEY
    }
})

export default axiosInstance;



