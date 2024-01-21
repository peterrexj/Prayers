using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.Extensions
{
    public class ExceptionHandler
    {
        public static void CaptureException(Exception exception, params string[] specificDetails)
        {
            try
            {
#if DEBUG
                throw exception;
#else
                Dictionary<string, string> errorContext = new();
                if (specificDetails != null)
                {
                    int counter = 0;
                    foreach (var detail in specificDetails.Where(f => f.HasValue()))
                    {
                        errorContext.Add($"Context{counter++}", detail);
                    }
                }
                ViewHelper.RunOnAppDispatcher(() => Crashes.TrackError(exception, DeviceDetails.GenerateMetaInformation(errorContext)));
#endif
            }
            catch (Exception)
            {
            }
        }

    }
}