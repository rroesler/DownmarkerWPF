using System;
using System.Windows;
using System.Windows.Controls;
using CefSharp;
using Caliburn.Micro;

namespace MarkPad.Preview
{
    public partial class HtmlPreview : UserControl
    {
        public static void Init(IEventAggregator eventAggregator)
        {
            CefSharp.Settings settings = new CefSharp.Settings();

            if (CEF.Initialize(settings))
            {
                CEF.RegisterScheme("theme", new ThemeSchemeHandlerFactory());
                //CEF.RegisterScheme("test", new SchemeHandlerFactory());
                //CEF.RegisterJsObject("bound", new BoundObject());
                CEF.RegisterScheme("md", new MarkdownHandlerFactory(eventAggregator,() => DocDirectory ));
                //CEF.RegisterJsObject("MDLink", new MDLink());
            }
        }

        // these are accessed from the HTML "thread" not the UI thread 
        public static string DocDirectory { get; set; }
        public static string ThemeDirectory { get; set; }

        #region public string Html

        public static DependencyProperty HtmlProperty =
            DependencyProperty.Register("Html", typeof(string), typeof(HtmlPreview), new PropertyMetadata(" ", HtmlChanged));

        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }

        #endregion public string Html

        #region public string Title

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(HtmlPreview), new PropertyMetadata("", TitleChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion public string Location

        #region public string Location

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(HtmlPreview), new PropertyMetadata("", LocationChanged));

        public string Location
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        #endregion public string Title

        #region public double ScrollPercentage

        public static DependencyProperty ScrollPercentageProperty =
            DependencyProperty.Register("ScrollPercentage", typeof(double), typeof(HtmlPreview), new PropertyMetadata(0d, ScrollPercentageChanged));

        public string ScrollPercentage
        {
            get { return (string)GetValue(ScrollPercentageProperty); }
            set { SetValue(ScrollPercentageProperty, value); }
        }

        #endregion public double ScrollPercentage

        public HtmlPreview()
        {
            InitializeComponent();
        }

        public void Print()
        {
            if (host.IsBrowserInitialized)
                host.Print();
        }

        private static void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var htmlPreview = (HtmlPreview)d;
            if (htmlPreview.host != null && htmlPreview.host.IsBrowserInitialized)
                htmlPreview.host.LoadHtml((string)e.NewValue);
        }

        private static void TitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var htmlPreview = (HtmlPreview)d;
            if (htmlPreview.host != null && htmlPreview.host.IsBrowserInitialized)
                htmlPreview.host.Title = (string)e.NewValue;
        }

        private static void LocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DocDirectory = (string)e.NewValue;
        }

        private static void ScrollPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var htmlPreview = (HtmlPreview)d;
            if (htmlPreview.host != null && htmlPreview.host.IsBrowserInitialized)
            {
                var javascript = string.Format("window.scrollTo(0,{0} * (document.body.scrollHeight - document.body.clientHeight));", e.NewValue);
                htmlPreview.host.ExecuteScript(javascript);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            host.PropertyChanged += host_PropertyChanged;
        }

        private void host_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsBrowserInitialized":
                    if (host.IsBrowserInitialized)
                        Dispatcher.BeginInvoke((System.Action)InitializeData);
                    break;
            }
        }

        private void InitializeData()
        {
            host.LoadHtml(Html);
            host.Title = Title;
        }
    }
}