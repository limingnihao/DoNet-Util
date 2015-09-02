using Org.Limingnihao.Application.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Data
{
    public interface IUserDao
    {
        void SaveEntity(UserEntity entity);

        UserEntity GetEntity(Int32 id);

        UserEntity GetEntityByUsername(String username);

        List<UserEntity> GetListAll();

    }
}
