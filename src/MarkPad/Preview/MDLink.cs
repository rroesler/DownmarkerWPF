using MarkPad.Events;
using MarkPad.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkPad.Preview
{
    public class MDLink
    {
        private AppBootstrapper bootstrapper = null;

        public MDLink(AppBootstrapper bootstrapper)
        {
            this.bootstrapper = bootstrapper;
        }

        public void Load(string path)
        {
            bootstrapper.GetEventAggregator().Publish(new FileOpenEvent(path));
        }
    }
}
