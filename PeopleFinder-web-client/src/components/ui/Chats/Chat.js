import React, { forwardRef, useMemo } from 'react'
import classes from './Chat.module.css'
import { Link, useNavigate } from 'react-router-dom'
import formatDateTime from '../../../helpers/formatDateTime'


const Chat = forwardRef((props, ref) => {

    const chat = props.chat;
    const formatedDateTime = useMemo(() => {
        if (!chat.lastMessageAt) return formatDateTime(chat.createdAt);
        return formatDateTime(chat.lastMessageAt)}
        , [chat.lastMessageAt, chat.createdAt]
    );


    return (
        <Link id={props.activeChatId === chat.id ? `${classes.active}` : ''}
            onClick={() => {
                props.setActiveChat();
            }}
            className={`${classes.chatitem} nondecoration`} ref={ref}>
            <div className={classes.chaticon}>
                <img src={chat.displayLogoUrl} alt='chat icon' />
                {
                    chat?.isOnline ? <div className={classes.onlineindicator}></div> : null
                }
            </div>
            <div className={classes.chatinfo}>
                <div className={classes.chattop}>
                    <div className={classes.chattitle}>
                        <span className={classes.chattitle}>{chat.displayTitle}</span>
                    </div>
                    <span className={classes.chatdate}>{formatedDateTime}</span>
                </div>
                <div className={classes.chatbottom}>
                    <span className={classes.lastmessage}>{chat.lastMessage}</span>
                </div>
            </div>

        </Link>
    )
});

export default Chat