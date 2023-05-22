import React, { forwardRef, useEffect, useMemo, useRef, useState } from 'react'
import classes from './Message.module.css'
import { faEllipsisVertical } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import OverlayActions from '../Overlay/OverlayActions';
import { AiTwotoneEdit, AiFillDelete } from 'react-icons/ai'
import { BsFillFileEarmarkBinaryFill } from 'react-icons/bs'
import { apiPrivate } from '../../../api/axios';

const Message = forwardRef((props, ref) => {

  const [isActionMenuOpen, setIsActionMenuOpen] = useState(false);
  const messageRef = useRef(null);

  const media = useRef(null);
  const {
    data,
    showDateStamp
  } = props;

  const [attachment, setAttachment] = useState(null);
  const fileLink = useRef(null);

  useEffect(() => {


    let url;
    async function loadFile() {

      if (data.attachmentUrl) {
        //media.current.height = 500; 

        if (data.attachmentType === 'image') {
          const response = await apiPrivate.get(`${data.attachmentUrl}`, { responseType: 'blob' });
          const blob = response?.data;
          url = URL.createObjectURL(blob);
          setAttachment(url);

        }
        else {
          setAttachment(data.attachmentUrl);
        }
      }

      //get image height from attachment
      // media.current.height = 500;
    }
    loadFile();
    return () => {
      if (url) URL.revokeObjectURL(url);
    }

  }, [data, data.attachmentUrl]);

  function renderMedia() {

    if (!data.attachmentUrl)
      return null;
    let mediaElement;
    switch (data.attachmentType) {
      case 'image':
        mediaElement = <img ref={media} src={attachment} alt="attachment" className={classes.mediacontent} />
        break;
      case 'video':
        mediaElement = <video ref={media} src={attachment} alt="attachment" className={classes.mediacontent} controls autoPlay />
        break;
      case 'audio':
        mediaElement = <audio ref={media} src={attachment} alt="attachment" controls />
        break;
      default:

        mediaElement = (
          <div className={classes.filecontent} onClick={() => fileLink.current.click()}>
            <button className='emptybutton'>
              <BsFillFileEarmarkBinaryFill size={30} />
            </button>
            <a href={attachment} ref={fileLink}  rel="noreferrer" hidden></a>
            <div className={classes.fileinfo}>
              <span className={classes.filename}>{data.attachmentName}</span>
            </div>
          </div>
        );
        break;
    }
    return mediaElement
  }

  const datetime = useMemo(() => {
    const mesdatetime = new Date(data.sentAt);
    let dateOptions;
    if (mesdatetime.getFullYear() !== new Date().getFullYear()) {
      dateOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    } else {
      dateOptions = { month: 'long', day: 'numeric' };
    }
    return {
      date: mesdatetime.toLocaleDateString([], dateOptions),
      senttime: mesdatetime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
    };

  }, [data.sentAt]);

  const editedtime = useMemo(() => {
    if (!data.editedAt) return null;
    const mesSentDateTime = new Date(data.sentAt);
    const mesEditDateTime = new Date(data.editedAt);

    let timeOptions;

    if (mesEditDateTime.toDateString() === mesSentDateTime.toDateString()) {
      timeOptions = { hour: '2-digit', minute: '2-digit' };
    } else if (mesEditDateTime.getFullYear() === mesSentDateTime.getFullYear()) {
      timeOptions = { hour: '2-digit', minute: '2-digit', month: 'numeric', day: 'numeric' };
    } else {
      timeOptions = { hour: '2-digit', minute: '2-digit', year: 'numeric', month: 'numeric', day: 'numeric' };
    }

    return mesEditDateTime.toLocaleTimeString([], timeOptions)
  }, [data.editedAt, data.sentAt]);


  return (
    <div className={`${classes.message} ${props.className} ${data.isMine ? classes.mine : ''}`}
      ref={ref}>
      {
        showDateStamp &&
        <div className={classes.datestamp}>
          {datetime.date}
        </div>
      }
      <div className={classes.messagedatacontainer}>

        <div className={classes.bubble}>
          {
            renderMedia()
          }
          <span className={classes.textcontent}>{data.text}</span>
          <div className={classes.bottombar}>
            <span className={classes.time}>{!editedtime ? datetime.senttime : `edited at ${editedtime}`}</span>
            <div className={classes.actionbuttoncontainer}>
              <button className={classes.action} onClick={() => { setIsActionMenuOpen(true) }}>
                <FontAwesomeIcon className='' icon={faEllipsisVertical}
                  style={{ color: "#0a0a0a", transform: "rotate(90deg) scale(1.5)" }} />
              </button>
              <OverlayActions
                onClick={() => { setIsActionMenuOpen(false) }}
                isOpen={isActionMenuOpen}>
                <button onClick={() => {
                  setIsActionMenuOpen(false);
                  props.deleteMessage()
                }
                }>
                  <AiFillDelete size={15} />
                  <span>Delete</span>
                </button>
                <button onClick={() => {
                  setIsActionMenuOpen(false);
                  props.startEditing(data);
                }}>
                  <AiTwotoneEdit size={15} />
                  <span> Edit </span>
                </button>
              </OverlayActions>
            </div>

          </div>


        </div>

      </div>


    </div>
  )

});

export default Message