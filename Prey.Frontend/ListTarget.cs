using System.Collections.Generic;
using NLog;
using NLog.Targets;

namespace Prey.Frontend
{
    public class ListTarget : Target
    {
        private readonly Queue<string> _list = new Queue<string>();

        public Queue<string> List
        {
            get { return _list; }
        }


        protected override void Write(LogEventInfo logEvent)
        {
            List.Enqueue(logEvent.FormattedMessage);
        }
    }
}