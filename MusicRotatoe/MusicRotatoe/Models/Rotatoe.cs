using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRotatoe.Models
{
    public class Rotatoe
    {
        public Rotatoe()
        {
            RotatoeId = Guid.NewGuid();
            Songs = new List<Song>();
            MinPopularity = 0;
            MinEnergy = 0;
            MaxPopularity = 100;
            MaxEnergy = 100;

        }
        public Guid RotatoeId { get; set; }
        public string Title { get; set; }
        public string NextRefreshMessage
        {
            get
            {
                return string.Format("Runs Every {0} days. Next Run: {1}", (Interval / 60) / 24, NextReload.ToString("MM/dd/yyyy hh:mm tt"));
            }
        }
        public int TotalSongs { get; set; }
        public int Interval { get; set; }    //minutes
        public DateTime StartDate { get; set; }
        public DateTime NextReload
        {
            get
            {

                var timeBetwenCurrentAndBase = DateTime.Now - StartDate;
                var totalPeriodsBetwenCurrentAndBase = timeBetwenCurrentAndBase.TotalMinutes;
                var fractionalIntervals = totalPeriodsBetwenCurrentAndBase % Interval;
                var partialIntervalsLeft = Interval - fractionalIntervals;
                partialIntervalsLeft = partialIntervalsLeft - 1;
                var nextRunTime = StartDate.AddMinutes(partialIntervalsLeft);
                return nextRunTime;
            }
        }
        public List<Song> Songs { get; set; }
        public List<string> Genres { get; set; }
        public int MinPopularity { get; set; }
        public int MaxPopularity { get; set; }
        public int MinEnergy { get; set; }
        public int MaxEnergy { get; set; }
    }

}
