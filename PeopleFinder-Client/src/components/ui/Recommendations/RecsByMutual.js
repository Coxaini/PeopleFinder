import React from 'react'
import { useEffect, useState } from "react";
import useApiPrivate from '../../../hooks/useApiPrivate';
import classes from './Recs.module.css';
import RecommendedProfile from '../Profile/RecommendedProfile';
import LoadMoreCard from '../Profile/LoadMoreCard';
import { useTranslation } from 'react-i18next';


function RecsByMutual() {

    const [mutualrecs, setMutualrecs] = useState([]);
    const apiPrivate = useApiPrivate();
    const [isLoading, setIsLoading] = useState(true);
    const [metadata, setMetadata] = useState(null);

    const [page, setPage] = useState(1);

    const {t} = useTranslation();

    useEffect(() => {
        if (metadata?.HasNext === false) return;

        const controller = new AbortController();
        const { signal } = controller;
    
        apiPrivate.get("/recs/mutual?PageNumber=" + page + "&PageSize=4", { signal }).then((res) => {
            setMutualrecs((prev) => [...prev, ...res?.data]);
            const metadata = JSON.parse(res?.headers?.['x-pagination']);
            setMetadata(metadata);
            setIsLoading(false);
        }).catch((err) => {
            console.log(err);
        });

        return () => controller.abort();
    }, [page]);

    function handleLoadMore() {
        if (metadata?.HasNext === false) return;
        setIsLoading(true);
        setPage(prev => prev + 1);
    }

    if(!isLoading && mutualrecs.length === 0) {
        return <div className="center">{t("recs.noMutualFriends")}</div>
    }

    return (
        <div className={classes.recslist}>
            {mutualrecs.map((profile) => {
                return <RecommendedProfile key={profile.id} profile={profile} />
            }
            )}
            {
                (metadata === null || metadata?.HasNext) ?
                    <LoadMoreCard {...{isLoading, handleLoadMore, TotalCount : metadata?.TotalCount, page}} />
                    : null
            }
        </div>
    )
}

export default RecsByMutual