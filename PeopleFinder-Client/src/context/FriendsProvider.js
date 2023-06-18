import { useState,createContext } from "react";

const FriendsContext = createContext({});

export const FriendsProvider = ({ children }) => {
    const [friends, setFriends] = useState([]);
    const [pageNum, setPageNum] = useState(1);

    return (
        <FriendsContext.Provider value={{ friends, setFriends, pageNum, setPageNum }}>
        {children}
        </FriendsContext.Provider>
    );
}

export default FriendsContext;