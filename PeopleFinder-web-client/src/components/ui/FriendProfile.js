import React, { forwardRef, useContext } from 'react'
import classes from './Profile.module.css'
import useApiPrivate from '../../hooks/useApiPrivate';
import { Link, useNavigate } from 'react-router-dom';
import useProfileApiActions from '../../hooks/useProfileApiActions';
import { AiFillDelete } from 'react-icons/ai'
import { RiMessage3Fill } from 'react-icons/ri'
import { useTranslation } from 'react-i18next';

const FriendProfile = forwardRef((props, ref) => {

    const apiPrivate = useApiPrivate();

    const { createDirectChat } = useProfileApiActions({ id: props.id });
    const navigate = useNavigate();

    const { t } = useTranslation();

    async function handleRemovingFriend(e) {

        e.preventDefault();

        props.setFriends((prev) => {

            return prev.filter(friend => friend.id !== props.id);
        });

        try {
            await apiPrivate.delete(`/friends/${props.id}`);
            props.setTotalFriends((prev) => prev - 1);

        } catch (error) {
            console.log(error);
        }


    }

    function handleCreateDirectChat(e) {
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
                        <button className={classes.approve} title={t("profile.sendMessage")} onClick={handleCreateDirectChat}>
                            <RiMessage3Fill size={21} />
                        </button>
                        <button className={classes.decline}
                            title={t("profile.deleteFromFriends")} onClick={handleRemovingFriend}>
                            <AiFillDelete size={21} />
                        </button>
                    </div>
                </div>
            </div>
        </Link>
    )
});

export default FriendProfile