import React, { useEffect, useRef, useState } from 'react'
import { useNavigate, useParams } from "react-router-dom";

import { Outlet } from 'react-router-dom';
import useApiPrivate from '../../hooks/useApiPrivate';
import useInfiniteLoadObserver from '../../hooks/useInfiniteLoadObserver';
import useCursorPagedData from '../../hooks/useCursorPagedData';
import Chat from '../ui/Chats/Chat';

import '../../css/chats.css'
import { useContext } from 'react';
import ChatHubContext from '../../context/ChatsHubProvider';


function Chats() {

    //const [chats, setChats] = useState([]);

    const params = useParams();

    const [isChatOpen, setIsChatOpen] = useState(params.chatid ? true : false);

    const [activeChatId, setActiveChatId] = useState(params?.chatid);

    const [afterCursor, setAfterCursor] = useState(null);

    const { hubConnection, chats, setChats, activeChat, setActiveChat } = useContext(ChatHubContext);

    const [delayedChats, setDelayedChats] = useState([]);


    const { isLoading, isError, error, metadata } =
        useCursorPagedData('/chats', setChats, afterCursor, 20, false, (newchats) => setDelayedChats(newchats));

    const { lastRef } = useInfiniteLoadObserver(metadata, isLoading, setAfterCursor);

    const navigate = useNavigate();

    useEffect(() => {

        if (params?.chatid !== activeChatId) {
            setActiveChatId(params?.chatid);
        }
    }, [params?.chatid]);

    useEffect(() => {
        if (hubConnection?.state ==="Connected" && delayedChats.length > 0) {
            hubConnection.invoke('WatchUsersOnlineStatus',
                delayedChats.filter(chat => chat.chatType === 'Direct')
                    .map(chat => chat.uniqueTitle))
                    .then(() => {
                        setDelayedChats([]); //reset delayed chats
                    });
        }

    }, [delayedChats, hubConnection?.state, hubConnection]);

    useEffect(() => {
        if (activeChatId) {
            let chat = chats.find(chat => chat.id === activeChatId);
            if (chat) {
                setActiveChat(chat, () => { navigate(`/chats/${activeChatId}`); });
            }
        }else{
            setActiveChat(null);
        }
    }, [activeChatId, chats, navigate, setActiveChat]);


    return (
        <div className='chat-nav-grid'>
            <div className={`border-right chats-container ${isChatOpen ? `hidden` : ``}`}>
                <div className='chat-list'>
                    {isLoading && <p className='center'>Loading...</p>}
                    {chats.map((chat, index) => (
                        <Chat
                            ref={index === chats.length - 1 ? lastRef : null}
                            key={chat.id} chat={chat}
                            activeChatId={activeChatId}
                            setActiveChat={() => {
                                setActiveChatId(chat.id);
                                setIsChatOpen(true)
                            }}
                        />
                    ))
                    }
                </div>
            </div>

            <section className={`chat ${isChatOpen ? `` : `hidden`}`}>
                <Outlet context={[activeChat, setActiveChat, setIsChatOpen]} />
            </section>
        </div>


    )
}

export default Chats