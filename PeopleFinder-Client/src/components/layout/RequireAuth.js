import { useLocation, Navigate, Outlet } from "react-router-dom";
import useUserData from "../../hooks/useUserData";

const RequireAuth = () => {
    const [userData] = useUserData();
    const location = useLocation();
    if (userData===null || !userData.id || !userData.username) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }
    return <Outlet />;
}

export default RequireAuth;