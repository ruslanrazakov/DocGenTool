using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Client.MVVM
{
    public static class TaskUtilities
    {
        /// <summary>
        /// Fire and forget extension method for AsyncCommand
        /// </summary>
        /// <param name="task"></param>
        /// <param name="handler"></param>
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                handler?.HandleError(ex);
            }
        }
    }
}
