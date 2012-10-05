using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class FlightReviewMapping : EntityTypeConfiguration<FlightReview>
    {
        public FlightReviewMapping()
        {
            ToTable("FlightReview");

            HasKey(o => o.Id);
            HasRequired(o => o.Pilot).WithMany(m => m.FlightReviews);
            //HasRequired(o => o.Instructor);//.WithMany(i => i.FlightReviews).WillCascadeOnDelete(false);
        }
    }
}
