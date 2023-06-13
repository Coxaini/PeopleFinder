namespace PeopleFinder.Application.Common.Interfaces.ConnectionStorage;

public interface IConnectionStorage
{
    int Count { get; }
    void Add(int userId, string connectionId, out bool isNewUser);
    IEnumerable<string> GetConnections(int userId);
    void Remove(int userId, string connectionId, out bool userRemoved);
}