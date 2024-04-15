import ApiClient from "../services/ApiClient.ts";

const apiClient = new ApiClient<void,void>('account/logout');
const useLogout = async () => {
    await apiClient.logout();
    localStorage.removeItem('userToken')
    localStorage.removeItem('userEmail')
    localStorage.removeItem('userId')
}

export default useLogout;