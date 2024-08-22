using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpMoku
{
	public static class MouseAction
	{
		private const int MOUSE_LEFTDOWN = 0x00000002;
		private const int MOUSE_LEFTUP = 0x00000004;

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetCursorPos(out Point lpPoint);

		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int X, int Y);

		[DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		public static Point GetCursorPosition()
		{
			GetCursorPos(out Point mousePoint);
			return mousePoint;
		}
		public static bool SetCursorPosition(int X, int Y)
		{
			return SetCursorPos(X, Y);

		}
		public static bool SetCursorPosition(Point p)
		{
			return SetCursorPosition(p.X, p.Y);
		}
		public static void Click()
		{
			GetCursorPos(out Point currentPosition);
			mouse_event(MOUSE_LEFTDOWN, currentPosition.X, currentPosition.Y, 0, 0);
			mouse_event(MOUSE_LEFTUP, currentPosition.X, currentPosition.Y, 0, 0);

		}

		private static readonly Random s_random = new();
		private static readonly int s_mouseSpeed = 15;
		/*
         Credit::https://stackoverflow.com/questions/913646/c-sharp-moving-the-mouse-around-realistically
         */
		public static void MoveMouse(int x, int y, int rx, int ry)
		{
			_ = new Point();
			Point c;
			GetCursorPos(out c);

			x += s_random.Next(rx);
			y += s_random.Next(ry);

			double randomSpeed = Math.Max(((s_random.Next(s_mouseSpeed) / 2.0) + s_mouseSpeed) / 10.0, 0.1);

			WindMouse(c.X, c.Y, x, y, 9.0, 3.0, 10.0 / randomSpeed,
				15.0 / randomSpeed, 10.0 * randomSpeed, 10.0 * randomSpeed);
		}
		private static double Hypot(double side1, double side2)
		{
			return Math.Sqrt(Math.Pow(side1, 2) + Math.Pow(side2, 2));
		}
		public static void WindMouse(double xs, double ys, double xe, double ye,
			double gravity, double wind, double minWait, double maxWait,
			double maxStep, double targetArea)
		{

			double dist, windX = 0, windY = 0, veloX = 0, veloY = 0, randomDist, veloMag, step;
			int oldX, oldY;
			_ = (int)Math.Round(xs);
			_ = (int)Math.Round(ys);

			double waitDiff = maxWait - minWait;
			double sqrt2 = Math.Sqrt(2.0);
			double sqrt3 = Math.Sqrt(3.0);
			double sqrt5 = Math.Sqrt(5.0);

			dist = Hypot(xe - xs, ye - ys);

			while (dist > 1.0)
			{

				wind = Math.Min(wind, dist);

				if (dist >= targetArea)
				{
					int w = s_random.Next(((int)Math.Round(wind) * 2) + 1);
					windX = (windX / sqrt3) + ((w - wind) / sqrt5);
					windY = (windY / sqrt3) + ((w - wind) / sqrt5);
				}
				else
				{
					windX /= sqrt2;
					windY /= sqrt2;
					if (maxStep < 3)
					{
						maxStep = s_random.Next(3) + 3.0;
					}
					else
					{
						maxStep /= sqrt5;
					}
				}

				veloX += windX;
				veloY += windY;
				veloX += gravity * (xe - xs) / dist;
				veloY += gravity * (ye - ys) / dist;

				if (Hypot(veloX, veloY) > maxStep)
				{
					randomDist = (maxStep / 2.0) + s_random.Next((int)Math.Round(maxStep) / 2);
					veloMag = Hypot(veloX, veloY);
					veloX = veloX / veloMag * randomDist;
					veloY = veloY / veloMag * randomDist;
				}

				oldX = (int)Math.Round(xs);
				oldY = (int)Math.Round(ys);
				xs += veloX;
				ys += veloY;
				dist = Hypot(xe - xs, ye - ys);
				int newX = (int)Math.Round(xs);
				int newY = (int)Math.Round(ys);

				if (oldX != newX || oldY != newY)
				{
					SetCursorPos(newX, newY);
				}

				step = Hypot(xs - oldX, ys - oldY);
				int wait = (int)Math.Round((waitDiff * (step / maxStep)) + minWait);
				Thread.Sleep(wait);
			}
		}

		public static Point Round(PointF poi)
		{
			double x = (double)poi.X;
			double y = (double)poi.Y;

			return new Point((int)Math.Round(x), (int)Math.Round(y));
		}
		/*
         *
         Credit::https://stackoverflow.com/questions/913646/c-sharp-moving-the-mouse-around-realistically
         Author::https://stackoverflow.com/users/16942/erik-forbes
         */
		public static event EventHandler HasFinishedMoved;
		public static void LinearSmoothMove(Point newPosition, int steps)
		{
			int MouseEventDelayMS = 10;
			Point start = GetCursorPosition();
			PointF iterPoint = new(start.X, start.Y);

			// Find the slope of the line segment defined by start and newPosition
			PointF slope = new(newPosition.X - start.X, newPosition.Y - start.Y);

			// Divide by the number of steps
			slope.X /= steps;
			slope.Y /= steps;

			// Move the mouse to each iterative point.
			for (int i = 0; i < steps; i++)
			{
				iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);

				SetCursorPosition(Round(iterPoint));
				Thread.Sleep(MouseEventDelayMS);
			}

			// Move the mouse to the final destination.
			SetCursorPosition(newPosition);
			HasFinishedMoved?.Invoke(null, null);
		}
		/*
        This below link explain the reason you cannot use System.Drawing.Point
        https://www.pinvoke.net/default.aspx/user32.getcursorpos

        */
		[StructLayout(LayoutKind.Sequential)]
		public struct Point(int x, int y)
		{
			public int X = x;
			public int Y = y;
		}

		public static Point convertDrawingPointToStructPoint(System.Drawing.Point poi)
		{
			Point poiResult = new(poi.X, poi.Y);
			return poiResult;
		}
	}
}
