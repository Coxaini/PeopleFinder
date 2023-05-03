import React, { forwardRef } from 'react'
import FriendProfile from './FriendProfile'

import InfiniteVirtualScroller from './InfiniteVirtualScroller';

const ProfileList = forwardRef((props, ref) => {

    return (
            <InfiniteVirtualScroller items ={props.profiles}>
                {(item, index, measure)=>{
                    return (
                        <props.Profile
                            ref={index === props.length - 1 ? ref : null}
                            {...item}
                            measure = {measure}
                        />
                    )
                }}
            </InfiniteVirtualScroller>
    )
});

export default ProfileList