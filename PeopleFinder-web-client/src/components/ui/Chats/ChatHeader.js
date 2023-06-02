import React from 'react'
import classes from './ChatHeader.module.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useState, useEffect, useContext } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import useApiPrivate from '../../../hooks/useApiPrivate';
import formatTimeAgo from '../../../helpers/formatTimeAgo';
import ChatHubContext from '../../../context/ChatsHubProvider';
import { faEllipsisVertical, faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import OverlayActions from '../Overlay/OverlayActions';
import { AiFillDelete } from 'react-icons/ai';
import { useTranslation } from 'react-i18next';


function ChatHeader({ activeChat, isChatLoading, setIsChatOpen }) {

    const navigate = useNavigate();

    const [lastSeenAt, setLastSeenAt] = useState("");

    const apiPrivate = useApiPrivate();

    const { hubConnection } = useContext(ChatHubContext);
    const [isChatMenuOpen, setIsChatMenuOpen] = useState(false);

    const {t} = useTranslation();


    useEffect(() => {
        console.log(!activeChat);
        let interval = null;
        if (activeChat) {
            setLastSeenAt(formatTimeAgo(new Date(activeChat.lastSeenAt)));

            interval = setInterval(() => {
                setLastSeenAt(formatTimeAgo(new Date(activeChat.lastSeenAt)));
            }, 60000);
        }
        return () => {
            if (interval)
                clearInterval(interval);
        }

    }, [activeChat]);

    async function handleChatDelete(e) {
        e.preventDefault();
        setIsChatOpen(false);
        try {
            if(activeChat.chatType === 'Direct'){
                await hubConnection.invoke('StopWatchingUserOnlineStatus', activeChat.uniqueTitle);
            }

            await apiPrivate.delete(`/chats/${activeChat.id}`)
        } catch (e) {
            console.log(e);
        }

    }

    function handleChatHeaderClick() {
        if (activeChat.chatType === 'Direct') {
            navigate(`/profile/${activeChat.uniqueTitle}`);
        }
    }

    return (
        <div className={`${classes.chatheader}`}>
            <div className="flexrow">
                <div className="center">
                    <div className={classes.backbutton} onClick={() => { setIsChatOpen(false) }}>
                        <FontAwesomeIcon icon={faArrowLeft} size='2x' style={{ color: "#0a0a0a", }} />
                    </div>
                </div>
                {!isChatLoading ?
                    <div className='flexrow cursorpointer' onClick={handleChatHeaderClick}>
                        <div className={classes.chaticon}>
                            <img src={activeChat.displayLogoUrl} alt='chat icon' />
                        </div>
                        <div className={classes.chatinfo}>
                            <span className={classes.chattitle}>{activeChat.displayTitle}</span>
                            {activeChat?.chatType === 'Direct' ?
                                <span className={classes.chatstatus}>
                                    {activeChat?.isOnline ? t("chat.header.activeNow") :
                                     `${t("chat.header.lastSeen")} ${lastSeenAt}`}
                                </span>
                                : null}
                        </div>
                    </div> : <p className='center'>Loading...</p>
                }
            </div>
            <div className={classes.chatoptions}>
                <button className={` transparentbutton`}
                    onClick={() => setIsChatMenuOpen(true)}>
                    <FontAwesomeIcon icon={faEllipsisVertical} size='2x' style={{ color: "#0a0a0a", }} />
                </button>
                <OverlayActions className='chatheader-overlay-menu'
                    onClick={() => setIsChatMenuOpen(false)}
                    isOpen={isChatMenuOpen}>
                    <button onClick={handleChatDelete}>
                        <AiFillDelete size={18} />
                        <span>{t("chat.header.deleteChat")}</span>
                    </button>
                </OverlayActions>
            </div>

        </div>
    )
}

export default ChatHeader