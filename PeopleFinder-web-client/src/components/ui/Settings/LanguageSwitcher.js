import React from 'react'

import { useTranslation } from 'react-i18next';
import classes from './LanguageSwitcher.module.css';

const lngs = {
    en: { nativeName: 'English' },
    uk: { nativeName: 'Українська' },
};

function LanguageSwitcher() {
    const { t, i18n } = useTranslation();
    function handleSwitchLanguage(lng) {
        i18n.changeLanguage(lng)
    }
    return (
        <div className={classes.switcher}>
            <h1>{t('settings.chooseLanguage')}</h1>
            <div className='flexrow'>
                {
                    Object.keys(lngs).map((lng) => (
                        <button key={lng} id={i18n.language === lng ? classes.selected : ""}
                            type='button'
                            onClick={()=>handleSwitchLanguage(lng)}>
                            {lngs[lng].nativeName}
                        </button>
                    ))
                }
            </div>
        </div>
    )
}

export default LanguageSwitcher