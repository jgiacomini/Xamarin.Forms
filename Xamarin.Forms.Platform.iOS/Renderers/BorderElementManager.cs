using System;
using System.ComponentModel;
using NativeView = UIKit.UIView;

namespace Xamarin.Forms.Platform.iOS
{
	internal static class BorderElementManager
	{ 
		static nfloat _defaultCornerRadius = 5;

		public static void Init(IVisualNativeElementRenderer renderer)
		{
			renderer.ElementPropertyChanged += OnElementPropertyChanged;
			renderer.ElementChanged += OnElementChanged;
		}

		public static void Dispose(IVisualNativeElementRenderer renderer)
		{
			renderer.ElementPropertyChanged -= OnElementPropertyChanged;
			renderer.ElementChanged -= OnElementChanged;
		}

		private static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			IVisualNativeElementRenderer renderer = (IVisualNativeElementRenderer)sender;
			IBorderController backgroundView = (IBorderController)renderer.Element;

			if (e.PropertyName == backgroundView.BorderWidthProperty.PropertyName || e.PropertyName == backgroundView.CornerRadiusProperty.PropertyName || e.PropertyName == backgroundView.BorderColorProperty.PropertyName)
				UpdateBorder(renderer, backgroundView);
		}

		private static void OnElementChanged(object sender, VisualElementChangedEventArgs e)
		{
			if(e.NewElement != null)
			{
				UpdateBorder((IVisualElementRenderer)sender, (IBorderController)e.NewElement);
			}
		}

		public static void UpdateBorder(IVisualElementRenderer renderer, IBorderController backgroundView)
		{
			var uiButton = renderer.NativeView;
			var ImageButton = backgroundView;

			if (ImageButton.BorderColor != Color.Default)
				uiButton.Layer.BorderColor = ImageButton.BorderColor.ToCGColor();

			uiButton.Layer.BorderWidth = Math.Max(0f, (float)ImageButton.BorderWidth);

			nfloat cornerRadius = _defaultCornerRadius;

			if (ImageButton.IsSet(ImageButton.CornerRadiusProperty) && ImageButton.CornerRadius != (int)ImageButton.CornerRadiusProperty.DefaultValue)
				cornerRadius = ImageButton.CornerRadius;

			uiButton.Layer.CornerRadius = cornerRadius;
		}
	}
}