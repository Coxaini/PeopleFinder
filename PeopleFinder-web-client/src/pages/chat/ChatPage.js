import { useOutletContext, useParams } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import classes from './Chat.module.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEllipsisVertical, faArrowLeft, faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import Message from "../../components/ui/Message/Message";

import useInfiniteLoadObserver from "../../hooks/useInfiniteLoadObserver";
import useCursorPagedData from "../../hooks/useCursorPagedData";
import useApiPrivate from "../../hooks/useApiPrivate";

import { AiOutlineSend, AiOutlineEdit, AiOutlineCheckCircle } from "react-icons/ai";
import { useStateWithCallbackLazy } from "use-state-with-callback";
import MessageInputBar from "../../components/ui/Chats/MessageInputBar";


function ChatPage(props) {

    const [messages, setMessages] = useStateWithCallbackLazy([]);
    const [activeChat, setActiveChat, setIsChatOpen] = useOutletContext();

    const params = useParams();
    const [afterCursor, setAfterCursor] = useState(null);

    const apiPrivate = useApiPrivate();
    const [isChatLoading, setIsChatLoading] = useState(true);

    const messageTextArea = useRef(null);
    const messageList = useRef(null);

    const [isMessagesInit, setMessagesInit] = useState(false);
    const [message, setMessage] = useState('');

    const [editableMessage, setEditableMessage] = useState(null);

    useEffect(() => {
        setMessages([]);
        setMessagesInit(false);
        setAfterCursor(null);

        if (!activeChat) {
            apiPrivate.get(`/chats/${params.chatid}`)
                .then(response => {
                    setActiveChat(response.data);
                    setIsChatLoading(false);
                })
                .catch(error => {
                    console.log(error);
                });
        } else {
            setIsChatLoading(false);
        }
    }, [params.chatid]);

    useEffect(() => {
        auto_grow();
    }, [message]);


    const { isLoading, isError, error, metadata } = useCursorPagedData(`/messages/${params.chatid}`, setMessages, afterCursor, 20, true);

    useEffect(() => {
        if (messageList.current && !isMessagesInit && messages.length > 0 && !isLoading) {
            console.log('scrolling to bottom');
            messageList.current.scrollTop = messageList.current.scrollHeight - messageList.current.clientHeight;
            setMessagesInit(true);
        }

    }, [messages]);

    const { lastRef } = useInfiniteLoadObserver(metadata, isLoading, setAfterCursor);



    function handleKeyDown(e) {
        if (e.keyCode === 13 && !e.shiftKey) {
            if (editableMessage !== null) {
                editMessage(e);
                return;
            }
            sendMessage(e);
        }
    }

    function auto_grow() {
        if (messageTextArea.current === null) return;

        const c = getScrollProportion(messageList);

        messageTextArea.current.style.height = "5px";
        const scrollHeight = messageTextArea.current.scrollHeight;
        messageTextArea.current.style.height = (scrollHeight) + "px";

        if (c === 1) {
            messageList.current.scrollTop = messageList.current.scrollHeight - messageList.current.clientHeight;
        }

    }

    function getScrollProportion(ref) {
        const a = ref.current.scrollTop;
        const b = ref.current.scrollHeight - ref.current.clientHeight;
        return a / b;
    }

    async function sendMessage(e) {

        e.preventDefault();

        const pattern = /^(|\n|\t)*$/;

        if (pattern.test(message) === true) return;
        try {

            let formData = new FormData();
            formData.append('ChatId', activeChat.id);
            formData.append('Text', message);

            const response = await apiPrivate.post(`/messages`, formData)
            const newMessage = { ...response.data, isMine: true };
            //scrollBarProgress.current = getScrollProportion(messageList);
            const progress = getScrollProportion(messageList);
            setMessages(prev => [...prev, newMessage], () => {
                if (progress === 1)
                    messageList.current.scrollTop = messageList.current.scrollHeight - messageList.current.clientHeight;
            });
            setMessage('');

            // setTimeout(() => {
            //     if(scrollBarProgress.current === 1)
            //     messageList.current.scrollTop = messageList.current.scrollHeight - messageList.current.clientHeight;
            // }, 0);

        }
        catch (error) {
            console.log(error);
        }
    }

    const deleteMessage = async (messageId) => {


        try {
            await apiPrivate.delete(`/messages/${messageId}`);

            setMessages(prev => prev.filter(m => m.id !== messageId));
        }
        catch (error) {
            console.log(error);
        }

    }

    const startEditingMessage = (message) => {
        messageTextArea.current.focus();
        setEditableMessage(message);
        setMessage(message.text);
    }

    const editMessage = async (e) => {

            e.preventDefault();
        
            const pattern = /^(|\n|\t)*$/;
            const editedMessage = editableMessage;

            setEditableMessage(null);
            setMessage('');

            if (pattern.test(message) === true || message === editedMessage.text) return;

            try {
                let formData = new FormData();
                formData.append('messageId', editedMessage.id);
                formData.append('chatId', activeChat.id);
                formData.append('text', message);

                const response = await apiPrivate.put(`/messages`, formData);
                const newMessage = { 
                    ...response.data,
                     isMine: true,
                     displayName : editedMessage.displayName,
                     avatarUrl : editedMessage.avatarUrl };
    
                setMessages(prev => prev.map(m => m.id === editedMessage.id ? newMessage : m));
            }
            catch (error) {
                console.log(error);
            }
    
    }

    function renderMessages() {

        return messages.map((message, index) => {

            let previos = messages[index - 1];
            let next = messages[index + 1];

            let showDateStamp = true;
            if (previos) {
                const previousDateTime = new Date(previos.sentAt).getDate();
                if (previousDateTime !== new Date(message.sentAt).getDate()) {
                    showDateStamp = true;
                } else {
                    showDateStamp = false;
                }
            }

            return (
                <Message
                    ref={index === 0 && isMessagesInit ? lastRef : null}
                    key={message.id}
                    data={message}
                    showDateStamp={showDateStamp}
                    deleteMessage ={()=>{deleteMessage(message.id)}}
                    startEditing= {()=>{startEditingMessage(message)}}
                />
            )
        })
    }

  


    return (
        <div className={classes.chat}>
            <div className={classes.chatheader}>
                <div className="flexrow">
                    <div className="center">
                        <div className={classes.backbutton} onClick={() => { setIsChatOpen(false) }}>
                            <FontAwesomeIcon icon={faArrowLeft} size='2x' style={{ color: "#0a0a0a", }} />
                        </div>
                    </div>
                    {!isChatLoading ?
                        <>
                            <div className={classes.chaticon}>
                                <img src={activeChat.displayLogoUrl} alt='chat icon' />
                            </div>
                            <div className={classes.chatinfo}>
                                <span className={classes.chattitle}>{activeChat.displayTitle}</span>
                                <span className={classes.chatstatus}>Last seen 2 hours ago</span>
                            </div>
                        </> : <p className='center'>Loading...</p>
                    }
                </div>

                <div className={classes.chatoptions}>
                    <FontAwesomeIcon icon={faEllipsisVertical} size='2x' style={{ color: "#0a0a0a", }} />
                </div>
            </div>

            <div className={classes.messagelist} ref={messageList}>
                {isLoading && <p className='center'>Loading...</p>}
                {renderMessages()}
            </div>
            <div className={classes.chatbottom}>
            {
                editableMessage &&
            <MessageInputBar icon={<AiOutlineEdit size={36}/>} title={"Editing"}
            info={editableMessage.text}
            onCancel={()=>{
                setEditableMessage(null);
                setMessage('');
            }} />
            }
            <form className={classes.messagesendform} onSubmit={sendMessage}>
                <textarea
                    onKeyDown={handleKeyDown}
                    value={message}
                    onChange={(event) => { setMessage(event.target.value) }}
                    ref={messageTextArea}
                    placeholder="Type your message..."
                />
                {
                 !editableMessage ?
                <button type="submit">
                    <AiOutlineSend size={40} />
                </button>
                : <button onClick={editMessage}>
                    <AiOutlineCheckCircle size={40} />
                 </button>
                }   
            </form>
            </div>

        </div>
    );


}

export default ChatPage;