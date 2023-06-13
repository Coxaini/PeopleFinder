import React from 'react'
import  classes  from "./LoadMoreCard.module.css"
import LoaderSpinner from '../LoaderSpinner'
import { useTranslation } from 'react-i18next';

function LoadMoreCard({ isLoading, handleLoadMore, TotalCount, page }) {

    const {t} = useTranslation();

    return (
        <div className={classes.loadmorecontainer}>
            {
                !isLoading ?
                    <button className={`${classes.loadmore}`} onClick={() => { handleLoadMore() }}>
                        {t("basic.loadmore")}
                        ({TotalCount - page * 4})
                        {t("basic.left")}
                    </button>
                    :
                    <LoaderSpinner scale={1.5} />
            }
        </div>
    )
}

export default LoadMoreCard