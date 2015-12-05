using System;
using System.Collections.Generic;

namespace NichelyPrototype
{
	public class User
	{
		public User ()
		{
			UserId = Guid.NewGuid();
			Niches = new List<Niche> ();
		}

		public Guid UserId { get; set; }
		public string Email { get; set; }
		public string FacebookOAuth{get;set;}
		public string TwitterOAuth{get;set;}
		public string GoogleOAuth{get;set;}
		public string iOSOAuth{get;set;}

		public List<Niche> Niches{get;set;}
	}
}

