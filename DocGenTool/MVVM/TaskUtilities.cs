using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGenTool.MVVM
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
                //здесь можно также вызывать провайдер логгирования и т.д.
                handler?.HandleError(ex);
            }
        }
    }
}
