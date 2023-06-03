import React from 'react'
import { useTranslation } from 'react-i18next';
import RecsByMutual from '../../components/ui/Recommendations/RecsByMutual';

function MutualFriendsProfiles() {
    const { t } = useTranslation();
    return (
        <>
            <h2 className="center">{t("recs.recsByMutual")}</h2>
            <RecsByMutual />
        </>
    )
}

export default MutualFriendsProfiles