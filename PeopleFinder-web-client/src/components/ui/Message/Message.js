import React, { forwardRef, useMemo } from 'react'
import classes from './Message.module.css'

const Message = forwardRef((props, ref) => {

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
      date: mesdatetime.toLocaleDateString(undefined, dateOptions),
      time: mesdatetime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
    };

  }, [data.sentAt]);



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
            <span>{data.text}</span>
            <span className={classes.time}>{datetime.time}</span>
          </div>

      </div>

    </div>
  )

});

export default Message