# DLL side loading vulnerability in VMware Host Guest Client Redirector
A DLL side loading vulnerability was found in the VMware Host Guest Client Redirector, a component of VMware Tools. This issue can be exploited by luring a victim into opening a document from the attacker's share. An attacker can exploit this issue to execute arbitrary code with the privileges of the target user. This can potentially result in the attacker taking complete control of the affected system. If the WebDAV Mini-Redirector is enabled, it is possible to exploit this issue over the internet.

This issue has been fixed in VMware Tools for Windows version 10.0.6.

See also:
https://securify.nl/en/advisory/SFY20151201/dll-side-loading-vulnerability-in-vmware-host-guest-client-redirector.html