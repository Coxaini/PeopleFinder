import React, { useEffect, useState } from 'react'

import useCursorPagedData from '../hooks/useCursorPagedData';
import useInfiniteLoadObserver from '../hooks/useInfiniteLoadObserver';

import FriendProfile from '../components/ui/FriendProfile';

import InfiniteVirtualScroller from '../components/ui/InfiniteVirtualScroller';
import FixedVirtualScroller from '../components/ui/FixedVirtualScroller';
import SearchBar from '../components/ui/common/SearchBar';
import { useTranslation } from 'react-i18next';

function AllFriends() {

  const {t} = useTranslation();
  const [afterCursor, setAfterCursor] = useState(null);
  const [friends, setFriends] = useState([])
  const [searchQuery, setSearchQuery] = useState('');

  const { isLoading, isError, error, metadata } =
    useCursorPagedData("/friends", setFriends, afterCursor, 10, false, null, `&searchQuery=${searchQuery}`);

  const { lastFriendRef } = useInfiniteLoadObserver(metadata, isLoading, setAfterCursor);

  const [totalFriends, setTotalFriends] = useState(0);

  useEffect(() => {
    setTotalFriends(metadata?.TotalCount);
  }, [metadata]);

  if (isError) return <p className='center'>{t("common.error")}: {error.message}</p>

  function searchFriends(search) {
    if (search === searchQuery) return;
    //setFriends([]);

    setAfterCursor(null);
    setSearchQuery(search);
  }

  return (
    <>
      <SearchBar placeholder={t("friends.searchForFriends")}
       delay={100} search={searchFriends} />
      <h1>{t("friends.friends", {count : totalFriends})}</h1>
      <InfiniteVirtualScroller items={friends}>
        {(item, index, measure) => {
          return (
            <FriendProfile
              ref={index === friends.length - 1 ? lastFriendRef : null}
              length={friends.length}
              {...item}
              setFriends={setFriends}
              setTotalFriends={setTotalFriends}
              measure={measure}
            />
          )
        }}
      </InfiniteVirtualScroller>

      {/* <ProfileList ref={lastFriendRef} length={friends.length} profiles={friends} Profile = {FriendProfile} /> */}
      {isLoading && <p>{t("common.loading")}</p>}
    </>
  )
}

export default AllFriends