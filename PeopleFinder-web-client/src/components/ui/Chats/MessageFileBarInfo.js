import React from 'react'
import classes from './MessageFileBarInfo.module.css'

function MessageFileBarInfo(props) {

  function renderMedia() {

    switch (props.type.split('/')[0]) {
      case 'image':
        return <img className={classes.media} src={props.fileUrl} alt={'selected'} />
      case 'video':
        return <video className={classes.media} src={props.fileUrl} controls disablePictureInPicture autoPlay loop />
      case 'audio':
        return <audio className={classes.media} src={props.fileUrl} controls />
      default:
        return null
    }
  }

  return (
    <>
    {
      renderMedia()
    }
    </>
  )
}

export default MessageFileBarInfo