using System;
using System.Collections.Generic;
using System.Text;

namespace java.lang.common.api
{
    public class AssertionError : Exception
    {
        private object messageObj;

        public AssertionError()
        {

        }

        public AssertionError(string message) : base(message)
        {
        }

        public AssertionError(object detailmessage)
        {
            messageObj = detailmessage;
        }

        public override string Message
        {
            get
            {
                if (messageObj == null)
                {
                    return base.Message;
                }
                else if (messageObj is Exception ex)
                {
                    return ex.Message;
                }
                else
                {
                    return messageObj.ToString();
                }
            }
        }

        public override string StackTrace
        {
            get
            {
                if (messageObj == null)
                {
                    return base.Message;
                }
                else if (messageObj is Exception ex)
                {
                    return ex.Message;
                }
                else
                {
                    return messageObj.ToString();
                }
            }
        }

        public override string Source
        {
            get
            {
                if (messageObj is Exception ex)
                {
                    return ex.Source;
                }
                else
                {
                    return base.Source;
                }
            }
            set
            {
                if (messageObj is Exception ex)
                {
                    ex.Source = value;
                }
                else
                {
                    base.Source = value;
                }
            }
        }
    }
}

