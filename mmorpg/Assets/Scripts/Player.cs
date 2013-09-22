using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Player
	{
		public Vector3 Position;
		public string Name;
		public GameObject Model;
		public GameObject ModelP;
		public bool canKill = false;
		
		public Player (string name, Vector3 position)
		{
			Name = name;
			Position = position;			
		}		
		
		public void update()
		{
				
		}
	}
}

