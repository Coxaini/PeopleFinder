import { HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect } from "react";
import { createContext, useState } from "react";
import useUserData from "../hooks/useUserData";
import { useStateWithCallbackLazy } from 'use-state-with-callback';
import useApiPrivate from "../hooks/useApiPrivate";


const ChatHubContext = createContext({});
export const ChatHubProvider = ({ children }) => {

    const [hubConnection, setHubConnection] = useState(null);

    const [messages, setMessages] = useStateWithCallbackLazy([]);

    const [chats, setChats] = useState([]);
    const [userData] = useUserData();

    const apiPrivate = useApiPrivate();

    function messageSentHandler(message) {
        console.log("Message received", message, userData.id);
        if (message.senderId === Number(userData.id))
            message.isMine = true;
        else
            message.isMine = false;

        setMessages((prevMessages) => [...prevMessages, message]);

        const chat = chats.find(chat => chat.id === message.chatId);

        if (chat) {
            chat.lastMessage = message.text;
            chat.lastMessageAt = message.sentAt;
            chat.lastMessageAuthorName = message.displayName;
            setChats((prevChats) => [ chat, ...prevChats.filter(c => c.id !== chat.id)]);
        } else {
            apiPrivate.get(`/chats/${message.chatId}`)
                .then((response) => {
                    setChats((prevChats) => [response.data ,...prevChats.filter(c => c.id !== message.chatId) ]);
                });
        }
    }

    function messageEditedHandler(message){
        console.log("Message edited", message);

        if (message.senderId === Number(userData.id))
            message.isMine = true;
        else
            message.isMine = false;

        setMessages(prev => prev.map(m => m.id === message.id ? message : m));
    }

    function messageDeletedHandler(message) {
        console.log("Message deleted", message);
        setMessages((prevMessages) => [...prevMessages.filter(m => m.id !== message.messageId)]);
        if(Boolean(message.isLastMessage) === true){
        setChats((prevChats)=>{
            const chat = prevChats.find(chat => chat.id === message.chatId);
            if(chat)
            {
                chat.lastMessage = message.newLastMessage;
                chat.lastMessageAt = message.newLastMessageAt;
                chat.lastMessageAuthorName = message.newLastMessageAuthorName;

                prevChats.sort((a, b) => { return new Date(b.lastMessageAt) - new Date(a.lastMessageAt)});

                if(prevChats.length > 20 && prevChats[prevChats.length-1].id === chat.id){
                    return [...prevChats.filter(c => c.id !== chat.id)];
                }else{
                    return prevChats;
                }
            }else{
                return prevChats;
            }
        });
        }
    }

    useEffect(() => {

        let connection = null;
        const createHubConnection = () => {
            connection = new HubConnectionBuilder()
                .withUrl("https://localhost:7273/chats")
                .withAutomaticReconnect()
                .build();

            connection.on("MessageSent", messageSentHandler);

            connection.on("MessageDeleted", messageDeletedHandler)

            connection.on("MessageEdited", messageEditedHandler)

            connection.on()

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
        <ChatHubContext.Provider value={{ hubConnection, messages, setMessages, chats, setChats }}>
            {children}
        </ChatHubContext.Provider>
    )
}

export default ChatHubContext;