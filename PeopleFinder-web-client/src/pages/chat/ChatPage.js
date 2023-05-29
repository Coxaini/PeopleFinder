import { useOutletContext, useParams } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import classes from './Chat.module.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEllipsisVertical, faArrowLeft, faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import Message from "../../components/ui/Message/Message";

import useInfiniteLoadObserver from "../../hooks/useInfiniteLoadObserver";
import useCursorPagedData from "../../hooks/useCursorPagedData";
import useApiPrivate from "../../hooks/useApiPrivate";

import { AiOutlineSend, AiOutlineEdit, AiOutlineCheckCircle, AiOutlinePaperClip, AiFillFile, AiFillDelete } from "react-icons/ai";
import { useStateWithCallbackLazy } from "use-state-with-callback";
import MessageInputBar from "../../components/ui/Chats/MessageInputBar";
import MessageEditingBarInfo from "../../components/ui/Chats/MessageEditingBarInfo";
import { imageExtensions } from "../../constants/fileExtensions";
import MessageFileBarInfo from "../../components/ui/Chats/MessageFileBarInfo";
import { useContext } from "react";
import ChatHubContext from "../../context/ChatsHubProvider";
import OverlayActions from "../../components/ui/Overlay/OverlayActions";

const MessagesPageSize = 20;

function ChatPage(props) {

    //const [messages, setMessages] = useStateWithCallbackLazy([]);
    const [activeChat, setActiveChat, setIsChatOpen] = useOutletContext();

    const params = useParams();
    const [afterCursor, setAfterCursor] = useState(null);

    const apiPrivate = useApiPrivate();
    const [isChatLoading, setIsChatLoading] = useState(true);

    const messageTextArea = useRef(null);
    const messageList = useRef(null);

    const [isMessagesInit, setMessagesInit] = useState(false);
    const [message, setMessage] = useState('');
    const [attachment, setAttachment] = useStateWithCallbackLazy(null);
    const fileUpload = useRef(null);

    const [editableMessage, setEditableMessage] = useStateWithCallbackLazy(null);

    const [isAnchoring, setIsAnchoring] = useState(false);

    const { hubConnection, messages, setMessages } = useContext(ChatHubContext);

    const [isChatMenuOpen, setIsChatMenuOpen] = useState(false);

    useEffect(() => {
        setMessages([]);
        setMessagesInit(false);
        setIsAnchoring(false);
        setAfterCursor(null);

        const controller = new AbortController();


        if (!activeChat || activeChat.id !== params.chatid) {
            console.log('chatid changed');
            setIsChatLoading(true);
            apiPrivate.get(`/chats/${params.chatid}`, {
                signal: controller.signal
            })
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

        return () => {
            controller.abort();
            setMessages([]);
        };

    }, [params.chatid, apiPrivate, setActiveChat, setMessages]);

    useEffect(() => {
        auto_grow();
    }, [message]);


    const { isLoading, isError, error, metadata }
        = useCursorPagedData(`/messages/${params.chatid}`, setMessages, afterCursor, MessagesPageSize, true);

    useEffect(() => {
        if (messageList.current && !isMessagesInit && messages.length > 0 && !isLoading) {

            console.log('scrolling to bottom');
            scrollToBottom(messageList);
            setMessagesInit(true);
        }

    }, [messages, isLoading, isMessagesInit]);

    // useEffect(() => {
    //     if (messageList.current && isMessagesInit) {
    //         const c = getScrollProportion(messageList);
    //         if (c === 1) {
    //             setIsAnchoring(true);
    //         } else {
    //             if (isAnchoring)
    //                 setIsAnchoring(false);
    //         }
    //     }

    // }, [messageList.current?.scrollHeight]);

    const { lastRef } = useInfiniteLoadObserver(metadata, isLoading, setAfterCursor);

    function handleMediaLoad() {
        console.log('media loaded');
        if (messages.length <= MessagesPageSize) {
            scrollToBottom(messageList);
        }
    }

    function handleMessageListScroll(e) {
        const c = getScrollProportion(messageList);
        if (c === 1) {
            setIsAnchoring(true);
        } else {
            if (isAnchoring)
                setIsAnchoring(false);
        }
    }


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
            scrollToBottom(messageList);
        }
    }

    function scrollToBottom(ref) {
        ref.current.scrollTop = ref.current.scrollHeight - ref.current.clientHeight;
    }

    function getScrollProportion(ref) {
        const a = ref.current.scrollTop;
        const b = ref.current.scrollHeight - ref.current.clientHeight;
        return a / b;
    }

    async function sendMessage(e) {

        e.preventDefault();

        const pattern = /^(|\n|\t)*$/;

        let attachmentFile;

        if (fileUpload.current.files && fileUpload.current.files[0]) {
            attachmentFile = fileUpload.current.files[0];
        }

        if (pattern.test(message) === true && !attachmentFile) return;
        try {

            let formData = new FormData();
            formData.append('ChatId', activeChat.id);

            let messageText = message;

            if (message.length === 0) {
                messageText = '\u200B';
            }

            formData.append('Text', messageText);

            if (attachmentFile) {
                console.log(attachmentFile);
                formData.append('Attachment', attachmentFile);
            }


            const response = await apiPrivate.post(`/messages`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });
            // const newMessage = { ...response.data, isMine: true };
            // //scrollBarProgress.current = getScrollProportion(messageList);
            // const progress = getScrollProportion(messageList);

            // setMessages(prev => [...prev, newMessage], () => {
            //     if (progress === 1){
            //         scrollToBottom(messageList);
            //     }
            // });
            setMessage('');
            clearFileSelection();
        }
        catch (error) {
            console.log(error);
        }
    }

    async function handleFileSelect(e) {
        e.preventDefault();

        if (!e.target.files || !e.target.files[0]) {
            return;
        }

        let reader = new FileReader();
        let file = e.target.files[0];
        if (file.size > 100_000_000) {
            console.log("File size is too big. Max size is 100MB");
            return;
        }

        reader.onloadend = () => {
            const progress = getScrollProportion(messageList);
            setAttachment({ name: file.name, url: reader.result, type: file.type }, () => {
                if (progress === 1)
                    scrollToBottom(messageList);
            });
        }
        reader.readAsDataURL(file)


        //setAttachment(e.target.files[0]);
    }

    const deleteMessage = async (messageId) => {
        try {
            await apiPrivate.delete(`/messages/${messageId}`);

            //setMessages(prev => prev.filter(m => m.id !== messageId));
        }
        catch (error) {
            console.log(error);
        }

    }

    const startEditingMessage = (message) => {
        messageTextArea.current.focus();

        const meslistprogress = getScrollProportion(messageList);

        setEditableMessage(message, () => {
            if (meslistprogress === 1)
                scrollToBottom(messageList);
        });
        setMessage(message.text);
        clearFileSelection();
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
            // const newMessage = {
            //     ...response.data,
            //     isMine: true,
            //     displayName: editedMessage.displayName,
            //     avatarUrl: editedMessage.avatarUrl
            // };

            // setMessages(prev => prev.map(m => m.id === editedMessage.id ? newMessage : m));
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
                    className={isAnchoring ? `${classes.lastmessage}` : ``}
                    key={message.id}
                    data={message}
                    showDateStamp={showDateStamp}
                    onMediaLoad={handleMediaLoad}
                    deleteMessage={() => { deleteMessage(message.id) }}
                    startEditing={() => { startEditingMessage(message) }}
                />
            )
        })
    }

    function clearFileSelection() {
        setAttachment(null);
        fileUpload.current.value = null;
    }

    async function handleChatDelete() {
        setIsChatOpen(false);
        try{
            await apiPrivate.delete(`/chats/${activeChat.id}`)
        }catch(e){
            console.log(e);
        }

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
                    <button className={` transparentbutton`}
                        onClick={() => setIsChatMenuOpen(true)}>
                        <FontAwesomeIcon icon={faEllipsisVertical} size='2x' style={{ color: "#0a0a0a", }} />
                    </button>
                    <OverlayActions className='chatheader-overlay-menu'
                        onClick={() => setIsChatMenuOpen(false)}
                        isOpen={isChatMenuOpen}>
                            <button onClick={handleChatDelete}>
                                <AiFillDelete size={18} />
                                <span>Delete chat</span>
                            </button>
                    </OverlayActions>
                    </div>
                
            </div>

            <div className={classes.messagelist} ref={messageList} onScroll={handleMessageListScroll}>
                {isLoading && <p className='center'>Loading...</p>}
                {renderMessages()}
                <div id={classes.anchor}></div>
            </div>
            <div className={classes.chatbottom}>
                {
                    editableMessage && attachment === null ?
                        <MessageInputBar icon={<AiOutlineEdit size={36} />}
                            onCancel={() => {
                                setEditableMessage(null);
                                setMessage('');
                            }}>
                            <MessageEditingBarInfo title={"Editing..."} info={editableMessage.text} />
                        </MessageInputBar>
                        : attachment &&
                        <MessageInputBar icon={<AiFillFile size={36} />}
                            onCancel={() => {
                                clearFileSelection();
                            }}
                        >
                            <MessageFileBarInfo fileName={attachment.name} fileUrl={attachment.url} type={attachment.type} />
                        </MessageInputBar>
                }

                <form className={classes.messagesendform} onSubmit={sendMessage}>
                    <input type="file" id="upload" onChange={handleFileSelect} ref={fileUpload} hidden />
                    <button onClick={(e) => {
                        e.preventDefault();
                        fileUpload.current.click();
                    }}>
                        <AiOutlinePaperClip size={40} />
                    </button>

                    <textarea
                        onKeyDown={handleKeyDown}
                        value={message}
                        onChange={(event) => { setMessage(event.target.value) }}
                        ref={messageTextArea}
                        placeholder="Type your message..."
                    />
                    {
                        !editableMessage || attachment !== null ?
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