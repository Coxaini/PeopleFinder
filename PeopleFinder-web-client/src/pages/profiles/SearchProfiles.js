import React from 'react'
import SearchBar from '../../components/ui/common/SearchBar'
import { useState } from 'react'
import useCursorPagedData from '../../hooks/useCursorPagedData'
import ShortProfile from '../../components/ui/Profile/ShortProfile';
import useInfiniteLoadObserver from '../../hooks/useInfiniteLoadObserver';
import { useSearchParams } from 'react-router-dom';
import { useNavigate, useLocation } from 'react-router';

function SearchProfiles() {

    const [profiles, setProfiles] = useState([]);
    const [afterCursor, setAfterCursor] = useState(null);
    const [searchParams, setSearchParams] = useSearchParams();
    const [searchQuery, setSearchQuery] = useState(searchParams.get('searchQuery'));
    const [isSearching, setIsSearching] = useState(false);
    const navigate = useNavigate()
    

    const { isLoading, isError, error, metadata } =
        useCursorPagedData("/profile/search", setProfiles, afterCursor, 10, false, null, `&searchQuery=${searchQuery}`, !isSearching);

        const {  lastRef } = useInfiniteLoadObserver( metadata, isLoading, setAfterCursor);

    function searchProfiles(search) {
        if(search.length > 0 )
        setSearchParams({searchQuery: search});
        //navigate(`/profiles/search?searchQuery=${search}`, {replace: true});

        if ( search.length <= 3) {
            setIsSearching(false);
            setProfiles([]);
            setAfterCursor(null);
            return;
        }
        //setFriends([]);
        setIsSearching(true);
        setAfterCursor(null);
        setSearchQuery(search);
    }

    return (
        <>
            <SearchBar placeholder={"search for profiles"} delay={100} search={searchProfiles} searchQuery={searchQuery} />
             <div className="flexlist width100 basicmargin">
                {profiles.map((profile, index) => {
                    return(
                        <ShortProfile 
                        ref={index === profiles.length - 1 ? lastRef : null}
                        key={index}
                        profile={profile} />
                    )
                })}
            </div>
            { !isSearching ? <h2 className='center'>Type at least 4 characters to search</h2> : null}
            {!isLoading && profiles.length === 0 && isSearching ? <h2 className='center'>No profiles found</h2> : null}
            {isLoading && isSearching ? <p>Loading...</p> : null}
        </>
    )
}

export default SearchProfiles