using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogre.Framework
{
	public enum TrayLocation   // enumerator values for widget tray anchoring locations
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight,
		None
	}

	public enum ButtonState   // enumerator values for button states
	{
		Up,
		Over,
		Down
	}

	public class SdkTrayManager
	{
		readonly Timer _timer;
		uint _lastStatUpdateTime;

		Overlay _backdropLayer;        // backdrop layer
		Overlay _traysLayer;           // widget layer
		Overlay _priorityLayer;        // top priority layer
		Overlay _cursorLayer;          // cursor layer
		OverlayContainer _cursor;
		OverlayContainer _backdrop;
		OverlayContainer _dialogShade;
		readonly OverlayContainer[] _trays = new OverlayContainer[10];   // widget trays
		readonly GuiHorizontalAlignment[] _trayWidgetAlign = new GuiHorizontalAlignment[10];   // tray widget alignments
		readonly List<Widget>[] _widgets = new List<Widget>[10];              // widgets
		float _widgetPadding = 8.0f;
		float _widgetSpacing = 2.0f;            // widget spacing
		float _trayPadding;              // tray padding
		DecorWidget _logo;
		Label _fpsLabel;
		ParamsPanel _statsPanel;             // frame stats panel

		public string Name { get; private set; }

		public RenderWindow Window { get; private set; }

		public SdkTrayManager(string name, RenderWindow window)
		{
			Name = name;
			string nameBase = Name + "/";
			nameBase = nameBase.Replace(" ", "_");

			for (var i = 0; i < _widgets.Length; i++)
			{
				_widgets[i] = new List<Widget>();
			}

			Window = window;
			_timer = Root.Singleton.Timer;
			OverlayManager om = OverlayManager.Singleton;

			_backdropLayer = om.Create(nameBase + "BackdropLayer");
			_traysLayer = om.Create(nameBase + "WidgetsLayer");
			_priorityLayer = om.Create(nameBase + "PriorityLayer");
			_cursorLayer = om.Create(nameBase + "CursorLayer");
			_backdropLayer.ZOrder = 100; ;
			_traysLayer.ZOrder = 200;
			_priorityLayer.ZOrder = 300;
			_cursorLayer.ZOrder = 400;

			// make backdrop and cursor overlay containers
			_cursor = (OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Cursor", "Panel", nameBase + "Cursor");
			_cursorLayer.Add2D(_cursor);
			_backdrop = (OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "Backdrop");
			_backdropLayer.Add2D(_backdrop);
			_dialogShade = (OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "DialogShade");
			_dialogShade.MaterialName = "SdkTrays/Shade";
			_dialogShade.Hide();
			_priorityLayer.Add2D(_dialogShade);

			string[] trayNames = new[]
			{
				"TopLeft", "Top", "TopRight", "Left", "Center", "Right", "BottomLeft", "Bottom", "BottomRight"
			};

			for (var i = 0; i < 9; i++)    // make the real trays
			{
				_trays[i] = (OverlayContainer)om.CreateOverlayElementFromTemplate
					("SdkTrays/Tray", "BorderPanel", nameBase + trayNames[i] + "Tray");
				_traysLayer.Add2D(_trays[i]);

				_trayWidgetAlign[i] = GuiHorizontalAlignment.GHA_CENTER;

				// align trays based on location
				if (i == (int)TrayLocation.Top || i == (int)TrayLocation.Center || i == (int)TrayLocation.Bottom)
					_trays[i].HorizontalAlignment = GuiHorizontalAlignment.GHA_CENTER;

				if (i == (int)TrayLocation.Left || i == (int)TrayLocation.Center || i == (int)TrayLocation.Right)
					_trays[i].VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;

				if (i == (int)TrayLocation.TopRight || i == (int)TrayLocation.Right || i == (int)TrayLocation.BottomRight)
					_trays[i].HorizontalAlignment = GuiHorizontalAlignment.GHA_RIGHT;

				if (i == (int)TrayLocation.BottomLeft || i == (int)TrayLocation.Bottom || i == (int)TrayLocation.BottomRight)
					_trays[i].VerticalAlignment = GuiVerticalAlignment.GVA_BOTTOM;
			}

			// create the null tray for free-floating widgets
			_trays[9] = (OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "NullTray");
			_trayWidgetAlign[9] = GuiHorizontalAlignment.GHA_LEFT;
			_traysLayer.Add2D(_trays[9]);
			AdjustTrays();

			ShowTrays();
			ShowCursor();
		}

		public Label CreateLabel(TrayLocation trayLoc, string name, string caption, float width = 0.0f)
		{
			Label l = new Label(name, caption, width);
			MoveWidgetToTray(l, trayLoc);
			//l->_assignListener(mListener);
			return l;
		}

		public DecorWidget CreateDecorWidget(TrayLocation trayLoc, string name, string templateName)
		{
			DecorWidget dw = new DecorWidget(name, templateName);
			MoveWidgetToTray(dw, trayLoc);
			return dw;
		}

		public void ShowTrays()
		{
			_traysLayer.Show();
			_priorityLayer.Show();
		}

		public void ShowCursor(string materialName = "")
		{
			if (!string.IsNullOrEmpty(materialName))
				GetCursorImage().MaterialName = materialName;

			if (!_cursorLayer.IsVisible)
			{
				_cursorLayer.Show();

				RefreshCursor();
				System.Windows.Forms.Cursor.Hide();
			}
		}

		public void HideCursor()
		{
			_cursorLayer.Hide();

			// give widgets a chance to reset in case they're in the middle of something
			for (var i = 0; i < 10; i++)
			{
				for (var j = 0; j < _widgets[i].Count; j++)
				{
					_widgets[i][j].OnFocusLost();
				}
			}

			//SetExpandedMenu(0);
			System.Windows.Forms.Cursor.Show();
		}

		public void RefreshCursor()
		{
			var position = System.Windows.Forms.Cursor.Position;
			_cursor.SetPosition(position.X, position.Y);
		}

		public void ShowFrameStats(TrayLocation trayLoc, int place = -1)
		{
			if (!AreFrameStatsVisible())
			{
				StringVector stats = new StringVector();
				stats.Add("Average FPS");
				stats.Add("Best FPS");
				stats.Add("Worst FPS");
				stats.Add("Triangles");
				stats.Add("Batches");

				_fpsLabel = CreateLabel(TrayLocation.BottomLeft, Name + "/FpsLabel", "FPS:", 180);
				//_fpsLabel._AssignListener(this);
				_statsPanel = CreateParamsPanel(TrayLocation.None, Name + "/StatsPanel", 180, stats);
			}
		}

		public ParamsPanel CreateParamsPanel(TrayLocation trayLoc, string name, float width, StringVector paramNames)
		{
			ParamsPanel pp = new ParamsPanel(name, width, paramNames.Count);
			pp.SetAllParamNames(paramNames);
			MoveWidgetToTray(pp, trayLoc);
			return pp;
		}

		public void HideFrameStats()
		{
			if (AreFrameStatsVisible())
			{
				DestroyWidget(_fpsLabel);
				DestroyWidget(_statsPanel);
				_fpsLabel = null;
				_statsPanel = null;
			}
		}

		public bool AreFrameStatsVisible()
		{
			return _fpsLabel != null;
		}

		public void ShowLogo(TrayLocation trayLoc, int place = -1)
		{
			if (!IsLogoVisible())
				_logo = CreateDecorWidget(TrayLocation.None, Name + "/Logo", "SdkTrays/Logo");

			MoveWidgetToTray(_logo, trayLoc, place);
		}

		public void HideLogo()
		{
			if (IsLogoVisible())
			{
				DestroyWidget(_logo);
				_logo = null;
			}
		}

		public bool IsLogoVisible()
		{
			return _logo != null;
		}

		public void DestroyWidget(Widget widget)
		{
			Contract.ArgumentNotNull(widget, "widget");

			// in case special widgets are destroyed manually, set them to 0
			if (widget == _logo) _logo = null;
			else if (widget == _statsPanel) _statsPanel = null;
			else if (widget == _fpsLabel) _fpsLabel = null;

			_trays[(int)widget.TrayLocation].RemoveChild(widget.Name);

			var wList = _widgets[(int)widget.TrayLocation];
			wList.Remove(widget);

			//if (widget == mExpandedMenu) setExpandedMenu(0);

			widget.Cleanup();

			//mWidgetDeathRow.push_back(widget);

			AdjustTrays();
		}


		public OverlayElement GetCursorImage()
		{
			return _cursor.GetChild(_cursor.Name + "/CursorImage");
		}

		protected virtual void AdjustTrays()
		{
			for (var i = 0; i < 9; i++)   // resizes and hides trays if necessary
			{
				float trayWidth = 0;
				float trayHeight = _widgetPadding;
				var labelsAndSeps = new List<OverlayElement>();

				if (_widgets[i] == null || _widgets[i].Count == 0)   // hide tray if empty
				{
					_trays[i].Hide();
					continue;
				}
				else
					_trays[i].Show();

				// arrange widgets and calculate final tray size and position
				for (int j = 0; j < _widgets[i].Count; j++)
				{
					OverlayElement e = _widgets[i][j].OverlayElement;

					if (j != 0) trayHeight += _widgetSpacing;   // don't space first widget

					e.VerticalAlignment = GuiVerticalAlignment.GVA_TOP;
					e.Top = trayHeight;

					switch (e.HorizontalAlignment)
					{
						case GuiHorizontalAlignment.GHA_LEFT:
							e.Left = _widgetPadding;
							break;
						case GuiHorizontalAlignment.GHA_RIGHT:
							e.Left = -(e.Width + _widgetPadding);
							break;
						default:
							e.Left = -(e.Width / 2);
							break;
					}

					// prevents some weird texture filtering problems (just some)
					e.SetPosition((int)e.Left, (int)e.Top);
					e.SetDimensions((int)e.Width, (int)e.Height);

					trayHeight += e.Height;

					Label label = _widgets[i][j] as Label;
					if (label != null && label.IsFitToTray())
					{
						labelsAndSeps.Add(e);
						continue;
					}

					//Separator* s = dynamic_cast<Separator*>(mWidgets[i][j]);
					//if (s && s->_isFitToTray())
					//{
					//	labelsAndSeps.Add(e);
					//	continue;
					//}

					if (e.Width > trayWidth)
						trayWidth = e.Width;
				}

				// add paddings and resize trays
				_trays[i].Width = trayWidth + 2 * _widgetPadding;
				_trays[i].Height = trayHeight + _widgetPadding;

				for (var j = 0; j < labelsAndSeps.Count; j++)
				{
					labelsAndSeps[j].Width = (int)trayWidth;
					labelsAndSeps[j].Left = -(int)(trayWidth / 2);
				}
			}

			for (var i = 0; i < 9; i++)    // snap trays to anchors
			{
				if (i == (int)TrayLocation.TopLeft || i == (int)TrayLocation.Left || i == (int)TrayLocation.BottomLeft)
					_trays[i].Left = _trayPadding;
				if (i == (int)TrayLocation.Top || i == (int)TrayLocation.Center || i == (int)TrayLocation.Bottom)
					_trays[i].Left = -_trays[i].Width / 2;
				if (i == (int)TrayLocation.TopRight || i == (int)TrayLocation.Right || i == (int)TrayLocation.BottomRight)
					_trays[i].Left = -(_trays[i].Width + _trayPadding);

				if (i == (int)TrayLocation.TopLeft || i == (int)TrayLocation.Top || i == (int)TrayLocation.TopRight)
					_trays[i].Top = _trayPadding;
				if (i == (int)TrayLocation.Left || i == (int)TrayLocation.Center || i == (int)TrayLocation.Right)
					_trays[i].Top = -(_trays[i].Height / 2);
				if (i == (int)TrayLocation.BottomLeft || i == (int)TrayLocation.Bottom || i == (int)TrayLocation.BottomRight)
					_trays[i].Top = -(_trays[i].Height - _trayPadding);

				// prevents some weird texture filtering problems (just some)
				_trays[i].SetPosition((int)_trays[i].Left, (int)_trays[i].Top);
				_trays[i].SetDimensions((int)_trays[i].Width, (int)_trays[i].Height);
			}
		}

		void MoveWidgetToTray(Widget widget, TrayLocation trayLoc, int place = -1)
		{
			Contract.ArgumentNotNull(widget, "widget");

			// remove widget from old tray
			var wList = _widgets[(int)widget.TrayLocation];
			if (wList.Contains(widget))
			{
				wList.Remove(widget);
				_trays[(int)widget.TrayLocation].RemoveChild(widget.Name);
			}

			// insert widget into new tray at given position, or at the end if unspecified or invalid
			if (place == -1 || place > (int)_widgets[(int)trayLoc].Count)
				place = (int)_widgets[(int)trayLoc].Count;

			_widgets[(int)trayLoc].Insert(place, widget);
			_trays[(int)trayLoc].AddChild(widget.OverlayElement);

			widget.OverlayElement.HorizontalAlignment = _trayWidgetAlign[(int)trayLoc];

			// adjust trays if necessary
			if (widget.TrayLocation != TrayLocation.None || trayLoc != TrayLocation.None)
				AdjustTrays();

			widget.TrayLocation = trayLoc;
		}

		public void Update(float elapsedTime)
		{
			//for (unsigned int i = 0; i < mWidgetDeathRow.size(); i++)
			//{
			//	delete mWidgetDeathRow[i];
			//}
			//mWidgetDeathRow.clear();

			uint currentTime = _timer.Milliseconds;
			if (AreFrameStatsVisible() && currentTime - _lastStatUpdateTime > 250)
			{
				var stats = Window.GetStatistics();
				var frameStats = Root.Singleton.GetFrameStats();
				_lastStatUpdateTime = currentTime;
				float avgTime = frameStats.AvgTime;

				var s = "FPS: " + string.Format("{0} ms - {1} fps", avgTime, 1000.0f / avgTime);
				_fpsLabel.Caption = s;

				if (_statsPanel.OverlayElement.IsVisible)
				{
					var values = new StringVector();
					values.Add(frameStats.BestTime.ToString());
					values.Add(frameStats.WorstTime.ToString());
					values.Add(stats.TriangleCount.ToString());
					values.Add(stats.BatchCount.ToString());

					_statsPanel.SetAllParamValues(values);
				}
			}
		}

		public void InjectMouseMove(int mouseX, int mouseY)
		{
			if (!_cursorLayer.IsVisible)
				return;   // don't process if cursor layer is invisible

			_cursor.SetPosition(mouseX, mouseY);
		}
	}
}
