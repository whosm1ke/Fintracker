import ApiClient from "../services/ApiClient.ts";

const apiClient = new ApiClient<void,void>('account/logout');
const useLogout = () => {
    apiClient.logout().then(x => x)
    localStorage.removeItem('userToken')
    localStorage.removeItem('userEmail')
    localStorage.removeItem('userId')
}

export default useLogout;