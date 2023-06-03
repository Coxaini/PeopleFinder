import { HttpTransportType, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useEffect, useRef } from "react";
import { createContext, useState } from "react";
import useUserData from "../hooks/useUserData";
import { useStateWithCallbackLazy } from 'use-state-with-callback';
import useApiPrivate from "../hooks/useApiPrivate";
import axios from "axios";
import { BASE_URL } from './../api/axios';
import { useNavigate } from "react-router-dom";


const ChatHubContext = createContext({});
export const ChatHubProvider = ({ children }) => {

    const navigate = useNavigate();

    const [hubConnection, setHubConnection] = useState(null);

    const [messages, setMessages] = useStateWithCallbackLazy([]);

    const [activeChat, setActiveChat] = useStateWithCallbackLazy(null);

    const [chats, setChats] = useState([]);
    const [userData] = useUserData();

    const apiPrivate = useApiPrivate();

    const activeChatSnapshot = useRef(activeChat);
    const chatsSnapshot = useRef(chats);
    const hubConnectionSnapshot = useRef(hubConnection);

    useEffect(() => {
        chatsSnapshot.current = chats;
    }, [chats]);

    useEffect(() => {
        activeChatSnapshot.current = activeChat;
    }, [activeChat]);

    useEffect(() => {
        hubConnectionSnapshot.current = hubConnection;
    }, [hubConnection]);


    const currentMessages = useRef(messages);

    function messageSentHandler(message) {

        if (activeChatSnapshot.current?.id === message.chatId) {
            if (message.senderId === Number(userData.id))
                message.isMine = true;
            else
                message.isMine = false;

            setMessages((prevMessages) => [...prevMessages, message]);
        }

        const prevChats = chatsSnapshot.current;
        console.log("prevChats", prevChats);

        const chat = prevChats.find(chat => chat.id === message.chatId);
        if (chat) {
            chat.lastMessage = message.text;
            chat.lastMessageAt = message.sentAt;
            chat.lastMessageAuthorName = message.displayName;
            chat.lastMessageId = message.id;
            setChats([chat, ...prevChats.filter(c => c.id !== chat.id)]);
        } else {
            apiPrivate.get(`/chats/${message.chatId}`)
                .then(response => setChats([response.data, ...prevChats.filter(c => c.id !== message.chatId)]))
        }

    }

    function messageEditedHandler(message) {
        console.log("Message edited", message);
        if (activeChatSnapshot.current?.id === message.chatId) {
            if (message.senderId === Number(userData.id))
                message.isMine = true;
            else
                message.isMine = false;

            setMessages(prev => prev.map(m => m.id === message.id ? message : m));
        }
        
        setChats((prevChats) => {
            return prevChats.map(chat => chat.lastMessageId === message.id ? { ...chat, lastMessage: message.text } : chat);
        });


    }

    function messageDeletedHandler(message) {
        console.log("Message deleted", message);
        if (activeChatSnapshot.current?.id === message.chatId) {
            setMessages((prevMessages) => [...prevMessages.filter(m => m.id !== message.messageId)]);
        }
        if (Boolean(message.isLastMessage) === true) {
            setChats((prevChats) => {
                const newChats = [...prevChats];
                const chat = newChats.find(chat => chat.id === message.chatId);
                if (chat) {
                    chat.lastMessage = message.newLastMessage;
                    chat.lastMessageAt = message.newLastMessageAt;
                    chat.lastMessageAuthorName = message.newLastMessageAuthorName;
                    chat.lastMessageId = message.newLastMessageId;
                    
                    console.log('last message changed', chat);
                    newChats.sort((a, b) => {
                        const dateA = a.lastMessageAt ? new Date(a.lastMessageAt) : new Date(a.createdAt);
                        const dateB = b.lastMessageAt ? new Date(b.lastMessageAt) : new Date(b.createdAt);
                        return dateB - dateA
                    });

                    if (newChats.length > 20 && newChats[newChats.length - 1].id === chat.id) {
                        return [...newChats.filter(c => c.id !== chat.id)];
                    } else {
                        return newChats;
                    }
                } else {
                    return newChats;
                }
            });
        }


    }

    function chatCreatedHandler(chatId) {
        hubConnectionSnapshot.current?.invoke("JoinChat", chatId);
    }

    function chatDeletedHandler(chatId) {
        setChats((prevChats) => [...prevChats.filter(c => c.id !== chatId)]);
        navigate("/chats");
    }

    function userOnlineHandler(username) {
        setChats((prevChats) => {
            if (prevChats.find(chat => chat?.uniqueTitle === username))
                return prevChats.map(chat => chat?.uniqueTitle === username ? { ...chat, isOnline: true } : chat);
            else
                return prevChats;
        });
        // setActiveChat((prevChat) => { 
        //     if (prevChat?.uniqueTitle === username)
        //     return { ...prevChat, isOnline: true } 
        //     else
        //     return prevChat;
        // });
    }

    function userOfflineHandler(username, lastSeenAt) {

        setChats((prevChats) => {
            if (prevChats.find(chat => chat?.uniqueTitle === username))
                return prevChats.map(chat => chat?.uniqueTitle === username ? { ...chat, isOnline: false, lastSeenAt: lastSeenAt } : chat);
            else
                return prevChats;
        });
        // setActiveChat((prevChat) => { 
        //     if (prevChat?.uniqueTitle === username)
        //     return { ...prevChat, isOnline: false, lastSeenAt:lastSeenAt} 
        //     else
        //     return prevChat;
        // });

        //setActiveChat((prevChat) => { return { ...prevChat, isOnline: false, lastSeenAt: lastSeenAt } });
    }

    useEffect(() => {

        let connection = null;
        const createHubConnection = () => {
            connection = new HubConnectionBuilder()
                .configureLogging(LogLevel.Debug)
                .withUrl(`${process.env.REACT_APP_API_URL}/chatHub`)
                .withAutomaticReconnect()
                .build();

            connection.on("MessageSent", messageSentHandler);

            connection.on("MessageDeleted", messageDeletedHandler)

            connection.on("MessageEdited", messageEditedHandler)

            connection.on("DirectChatCreated", chatCreatedHandler)

            connection.on("ChatDeleted", chatDeletedHandler)

            connection.on("UserOnline", userOnlineHandler)

            connection.on("UserOffline", userOfflineHandler);

            connection.start()
                .then(() => {
                    setHubConnection(connection);
                    console.log("Connected to chat hub");
                }).catch((error) => {
                    console.log("Error connecting to chat hub", error);
                });

        }
        if (!hubConnection)
            createHubConnection();

        return () => {
            if (connection) {
                connection.stop();
            }
        }

    }, []);



    return (
        <ChatHubContext.Provider value={{ hubConnection, messages, setMessages, chats, setChats, activeChat, setActiveChat }}>
            {children}
        </ChatHubContext.Provider>
    )
}

export default ChatHubContext;