using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogPost.Common.CustomException
{
    public class EntityNotFoundException : ArgumentException
    {

        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}
