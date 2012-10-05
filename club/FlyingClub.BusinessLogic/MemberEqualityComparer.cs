using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FlyingClub.Common;
using FlyingClub.Data.Repository;
using FlyingClub.Data.Repository.EntityFramework;
using FlyingClub.Data.Model.Entities;

namespace FlyingClub.BusinessLogic
{
    public class MemberEqualityComparer : IEqualityComparer<Member>
    {
        #region IEqualityComparer<Member> Members

        public bool Equals(Member x, Member y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Member obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
