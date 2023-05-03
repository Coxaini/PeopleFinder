namespace PeopleFinder.Infrastructure.Common.Helpers;

public static class FileFolderHelper
{
    public static string GetFileFolderPath(string baseFolder, DateTime uploadTime)
    {
        int year = uploadTime.Year;
        int month = uploadTime.Month;
        int day = uploadTime.Day;
        int hour = uploadTime.Hour;
        string folderPath = Path.Combine(baseFolder, year.ToString(), month.ToString(), day.ToString(), hour.ToString());
        return folderPath;
    }

}