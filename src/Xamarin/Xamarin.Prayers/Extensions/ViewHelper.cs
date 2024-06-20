using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.Extensions
{
    public class ViewHelper
    {
        public static void RunOnAppDispatcher(Action action)
        {
            try
            {
                Xamarin.Forms.Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    action();
                });
            }
            catch (Exception ex)
            {
                ExceptionHandler.CaptureException(ex);
            }
        }
    }
}
