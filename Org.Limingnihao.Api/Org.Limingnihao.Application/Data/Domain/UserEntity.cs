using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Limingnihao.Application.Data.Domain
{
    [Serializable]
    public class UserEntity
    {
        public virtual Int32 UserId { get; set; }
        public virtual String Username { get; set; }
        public virtual String Nickname { get; set; }
        public virtual String Password { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime LastTime { get; set; }
        public virtual Int32 UserType { get; set; }
        public virtual Int32 UseFlag { get; set; }
    }
}
