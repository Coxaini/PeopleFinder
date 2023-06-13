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
        {props.children} 
        </div>
      </div>
      <button type='button' className='transparentbutton' onClick={props.onCancel}>
        <ImCancelCircle size={36} />
      </button>
    </div>
  )
}

export default MessageInputBar
