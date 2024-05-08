import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";
import { AppUser } from "../entities/AppUser";


interface AppUserStore {
    user: AppUser;
    setUserToken: (token: string) => void;
    getUserToken: () => string | null;
    setUserId: (id: string) => void;
    getUserId: () => string | null;
    isSignedIn: () => boolean;
}

const useUserStore = createWithEqualityFn<AppUserStore>((set, get) => ({
    user: {} as AppUser,
    isSignedIn: () => {
      const userToken = get().getUserToken();
      return userToken !== null;
    },
    setUserToken: (token: string) => {
        sessionStorage.setItem('userToken', token);
        set(store => ({user: {...store.user, token: token}}));
    },
    getUserToken: () => {
        const { user } = get();
        return user.token || sessionStorage.getItem('userToken');
    },
    setUserId: (id: string) => {
        sessionStorage.setItem('userId', id);
        set(store => ({user: {...store.user, id: id}}));
    },
    getUserId: () => {
        const { user } = get();
        return user.id || sessionStorage.getItem('userId');
    },
}), shallow);

export default useUserStore;