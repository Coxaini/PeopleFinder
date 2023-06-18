import axios from "axios";

//const BASE_URL = "https://localhost:7273";
//const BASE_URL = "http://217.66.99.154:7273";

export const BASE_URL = process.env.REACT_APP_API_URL;

console.log("BASE_URL", BASE_URL);

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

const apiPrivate = axios.create({
    baseURL: BASE_URL,
    //headers: { 'Content-Type': 'application/json' },
    withCredentials: true
});


export default api;
export { apiPrivate };