using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.UWP
{
	public interface IImageVisualElementRenderer : IVisualNativeElementRenderer
	{
		bool IsDisposed { get; }
		void SetImage(Windows.UI.Xaml.Media.ImageSource image);

		Windows.UI.Xaml.Controls.Image GetImage();
	}
}
