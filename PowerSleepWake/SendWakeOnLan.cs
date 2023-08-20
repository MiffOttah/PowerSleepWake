using System.Globalization;
using System.Management.Automation;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MiffTheFox.PowerSleepWake;

[Cmdlet(VerbsCommunications.Send, "WakeOnLan")]
public class SendWakeOnLan : Cmdlet
{
    [Parameter(HelpMessage = "The MAC address of the system to wake", Mandatory = true, Position = 0)]
    public string? MacAddress { get; set; }

    protected override void ProcessRecord()
    {
        try
        {
            var mac = _ParseMac();
            var magicPacket = _BuildMagicPacket(mac);
            _SendMagicPacket(magicPacket);
            //WriteDebug("magic packet data = " + string.Join("-", magicPacket.Select(x => x.ToString("x2"))));
        }
        catch (ErrorRecordException ex)
        {
            WriteError(ex.ErrorRecord);
        }
    }

    private byte[] _ParseMac()
    {
        if (string.IsNullOrEmpty(MacAddress)) throw new ErrorRecordException("Mac address cannot be empty", "MacEmpty", ErrorCategory.InvalidArgument);

        Span<char> macText = stackalloc char[12];
        int i = 0;

        foreach (char c in MacAddress)
        {
            if ("0123456789ABCDEFabcdef".Contains(c))
            {
                if (i >= macText.Length)
                {
                    throw new ErrorRecordException("Mac address is too long", "MacLong", ErrorCategory.InvalidArgument);
                }
                macText[i++] = c;
            }
            else if (c != ':' && c != '-')
            {
                throw new ErrorRecordException("Mac address contains invalid characters", "MacInvalid", ErrorCategory.InvalidArgument);
            }
        }

        if (i != 12)
        {
            throw new ErrorRecordException("Mac address is too short", "MacLong", ErrorCategory.InvalidArgument);
        }

        var macBytes = new byte[6];

        for (int j = 0; j < macBytes.Length; j++)
        {
            if (byte.TryParse(macText.Slice(j * 2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out byte b))
            {
                macBytes[j] = b;
            }
            else
            {
                throw new ErrorRecordException("Failed to parse mac address byte", "MacParseFail", ErrorCategory.InvalidArgument);
            }
        }

        return macBytes;
    }

    private static byte[] _BuildMagicPacket(byte[] mac)
    {
        const int packetLength = 16 * 6 + 6;
        var packet = new byte[packetLength];

        for (int i = 0; i < 6; i++)
        {
            packet[i] = 0xFF;
        }

        for (int i = 0; i < 16; i++)
        {
            mac.CopyTo(packet, (i + 1) * 6);
        }

        return packet;
    }

    private void _SendMagicPacket(byte[] magicPacket)
    {
        foreach (var iface in NetworkInterface.GetAllNetworkInterfaces().Where(i => i.OperationalStatus == OperationalStatus.Up))
        {
            foreach (var addr in iface.GetIPProperties().UnicastAddresses.Select(x => x.Address).Where(x => x.AddressFamily == AddressFamily.InterNetwork))
            {
                try
                {
                    using var udp = new UdpClient(new IPEndPoint(addr, 0));
                    udp.Send(magicPacket, "224.0.0.1", 9);
                    WriteDebug($"Sent magic packet on {addr}");
                }
                catch
                {
                }
            }
        }
    }
}
