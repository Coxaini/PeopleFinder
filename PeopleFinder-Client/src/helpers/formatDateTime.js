export default function formatDateTime(datetimeString) {
    const datetime = new Date(datetimeString);
    const now = new Date();
  
    if (datetime.toDateString() === now.toDateString()) {
      // It's today, so display only the time
      const options = { hour: '2-digit', minute: '2-digit' };
      return datetime.toLocaleTimeString(undefined, options);
    } else {
      const options = { year: 'numeric', month: 'long', day: 'numeric' };
      return datetime.toLocaleDateString(undefined, options);
    }
}