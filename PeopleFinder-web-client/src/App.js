import { Routes, Route } from "react-router-dom";

import FindPeoplePage from "./pages/recommendations/FindPeople";
import ProfilePage from "./pages/Profile";
import ChatPage from "./pages/chat/ChatPage";
import FriendNav from "./components/layout/Friends";
import AllFriends from "./pages/AllFriends";
import FriendRequests from "./pages/FriendRequests";

import Layout from "./components/layout/Layout";

import Register from "./pages/authentication/Register";
import Login from "./pages/authentication/Login";
import RequireAuth from "./components/layout/RequireAuth";
import UserEdit from "./pages/UserEdit";
import AccountEdit from "./components/layout/AccountEdit";
import ProfileEdit from "./pages/ProfileEdit";

import ChatNav from "./components/layout/Chats";
import EmptyChatPage from "./pages/chat/EmptyChatPage";
import SearchProfiles from "./pages/profiles/SearchProfiles";

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route element={<RequireAuth />}>
        <Route element={<Layout />}>
          <Route path="/" element={<FindPeoplePage />} />
          <Route path="/chats" element={<ChatNav />} >
            <Route path=":chatid?" element={<ChatPage />} />
            <Route index element={<EmptyChatPage />} />
          </Route>
          <Route path="/edit" element={<AccountEdit />}>
            <Route path="profile" element={<ProfileEdit />} />
            <Route path="user" element={<UserEdit />} />
            <Route index element={<ProfileEdit />} />
          </Route>
          <Route path="/profile/:username?" element={<ProfilePage />} />
          <Route path="/friends" element={<FriendNav />}>
            <Route path="all" element={<AllFriends />} />
            <Route path="requests" element={<FriendRequests />} />
            <Route path="search" element={<SearchProfiles/>}/>
            <Route index element={<AllFriends />} />
          </Route>
        </Route>
      </Route>
      <Route path="*" element={<h1>404</h1>} />
    </Routes>
  );
}

export default App;
