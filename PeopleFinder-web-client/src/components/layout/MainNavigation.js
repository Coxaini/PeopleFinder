import classes from "./MainNavigation.module.css";
import { Link } from "react-router-dom";

import useUserData from "../../hooks/useUserData";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMessage, faUser, } from "@fortawesome/free-regular-svg-icons";
import {  faUsersViewfinder, faUserGroup } from '@fortawesome/free-solid-svg-icons';

function MainNavigation() {

  const { userData } = useUserData();

  return (
    <div className={classes.topcontainer}>
      <div className={classes.topnav}>

      <Link to="/">
          <div className={classes.logo}>
            <img src="https://i.imgur.com/Pz7MJvt.png"></img>
            <div className={classes.sitename}>
              <span>My social </span>
              <span>media</span>
            </div>
          </div>
        </Link>

        <nav className={classes.topnavmain}>
        
          <ul>
            <li id = {classes["profileitem"]}>
              <Link to={`/profile/${userData.unique_name}`}>
                <div className={classes.icon} id={classes["profile"]}>
                  <FontAwesomeIcon icon={faUser} className={classes.linkicon} />
                 <span>Profile</span> 
                </div>
              </Link>
            </li>
            <li>
              <Link to="/chat">
                <div className={classes.icon} id={classes["chat"]}>
                <FontAwesomeIcon icon={faMessage} className={classes.linkicon}/>
                  <span>Messages</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/">
                <div className={classes.icon} id={classes["search"]}>
                <FontAwesomeIcon icon={faUsersViewfinder} className={classes.linkicon} />
                  <span>Find People</span>
                </div>
              </Link>
            </li>
            <li>
              <Link to="/friends">
                <div className={classes.icon} id={classes["friends"]}>
                <FontAwesomeIcon icon={faUserGroup} className={classes.linkicon} />
                  <span>Friends</span>
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
