using System;
using System.Collections.Generic;

namespace NichelyPrototype
{
    public class Niche
    {
		public Guid NicheId { get; set; }
		public Guid UserId {get;set;}
		public string Description{ get; set; }
        public string Title { get; set; }
        public string ImageUri { get; set; }
		public DateTime CreatedOn {get;set;}
		public List<NicheAttribute> Attributes { get; set; }

        public Niche()
        {
			NicheId = Guid.NewGuid();
			Attributes = new List<NicheAttribute>();
			CreatedOn = DateTime.Now;
        }
    }
}

