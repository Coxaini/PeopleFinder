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

function useCursorPagedData(url, setResults, after, pageSize = 10, isReverse = false, dataPipeLineProcessor = null, searchQuery = null) {


    const [isLoading, setIsLoading] = useState(true)
    const [isError, setIsError] = useState(false)
    const [error, setError] = useState({})
    const [metadata, setMetadata] = useState({})
    const apiPrivate = useApiPrivate();

    useEffect(() => {

        setIsLoading(true);
        setIsError(false);
        setError({});
        const controller = new AbortController();
        const { signal } = controller;


        apiPrivate.get(url + `?pageSize=${pageSize}` + (after ? `&after=${after}` : ``) + (searchQuery ?? ''), { signal })
            .then(async (data) => {
                //setResults(prev => [...prev, ...data?.data.map((item, key) => { return {...item, keyId: prev.length + key}})]);
                let proccesedData = data?.data;
                if (dataPipeLineProcessor) {
                    proccesedData = await dataPipeLineProcessor(proccesedData);
                }

                if (!isReverse)
                    setResults(prev => [...prev, ...proccesedData]);
                else {
                    proccesedData.reverse();
                    if (after)
                        setResults(prev => [...proccesedData, ...prev]);
                    else
                        setResults(proccesedData);
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
    }, [apiPrivate, url, after, pageSize, isReverse, dataPipeLineProcessor, searchQuery]);

    return { isLoading, isError, error, metadata }
}

export default useCursorPagedData