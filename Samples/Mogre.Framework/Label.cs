namespace Mogre.Framework
{
	public class Label : Widget
	{
		protected TextAreaOverlayElement _textArea;
		protected bool _fitToTray;

		public string Caption
		{
			get { return _textArea.Caption; }
			set { _textArea.Caption = value; }
		}

		internal Label(string name, string caption, float width = 0.0f)
		{
			OverlayElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Label", "BorderPanel", name);
			_textArea = (TextAreaOverlayElement)((OverlayContainer)OverlayElement).GetChild(Name + "/LabelCaption");
			Caption = caption;
			if (width <= 0)
				_fitToTray = true;
			else
			{
				_fitToTray = false;
				OverlayElement.Width = width;
			}
		}

		internal bool IsFitToTray()
		{
			return _fitToTray;
		}
	}
}
