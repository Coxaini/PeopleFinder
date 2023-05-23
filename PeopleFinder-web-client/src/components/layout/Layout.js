
import { ChatHubProvider } from "../../context/ChatsHubProvider";
import classes from "./Layout.module.css";

import MainNavigation from "./MainNavigation";
import { Outlet } from "react-router-dom";

function Layout() {
  return (
    <ChatHubProvider>
    <div>
      <MainNavigation />
      <main className={classes.main}>
          <Outlet/>   
      </main>
    </div>
    </ChatHubProvider>

  );
}

export default Layout;