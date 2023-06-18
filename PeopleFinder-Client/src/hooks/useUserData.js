import {useContext} from 'react';
import AuthContext from '../context/AuthProvider';

function useUserData() {
 const [userData, setUser] = useContext(AuthContext);
    return [userData, setUser];
}

export default useUserData