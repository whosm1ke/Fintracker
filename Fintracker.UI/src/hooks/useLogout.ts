import ApiClient from "../services/ApiClient.ts";

const apiClient = new ApiClient<void,void>('account/logout');
const useLogout = async () => {
    localStorage.removeItem('userToken')
    localStorage.removeItem('userEmail')
    localStorage.removeItem('userId')
    console.log("asdasd")
    await apiClient.logout();
}

export default useLogout;