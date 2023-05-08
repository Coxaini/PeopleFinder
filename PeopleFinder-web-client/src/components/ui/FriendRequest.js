import React, { forwardRef } from 'react'
import classes from './Profile.module.css'
import { useMemo } from 'react'
import getTimeAgo from '../../helpers/getTimeAgo'

const FriendRequest = forwardRef((props, ref) => {


    const timeAgo = useMemo(() => getTimeAgo(new Date(props.sentAt)), [props.sentAt]);

    const profile = props.profile;

    return (
        <div className={classes.profile} ref={ref}>
            <img className={classes.largeimage} src={profile.mainPictureUrl} alt="profile" />
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
                <div className={classes.actions}>
                    <button className={classes.approve}>Approve</button>
                    <button className={classes.decline}>Ignore</button>
                </div>
            </div>
        </div>
    )
});

export default FriendRequest