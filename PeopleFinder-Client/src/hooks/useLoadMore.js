import React from 'react'
import { apiPrivate } from '../api/axios';

const useLoadMore =(url, setResults, pageNum , pageSize = 10)=> {

    function loadmore(){
        return apiPrivate.get(url+ `?pageNumber=${pageNum}&pageSize=${pageSize}`)
            .then(data => {
                //setResults(prev => [...prev, ...data?.data.map((item, key) => { return {...item, keyId: prev.length + key}})]);
                setResults(prev => {
                    //console.log(prev.slice(0, (pageNum-1) * pageSize).concat(data?.data));
                    return prev.slice(0, (pageNum-1) * pageSize).concat(data?.data); });
                const metadata = JSON.parse(data?.headers?.['x-pagination']);              
            })
            .catch(error => {
                console.log(error);
            });

    }

  return loadmore;
}
export default useLoadMore;
