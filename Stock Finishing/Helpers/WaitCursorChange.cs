using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Helpers
{
    public class WaitCursorChange : IDisposable
    {
        static bool _waitCursorIsActive;
        private object isBusy;
        PropertyInfo property;

        public WaitCursorChange(object isBusy)
        {
            _waitCursorIsActive = true;
            Type objectType = isBusy.GetType();
            property = objectType.GetProperty("IsBusy");
            property.SetValue(isBusy, true);
            this.isBusy = isBusy;
        }

        public static IDisposable BeginWaitCursorBlock(object isBusy)
        {
            return !_waitCursorIsActive ? (IDisposable)new WaitCursorChange(isBusy) : null;
        }

        public void Dispose()
        {
            property.SetValue(isBusy, false);
            _waitCursorIsActive = false;
        }
    }
}
