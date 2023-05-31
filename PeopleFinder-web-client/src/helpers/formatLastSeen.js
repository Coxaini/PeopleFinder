export default function formatLastSeen(timestamp) {
    const currentTime = Date.now();
    const timeDifference = currentTime - timestamp;
  
    // Convert milliseconds to minutes
    const minutes = Math.floor(timeDifference / 60000);
  
    if (minutes < 1) {
      return "just now";
    } else if (minutes === 1) {
      return "1 minute ago";
    } else if (minutes < 60) {
      return `${minutes} minutes ago`;
    } else if (minutes < 1440) {
      const hours = Math.floor(minutes / 60);
      return `${hours} hour${hours === 1 ? "" : "s"} ago`;
    } else if (minutes < 43200) {
      const days = Math.floor(minutes / 1440);
      return `${days} day${days === 1 ? "" : "s"} ago`;
    } else {
      const lastSeenDate = new Date(timestamp);
      const options = { year: "numeric", month: "numeric", day: "numeric" };
      return lastSeenDate.toLocaleDateString(undefined, options);
    }
  }