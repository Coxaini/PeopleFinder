import React from 'react'
import classes from './MessageInputBar.module.css'
import { ImCancelCircle } from 'react-icons/im'

function MessageInputBar(props) {

  return (
    <div className={classes.panel}>
      <div className={classes.infogroup}>
        <div className={classes.icon}>
        {props.icon}
        </div>
        <div className={classes.infoflexlist}>
          <span className={classes.title}>{props.title}</span>
          <span className={classes.info}>{props.info}</span>
        </div>
      </div>
      <button className='emptybutton marginright10' onClick={props.onCancel}>
        <ImCancelCircle size={36} />
      </button>
    </div>
  )
}

export default MessageInputBar

// return (
//   <div className={classes.panel}>
//     <div className={classes.infogroup}>
//       {props.icon}
//       <div className={classes.infoflexlist}>
//           <span className={classes.title}>{props.title}</span>
//           <span className={classes.info}>{props.info}</span>
//       </div>
//     </div>
//       <button className='emptybutton' onClick={props.onCancel}>
//           <ImCancelCircle size={24} />
//       </button>
//   </div>