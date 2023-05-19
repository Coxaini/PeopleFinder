import React, { forwardRef, useMemo, useState } from 'react'
import classes from './Message.module.css'
import { faEllipsisVertical } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import OverlayActions from '../Overlay/OverlayActions';
import {AiTwotoneEdit, AiFillDelete} from 'react-icons/ai'

const Message = forwardRef((props, ref) => {

  const [isActionMenuOpen, setIsActionMenuOpen] = useState(false);

  const {
    data,
    showDateStamp
  } = props;

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
    if(!data.editedAt) return null;
    const mesSentDateTime = new Date(data.sentAt);
    const mesEditDateTime = new Date(data.editedAt);

    let timeOptions;

    if(mesEditDateTime.toDateString() === mesSentDateTime.toDateString()) {
      timeOptions = { hour: '2-digit', minute: '2-digit' };
    } else if(mesEditDateTime.getFullYear() === mesSentDateTime.getFullYear()) {
      timeOptions = {hour: '2-digit', minute: '2-digit' , month: 'numeric', day: 'numeric' };
    }else{
      timeOptions = {hour: '2-digit', minute: '2-digit' ,year: 'numeric', month: 'numeric', day: 'numeric' };
    }

    return mesEditDateTime.toLocaleTimeString([], timeOptions)
  }, [data.editedAt, data.sentAt]);


  return (
    <div className={`${classes.message} ${data.isMine ? classes.mine : ''}`} ref={ref}>
      {
        showDateStamp &&
        <div className={classes.datestamp}>
          {datetime.date}
        </div>
      }
      <div className={classes.messagedatacontainer}>

        <div className={classes.bubble}>
          <span className={classes.textcontent}>{data.text}</span>
          <div className={classes.bottombar}>
            <span className={classes.time}>{!editedtime ? datetime.senttime : `edited at ${editedtime}`}</span>
            <div className={classes.actionbuttoncontainer}>
            <button className={classes.action} onClick={() => { setIsActionMenuOpen(true) }}>
              <FontAwesomeIcon className='' icon={faEllipsisVertical} 
              style={{ color: "#0a0a0a", transform:"rotate(90deg) scale(1.5)" }} />
            </button>
            <OverlayActions
              onClick={() => { setIsActionMenuOpen(false) }}
              isOpen={isActionMenuOpen}>
                <button onClick={() => { 
                  setIsActionMenuOpen(false);
                  props.deleteMessage()}
                  }>
                   <AiFillDelete size={15}/>
                   <span>Delete</span>
                </button>
                <button onClick={() => { 
                  setIsActionMenuOpen(false);
                  props.startEditing(data);
                }}>
                  <AiTwotoneEdit size={15}/>
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