using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogre.Framework
{
	public class DebugOverlay
	{
		protected readonly RenderWindow _window;
		protected float timeSinceLastDebugUpdate = 1;
		protected OverlayElement mGuiAvg;
		protected OverlayElement mGuiCurr;
		protected OverlayElement mGuiBest;
		protected OverlayElement mGuiWorst;
		protected OverlayElement mGuiTris;
		protected OverlayElement mModesText;


		public DebugOverlay(RenderWindow window)
		{
			_window = window;
			AdditionalInfo = string.Empty;

			var name = "DebugOverlay/FpsLabel";

			//var debugOverlay = OverlayManager.Singleton.GetByName("Core/DebugOverlay");
			//debugOverlay.Show();

			//mGuiAvg = OverlayManager.Singleton.GetOverlayElement("Core/AverageFps");
			//mGuiCurr = OverlayManager.Singleton.GetOverlayElement("Core/CurrFps");
			//mGuiBest = OverlayManager.Singleton.GetOverlayElement("Core/BestFps");
			//mGuiWorst = OverlayManager.Singleton.GetOverlayElement("Core/WorstFps");
			//mGuiTris = OverlayManager.Singleton.GetOverlayElement("Core/NumTris");
			//mModesText = OverlayManager.Singleton.GetOverlayElement("Core/NumBatches");
		}

		public string AdditionalInfo
		{
			set;
			get;
		}

		public void Update(float timeFragment)
		{
			if (timeSinceLastDebugUpdate > 0.5f)
			{
				var stats = _window.GetStatistics();
				var frameStats = Root.Singleton.GetFrameStats();
				float avgTime = frameStats.AvgTime;

				//mGuiAvg.Caption = string.Format("FPS: {0} ms - {1} fps", frameStats.FPS, 1000.0f / avgTime);
				//mGuiCurr.Caption = "Current FPS: " + frameStats.LastTime;
				//mGuiBest.Caption = "Best FPS: " + frameStats.BestTime + " ms";
				//mGuiWorst.Caption = "Worst FPS: " + frameStats.WorstTime + " ms";
				//mGuiTris.Caption = "Triangle Count: " + stats.TriangleCount;
				//mModesText.Caption = AdditionalInfo;

				timeSinceLastDebugUpdate = 0;
			}
			else
			{
				timeSinceLastDebugUpdate += timeFragment;
			}
		}
	}
}
