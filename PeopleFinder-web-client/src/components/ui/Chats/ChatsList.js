import React from 'react'

import Chat from './Chat'

function ChatsList(props) {
  return (
    <div className='chat-list'>
        {props.chats.map((chat)=>{
            return <Chat key={chat.id} {...chat}/>   
        })   
        }
    </div>
  )
}

export default ChatsList