using System.ComponentModel;
using System.Threading.Tasks;
using Android.Widget;

namespace Xamarin.Forms.Platform.Android.FastRenderers
{
	public static class ImageElementManager
	{
		public static void Init(IVisualElementRenderer renderer)
		{
			renderer.ElementPropertyChanged += OnElementPropertyChanged;
			renderer.ElementChanged += OnElementChanged;
		}

		private async static void OnElementChanged(object sender, VisualElementChangedEventArgs e)
		{
			var renderer = (sender as IVisualElementRenderer);
			var view = (ImageView)renderer.View;
			var newImageElementManager = (IImageController)e.NewElement;
			var oldImageElementManager = (IImageController)e.OldElement;

			await TryUpdateBitmap(view, newImageElementManager, oldImageElementManager);
			UpdateAspect(view, newImageElementManager, oldImageElementManager);
			ElevationHelper.SetElevation(view, renderer.Element);
		}

		private async static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var renderer = (sender as IVisualElementRenderer);
			var ImageElementManager = (IImageController)renderer.Element;
			if (e.PropertyName == ImageElementManager.SourceProperty.PropertyName)
			{ 
				await TryUpdateBitmap((ImageView)renderer.View, (IImageController)renderer.Element);
			}
			else if (e.PropertyName == ImageElementManager.AspectProperty.PropertyName)
			{ 
				UpdateAspect((ImageView)renderer.View, (IImageController)renderer.Element);
			}
		}


		async static Task TryUpdateBitmap(ImageView Control, IImageController newImage, IImageController previous = null)
		{
			if (newImage == null)
			{
				return;
			}

			await Control.UpdateBitmap(newImage, previous);
		}

		static void UpdateAspect(ImageView Control, IImageController newImage, IImageController previous = null)
		{
			if (newImage == null)
			{
				return;
			}

			ImageView.ScaleType type = newImage.Aspect.ToScaleType();
			Control.SetScaleType(type);
		}
	}
}