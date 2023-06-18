import React from 'react'
import classes from './Tag.module.css'

function Tag({title, isSelected, setIsSelected}) {
  return (
    <button type="button" className={`emptybutton ${classes.tag} ${isSelected ? classes.selected : '' }`} 
    onClick={()=>setIsSelected(!isSelected)}>
        <span>{title}</span>
    </button>
  )
}

export default Tag