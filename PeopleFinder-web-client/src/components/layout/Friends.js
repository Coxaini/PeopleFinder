

import "../../css/basic.css"
import "../../css/navigation.css"
import { Link, Outlet } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faUserPlus, faUserTag } from "@fortawesome/free-solid-svg-icons";
import {MdPersonSearch} from "react-icons/md";

import { useTranslation } from 'react-i18next';

function Friends() {

  const {t} = useTranslation();

  return (
      <div className="navigation-grid-centered">
        <div className="border-right navigation">
          <h2>{t("friends.friends")}</h2>
          <ul>
            <li>
              <Link to="/friends/all">
                <div>
                  <FontAwesomeIcon icon={faUser} />
                  <span>{t("friends.allFriends")}</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/friends/requests">
                <div>
                  <FontAwesomeIcon icon={faUserPlus} />
                  <span>{t("friends.friendRequests")}</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/friends/search">
                <div className="flexrow aligncenter">
                  <MdPersonSearch size={27}/>
                  <span>{t("friends.searchForUsers")}</span>
                </div>
              </Link>
            </li>
          </ul>
        </div>

        <section>

          <Outlet />

        </section>

      </div>
  );
}

export default Friends;