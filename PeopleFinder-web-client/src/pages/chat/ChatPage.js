import { useOutletContext, useParams } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import classes from './Chat.module.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEllipsisVertical, faArrowLeft, faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import Message from "../../components/ui/Message/Message";

import useInfiniteLoadObserver from "../../hooks/useInfiniteLoadObserver";
import useCursorPagedData from "../../hooks/useCursorPagedData";
import useApiPrivate from "../../hooks/useApiPrivate";

import { AiOutlineSend } from "react-icons/ai";

function ChatPage(props) {

    const [messages, setMessages] = useState([]);
    const [activeChat, setActiveChat, setIsChatOpen] = useOutletContext();

    const params = useParams();
    const [afterCursor, setAfterCursor] = useState(null);

    const apiPrivate = useApiPrivate();
    const [isChatLoading, setIsChatLoading] = useState(true);

    const messageTextArea = useRef(null);
    const messageList = useRef(null);

    const [message, setMessage] = useState('');
    const { isLoading, isError, error, metadata } = useCursorPagedData(`/messages/${params.chatid}`, setMessages, afterCursor, 20, true);
    const { lastRef } = useInfiniteLoadObserver(metadata, isLoading, setAfterCursor);

    useEffect(() => {
        setMessages([]);

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

    function auto_grow() {
        if (messageTextArea.current === null) return;
       // console.log(messageTextArea.current.scrollHeight);
      
        var a =  messageList.current.scrollTop;
        var b = messageList.current.scrollHeight - messageList.current.clientHeight;
        var c = a / b;
    

        messageTextArea.current.style.height = "5px";
        const scrollHeight = messageTextArea.current.scrollHeight;
        messageTextArea.current.style.height = (scrollHeight) + "px";

        // messageList.current.style.height= `calc(100vh - 58px - 77px - ${(Math.min(scrollHeight, 300))+1}px)`;
        if(c === 1){
            messageList.current.scrollTop = messageList.current.scrollHeight - messageList.current.clientHeight;
        }
        
        
       // messageList.current.scrollTop = messageList.current.scrollHeight;

    }

    async function sendMessage(e) {

        e.preventDefault();

        if (message === '') return;
        try {

            let formData = new FormData();
            formData.append('ChatId', activeChat.id);
            formData.append('Text', message);

            const response = await apiPrivate.post(`/messages`, formData)
            const newMessage = { ...response.data, isMine: true };
            setMessages(prev => [...prev, newMessage]);
            setMessage('');
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
                    ref={index === 0 ? lastRef : null}
                    key={message.id}
                    data={message}
                    showDateStamp={showDateStamp} />
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
                {!isLoading ? renderMessages() :
                    <p className='center'>Loading...</p>
                }

                <div id={classes.anchor}></div>
            </div>
            <form className={classes.messagesendform} onSubmit={sendMessage}>
                <textarea
                    value={message}
                    onChange={(event) => { setMessage(event.target.value) }}
                    ref={messageTextArea}
                    placeholder="Type your message..."
                />

                <button type="submit">
                    <AiOutlineSend size={40} />
                </button>
            </form>
            
        </div>
    );


}

export default ChatPage;