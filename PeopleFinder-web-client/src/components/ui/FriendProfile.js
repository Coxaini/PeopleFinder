import React, { forwardRef, useContext } from 'react'
import classes from './Profile.module.css'
import useApiPrivate from '../../hooks/useApiPrivate';
import { Link } from 'react-router-dom';

const FriendProfile = forwardRef((props, ref) => {

    const apiPrivate = useApiPrivate();
    //const {friends, setFriends, pageNum,setPageNum} = useContext(FriendsContext);
    //const loadmore = useLoadMore("/friends" ,setFriends, pageNum, 10);

    async function removeFriend() {

        props.setFriends((prev) => {
            // console.log(prev , props)
            // console.log( prev.filter(friend => friend.key !== props.key));
            return prev.filter(friend => friend.id !== props.id);
        });

        try {
            await apiPrivate.delete(`/friends/${props.id}`);

        } catch (error) {
            console.log(error);
        }


    }

    return (
        <Link className='profilelinkpanel' to={`/profile/${props.username}`}>
        <div className={classes.profile} ref={ref} >
            <img className={classes.smallimage} src="https://placehold.it/100x100" alt="profile" />
            <div className={classes.smallinfo}>
                <div className={classes.columncontainer}>
                <h2>{props.name}</h2>
                <span className='username'>@{props.username}</span>
                </div>
                <div className={classes.horizontallayout}>
                    <button className={classes.approve}>Send Message</button>
                    <button className={classes.decline} onClick={(e) => { removeFriend() }}>Remove from Friends</button>
                </div>
            </div>
        </div>
        </Link>
    )
});

export default FriendProfile