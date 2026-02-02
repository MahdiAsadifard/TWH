import React, {
    createContext,
    useContext,
    useEffect,
    useState
} from "react";

import { IUserReponseDto  } from "../../Login/Types";

interface IUserContext {
    user: IUserReponseDto | null;
    setUser: (user: IUserReponseDto) => void;
    clearUser: () => void;
    isLoggeding: boolean;
};

const UserContext = createContext<IUserContext | undefined>(undefined);

export const UserProvider = ({children}: {children: React.ReactNode}) => {
    const [user, setUserState] = useState<IUserReponseDto | null>(null);

    // Load from localstorage
    useEffect(()=>{
        const saved = localStorage.getItem('user');
        if(saved){
            setUserState(JSON.parse(saved));
        }
    }, []);

    // Save to localstorage
    const setUser = (user: IUserReponseDto) => {
        setUserState(user);
        localStorage.setItem('user', JSON.stringify(user));
    }

    const clearUser = () => {
        setUserState(null);
        localStorage.removeItem('user');
    };
    const isLoggeding = user !== null;

    return (
        <UserContext.Provider 
            value={{
                user,
                setUser,
                clearUser,
                isLoggeding}}
            >
            {children}
        </UserContext.Provider>
    );
};

export const useUser = () => {
    const ctx = useContext(UserContext);
    if(!ctx) {
        throw new Error("useUser must be used within UserProvider");
    }
    return ctx;
};