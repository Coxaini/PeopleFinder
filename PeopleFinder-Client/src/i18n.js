import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from "i18next-browser-languagedetector";
import Backend from 'i18next-http-backend';

import translationEng from './locales/en/translation.json';
import translationUkr from './locales/uk/translation.json';

i18n
.use(Backend)
.use(LanguageDetector)
.use(initReactI18next)
.init({
    debug: true,
    // lng: "en",
    fallbackLng: "en", // use en if detected lng is not available

    interpolation: {
      escapeValue: false // react already safes from xss
    },

    resources: {
      en: {
        translations: translationEng
      },
      uk: {
        translations: translationUkr
      }
      
    },
    // have a common namespace used around the full app
    ns: ["translations"],
    defaultNS: "translations"
  });

export default i18n;
    