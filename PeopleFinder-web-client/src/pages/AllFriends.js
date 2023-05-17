import React, { useState} from 'react'

import useCursorPagedData from '../hooks/useCursorPagedData';
import useInfiniteLoadObserver from '../hooks/useInfiniteLoadObserver';

import FriendProfile from '../components/ui/FriendProfile';

import InfiniteVirtualScroller from '../components/ui/InfiniteVirtualScroller';
import FixedVirtualScroller from '../components/ui/FixedVirtualScroller';
function AllFriends() {

  const [afterCursor, setAfterCursor] = useState(null);
  const [friends, setFriends] = useState([])

  
  const {isLoading, isError, error, metadata} = 
  useCursorPagedData("/friends", setFriends, afterCursor, 10);

   const { lastFriendRef } = useInfiniteLoadObserver( metadata, isLoading, setAfterCursor);

    if (isError) return <p className='center'>Error: {error.message}</p>

  return (
    <>    
      <h1>Your friends ({metadata?.TotalCount})</h1>
      <InfiniteVirtualScroller items ={friends}>
                {(item, index, measure)=>{
                    return (
                        <FriendProfile
                            ref={index === friends.length - 1 ? lastFriendRef : null}
                            length={friends.length} 
                            {...item}
                            setFriends = {setFriends}
                            measure = {measure}
                        />
                    )
                }}
            </InfiniteVirtualScroller>

      {/* <ProfileList ref={lastFriendRef} length={friends.length} profiles={friends} Profile = {FriendProfile} /> */}
      {isLoading && <p>Loading...</p>}
    </>
  )
}

export default AllFriends