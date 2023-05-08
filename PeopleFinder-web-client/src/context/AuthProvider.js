
import { createContext, useState} from "react";

const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
    
    const [userData, setUserData] = useState({id: localStorage.getItem("id") ,
     username: localStorage.getItem("username")});


    const setUser = (user)=>{
        localStorage.setItem("id", user.id)
        localStorage.setItem("username", user.username)
        setUserData(user);
    }

    return (
        <AuthContext.Provider value={[userData,setUser]}>
        {children}
        </AuthContext.Provider>
    );
}
export default AuthContext;