using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator
{
    public class Path
    {
		public Waypoint Start { get; private set; }

		public Waypoint End { get; private set; }
		public Point Dir { get; private set; }

		public Path(Waypoint start, Waypoint end)
		{
			Start = start;
			End = end;
			float x = End.Pos.X - Start.Pos.X;
			float y = End.Pos.Y - start.Pos.Y;

			float norm = (float)Math.Sqrt((x*x + y*y));

			Dir = new Point(x / norm, y / norm);
		}

		public Path Reverse
		{
			get { return new Path(End, Start); }
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;

			var castle = obj as Path;

			if (castle == null) return false;

			return (Start == castle.Start) && (End == castle.End);
		}

		public override int GetHashCode()
		{
			return Start.GetHashCode() + End.GetHashCode();
		}

	}
}
