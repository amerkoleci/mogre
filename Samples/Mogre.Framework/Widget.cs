using System.Collections.Generic;

namespace Mogre.Framework
{
	public abstract class Widget
	{
		public OverlayElement OverlayElement { get; protected set; }

		public string Name { get { return OverlayElement.Name; } }

		public TrayLocation TrayLocation { get; internal set; }

		protected Widget()
		{
			TrayLocation = TrayLocation.None;
		}

		public void Cleanup()
		{
			if (OverlayElement != null)
				NukeOverlayElement(OverlayElement);
			OverlayElement = null;
		}

		public static void NukeOverlayElement(OverlayElement element)
		{
			var container = element as OverlayContainer;
			if (container != null)
			{
				var toDelete = new List<OverlayElement>();

				//Ogre::OverlayContainer::ChildIterator children = container->getChildIterator();
				//while (children.hasMoreElements())
				//{
				//	toDelete.push_back(children.getNext());
				//}

				//for (int i = 0; i < toDelete.Count; i++)
				//{
				//	NukeOverlayElement(toDelete[i]);
				//}
			}

			if (element != null)
			{
				OverlayContainer parent = element.Parent;
				if (parent != null)
					parent.RemoveChild(element.Name);
				OverlayManager.Singleton.DestroyOverlayElement(element);
			}
		}

		protected internal void OnFocusLost() { }
	}
}
