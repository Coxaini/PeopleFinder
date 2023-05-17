import React from 'react'
import { Link, Outlet } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAddressCard, faUser } from "@fortawesome/free-regular-svg-icons";

function AccountEdit() {
    return (
        <div className="navigation-grid-centered">
            <div className="border-right navigation">
                <h2>Account Settings</h2>
                <ul>
                    <li>
                        <Link to="/edit/profile">
                            <div>
                                <FontAwesomeIcon icon={faAddressCard} />
                                <span>Profile</span>
                            </div>
                        </Link>
                    </li>
                    <li>
                        <Link to="/edit/user">
                            <div>
                                <FontAwesomeIcon icon={faUser} />
                                <span>Login and password</span>
                            </div>
                        </Link>
                    </li>
                </ul>
            </div>

            <section>

                <Outlet />

            </section>

        </div>
    )
}

export default AccountEdit