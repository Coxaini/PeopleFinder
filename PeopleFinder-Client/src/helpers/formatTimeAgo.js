
import { t } from "i18next";

export default function formatTimeAgo(timestamp) {
    const currentTime = Date.now();
    const timeDifference = currentTime - timestamp;
  
    // Convert milliseconds to minutes
    const minutes = Math.floor(timeDifference / 60000);
    if (minutes < 1) {
      return t("chat.header.justNow");
    } else if (minutes < 60) {
      return t("timeAgo.minuteAgo", {count: minutes});
    } else if (minutes < 1440) {
      const hours = Math.floor(minutes / 60);
      return t("timeAgo.hourAgo", {count: hours});
    } else if (minutes < 43200) {
      const days = Math.floor(minutes / 1440);
      return t("timeAgo.dayAgo", {count: days});
    } else {
      const lastSeenDate = new Date(timestamp);
      const options = { year: "numeric", month: "numeric", day: "numeric" };
      return lastSeenDate.toLocaleDateString(undefined, options);
    }
  }