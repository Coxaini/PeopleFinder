import classes from "./MainNavigation.module.css";
import { Link } from "react-router-dom";

import useUserData from "../../hooks/useUserData";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMessage, faUser, } from "@fortawesome/free-regular-svg-icons";
import { faUsersViewfinder, faUserGroup } from '@fortawesome/free-solid-svg-icons';

import logo from "../../images/sitelogo.png";
import { useTranslation } from 'react-i18next';

function MainNavigation() {

  const [userData,] = useUserData();
  const { t } = useTranslation();

  return (
    <div className={classes.topcontainer}>
      <div className={classes.topnav}>

        <Link to="/">
          <div className={classes.logo}>
            <img src={logo} alt="messenger logo"></img>
            <div className={classes.sitename}>
              <span>PeopleFinder</span>
            </div>
          </div>
        </Link>

        <nav className={classes.topnavmain}>

          <ul>
            <li id={classes["profileitem"]}>
              <Link to={`/profile/${userData.username}`}>
                <div className={classes.icon} id={classes["profile"]}>
                  <FontAwesomeIcon icon={faUser} className={classes.linkicon} />
                  <span>{t("navigation.profile")}</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/chats">
                <div className={classes.icon} id={classes["chat"]}>
                  <FontAwesomeIcon icon={faMessage} className={classes.linkicon} />
                  <span>{t("navigation.messages")}</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/discover">
                <div className={classes.icon} id={classes["search"]}>
                  <FontAwesomeIcon icon={faUsersViewfinder} className={classes.linkicon} />
                  <span>{t("navigation.discoverPeople")}</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/friends">
                <div className={classes.icon} id={classes["friends"]}>
                  <FontAwesomeIcon icon={faUserGroup} className={classes.linkicon} />
                  <span>{t("navigation.friends")}</span>
                </div>
              </Link>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  );
}

export default MainNavigation;
