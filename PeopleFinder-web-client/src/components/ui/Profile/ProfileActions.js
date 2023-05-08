import React from 'react'

import '../../../css/buttons.css'
import useUserData from '../../../hooks/useUserData';
import { useState } from 'react';
import OverlayCentredPanel from '../OverlayCentredPanel';
import { useNavigate } from 'react-router-dom';
function ProfileActions(props) {


    const [userData] = useUserData();
    const navigate = useNavigate()

    function gotoEdit() {
        navigate('/edit');
    }

    let actions = null;
    if (props.id !== Number(userData.id)) {
        switch (props.status) {
            case "friend":
                actions = <>
                    <button className='approve'>Message</button>
                    <button className='decline'>Remove Friend</button>
                </>
                break;
            case "requestsent":
                actions = <>
                    <button className='decline'>Cancel request</button>
                </>
                break;
            case "requestreceived":
                actions = <>
                    <button className='approve'>Accept Friend Request</button>
                    <button className='decline'>Decline</button>
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
                    <button className='approve'>Add To Friends</button>
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

export default ProfileActions