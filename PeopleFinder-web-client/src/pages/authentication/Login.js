
import { useRef, useState, useEffect} from "react";
import { faCheck, faTimes, faInfoCircle, faCircleQuestion, faRedo } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classes from "./Register.module.css";
import Tooltip from "../../components/ui/Tooltip";
import logo from "../../images/PeopleFinder.png"
import api from "../../api/axios";
import useAccessToken from "../../hooks/useAccessToken";
import { Link, useNavigate , useLocation} from "react-router-dom";
import useUserData from "../../hooks/useUserData";

const Login = () => {

  const [user, setUser] = useState('');


  const [errmsg, setErrMsg] = useState('');


  const [pwd, setPwd] = useState('');

  const [validForm, setValidForm] = useState(true);
  const [showPwd, setShowPwd] = useState(false);

  const pwdRef = useRef();
  //const [, setToken] = useAccessToken();

  const [,setToken] = useAccessToken();
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
        setErrMsg('No Server Response');
    } else if (err.response?.status === 400) {
        setErrMsg('Wrong Credentials');
    } else if (err.response?.status === 401) {
        setErrMsg('Unauthorized');
    } else if (err.response?.status === 404) {
        setErrMsg('User Not Found');
    } else {
        setErrMsg('Login Failed');
    }

    }


  }
  return (
    <>
      <div className={classes.section}>

        <div className={classes.authform}>
          <form onSubmit={handleSubmit}>
            <h1>Login</h1>

            <label htmlFor="username">Username</label>
            <div className={classes.inputblock}>
              <input type="text" name="username" id="username"
                onChange={(e) => setUser(e.target.value)}
              />
            </div>


            <label htmlFor="password">Password</label>
            <div className={classes.inputblock}>
              <input type="password" name="password" id="password"
                ref={pwdRef}
                onChange={(e) => setPwd(e.target.value)}
              />
            </div>

            <div className={classes.pwdsection}>
              <label htmlFor="showpwd">Show password</label>
              <input id="showpwd" type="checkbox" name="showpwd" onChange={(e) => setShowPwd(e.target.checked)} className={classes.showpwdcheckbox} />
            </div>


            <input type="submit" value="Login" />
            <p>
              Need an Account?<br />
              <span className={classes.line}>
                <Link to="/register">Register</Link>
              </span>
            </p>
          </form>

          <div className={`${classes.errormsg} ${errmsg ? '':  classes.offscreen}`}>
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