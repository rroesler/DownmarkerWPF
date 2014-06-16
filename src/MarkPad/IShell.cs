using System;

namespace MarkPad
{
    public interface IShell
    {
        IDisposable DoingWork(string work);

        // should this be here ?
        bool ShowWeb { get; }
        bool ShowMD { get; }
    }
}