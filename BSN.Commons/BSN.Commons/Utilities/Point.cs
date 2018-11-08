﻿namespace Commons.Utilities
{
	public class Point
	{
		public int X { get; set; }
		public int Y { get; set; }


		public Point() { }

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}


		public System.Drawing.Point AsDrawingPoint()
		{
			return new System.Drawing.Point(X, Y);
		}
	}
}
