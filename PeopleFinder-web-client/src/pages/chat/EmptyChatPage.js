import React from 'react'
import { useTranslation } from 'react-i18next';

function EmptyChatPage() {
    const {t} = useTranslation()
    return (
        <div className="centerpanel">
            {t("chat.selectOrStartConversation")}
        </div>
    );
}

export default EmptyChatPage