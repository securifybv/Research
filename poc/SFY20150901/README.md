# HP Color LaserJet CM2320 MFP Series multiple DLL side loading vulnerabilities
Multiple DLL side loading vulnerabilities were found in various COM components. These issues can be exploited by loading various these components as an embedded OLE object. When instantiating a vulnerable object Windows will try to load one or more DLLs from the current working directory. If an attacker convinces the victim to open a specially crafted (Office) document from a directory also containing the attacker's DLL file, it is possible to execute arbitrary code with the privileges of the target user. This can potentially result in the attacker taking complete control of the affected system.

There is currently no fix available, HP reports: "Unfortunately, the driver software for these devices can no longer be updated. The devices have ended support life and the engineering resources are no longer available to provide any firmware updates. We do understand the issue, and current drivers are no longer vulnerable to the OLE side load issue".

See also:
https://securify.nl/en/advisory/SFY20150901/leadtools-activex-control-multiple-dll-side-loading-vulnerabilities.html
https://securify.nl/en/advisory/SFY20150902/hp-tocommsg-dll-side-loading-vulnerability.html
https://securify.nl/en/advisory/SFY20150903/hp-laserjet-fax-preview-dll-side-loading-vulnerability.html