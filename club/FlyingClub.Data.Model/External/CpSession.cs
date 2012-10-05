using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.External
{
    [Table("vb3_cpsession")]
    public class CpSession
    {
        [Key()]
        public string hash { get; set; }
        public int userid { get; set; }
        public int dateline { get; set; }
    }
}
