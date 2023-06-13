import React from 'react'
import { Link, Outlet } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAddressCard, faUser } from "@fortawesome/free-regular-svg-icons";
import { useTranslation } from 'react-i18next';

function AccountEdit() {
    const {t} = useTranslation();
    return (
        <div className="navigation-grid-centered">
            <div className="border-right navigation">
                <h2>{t("account.account")}</h2>
                <ul>
                    <li>
                        <Link to="/edit/profile">
                            <div>
                                <FontAwesomeIcon icon={faAddressCard} />
                                <span>{t("account.profile")}</span>
                            </div>
                        </Link>
                    </li>
                    <li>
                        <Link to="/edit/user">
                            <div>
                                <FontAwesomeIcon icon={faUser} />
                                <span>{t("account.settings")}</span>
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