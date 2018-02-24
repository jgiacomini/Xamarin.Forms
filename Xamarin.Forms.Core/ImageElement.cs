using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{

	internal static class ImageElementManager
	{
		public static SizeRequest Measure(
			IImageController ImageElementManager,
			SizeRequest desiredSize, 
			double widthConstraint, 
			double heightConstraint)
		{ 
			double desiredAspect = desiredSize.Request.Width / desiredSize.Request.Height;
			double constraintAspect = widthConstraint / heightConstraint;

			double desiredWidth = desiredSize.Request.Width;
			double desiredHeight = desiredSize.Request.Height;

			if (desiredWidth == 0 || desiredHeight == 0)
				return new SizeRequest(new Size(0, 0));

			double width = desiredWidth;
			double height = desiredHeight;
			if (constraintAspect > desiredAspect)
			{
				// constraint area is proportionally wider than image
				switch (ImageElementManager.Aspect)
				{
					case Aspect.AspectFit:
					case Aspect.AspectFill:
						height = Math.Min(desiredHeight, heightConstraint);
						width = desiredWidth * (height / desiredHeight);
						break;
					case Aspect.Fill:
						width = Math.Min(desiredWidth, widthConstraint);
						height = desiredHeight * (width / desiredWidth);
						break;
				}
			}
			else if (constraintAspect < desiredAspect)
			{
				// constraint area is proportionally taller than image
				switch (ImageElementManager.Aspect)
				{
					case Aspect.AspectFit:
					case Aspect.AspectFill:
						width = Math.Min(desiredWidth, widthConstraint);
						height = desiredHeight * (width / desiredWidth);
						break;
					case Aspect.Fill:
						height = Math.Min(desiredHeight, heightConstraint);
						width = desiredWidth * (height / desiredHeight);
						break;
				}
			}
			else
			{
				// constraint area is same aspect as image
				width = Math.Min(desiredWidth, widthConstraint);
				height = desiredHeight * (width / desiredWidth);
			}

			return new SizeRequest(new Size(width, height));
		}

		internal static void OnBindingContextChanged(IImageController image, VisualElement visualElement)
		{
			if (image.Source != null)
				BindableObject.SetInheritedBindingContext(image.Source, visualElement?.BindingContext);
		}


		public static async void ImageSourceChanging(
			object sender, 
			BindableValueChangedEventArgs eventArgs)
		{
			var oldvalue = (ImageSource)eventArgs.OldValue;
			if (oldvalue == null)
				return;

			try
			{
				await oldvalue.Cancel().ConfigureAwait(false);
			}
			catch (ObjectDisposedException)
			{
				// Workaround bugzilla 37792 https://bugzilla.xamarin.com/show_bug.cgi?id=37792
			}
		}

		public static void ImageSourceChanged(
			object sender,
			BindableValueChangedEventArgs eventArgs)
		{
			var newvalue = (ImageSource)eventArgs.NewValue;
			var visualElement = (VisualElement)eventArgs.Owner;
			if (newvalue != null)
			{
				BindableObject.SetInheritedBindingContext(newvalue, visualElement?.BindingContext);
			}

			visualElement?.InvalidateMeasureInternal(InvalidationTrigger.MeasureChanged);
		}

		public static void Initialize(IImageController image)
		{
			image.ImageSourceChanging += ImageSourceChanging;
			image.ImageSourceChanged += ImageSourceChanged;
			image.ImageSourcesSourceChanged += ImageSourcesSourceChanged;
		}

		private static void ImageSourcesSourceChanged(object sender, EventArgs e)
		{
			((IImageController)sender).RaiseImageSourcePropertyChanged();
			((VisualElement)sender).InvalidateMeasureInternal(InvalidationTrigger.MeasureChanged);
		}
	}
}