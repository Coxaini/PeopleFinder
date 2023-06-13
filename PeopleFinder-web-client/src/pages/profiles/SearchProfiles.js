import React from 'react'
import SearchBar from '../../components/ui/common/SearchBar'
import { useState } from 'react'
import useCursorPagedData from '../../hooks/useCursorPagedData'
import ShortProfile from '../../components/ui/Profile/ShortProfile';
import useInfiniteLoadObserver from '../../hooks/useInfiniteLoadObserver';
import { useSearchParams } from 'react-router-dom';
import { useNavigate, useLocation } from 'react-router';
import { useTranslation } from 'react-i18next';

function SearchProfiles() {

    const [profiles, setProfiles] = useState([]);
    const [afterCursor, setAfterCursor] = useState(null);
    const [searchParams, setSearchParams] = useSearchParams();
    const [searchQuery, setSearchQuery] = useState(searchParams.get('searchQuery'));
    const [isSearching, setIsSearching] = useState(false);
    const navigate = useNavigate()
    
    const {t} = useTranslation();

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
            <SearchBar placeholder={t("profile.searchForPeople")} delay={100} search={searchProfiles} searchQuery={searchQuery} />
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
            { !isSearching ? <h2 className='center'>{t("profile.typeAtLeast4chars")}</h2> : null}
            {!isLoading && profiles.length === 0 && isSearching ? <h2 className='center'>{t("profile.noUsersFound")}</h2> : null}
            {isLoading && isSearching ? <p>{t("common.loading")}</p> : null}
        </>
    )
}

export default SearchProfiles