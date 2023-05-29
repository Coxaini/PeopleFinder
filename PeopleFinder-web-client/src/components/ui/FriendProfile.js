import React, { forwardRef, useContext } from 'react'
import classes from './Profile.module.css'
import useApiPrivate from '../../hooks/useApiPrivate';
import { Link, useNavigate } from 'react-router-dom';
import useProfileApiActions from '../../hooks/useProfileApiActions';

const FriendProfile = forwardRef((props, ref) => {

    const apiPrivate = useApiPrivate();
  
    const {createDirectChat} = useProfileApiActions({id: props.id});
    const navigate = useNavigate();

    async function removeFriend() {

        props.setFriends((prev) => {

            return prev.filter(friend => friend.id !== props.id);
        });

        try {
            await apiPrivate.delete(`/friends/${props.id}`);

        } catch (error) {
            console.log(error);
        }


    }

    function handleCreateDirectChat(e){
        e.preventDefault();
        createDirectChat().then((res) => {
            navigate(`/chats/${res.data?.id}`);
        }).catch((err) => {
            console.log(err);
        });
    }

    return (
        <Link className='nondecoration' to={`/profile/${props.username}`}>
        <div className={classes.profile} ref={ref} >
            <img className={classes.smallimage} src={props.mainPictureUrl} alt="profile" />
            <div className={classes.smallinfo}>
                <div className={classes.columncontainer}>
                <h2>{props.name}</h2>
                <span className='username'>@{props.username}</span>
                </div>
                <div className={classes.horizontallayout}>
                    <button className={classes.approve} onClick={handleCreateDirectChat}>Send Message</button>
                    <button className={classes.decline} onClick={(e) => { removeFriend() }}>Remove from Friends</button>
                </div>
            </div>
        </div>
        </Link>
    )
});

export default FriendProfile