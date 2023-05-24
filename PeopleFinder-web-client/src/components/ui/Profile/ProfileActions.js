import React from 'react'

import '../../../css/buttons.css'
import useUserData from '../../../hooks/useUserData';
import { useState } from 'react';
import OverlayCentredPanel from '../Overlay/OverlayCentredPanel';
import { useNavigate } from 'react-router-dom';
import useApiPrivate from './../../../hooks/useApiPrivate';
import useProfileApiActions from '../../../hooks/useProfileApiActions';
function ProfileActions(props) {


    const [userData] = useUserData();
    const navigate = useNavigate();
    const apiPrivate = useApiPrivate();
    const {addToFriends, removeFromFriends, acceptFriendRequest,declineFriendRequest,cancelFriendRequest ,createDirectChat} = useProfileApiActions({id:props.id});
    const [error , setError] = useState(null);

    function gotoEdit() {
        navigate('/edit');
    }

    function handleAddToFriends() {
        addToFriends().then(() => {
            props.setStatus('requestsent');
        }).catch((err) => {
            console.log(err);
            setError(err);
        });
    }
    function handleRemoveFromFriends(){
        removeFromFriends().then(() => {
            props.setStatus('none');
        }).catch((err) => {
            console.log(err);
            setError(err);
        });
    }

    function handleAcceptFriendRequest(){
        acceptFriendRequest().then(() => {
            props.setStatus('friend');
        }).catch((err) => {
            console.log(err);
            setError(err);
        });
    }

    function handleDeclineFriendRequest(){
        declineFriendRequest().then(() => {
            props.setStatus('none');
        }).catch((err) => {
            console.log(err);
            setError(err);
        });
    }

    function handleCreateDirectChat(){
        createDirectChat().then((res) => {
            navigate(`/chats/${res.data.id}`);
        }).catch((err) => {
            console.log(err);
            setError(err);
        });
    }
    function handleCancelFriendRequest(){
        cancelFriendRequest().then(() => {
            props.setStatus('none');
        }).catch((err) => {
            console.log(err);
            setError(err);
        });
    }

    let actions = null;
    if (props.id !== Number(userData.id)) {
        switch (props.status) {
            case "friend":
                actions = <>
                    <button className='approve' onClick={handleCreateDirectChat}>Message</button>
                    <button className='decline' onClick={handleRemoveFromFriends}>Remove Friend</button>
                </>
                break;
            case "requestsent":
                actions = <>
                    <button className='decline' onClick={handleCancelFriendRequest}>Cancel request</button>
                </>
                break;
            case "requestreceived":
                actions = <>
                    <button className='approve' onClick={handleAcceptFriendRequest}>Accept Friend Request</button>
                    <button className='decline' onClick={handleDeclineFriendRequest}>Decline</button>
                </>
                break;
            case "blockedbyyou" || "blockedbyboth":
                actions = <>
                    <button className='decline'>Unblock</button>
                </>
                break;
            case "blockedbyperson":
                actions = <>
                    <h1>You are blocked by this profile</h1>
                </>
                break;
            default:
                actions = <>
                    <button className='approve' onClick={handleAddToFriends}>Add To Friends</button>
                </>;
        }
    } else {
        actions = <>
            <button className='approve' onClick={gotoEdit}>Edit Profile</button>
        </>;
    }

    return (
        <>
            {actions}
        </>
    )
}

export default ProfileActions;