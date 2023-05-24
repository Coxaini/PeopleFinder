import React, { forwardRef, useContext } from 'react'
import classes from '.././Profile.module.css'

import { Link } from 'react-router-dom';

const ShortProfile = forwardRef((props, ref) => {

    return (
        <Link className='nondecoration' to={`/profile/${props.profile.username}`}>
        <div className={classes.profile} ref={ref} >
            <img className={classes.smallimage} src={props.profile.mainPictureUrl} alt="profile" />
            <div className={classes.smallinfo}>
                <div className={classes.columncontainer}>
                <h2>{props.profile.name}</h2>
                <span className='username'>@{props.profile.username}</span>
                </div>
                <div className={classes.horizontallayout}>
                    
                </div>
            </div>
        </div>
        </Link>
    )
});

export default ShortProfile