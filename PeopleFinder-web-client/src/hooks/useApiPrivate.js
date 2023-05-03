
import useAccessToken from "./useAccessToken";
import api, { apiPrivate } from "../api/axios";
import { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
 
const useApiPrivate = () => {

    const [accessToken, setToken] = useAccessToken();

    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        const requestIntercept = apiPrivate.interceptors.request.use(
            config => {
                if (!config.headers['Authorization']) {
                
                    config.headers.Authorization = `Bearer ${accessToken}`;
                }
                return config;
            },
            (error) => Promise.reject(error)
        );

        const responseIntercept = apiPrivate.interceptors.response.use(
            response => response,
            async (error) => {
                const originalRequest = error?.config;
                
                if (error?.response?.status === 401 && !originalRequest?._retry) {
                   
                    originalRequest._retry = true;
                    if (!accessToken) {
                        navigate('/login', { state: { from: location }, replace: true });
                        return Promise.reject(error);
                    }
                    try{
                        const response = await api.get("/auth/refresh",
                            {
                                headers: { 'Authorization': `Bearer ${accessToken}`},
                            }
                        );

                        if (response.status === 200) {
                            setToken(response.data.token);
                            originalRequest.headers['Authorization'] = `Bearer ${response.data.token}`;
                            
                            return apiPrivate(originalRequest);
                        }
                    }catch(err){
                        navigate('/login', { state: { from: location }, replace: true });
                    }
                }
                return Promise.reject(error);
            });

        return () => {
            apiPrivate.interceptors.request.eject(requestIntercept);
            apiPrivate.interceptors.response.eject(responseIntercept);
        }

    }, [accessToken]);

    return apiPrivate;
}

export default useApiPrivate;