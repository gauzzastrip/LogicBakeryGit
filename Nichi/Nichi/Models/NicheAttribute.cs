using System;


namespace NichelyPrototype
{
	public class NicheAttribute 
	{
		public NicheAttribute ()
		{
			NicheAttributeId = Guid.NewGuid();
		}
		public Guid NicheAttributeId{get;set;}
		public Guid NicheAttributeTypeId {get;set;}
		public Guid NicheId{get;set;}
		public Guid UserId {get;set;}
		public int SortOrder{get;set;}
		public string Key{get;set;}
		public object Value{get;set;}
		public DateTime CreatedOn{ get; set; }

	}
}

