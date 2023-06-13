import { useEffect, useState } from "react";
import useApiPrivate from "../../hooks/useApiPrivate";
import classes from './FindPeople.module.css';

import { useTranslation } from "react-i18next";
import RecsByMutual from "../../components/ui/Recommendations/RecsByMutual";
import RecsByTags from "../../components/ui/Recommendations/RecsByTags";


function FindPeoplePage() {


    const { t } = useTranslation();




    return (
        <div className="panel">
            <h1 className="center">{t("recs.recsByInterests")}</h1>
            <RecsByTags />
        </div>
    );

}

export default FindPeoplePage;