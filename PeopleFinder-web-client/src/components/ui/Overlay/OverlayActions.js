import React from 'react'
import classes from './OverlayActions.module.css'

function OverlayActions(props) {
    return (
        <>

            <div className={`${classes.panel} ${!props.isOpen ? 'nonvisible':'' }`} >
                {props.children}
            </div>
            <div className={`${classes.centeredoverlay} ${!props.isOpen ? 'nonvisible':'' }`} onClick={props.onClick}>
            </div>

        </>
    )
}

export default OverlayActions