#if NET452
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.SessionState;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Headstone.Framework.Models.Session
{
    [Table("Sessions")]
    public class SessionStateItem
    {
        public SessionStateItem()
        {
            Items = new SessionStateItemCollection();
            StaticObjects = new HttpStaticObjectsCollection();
        }

#region [ NoSQL entity properties ]

        [Key, Column(Order = 0)]
        public string SessionId { get; set; }

        public string Namespace
        {
            get
            {
                return "SESSION";
            }
        }

        public string UniqueKey
        {
            get
            {
                return "S#" + AppKey + "#" + Environment + "#" + SessionId;
            }
        }

#endregion

#region [ Application information ]

        [MaxLength(255), Key, Column(Order = 1)]
        public string AppKey { get; set; }

        [MaxLength(50)]
        public string Environment { get; set; }

#endregion

#region [ End user infomation ]

        public string UserId { get; set; }

        public string Username { get; set; }

        public string UserIP { get; set; }

#endregion

        public DateTime? LockDate { get; set; }

        public long? LockId { get; set; }

        public int Timeout { get; set; }

        public SessionStateActions Flags { get; set; }

        public bool Locked { get; set; }

        public string SerializedSessionData {
            get
            {
                return JsonConvert.SerializeObject(Items);
            }
        }

        public byte[] BinarySessionData { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expires { get; set; }

        [JsonIgnore]
        public SessionStateItemCollection Items { get; set; }

        [JsonIgnore]
        public HttpStaticObjectsCollection StaticObjects { get; set; }

        public SessionStateStoreData ToStoreData(HttpContext context)
        {
            // Get static objects
            var sObjects = SessionStateUtility.GetSessionStaticObjects(context);

            return new SessionStateStoreData(Items, sObjects, Timeout);
        }
    }
}
#endif
