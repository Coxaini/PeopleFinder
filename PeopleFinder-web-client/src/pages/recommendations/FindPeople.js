import { useEffect, useState } from "react";
import useApiPrivate from "../../hooks/useApiPrivate";
import classes from './FindPeople.module.css';

import { useTranslation } from "react-i18next";
import RecsByMutual from "../../components/ui/Recommendations/RecsByMutual";


function FindPeoplePage() {


    const { t } = useTranslation();




    return (
        <div className="panel">
            <h2 className="center">{t("recs.recsByMutual")}</h2>
            <RecsByMutual />
            <h2 className="center">Suggestions by insterests</h2>
           {/*  <div className={classes.recslist}>
                {mutualrecs.map((profile) => {
                    return <RecommendedProfile key={profile.id} profile={profile} />
                }
                )}
            </div> */}

        </div>
    );

}

export default FindPeoplePage;