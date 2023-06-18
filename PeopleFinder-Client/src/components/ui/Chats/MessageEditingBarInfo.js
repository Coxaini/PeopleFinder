import React from 'react'
import classes from './MessageEditingBarInfo.module.css'

function MessageEditingBarInfo(props) {
    return (
        <>
            <span className={classes.title}>{props.title}</span>
            <span className={classes.info}>{props.info}</span>
        </>
    )
}

export default MessageEditingBarInfo