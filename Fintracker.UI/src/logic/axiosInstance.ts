import axios from "axios";

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7295/api/',
});
axiosInstance.interceptors.request.use(cfg => {
    const token = localStorage.getItem('userToken')
    cfg.headers.Authorization = `Bearer ${token || ''}`;
    return cfg;
}, error => {
    return Promise.reject(error);
})

export const axiosInstanceCurrencyConverter = axios.create({
    baseURL: 'https://api.currencybeacon.com/v1/',
    params: {
        api_key: 'b8IMjBLpaZDJFBnu40jAOc6EWjin0IQg'
    }
})

export default axiosInstance;



