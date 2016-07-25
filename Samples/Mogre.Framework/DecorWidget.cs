namespace Mogre.Framework
{
	public class DecorWidget : Widget
	{
		public DecorWidget(string name, string templateName)
		{
			OverlayElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate(templateName, "", name);
		}
	}
}
