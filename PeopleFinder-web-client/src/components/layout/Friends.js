

import "../../css/basic.css"
import "../../css/navigation.css"
import { Link, Outlet } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faUserPlus, faUserTag } from "@fortawesome/free-solid-svg-icons";
import {MdPersonSearch} from "react-icons/md";


function Friends() {

  return (
      <div className="navigation-grid-centered">
        <div className="border-right navigation">
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
              <Link to="/friends/search">
                <div className="flexrow aligncenter">
                  <MdPersonSearch size={27}/>
                  <span>Search for users</span>
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