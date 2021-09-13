using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBlogPost.Common.InterserviceContracts
{
    public class UserDataExchange
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string EmailId { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public UserDataExchange GetFromString(string json)
        {
            return JsonSerializer.Deserialize<UserDataExchange>(json);
        }
    }
}
