import axios from "axios";

const BASE_URL = "https://localhost:7273";

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