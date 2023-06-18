import React, { useEffect, useState } from 'react'
import { useTranslation } from 'react-i18next';
import useApiPrivate from '../../../hooks/useApiPrivate';
import classes from './Recs.module.css';
import RecommendedProfile from '../Profile/RecommendedProfile';
import Recommendations from './../../layout/Recommendations';

function RecsByTags() {
    const { t } = useTranslation();
    const [recs, setRecs] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const apiPrivate = useApiPrivate();
    const [error, setError] = useState(null);

    useEffect(() => {
        const controller = new AbortController();
        const { signal } = controller;

        apiPrivate.get("/recs/new", { signal }).then((res) => {
            setRecs( [...res?.data]);
            setIsLoading(false);
        }).catch((err) => {
            setError(err);
        });

        return () => controller.abort();
    }, []);

    if (isLoading) {
        return <div className="center">{t("common.loading")}</div>
    }

    if(!isLoading && recs.length === 0) {
        return <div className="center">{t("recs.noRecommendations")}</div>
    }

    return (
        <div className={classes.recslist}>
        {recs.map((profile) => {
            return <RecommendedProfile key={profile.id} profile={profile} />
        }
        )}
    </div>
    )
}

export default RecsByTags