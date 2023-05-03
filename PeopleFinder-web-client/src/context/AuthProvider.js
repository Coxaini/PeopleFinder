import jwtDecode from "jwt-decode";
import { createContext, useState , useEffect} from "react";

const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
    const [accessToken, setAccessToken] = useState(localStorage.getItem("accessToken") || "");
    
    const [userData, setUserData] = useState( accessToken!== "" ? jwtDecode(accessToken) : {});

    useEffect(() => {
        if(accessToken !== "") {
            setUserData(jwtDecode(accessToken));
        }
    }, [accessToken]);

    return (
        <AuthContext.Provider value={{ accessToken, setAccessToken, userData }}>
        {children}
        </AuthContext.Provider>
    );
}
export default AuthContext;