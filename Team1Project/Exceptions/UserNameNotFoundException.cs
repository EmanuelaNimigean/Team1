using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1Project.Exceptions
{

    [Serializable]
    public class UserNameNotFoundException : Exception
    {
        public UserNameNotFoundException() { }
        public UserNameNotFoundException(string message) : base(message) { }
        public UserNameNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected UserNameNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
