using System.Collections.Concurrent;
using PeopleFinder.Application.Common.Interfaces.ConnectionStorage;

namespace PeopleFinder.Infrastructure.ConnectionStorage;

public class UserConnectionMemoryStorage : IConnectionStorage
{
    private readonly Dictionary<int, HashSet<string>> _connections = new();

    public int Count
    {
        get
        {
            return _connections.Count;
        }
    }

    public void Add(int userId, string connectionId, out bool isNewUser)
    {
        isNewUser = false;
        lock (_connections)
        {
            HashSet<string> connections;
            if (!_connections.TryGetValue(userId, out connections))
            {
                isNewUser = true;
                connections = new HashSet<string>();
                _connections.Add(userId, connections);
            }

            lock (connections)
            {
                connections.Add(connectionId);
            }
        }
    }

    public IEnumerable<string> GetConnections(int userId)
    {
        HashSet<string> connections;
        if (_connections.TryGetValue(userId, out connections))
        {
            return connections;
        }

        return Enumerable.Empty<string>();
    }

    public void Remove(int userId, string connectionId, out bool userRemoved)
    {
        userRemoved = false;
        lock (_connections)
        {
            if (!_connections.TryGetValue(userId, out HashSet<string>? connections))
                return;

            lock (connections)
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    userRemoved = _connections.Remove(userId);
                }
                else
                {
                    connections.Remove(connectionId);
                }
            }
        }
    }
}