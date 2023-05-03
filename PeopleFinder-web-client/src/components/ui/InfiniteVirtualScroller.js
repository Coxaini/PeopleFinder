import React, { useEffect } from 'react'
import { useRef } from 'react'
import { List, AutoSizer, CellMeasurer, CellMeasurerCache, WindowScroller } from 'react-virtualized'

function InfiniteVirtualScroller(props) {
    const cache = useRef(
        new CellMeasurerCache({
            fixedWidth: true,
            defaultHeight: 160,
        }));
    


    const list = useRef(null);

    useEffect(() => {

        //cache.current.clearAll();
        
        if(list.current) {
        list.current.recomputeRowHeights();
        }

       
    }, [props.items, props.items.length])

    return (

        <WindowScroller>
            {({ height, isScrolling, registerChild, scrollTop }) => (
                <div style={{ width: "99%" }} ref={registerChild}>
                    <AutoSizer disableHeight >
                        {({width}) => (
                            <List
                                autoHeight
                                isScrolling={isScrolling}
                                scrollTop={scrollTop}
                                height={height}
                                width={width}
                                ref={list}
                                rowCount={props.items.length}
                                rowHeight={cache.current.rowHeight}
                                deferredMeasurementCache={cache.current}
                                rowRenderer={({ index, key, style, parent }) => {
                                    const item = props.items[index];
                                    return (
                                        <CellMeasurer key={key}
                                            cache={cache.current}
                                            parent={parent}
                                            columnIndex={0}
                                            rowIndex={index}>
                                            {({ registerChild, measure }) => (
                                                <div style={style} ref={registerChild} >
                                                    {props.children(item, index, measure)}
                                                </div>
                                            )}
                                        </CellMeasurer>
                                    )
                                }}
                            />
                        )}
                    </AutoSizer>
                </div>
            )}
        </WindowScroller>

    )
}

export default InfiniteVirtualScroller