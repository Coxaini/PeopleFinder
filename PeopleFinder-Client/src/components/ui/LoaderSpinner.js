import React from 'react'
import classes from './LoaderSpinner.module.css'

function LoaderSpinner(props) {
  return (
    <div style={{transform: `scale(${props.scale})`}} className={classes.ldsspinner}><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>
  )
}

export default LoaderSpinner