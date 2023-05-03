

import "../../css/basic.css"
import "../../css/navigation.css"
import { Link, Outlet } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faUserPlus, faUserTag } from "@fortawesome/free-solid-svg-icons";
import { FriendsProvider } from "../../context/FriendsProvider";

function Friends() {

  return (
    <FriendsProvider>
      <div className="navigation-grid">
        <div className="panel navigation">
          <h2>Friends</h2>
          <ul>
            <li>

              <Link to="/friends/all">
                <div>
                  <FontAwesomeIcon icon={faUser} />
                  <span>All friends</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/friends/requests">
                <div>
                  <FontAwesomeIcon icon={faUserPlus} />
                  <span>Friend requests</span>
                </div>
              </Link>
            </li>
            <li>
              <Link>
                <div>
                  <FontAwesomeIcon icon={faUserTag} />
                  <span>Sended requests</span>
                </div>
              </Link>
            </li>
          </ul>
        </div>

        <section>

          <Outlet />

        </section>

      </div>
    </FriendsProvider>
  );
}

export default Friends;