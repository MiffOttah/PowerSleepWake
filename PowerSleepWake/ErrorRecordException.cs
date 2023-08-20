using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.PowerSleepWake;

internal class ErrorRecordException : Exception
{
    internal ErrorRecord ErrorRecord { get; }

    public ErrorRecordException(Exception innerException, string errorId, ErrorCategory errorCategory)
        : base(innerException.Message, innerException)
    {
        ErrorRecord = new(innerException, errorId, errorCategory, null);
    }

    public ErrorRecordException(string message, string errorId, ErrorCategory errorCategory)
        : this (new Exception(message), errorId, errorCategory)
    {
    }
}
