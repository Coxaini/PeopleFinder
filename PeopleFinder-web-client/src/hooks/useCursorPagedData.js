import { useState, useEffect } from 'react'
import useApiPrivate from './useApiPrivate';


function useCursorPagedData(url, setResults, after, pageSize = 10, isReverse = false, dataPipeLineProcessor = null) {


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


        apiPrivate.get(url + `?pageSize=${pageSize}` + (after ? `&after=${after}` : ``), { signal })
            .then( async (data) =>  {
                //setResults(prev => [...prev, ...data?.data.map((item, key) => { return {...item, keyId: prev.length + key}})]);
                let proccesedData = data?.data;
                if (dataPipeLineProcessor) {
                    proccesedData= await dataPipeLineProcessor(proccesedData);
                    
                }
                

                if (!isReverse)
                    setResults(prev => [...prev, ...proccesedData]);
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
    }, [apiPrivate, url, after, pageSize, isReverse]);

    return { isLoading, isError, error, metadata }
}

export default useCursorPagedData