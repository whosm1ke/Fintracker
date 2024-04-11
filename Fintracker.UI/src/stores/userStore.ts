import {createWithEqualityFn} from "zustand/traditional";
import {shallow} from "zustand/shallow";


interface AppUserStore {
    user: AppUser;
    setUserToken: (token: string) => void;
    getUserToken: () => string | null;
    setUserId: (id: string) => void;
    getUserId: () => string | null;
    setUserEmail: (email: string) => void;
    getUserEmail: () => string | null;
    isSignedIn: () => boolean;
}

const useUserStore = createWithEqualityFn<AppUserStore>((set, get) => ({
    user: {} as AppUser,
    isSignedIn: () => {
      const userToken = get().getUserToken();
      return userToken !== null;
    },
    setUserToken: (token: string) => {
        localStorage.setItem('userToken', token);
        set(store => ({user: {...store.user, token: token}}));
    },
    getUserToken: () => {
        const { user } = get();
        return user.token || localStorage.getItem('userToken');
    },
    setUserId: (id: string) => {
        localStorage.setItem('userId', id);
        set(store => ({user: {...store.user, id: id}}));
    },
    getUserId: () => {
        const { user } = get();
        return user.id || localStorage.getItem('userId');
    },
    setUserEmail: (email: string) => {
        localStorage.setItem('userEmail', email);
        set(store => ({user: {...store.user, email: email}}));
    },
    getUserEmail: () => {
        const { user } = get();
        return user.email || localStorage.getItem('userEmail');
    },
}), shallow);

export default useUserStore;