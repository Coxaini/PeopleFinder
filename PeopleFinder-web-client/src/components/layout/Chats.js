import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from "react-router-dom";

import { Outlet } from 'react-router-dom';
import ChatsList from '../ui/Chats/ChatsList';
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
    
    const [isChatOpen, setIsChatOpen] = useState(params.chatid ?true : false);

    const [activeChatId, setActiveChatId] = useState(params?.chatid);

    const [afterCursor, setAfterCursor] = useState(null);

    const {hubConnection,chats, setChats, activeChat, setActiveChat} = useContext(ChatHubContext);

    const { isLoading, isError, error, metadata } = useCursorPagedData('/chats', setChats, afterCursor, 20);

    const { lastRef } = useInfiniteLoadObserver(metadata, isLoading, setAfterCursor);

   
    

    const navigate = useNavigate();

    useEffect(() => {   
        if (activeChatId) {
            let chat = chats.find(chat => chat.id === activeChatId);
            if(chat){
                setActiveChat(chat); 
            }
            navigate(`/chats/${activeChatId}`);
        }
    }, [activeChatId, chats,navigate, setActiveChat]);


    return (
        <div className='chat-nav-grid'>
            <div className={`border-right chats-container ${isChatOpen?`hidden`:``}`}>
                <div className='chat-list'>
                    {isLoading && <p className='center'>Loading...</p>}
                    {chats.map((chat, index) => (
                        <Chat 
                        ref={index === chats.length - 1 ? lastRef : null}
                        key={chat.id} chat={chat}
                        activeChatId={activeChatId} 
                        setActiveChatId={setActiveChatId}
                        setIsChatOpen= {setIsChatOpen}/>
                    ))
                    }
                </div>
            </div>

            <section className={`chat ${isChatOpen?``:`hidden`}`}>
               <Outlet context={[activeChat,setActiveChat, setIsChatOpen]}/>
            </section>
        </div>


    )
}

export default Chats