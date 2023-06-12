
import { useRef, useState, useEffect } from "react";
import { faCheck, faTimes, faInfoCircle, faCircleQuestion, faRedo } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classes from "./Register.module.css";
import Tooltip from "../../components/ui/Tooltip";
import { Link } from "react-router-dom";
import api from "../../api/axios";
import { useNavigate, useLocation } from "react-router-dom";
import useUserData from "../../hooks/useUserData";
import { useTranslation } from 'react-i18next';
import useValidationCheck from "../../hooks/useValidationCheck";

const USER_REGEX = /^[A-z][A-z0-9-_]{3,23}$/;
const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,24}$/;
const EMAIL_REGEX = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

const Register = () => {

  const {t} = useTranslation();

  const [user, setUser] = useState('');
  const [validName, setValidName] = useState(false);
  const [userFocus, setUserFocus] = useState(false);

  const [email, setEmail] = useState('');
  const [validEmail, setValidEmail] = useState(false);

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

  const nameAvailable = useValidationCheck(user, validName,"/auth/check_username");
  const emailAvailable = useValidationCheck(email, validEmail,"/auth/check_email");

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
      navigate("/edit", { replace: true });

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
                isVisible={user && !validName}
                hiddentip={!userFocus} />
              <Tooltip icontype={faCircleQuestion}
                tiptext={t('registration.usernameTaken')}
                color="#1900f7"
                isVisible={user && validName && !nameAvailable}
                hiddentip={!userFocus} />
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
                isVisible={email && !validEmail}
                hiddentip={!emailFocus} />
              <Tooltip icontype={faCircleQuestion}
                tiptext={t('registration.emailTaken')}
                color="#1900f7"
                isVisible={email && validEmail && !emailAvailable}
                hiddentip={!emailFocus} />
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
                isVisible={pwd && !validPwd}
                hiddentip={!pwdFocus} />
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
                isVisible={matchPwd && !validMatch}
                hiddentip={!matchFocus}
              />
            </div>

            <div className={classes.pwdsection}>
              <label htmlFor="showpwd">{t("registration.showPassword")}</label>
              <input id="showpwd" type="checkbox" name="showpwd" onChange={(e) => setShowPwd(e.target.checked)} className={classes.showpwdcheckbox} />
            </div>

            <input type="submit" value={t("registration.register")} />

          </form>

          <div className={`errormsg ${errmsg ? '' : classes.offscreen}`}>
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