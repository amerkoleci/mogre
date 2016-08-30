// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mogre.Framework
{
	public class DefaultInputHandler
	{
		protected readonly Sample _sample;
		readonly System.Windows.Forms.Timer _timer;
		Vector3 _translate = Vector3.ZERO;
		bool _rotating;
		bool _lastFocus;
		Point _lastLocation;
		protected float Trans = 10f;
		protected float Rot = -0.2f;

		public DefaultInputHandler(Sample sample)
		{
			_sample = sample;
			sample.KeyDown += HandleKeyDown;
			sample.KeyUp += HandleKeyUp;
			sample.MouseDown += HandleMouseDown;
			sample.MouseUp += HandleMouseUp;
			sample.MouseMove += HandleMouseMove;
			sample.Disposed += Window_Disposed;
			sample.LostFocus += Window_LostFocus;
			sample.GotFocus += Window_GotFocus;

			_timer = new System.Windows.Forms.Timer
			{
				Interval = 17,
				Enabled = true
			};
			_timer.Tick += Timer_Tick;
		}

		protected virtual void HandleKeyDown(object sender, KeyEventArgs e)
		{
			float amount = Trans;
			switch (e.KeyCode)
			{
				case Keys.Up:
				case Keys.W:
					_translate.z = -amount;
					break;

				case Keys.Down:
				case Keys.S:
					_translate.z = amount;
					break;

				case Keys.Left:
				case Keys.A:
					_translate.x = -amount;
					break;

				case Keys.Right:
				case Keys.D:
					_translate.x = amount;
					break;

				case Keys.PageUp:
				case Keys.Q:
					_translate.y = amount;
					break;

				case Keys.PageDown:
				case Keys.E:
					_translate.y = -amount;
					break;

				case Keys.Space:
					_sample.TakeScreenshot();
					break;

				case Keys.T:
					_sample.CycleTextureFilteringMode();
					break;

				case Keys.R:
					_sample.CyclePolygonMode();
					break;
			}
		}

		protected virtual void HandleKeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
				case Keys.W:
				case Keys.Down:
				case Keys.S:
					_translate.z = 0;
					break;

				case Keys.Left:
				case Keys.A:
				case Keys.Right:
				case Keys.D:
					_translate.x = 0;
					break;

				case Keys.PageUp:
				case Keys.Q:
				case Keys.PageDown:
				case Keys.E:
					_translate.y = 0;
					break;
			}
		}

		protected virtual void HandleMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Cursor.Hide();
				_rotating = true;
			}
		}

		protected virtual void HandleMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Cursor.Show();
				_rotating = false;
			}
		}

		void HandleMouseMove(object sender, MouseEventArgs e)
		{
			_sample.TrayManager.InjectMouseMove(e.X, e.Y);
		}

		void HandleMouseMove(Point delta)
		{
			if (_rotating)
			{
				_sample.Camera.Yaw(new Degree(delta.X * Rot));
				_sample.Camera.Pitch(new Degree(delta.Y * Rot));
			}
		}

		void Window_Disposed(object sender, EventArgs e)
		{
			_timer.Enabled = false;
		}

		void Window_GotFocus(object sender, EventArgs e)
		{
			_timer.Enabled = true;
		}

		void Window_LostFocus(object sender, EventArgs e)
		{
			_timer.Enabled = false;
			_translate = Vector3.ZERO;
		}

		void Timer_Tick(object sender, EventArgs e)
		{
			if (_lastFocus)
			{
				Point position = Cursor.Position;
				position.X -= _lastLocation.X;
				position.Y -= _lastLocation.Y;
				HandleMouseMove(position);
			}

			_lastLocation = Cursor.Position;
			_lastFocus = _sample.Focused;
			if (_lastFocus)
			{
				_sample.Camera.Position += _sample.Camera.Orientation * _translate;
			}
		}
	}
}
