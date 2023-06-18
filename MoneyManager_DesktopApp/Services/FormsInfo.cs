using System;

namespace MoneyManager_DesktopApp.Services;

public class FormsInfo
{
    private bool _recordWindowIsOpen;
    private bool _loginWindowIsOpen;
    public event Action? OnChange;
    public bool RecordWindowIsOpen
    {
        get { return _recordWindowIsOpen; }
        set
        {
            if (_recordWindowIsOpen != value)
            {
                _recordWindowIsOpen = value;
                NotifyStateChanged();
            }
        }
    }
    public bool LoginWindowIsOpen
    {
        get { return _loginWindowIsOpen; }
        set
        {
            if (_loginWindowIsOpen != value)
            {
                _loginWindowIsOpen = value;
                NotifyStateChanged();
            }
        }
    }

    public bool CategoryWindowIsOpen { get; set; }

    private void NotifyStateChanged() => OnChange?.Invoke();
}