using System.Management.Automation;
using System.Runtime.InteropServices;

namespace MiffTheFox.PowerSleepWake;

[Cmdlet(VerbsCommon.Enter, "Sleep")]
public partial class EnterSleep : Cmdlet
{
    [LibraryImport("Powrprof.dll", SetLastError = true)]
    private static partial uint SetSuspendState([MarshalAs(UnmanagedType.Bool)] bool hibernate, [MarshalAs(UnmanagedType.Bool)] bool forceCritical, [MarshalAs(UnmanagedType.Bool)] bool disableWakeEvent);

    protected override void ProcessRecord()
    {
        if (Platform.IsWindows)
        {
            SetSuspendState(false, false, false);
        }
        else
        {
            WriteError(new ErrorRecord(new PlatformNotSupportedException(), "NotWindows", ErrorCategory.NotSpecified, null));
        }
    }
}
