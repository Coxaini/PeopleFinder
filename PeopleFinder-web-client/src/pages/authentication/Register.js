
import { useRef, useState, useEffect } from "react";
import { faCheck, faTimes, faInfoCircle, faCircleQuestion, faRedo } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classes from "./Register.module.css";
import Tooltip from "../../components/ui/Tooltip";
import logo from "../../images/PeopleFinder.png"
import { Link } from "react-router-dom";
import api from "../../api/axios";
import { useNavigate, useLocation } from "react-router-dom";
import useUserData from "../../hooks/useUserData";
import { useTranslation } from 'react-i18next';

const USER_REGEX = /^[A-z][A-z0-9-_]{3,23}$/;
const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,24}$/;
const EMAIL_REGEX = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

const Register = () => {

  const {t} = useTranslation();

  const [user, setUser] = useState('');
  const [validName, setValidName] = useState(false);
  const [nameAvailable, setNameAvailable] = useState(true);
  const [userFocus, setUserFocus] = useState(false);

  const [email, setEmail] = useState('');
  const [validEmail, setValidEmail] = useState(false);
  const [emailAvailable, setEmailAvailable] = useState(false);
  const [emailFocus, setEmailFocus] = useState(false);

  const [pwd, setPwd] = useState('');
  const [validPwd, setValidPwd] = useState(false);
  const [pwdFocus, setPwdFocus] = useState(false);

  const [matchPwd, setMatchPwd] = useState('');
  const [validMatch, setValidMatch] = useState(false);
  const [matchFocus, setMatchFocus] = useState(false);

  const [errmsg, setErrMsg] = useState('');

  const [showPwd, setShowPwd] = useState(false);

  const pwdRef = useRef();
  const matchRef = useRef();

  const [userData, setUserData] = useUserData();
  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || "/";

  useEffect(() => {
    setValidName(USER_REGEX.test(user));
  }, [user])

  useEffect(() => {
    setValidEmail(EMAIL_REGEX.test(email));
  }, [email])

  useEffect(() => {
    setValidPwd(PWD_REGEX.test(pwd));
    setValidMatch(pwd === matchPwd);
  }, [pwd, matchPwd])

  useEffect(() => {
    pwdRef.current.type = showPwd ? 'text' : 'password';
    matchRef.current.type = showPwd ? 'text' : 'password';
  }, [showPwd])

  useEffect(() => {
    setErrMsg('');
  }, [user, email, pwd, matchPwd])

  useEffect(() => {
    if (validName) {
      setNameAvailable(true);
      const checkUser = setTimeout(async () => {
        try {
          const response = await api.get(`/auth/check_username/${user}`);
          setNameAvailable(true);
        } catch (err) {
          if (err.response.status === 409) {
            setNameAvailable(false);
          }
        }
      }, 300)

      return () => clearTimeout(checkUser);
    }
  }, [user, validName])

  useEffect(() => {
    setEmailAvailable(true);
    if (validEmail) {
      const checkEmail = setTimeout(async () => {
        try {
          const response = await api.get(`/auth/check_email/${email}`);
          setEmailAvailable(true);
        } catch (err) {
          if (err.response.status === 409) {
            setEmailAvailable(false);
          }
        }
      }, 300)

      return () => clearTimeout(checkEmail);
    }
  }, [email, validEmail])

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!(validName && validEmail && validPwd && validMatch)) {

      setErrMsg(t("registration.fillFieldsCorrectly"));
      return;
    }

    try {

      const payload = JSON.stringify({ username: user, email: email, password: pwd });

      const response = await api.post('/auth/register', payload,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        });

      setUserData(response?.data);
      navigate(from, { replace: true });

    } catch (err) {
      if (!err?.response) {
        setErrMsg(t('common.noServerResponse'));
      } else if (err.response?.status === 409) {
        let errMsg = err.response?.data?.title;
        if (errMsg.includes('username') && errMsg.includes('email'))
          setErrMsg(t('registration.userEmailExists'));
        else if (errMsg.includes('login'))
          setErrMsg(t('registration.userExists'));
        else
          setErrMsg(t('registration.emailExists'));
      } else {
        setErrMsg(t('common.serverError'));
      }
    }
  }

  return (
    <>
      <div className={classes.section}>
        <div className={classes.authform}>
          <form onSubmit={handleSubmit}>
            <h1>{t('registration.registration')}</h1>

            <label htmlFor="username">{t('registration.username')}</label>
            <div className={classes.inputblock}>
              <input type="text" name="username" id="username"
                onFocus={() => setUserFocus(true)}
                onBlur={() => setUserFocus(false)}
                onChange={(e) => setUser(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext={t('registration.usernameRequirements')}
                className={user && !validName ? '' : classes.offscreen}
                hiddentip={userFocus === true ? false : true} />
              <Tooltip icontype={faCircleQuestion}
                tiptext={t('registration.emailTaken')}
                color="#1900f7"
                className={user && validName && !nameAvailable ? '' : classes.offscreen}
                hiddentip={userFocus === true ? false : true} />
            </div>

            <label htmlFor="email">{t('registration.email')}</label>
            <div className={classes.inputblock}>
              <input type="email" name="email" id="email"
                onFocus={() => setEmailFocus(true)}
                onBlur={() => setEmailFocus(false)}
                onChange={(e) => setEmail(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext={t("registration.emailIncorrect")}
                className={email && !validEmail ? '' : classes.offscreen}
                hiddentip={emailFocus === true ? false : true} />
              <Tooltip icontype={faCircleQuestion}
                tiptext={t('registration.emailTaken')}
                color="#1900f7"
                className={email && validEmail && !emailAvailable ? '' : classes.offscreen}
                hiddentip={emailFocus === true ? false : true} />
            </div>

            <label htmlFor="password">{t('registration.password')}</label>
            <div className={classes.inputblock}>
              <input type="password" name="password" id="password"
                ref={pwdRef}
                onFocus={() => setPwdFocus(true)}
                onBlur={() => setPwdFocus(false)}
                onChange={(e) => setPwd(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext={t('registration.passwordRequirements')}
                className={pwd && !validPwd ? '' : classes.offscreen}
                hiddentip={pwdFocus === true ? false : true} />
            </div>

            <label htmlFor="password2">{t('registration.confirmPassword')}</label>
            <div className={classes.inputblock}>
              <input type="password" name="password2" id="password2"
                ref={matchRef}
                onFocus={() => setMatchFocus(true)}
                onBlur={() => setMatchFocus(false)}
                onChange={(e) => setMatchPwd(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext={t('registration.passwordMatch')}
                className={matchPwd && !validMatch ? '' : classes.offscreen}
                hiddentip={matchFocus === true ? false : true}
              />
            </div>

            <div className={classes.pwdsection}>
              <label htmlFor="showpwd">{t("registration.showPassword")}</label>
              <input id="showpwd" type="checkbox" name="showpwd" onChange={(e) => setShowPwd(e.target.checked)} className={classes.showpwdcheckbox} />
            </div>

            <input type="submit" value={t("registration.register")} />

          </form>

          <div className={`${classes.errormsg} ${errmsg ? '' : classes.offscreen}`}>
            <FontAwesomeIcon icon={faRedo} color="red" spin={true} />
            <span>{errmsg}</span>
            <FontAwesomeIcon icon={faRedo} color="red" spin={true} />
          </div>
          <p>
            {t("registration.haveAccount")}<br />
            <span className={classes.line}>
              <Link to="/login">{t("registration.log_in")}</Link>
            </span>
          </p>
        </div>
      </div>
    </>
  )

}
export default Register