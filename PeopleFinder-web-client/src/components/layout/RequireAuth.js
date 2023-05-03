import { useLocation, Navigate, Outlet } from "react-router-dom";
import useAccessToken from "../../hooks/useAccessToken";

const RequireAuth = () => {
    const [accessToken] = useAccessToken();
    const location = useLocation();
    if (!accessToken) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }
    return <Outlet />;
}

export default RequireAuth;