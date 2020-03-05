#if NET452
using System.Collections.Specialized;

namespace Headstone.Framework.Models.Session
{
    public interface ISessionChannel
    {
        SessionStateItem GetItem(string sessionItemId, string appKey, string env);

        bool SetItem(SessionStateItem item, bool isExclusive, int expirationInMinutes);

        bool RemoveItem(string sessionItemId, string appKey, string env);
       
        bool CheckForUnknownAttributes(NameValueCollection config);
    }
}
#endif
