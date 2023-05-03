
import { useRef, useState, useEffect } from "react";
import { faCheck, faTimes, faInfoCircle, faCircleQuestion, faRedo } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classes from "./Register.module.css";
import Tooltip from "../../components/ui/Tooltip";
import logo from "../../images/PeopleFinder.png"
import { Link } from "react-router-dom";
import api from "../../api/axios";
import useAccessToken from "../../hooks/useAccessToken";
import { useNavigate, useLocation } from "react-router-dom";


const USER_REGEX = /^[A-z][A-z0-9-_]{3,23}$/;
const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,24}$/;
const EMAIL_REGEX = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

const Register = () => {

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

  const [, setToken] = useAccessToken();
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
    if(validName){
      setNameAvailable(true);
      const checkUser = setTimeout( async ()=>{
        try{
        const response = await api.get(`/auth/check_username/${user}`);
        console.log("Username available");
        setNameAvailable(true);
        }catch(err){
          if(err.response.status === 409){
            console.log("Username is not available");
            setNameAvailable(false);
          }
        }
      }, 300)

      return () => clearTimeout(checkUser);
    }
  }, [user, validName])

  useEffect(() => {
    setEmailAvailable(true);
    if(validEmail){
      const checkEmail = setTimeout( async ()=>{
        try{
        const response = await api.get(`/auth/check_email/${email}`);
        console.log("Email available");
        setEmailAvailable(true);
        }catch(err){
          if(err.response.status === 409){
            console.log("Email is not available");
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

      setErrMsg('Please fill in all fields correctly');
      return;
    }

    try {

      const payload = JSON.stringify({ username: user, email: email, password: pwd });

      const response = await api.post('/auth/register', payload,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        });

      const accessToken = response?.data?.token;

      setToken(accessToken);
      navigate(from, { replace: true });

    } catch (err) {
      if (!err?.response) {
        setErrMsg('No Server Response');
      } else if (err.response?.status === 409) {
        let errMsg = err.response?.data?.title;
        if(errMsg.includes('username') && errMsg.includes('email'))
          setErrMsg('User with given username and email already exists');
        else if(errMsg.includes('login'))
        setErrMsg('User with given username already exists');
        else
        setErrMsg('User with given email already exists');
      } else {
        setErrMsg('Login Failed');
      }

    }

  }

  return (
    <>
      <div className={classes.section}>
        <img src={logo} className={classes.logoImage} />

        <div className={classes.authform}>
          <form onSubmit={handleSubmit}>
            <h1>Registration</h1>

            <label htmlFor="username">Username</label>
            <div className={classes.inputblock}>
              <input type="text" name="username" id="username"
                onFocus={() => setUserFocus(true)}
                onBlur={() => setUserFocus(false)}
                onChange={(e) => setUser(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext="Username must be at least 4 symbols and start with a letter and can contain only letters, numbers, dashes and underscores"
                className={user && !validName ? '' : classes.offscreen}
                hiddentip={userFocus === true ? false : true} />
                <Tooltip icontype={faCircleQuestion}
                tiptext="Username is already taken. Try another one"
                color="#1900f7"
                className={user && validName && !nameAvailable ? '' : classes.offscreen}
                hiddentip={userFocus === true ? false : true} />
            </div>

            <label htmlFor="email">Email</label>
            <div className={classes.inputblock}>
              <input type="email" name="email" id="email"
                onFocus={() => setEmailFocus(true)}
                onBlur={() => setEmailFocus(false)}
                onChange={(e) => setEmail(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext="Email is not correct"
                className={email && !validEmail ? '' : classes.offscreen}
                hiddentip={emailFocus === true ? false : true} />
            <Tooltip icontype={faCircleQuestion}
                tiptext="Email is already taken. Try another one"
                color="#1900f7"
                className={email && validEmail && !emailAvailable ? '' : classes.offscreen}
                hiddentip={emailFocus === true ? false : true} />
            </div>

            <label htmlFor="password">Password</label>
            <div className={classes.inputblock}>
              <input type="password" name="password" id="password"
                ref={pwdRef}
                onFocus={() => setPwdFocus(true)}
                onBlur={() => setPwdFocus(false)}
                onChange={(e) => setPwd(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext="Password should be 8-24 characters long and contain at least one uppercase letter, one lowercase letter, one digit"
                className={pwd && !validPwd ? '' : classes.offscreen}
                hiddentip={pwdFocus === true ? false : true} />
            </div>



            <label htmlFor="password2">Confirm Password</label>
            <div className={classes.inputblock}>
              <input type="password" name="password2" id="password2"
                ref={matchRef}
                onFocus={() => setMatchFocus(true)}
                onBlur={() => setMatchFocus(false)}
                onChange={(e) => setMatchPwd(e.target.value)}
              />
              <Tooltip icontype={faInfoCircle}
                tiptext="Must match the first password input field."
                className={matchPwd && !validMatch ? '' : classes.offscreen}
                hiddentip={matchFocus === true ? false : true}
              />
            </div>

            <div className={classes.pwdsection}>
              <label htmlFor="showpwd">Show password</label>
              <input id="showpwd" type="checkbox" name="showpwd" onChange={(e) => setShowPwd(e.target.checked)} className={classes.showpwdcheckbox} />
            </div>

            <input type="submit" value="Register" />

          </form>

          <div className={`${classes.errormsg} ${errmsg ? '' : classes.offscreen}`}>
            <FontAwesomeIcon icon={faRedo} color="red" spin={true} />
            <span>{errmsg}</span>
            <FontAwesomeIcon icon={faRedo} color="red" spin={true} />
          </div>
          <p>
            Already have an account ?<br />
            <span className={classes.line}>
              <Link to="/login">Log in</Link>
            </span>
          </p>
        </div>
      </div>
    </>
  )

}
export default Register