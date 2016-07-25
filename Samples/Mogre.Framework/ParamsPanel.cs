using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogre.Framework
{
	public sealed class ParamsPanel : Widget
	{
		TextAreaOverlayElement _namesArea;
		TextAreaOverlayElement _valuesArea;
		StringVector _names;
		StringVector _values = new StringVector();

		public ParamsPanel(string name, float width, int lines)
		{
			OverlayElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/ParamsPanel", "BorderPanel", name);
			OverlayContainer c = (OverlayContainer)OverlayElement;
			_namesArea = (TextAreaOverlayElement)c.GetChild(Name + "/ParamsPanelNames");
			_valuesArea = (TextAreaOverlayElement)c.GetChild(Name + "/ParamsPanelValues");
			OverlayElement.Width = width;
			OverlayElement.Height = _namesArea.Top * 2 + lines * _namesArea.CharHeight;
		}

		public void SetAllParamNames(StringVector paramNames)
		{
			_names = paramNames;
			_values.Clear();
			_values.Resize(_names.Count, "");
			OverlayElement.Height = _namesArea.Top * 2 + _names.Count * _namesArea.CharHeight;

			UpdateText();
		}

		protected void UpdateText()
		{
			string namesDS = string.Empty;
			string valuesDS = string.Empty;

			for (int i = 0; i < _names.Count; i++)
			{
				namesDS += _names[i] + ":\n";
				valuesDS += _values[i] + "\n";
			}

			_namesArea.Caption = namesDS;
			_valuesArea.Caption = valuesDS;
		}
	}
}
