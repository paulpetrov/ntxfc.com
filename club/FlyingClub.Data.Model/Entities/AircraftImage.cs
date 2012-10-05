using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlyingClub.Data.Model.Entities
{
    public class AircraftImage
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public string Type { get; set; }

        public string FileName_Small
        {
            get;
            set;
        }

        public string FileName_Medium
        {
            get;
            set;
        }

        public string FileName_Large
        {
            get;
            set;
        }
    }
}
