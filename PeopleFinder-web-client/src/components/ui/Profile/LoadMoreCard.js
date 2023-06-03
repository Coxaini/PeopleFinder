import React from 'react'
import  classes  from "./LoadMoreCard.module.css"
import LoaderSpinner from '../LoaderSpinner'

function LoadMoreCard({ isLoading, handleLoadMore, TotalCount, page }) {
    return (
        <div className={classes.loadmorecontainer}>
            {
                !isLoading ?
                    <button className={`${classes.loadmore}`} onClick={() => { handleLoadMore() }}>
                        Load more
                        ({TotalCount - page * 4} left)
                    </button>
                    :
                    <LoaderSpinner scale={1.5} />
            }
        </div>
    )
}

export default LoadMoreCard