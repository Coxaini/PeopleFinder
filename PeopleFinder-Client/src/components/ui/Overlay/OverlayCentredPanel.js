import React from 'react'

import classes from './OverlayCentredPanel.module.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { useEffect } from 'react';

function OverlayCentredPanel(props) {
    function closePanel() {
        props.onClick();
    }

    useEffect(() => {
        document.body.style.overflow ='hidden';
        return () => {
            document.body.style.overflow = 'auto';
        }
    }, []);
      

    return (
        <>
            <div className={classes.centeredpanel} onClick={closePanel}>
                <div className={classes.whitepanel} onClick={e => e.stopPropagation()}>
                    <div className={classes.panelheader}>
                        <div className={`${classes.spacer} ${classes.leftspacer}`} > </div>
                        <h1>
                            <div>
                                {props.title}
                            </div>
                        </h1>
                        <div className={`${classes.spacer} ${classes.rightspacer}`} >
                            <FontAwesomeIcon size='2x' icon={faXmark} className={classes.cancelbutton} onClick={closePanel} />
                        </div>
                    </div>
                    <div className={classes.panelbody}>
                        {props.children}
                    </div>
                </div>
            </div>
            <div className={classes.centeredoverlay} onClick={props.onClick}></div>
        </>
    )
}

export default OverlayCentredPanel