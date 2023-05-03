import React, { useState } from 'react'
import { useUserData } from '../../../hooks/useUserData'
import useCursorPagedData from '../../../hooks/useCursorPagedData';
import useInfiniteLoadObserver from '../../../hooks/useInfiniteLoadObserver';
import FriendProfile from '../FriendProfile';

function MutualFriendsList(props) {

  const [mutualFriends, setMutualFriends] = useState([]);

  const [afterCursor, setAfterCursor] = useState(null);
  const { isLoading, isError, error, metadata } =
   useCursorPagedData(`/profile/${props.profileId}/mutualFriends`, setMutualFriends, afterCursor, 12);

  const { lastMutualRef } = useInfiniteLoadObserver(metadata, isLoading, setAfterCursor);

  if (isError) return <p className='center'>Error: {error.message}</p>

  return (
    <div className='flexlist'>
      {mutualFriends.map((item, index) => (
        <FriendProfile ref={index === mutualFriends.length - 1 ? lastMutualRef : null}
          key = {item.id}
          {...item}
          setFriends={setMutualFriends} />))}
    </div>
  );
}

export default MutualFriendsList