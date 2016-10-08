using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator
{
	//길 또는 성.. 유닛의 목적지가 될 수 있는 지점
    public class Waypoint
    {
		public int Id { get; private set; }

		public Point Pos { get; protected set; }
		
		public Waypoint(int id)
		{
			Id = id;
		}
    }
}
