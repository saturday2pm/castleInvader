using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator
{
	public abstract class Player
	{
        List<Castle> ownCastles = new List<Castle>();
		public List<Castle> OwnCastles { get { return ownCastles; } private set { ownCastles = value; } }
        public int Id { get; set; }

		public void RemoveCastle(Castle c)
		{
			OwnCastles.Remove(c);
		}

		public void AddCastle(Castle c)
		{
			OwnCastles.Add(c);
		}

		public abstract void Init(Match match);
		public abstract void Update(Match match);
    }
}
