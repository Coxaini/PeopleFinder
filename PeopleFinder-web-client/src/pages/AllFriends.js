import React, { useState} from 'react'

import useCursorPagedData from '../hooks/useCursorPagedData';
import useInfiniteLoadObserver from '../hooks/useInfiniteLoadObserver';

import FriendProfile from '../components/ui/FriendProfile';

import InfiniteVirtualScroller from '../components/ui/InfiniteVirtualScroller';
import FixedVirtualScroller from '../components/ui/FixedVirtualScroller';
import SearchBar from '../components/ui/common/SearchBar';
function AllFriends() {

  const [afterCursor, setAfterCursor] = useState(null);
  const [friends, setFriends] = useState([])
  const [searchQuery, setSearchQuery] = useState('');
  
  const {isLoading, isError, error, metadata} = 
  useCursorPagedData("/friends", setFriends, afterCursor, 10,false, null, `&searchQuery=${searchQuery}`);

   const { lastFriendRef } = useInfiniteLoadObserver( metadata, isLoading, setAfterCursor);

    if (isError) return <p className='center'>Error: {error.message}</p>

  function searchFriends(search) {
      if(search === searchQuery) return;
      //setFriends([]);
      
      setAfterCursor(null);
      setSearchQuery(search);
  }

  return (
    <>    
      <SearchBar placeholder={"search for friends"} delay={100} search={searchFriends}/>
      <h1>{metadata?.TotalCount} friends</h1>
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