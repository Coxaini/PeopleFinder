import {useContext} from 'react';
import AuthContext from '../context/AuthProvider';

function useUserData() {
 const {userData} = useContext(AuthContext);
    return {userData};
}

export default useUserData