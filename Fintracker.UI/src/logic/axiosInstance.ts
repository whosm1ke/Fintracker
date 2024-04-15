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

export default axiosInstance;



