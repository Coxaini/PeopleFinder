import { HttpTransportType, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useEffect, useRef } from "react";
import { createContext, useState } from "react";
import useUserData from "../hooks/useUserData";
import { useStateWithCallbackLazy } from 'use-state-with-callback';
import useApiPrivate from "../hooks/useApiPrivate";
import axios from "axios";
import { BASE_URL } from './../api/axios';


const ChatHubContext = createContext({});
export const ChatHubProvider = ({ children }) => {

    const [hubConnection, setHubConnection] = useState(null);

    const [messages, setMessages] = useStateWithCallbackLazy([]);

    const [activeChat, setActiveChat] = useState(null);

    const [chats, setChats] = useState([]);
    const [userData] = useUserData();

    const apiPrivate = useApiPrivate();

    const activeChatSnapshot = useRef(activeChat);
    const hubConnectionSnapshot = useRef(hubConnection);

    useEffect(() => {
        activeChatSnapshot.current = activeChat;
    }, [activeChat]);

    useEffect(() => {
        hubConnectionSnapshot.current = hubConnection;
    }, [hubConnection]);

    const currentMessages = useRef(messages);

    function messageSentHandler(message) {
        console.log("Message received", chats, message.chatId);

        if (activeChatSnapshot.current?.id === message.chatId) {
            if (message.senderId === Number(userData.id))
                message.isMine = true;
            else
                message.isMine = false;

            setMessages((prevMessages) => [...prevMessages, message]);
        }

        const chat = chats.find(chat => chat.id === message.chatId);

        if (chat) {
            chat.lastMessage = message.text;
            chat.lastMessageAt = message.sentAt;
            chat.lastMessageAuthorName = message.displayName;
            setChats((prevChats) => [chat, ...prevChats.filter(c => c.id !== chat.id)]);
        } else {
            apiPrivate.get(`/chats/${message.chatId}`)
                .then((response) => {
                    setChats((prevChats) => [response.data, ...prevChats.filter(c => c.id !== message.chatId)]);
                });
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
                const chat = prevChats.find(chat => chat.id === message.chatId);
                if (chat) {
                    chat.lastMessage = message.newLastMessage;
                    chat.lastMessageAt = message.newLastMessageAt;
                    chat.lastMessageAuthorName = message.newLastMessageAuthorName;

                    prevChats.sort((a, b) => { return new Date(b.lastMessageAt) - new Date(a.lastMessageAt) });

                    if (prevChats.length > 20 && prevChats[prevChats.length - 1].id === chat.id) {
                        return [...prevChats.filter(c => c.id !== chat.id)];
                    } else {
                        return prevChats;
                    }
                } else {
                    return prevChats;
                }
            });
        }


    }

    function chatCreatedHandler(chatId) {
        console.log("Chat created", );
        hubConnectionSnapshot.current?.invoke("JoinChat", chatId);
    }


    useEffect(() => {

        let connection = null;
        const createHubConnection = () => {
            connection = new HubConnectionBuilder()
                .configureLogging(LogLevel.Debug)
                //.withUrl("https://localhost:7273/chats")
                .withUrl(`${process.env.REACT_APP_API_URL}/chatHub`, {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                .withAutomaticReconnect()
                .build();

            connection.on("MessageSent", messageSentHandler);

            connection.on("MessageDeleted", messageDeletedHandler)

            connection.on("MessageEdited", messageEditedHandler)

            connection.on("DirectChatCreated", chatCreatedHandler)

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