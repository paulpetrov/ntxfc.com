using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.Entities
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public string PluralName
        {
            get
            {
                if (Name == FlyingClub.Common.UserRoles.AircraftMaintenance.ToString())
                    return Description;
                else
                    return Description + "s"; 
            }
        }
    }
}
