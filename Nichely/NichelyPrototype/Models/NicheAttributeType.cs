using System;

namespace NichelyPrototype
{
	public class NicheAttributeType
	{
		public NicheAttributeType ()
		{
			NicheAttributeTypeId = Guid.NewGuid();
		}
		public Guid NicheAttributeTypeId{get;set;}
		public string Title{ get; set; }
	}
}

