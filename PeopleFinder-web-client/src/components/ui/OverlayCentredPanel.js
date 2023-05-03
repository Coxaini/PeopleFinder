import React from 'react'

import classes from './OverlayCentredPanel.module.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark } from '@fortawesome/free-solid-svg-icons';

function OverlayCentredPanel(props) {
    return (
        <div className={classes.centeredoverlay} onClick={props.onClick}>
            <div className={classes.whitepanel} onClick={e => e.stopPropagation()}>
                <div className={classes.panelheader}>
                    <div className={`${classes.spacer} ${classes.leftspacer}`} > </div>
                    <h1>
                        <div>
                            {props.title}
                        </div>
                    </h1>
                    <div className={`${classes.spacer} ${classes.rightspacer}`} >
                        <FontAwesomeIcon size='2x' icon={faXmark} className={classes.cancelbutton} onClick={() => {
                            props.onClick();
                        }} />
                    </div>
                </div>
                <div className={classes.panelbody}>
                {props.children}
                </div>
            </div>
        </div>
    )
}

export default OverlayCentredPanel