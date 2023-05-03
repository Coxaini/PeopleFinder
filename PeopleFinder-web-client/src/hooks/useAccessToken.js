
import { useEffect, useContext } from "react";
import AuthContext from "../context/AuthProvider";

const useAccessToken = () => {

    const {accessToken, setAccessToken} = useContext(AuthContext);

    const setToken = (token) => {
        localStorage.setItem("accessToken", token);
        setAccessToken(token);
    }

    return [accessToken , setToken];
}

export default useAccessToken;