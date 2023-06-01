import React, { forwardRef } from 'react'
import classes from './Profile.module.css'
import { useMemo } from 'react'
import getTimeAgo from '../../helpers/getTimeAgo'
import useProfileApiActions from '../../hooks/useProfileApiActions';
import { Link } from 'react-router-dom';

const FriendRequest = forwardRef((props, ref) => {


    
    const timeAgo = useMemo(() => getTimeAgo(new Date(props.sentAt)), [props.sentAt]);

    const profile = props.profile;

    const {acceptFriendRequest, declineFriendRequest} = useProfileApiActions({id: profile.id});

    function handleAcceptFriendRequest(e){
        e.preventDefault();
        acceptFriendRequest().then(() => {
            props.setTotalRequests((prev) => prev-1);
            props.setRequests((prev) => {
                return prev.filter((item) => item.profile.id !== profile.id);
            });
        }).catch((err) => {
            console.log(err);
        });
    }

    function handleDeclineFriendRequest(e){
        e.preventDefault();
        declineFriendRequest().then(() => {
            props.setRequests((prev) => {
                props.setTotalRequests((prev) => prev-1);
                return prev.filter((item) => item.profile.id !== profile.id);
            });
        }).catch((err) => {
            console.log(err);
        });
    }

    return (
        <Link  to={`/profile/${profile.id}`} className={`${classes.profile} nondecoration`} ref={ref}>
            <div>
            <img className={classes.largeimage} src={profile.mainPictureUrl} alt="profile" />
            <div className={classes.actions}>
                    <button className={classes.approve} onClick={handleAcceptFriendRequest}>Approve</button>
                    <button className={classes.decline} onClick={handleDeclineFriendRequest} >Reject</button>
            </div>
            </div>
            <div className={classes.info}>
                <div className={classes.horizontallayout}><h2>{profile.name}</h2> <span>sent {timeAgo}</span></div>
                <span className={classes.bio}>{profile.bio}</span>
                <div>
                    <ul className={classes.interests}>
                        {profile.tags?.map((interest) => (
                            <li key={interest.id}>{interest.title}</li>
                        ))}
                    </ul>
                </div>
                
            </div>
        </Link>
    )
});

export default FriendRequest