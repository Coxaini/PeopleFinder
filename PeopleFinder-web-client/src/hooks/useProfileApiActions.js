import useUserData from './useUserData';
import useApiPrivate from './useApiPrivate';
import { useContext } from 'react';
import ChatHubContext from '../context/ChatsHubProvider';

function useProfileApiActions({ id }) {

    const [userData] = useUserData();
    const apiPrivate = useApiPrivate();
    const { hubConnection } = useContext(ChatHubContext);

    function addToFriends() {
        return apiPrivate.post('/friends/request', { friendProfileId: id, comment: '' })
    }

    function removeFromFriends() {
        return apiPrivate.delete(`/friends/${id}`)
    }

    function acceptFriendRequest() {
        return apiPrivate.post(`/friends/requests/${id}`)
    }

    function declineFriendRequest() {
        return apiPrivate.put(`/friends/requests/${id}`)
    }

    function createDirectChat() {
        return apiPrivate.post(`/chats/${id}`)
            .then((response) => {
               
                if (response.data?.isNew === true) {
                    
                    let chatId = response.data?.id;
                    console.log("chat created", chatId);
                    hubConnection?.invoke("JoinChat", chatId);
                }
                return response;
            })
    }

    function cancelFriendRequest() {
        return apiPrivate.delete(`/friends/requests/${id}`)
    }


    return ({ addToFriends, removeFromFriends, acceptFriendRequest, declineFriendRequest, cancelFriendRequest, createDirectChat })
}

export default useProfileApiActions