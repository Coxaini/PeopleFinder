import { useRef, useCallback } from "react"

function useInfiniteLoadObserver(metadata, isLoading, setAfterCursor) {
    const intObserver = useRef();
    
    const lastRef = useCallback(item => {
        if (isLoading) return;

        if (intObserver.current) intObserver.current.disconnect();

        intObserver.current = new IntersectionObserver(items => {
            if (items[0].isIntersecting && metadata?.NextCursor != null) {
                console.log('We are near the last post!')
                setAfterCursor(metadata.NextCursor)
            }
        });

        if (item) intObserver.current.observe(item);
    }, [isLoading, metadata?.NextCursor, setAfterCursor])

  return {  lastRef };
}

export default useInfiniteLoadObserver