import React from 'react'
import { Outlet } from 'react-router-dom'

function Recommendations() {
    return (
        <div className="panel">
            <div>Recommendations</div>

            <Outlet />
        </div>
    )
}

export default Recommendations