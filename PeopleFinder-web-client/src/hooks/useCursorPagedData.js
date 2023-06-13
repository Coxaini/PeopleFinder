import { useState, useEffect } from 'react'
import useApiPrivate from './useApiPrivate';

/**
 * A hook to fetch data from an api with cursor pagination 
 *
 * @param  url  The url to fetch data from
 * @param  setResults  The function to set the results
 * @param  after  The cursor to fetch data after
 * @param  pageSize  The page size
 * @param  isReverse  If the data should be reversed
 * @param  dataPipeLineProcessor  A function to process the data before setting it
 * @param searchQuery The search query
 * @returns {isLoading, isError, error, metadata}  The loading state, the error state, the error and the metadata
 */


function useCursorPagedData(url, setResults, after, pageSize = 10, isReverse = false,
     dataPipeLineProcessor = null, searchQuery = null, delayed = false) {


    const [isLoading, setIsLoading] = useState(true)
    const [isError, setIsError] = useState(false)
    const [error, setError] = useState({})
    const [metadata, setMetadata] = useState({})
    const apiPrivate = useApiPrivate();

    useEffect(() => {
        if(delayed) return;

        setIsLoading(true);
        setIsError(false);
        setError({});
        const controller = new AbortController();
        const { signal } = controller;


        apiPrivate.get(url + `?pageSize=${pageSize}` + (after ? `&after=${after}` : ``) + (searchQuery ?? ''), { signal })
            .then( (data) => {
                //setResults(prev => [...prev, ...data?.data.map((item, key) => { return {...item, keyId: prev.length + key}})]);
                let proccesedData = data?.data;
                if (dataPipeLineProcessor) {
                     dataPipeLineProcessor(proccesedData);
                }

                if (!isReverse)
                    if (after)
                        setResults(prev => [...prev, ...proccesedData]);
                    else
                        setResults(proccesedData);
                else {
                    proccesedData.reverse();

                    setResults(prev => [...proccesedData, ...prev]);

                }

                const metadata = JSON.parse(data?.headers?.['x-pagination']);
                setMetadata(metadata);
                setIsLoading(false);
            })
            .catch(error => {
                setIsLoading(false);
                if (signal.aborted) return;
                setIsError(true);
                setError({ message: error.message });
            });

        return () => controller.abort();
    }, [apiPrivate, url, after, pageSize, isReverse, searchQuery, delayed]);

    return { isLoading, isError, error, metadata }
}

export default useCursorPagedData