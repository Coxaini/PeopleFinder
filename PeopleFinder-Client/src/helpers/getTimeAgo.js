export default function getTimeAgo(timestamp) {
    const timeDifference = new Date() - timestamp;
    const minute = 60 * 1000;
    const hour = 60 * minute;
    const day = 24 * hour;
    const week = 7 * day;
    const month = 30 * day;
  
    if (timeDifference < minute) {
      return 'just now';
    } else if (timeDifference < hour) {
      const minutes = Math.floor(timeDifference / minute);
      return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    } else if (timeDifference < day) {
      const hours = Math.floor(timeDifference / hour);
      return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    } else if (timeDifference < week) {
      const days = Math.floor(timeDifference / day);
      return `${days} day${days > 1 ? 's' : ''} ago`;
    } else if (timeDifference < month) {
      const weeks = Math.floor(timeDifference / week);
      return `${weeks} week${weeks > 1 ? 's' : ''} ago`;
    } else {
      const months = Math.floor(timeDifference / month);
      return `${months} month${months > 1 ? 's' : ''} ago`;
    }
}