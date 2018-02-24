using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Xamarin.Forms
{ 
	// fix closures
    internal static class ButtonElement
    {
		public static void Initialize(IButtonController buttonElement)
		{
			buttonElement.CommandChanged += CommandChanged;
			buttonElement.CommandCanExecuteChanged += CommandCanExecuteChanged;
		}

		public static void CommandChanged(object sender, BindableValueChangedEventArgs e)
		{
			IButtonController buttonElement = (IButtonController)sender;
			if (buttonElement.Command != null)
			{
				CommandCanExecuteChanged(buttonElement, EventArgs.Empty);
			}
			else
			{
				buttonElement.IsEnabledCore = true;
			}
		}

		public static void CommandCanExecuteChanged(object sender, EventArgs e)
		{
			IButtonController buttonElement = (IButtonController)sender;
			ICommand cmd = buttonElement.Command;
			if (cmd != null)
			{
				buttonElement.IsEnabledCore = cmd.CanExecute(buttonElement.CommandParameter);
			}
		}


		public static void SendClicked(VisualElement visualElement, IButtonController buttonElement)
		{
			if (visualElement.IsEnabled == true)
			{
				buttonElement.Command?.Execute(buttonElement.CommandParameter);
				buttonElement.OnClicked();
			}
		}
		 
		public static void SendPressed(VisualElement visualElement, IButtonController buttonElement)
		{
			if (visualElement.IsEnabled == true)
			{
				buttonElement.SetIsPressed(true);
				visualElement.ChangeVisualStateInternal();
				buttonElement.OnPressed();
			}
		}
		 
		public static void SendReleased(VisualElement visualElement, IButtonController buttonElement)
		{
			if (visualElement.IsEnabled == true)
			{
				buttonElement.SetIsPressed(false);
				visualElement.ChangeVisualStateInternal();
				buttonElement.OnReleased();
			}
		}


	}
}
