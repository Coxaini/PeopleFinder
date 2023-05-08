

import api, { apiPrivate } from "../api/axios";
import { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";

 
const useApiPrivate = () => {
    
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
    
        const responseIntercept = apiPrivate.interceptors.response.use(
            response => response,
            async (error) => {
                const originalRequest = error?.config;
                
                if (error?.response?.status === 401 && !originalRequest?._retry) {
                   
                    originalRequest._retry = true;

                    try{
                        const response = await api.get("/auth/refresh");

                        if (response.status === 200) {
                            
                            return apiPrivate(originalRequest);
                        }
                    }catch(err){
                        navigate('/login', { state: { from: location }, replace: true });
                    }
                }
                return Promise.reject(error);
            });

        return () => {
            apiPrivate.interceptors.response.eject(responseIntercept);
        }

    }, [location, navigate]);

    return apiPrivate;
}

export default useApiPrivate;