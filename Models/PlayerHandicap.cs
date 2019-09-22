using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfHandicap.Models
{
    public class PlayerHandicap
    {
        public long Id { get; set; }
        public DateTime PlayedDate { get; set; }
        public int Result { get; set; }
        public decimal Handicap { get; set; }
        public int CoursePar { get; set; }   
        public decimal CourseValue { get; set; }
        public int SlopeValue { get; set; }
        public int PlayedHandicap { get; set; }
    }
}
