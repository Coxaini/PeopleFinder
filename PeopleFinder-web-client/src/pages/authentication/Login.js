
import { useRef, useState, useEffect } from "react";
import { faCheck, faTimes, faInfoCircle, faCircleQuestion, faRedo } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classes from "./Register.module.css";
import Tooltip from "../../components/ui/Tooltip";
import logo from "../../images/PeopleFinder.png"
import api from "../../api/axios";
import useAccessToken from "../../hooks/useAccessToken";
import { Link, useNavigate, useLocation } from "react-router-dom";
import useUserData from "../../hooks/useUserData";
import { useTranslation } from 'react-i18next';

const Login = () => {

  const { t } = useTranslation();

  const [user, setUser] = useState('');
  const [errmsg, setErrMsg] = useState('');
  const [pwd, setPwd] = useState('');
  const [validForm, setValidForm] = useState(true);
  const [showPwd, setShowPwd] = useState(false);
  const pwdRef = useRef();
  //const [, setToken] = useAccessToken();
  const [, setToken] = useAccessToken();
  const [userData, setUserData] = useUserData();

  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || "/";

  useEffect(() => {
    pwdRef.current.type = showPwd ? 'text' : 'password';
  }, [showPwd])

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {

      const payload = JSON.stringify({ emailOrUsername: user, password: pwd });

      const response = await api.post('/auth/login', payload,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        });

      setUserData(response?.data);

      navigate(from, { replace: true });

    } catch (err) {
      if (!err?.response) {
        setErrMsg(t("common.noServerResponse"));
      } else if (err.response?.status === 400) {
        setErrMsg(t("registration.wrongCredentials"));
      } else if (err.response?.status === 401) {
        setErrMsg(t("registration.unauthorized"));
      } else if (err.response?.status === 404) {
        setErrMsg(t("registration.userNotFound"));
      } else {
        setErrMsg(t("registration.loginFailed"));
      }
    }
  }
  return (
    <>
      <div className={classes.section}>

        <div className={classes.authform}>
          <form onSubmit={handleSubmit}>
            <h1>{t("registration.login")}</h1>

            <label htmlFor="username">{t("registration.username")}</label>
            <div className={classes.inputblock}>
              <input type="text" name="username" id="username"
                onChange={(e) => setUser(e.target.value)}
              />
            </div>


            <label htmlFor="password">{t("registration.password")}</label>
            <div className={classes.inputblock}>
              <input type="password" name="password" id="password"
                ref={pwdRef}
                onChange={(e) => setPwd(e.target.value)}
              />
            </div>

            <div className={classes.pwdsection}>
              <label htmlFor="showpwd">{t("registration.showPassword")}</label>
              <input id="showpwd" type="checkbox" name="showpwd" onChange={(e) => setShowPwd(e.target.checked)} className={classes.showpwdcheckbox} />
            </div>


            <input type="submit" value={t("registration.login")} />
            <p>
              {t("registration.noAccount")}<br />
              <span className={classes.line}>
                <Link to="/register">{t("registration.register")}</Link>
              </span>
            </p>
          </form>

          <div className={`errormsg ${errmsg ? '' : classes.offscreen}`}>
            <FontAwesomeIcon icon={faRedo} color="red" spin={true} />
            <span>{errmsg}</span>
            <FontAwesomeIcon icon={faRedo} color="red" spin={true} />
          </div>
        </div>
      </div>
    </>
  )
}

export default Login