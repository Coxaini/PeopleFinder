import React from 'react'
import { useRef, useEffect } from 'react';

import { List, AutoSizer, WindowScroller } from 'react-virtualized'

function FixedVirtualScroller(props) {




    return (

        <WindowScroller>
            {({height, isScrolling, registerChild, scrollTop }) => (
                <div style={{ width: "99%" }} ref={registerChild}>
                    <AutoSizer disableHeight >
                        {({ width }) => (
                            <List
                                autoHeight
                                isScrolling={isScrolling}
                                scrollTop={scrollTop}
                                height={height}
                                width={width}
                                rowCount={props.items.length}
                                rowHeight={160}
                                rowRenderer={({ index, key, style }) => {
                                    const item = props.items[index];
                                    return (
                                        <div key={key} style={style} ref={registerChild} >
                                            {props.children(item, index)}
                                        </div>
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

export default FixedVirtualScroller