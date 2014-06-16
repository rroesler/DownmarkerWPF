using System;
using System.IO;
using System.Linq;
using CefSharp;

using MarkPad.Infrastructure;
using MarkPad.Events;
using Caliburn.Micro;

namespace MarkPad.Preview
{
    public class MarkdownHandlerFactory : ISchemeHandlerFactory
    {
        private IEventAggregator eventAggregator = null;
        private Func<string> location = null;

        public MarkdownHandlerFactory(IEventAggregator eventAggregator, Func<string> location)
        {
            this.eventAggregator = eventAggregator;
            this.location = location;
        }

        public ISchemeHandler Create()
        {
            return new MarkdownHandler(eventAggregator, location());
        }
    }

    public class MarkdownHandler : ISchemeHandler
    {
        private IEventAggregator eventAggregator = null;
        private string location = null;

        public MarkdownHandler(IEventAggregator eventAggregator, string location)
        {
            this.eventAggregator = eventAggregator;
            this.location = location;
        }

        public bool ProcessRequest(IRequest request, ref string mimeType, ref System.IO.Stream stream)
        {
            var uri = new Uri(request.Url);
            var segments = uri.Segments;

            var dir = location ?? Directory.GetCurrentDirectory();

            var path = Path.Combine(dir,
                string.Concat(uri.Segments.Skip(1).Select(p => p.Replace("/", "\\"))));

            eventAggregator.Publish(new FileOpenEvent(path));

            //var file = new FileInfo(path);

            //if (file.Exists)
            //{
            //    stream = file.OpenRead();
            //    mimeType = "text/html";
            //    return true;
            //}

            return false;
        }
    }
}