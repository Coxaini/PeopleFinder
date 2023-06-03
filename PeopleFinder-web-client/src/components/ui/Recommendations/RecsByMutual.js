import React from 'react'
import { useEffect, useState } from "react";
import useApiPrivate from '../../../hooks/useApiPrivate';
import classes from './RecsByMutual.module.css';
import RecommendedProfile from '../Profile/RecommendedProfile';
import LoaderSpinner from '../LoaderSpinner';


function RecsByMutual() {

    const [mutualrecs, setMutualrecs] = useState([]);
    const apiPrivate = useApiPrivate();
    const [isLoading, setIsLoading] = useState(true);
    const [metadata, setMetadata] = useState(null);

    const [page, setPage] = useState(1);


    useEffect(() => {
        if (metadata?.HasNext === false) return;

        const controller = new AbortController();
        const { signal } = controller;
    
        const res = apiPrivate.get("/recs/mutual?PageNumber=" + page + "&PageSize=4", { signal }).then((res) => {
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

    return (
        <div className={classes.recslist}>
            {mutualrecs.map((profile) => {
                return <RecommendedProfile key={profile.id} profile={profile} />
            }
            )}
            {
                (metadata === null || metadata?.HasNext) ?
                    <div className={classes.loadmorecontainer}>
                        {
                            !isLoading ?
                                <button className={`${classes.loadmore}`} onClick={() => { handleLoadMore()}}>
                                    Load more
                                    ({metadata?.TotalCount-page*4} left)
                                </button>
                                :
                                <LoaderSpinner scale={1.5} />
                        }
                    </div>
                    : null
            }
        </div>
    )
}

export default RecsByMutual