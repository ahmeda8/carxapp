using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace carXapp2
{
    class AsyncCallbackEvent : IAsyncResult
    {
        private object _AsyncState;

        public AsyncCallbackEvent(object result)
        {
            _AsyncState = result;
        }

        public object AsyncState
        {
            get { return _AsyncState; }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCompleted
        {
            get { throw new NotImplementedException(); }
        }
    }
}
