import React, { useEffect, useState } from 'react'

import useCursorPagedData from '../hooks/useCursorPagedData';
import useInfiniteLoadObserver from '../hooks/useInfiniteLoadObserver';

import FriendRequest from '../components/ui/FriendRequest';

import InfiniteVirtualScroller from '../components/ui/InfiniteVirtualScroller';
function FriendRequests() {
  const [afterCursor, setAfterCursor] = useState(null);

  const [requests, setRequests] = useState([]);
  const { isLoading, isError, error, metadata } =
  useCursorPagedData("/friends/updates", setRequests,afterCursor, 10 );

  const [totalRequests, setTotalRequests] = useState(0);

  const { lastFriendRef } = useInfiniteLoadObserver(metadata, isLoading,setAfterCursor );

  useEffect(() => { 
    setTotalRequests(metadata?.TotalCount);
  }, [metadata]);


  if (isError) return <p className='center'>Error: {error.message}</p>

  return (
    <>
      <h1>Friend requests ({totalRequests})</h1>
      <InfiniteVirtualScroller items ={requests}>
                {(item, index, measure)=>{
                    return (
                        <FriendRequest
                            ref={index === requests.length - 1 ? lastFriendRef : null}
                            length={requests.length} 
                            {...item}
                            setRequests = {setRequests}
                            setTotalRequests = {setTotalRequests}
                            measure = {measure}
                        />
                    )
                }}
      </InfiniteVirtualScroller>
      {isLoading && <p>Loading...</p>}
    </>
  )
}

export default FriendRequests