# Office OLE multiple DLL side loading vulnerabilities
Multiple DLL side loading vulnerabilities were found in various COM components. These issues can be exploited by loading various these components as an embedded OLE object. When instantiating a vulnerable object Windows will try to load one or more DLLs from the current working directory. If an attacker convinces the victim to open a specially crafted (Office) document from a directory also containing the attacker's DLL file, it is possible to execute arbitrary code with the privileges of the target user. This can potentially result in the attacker taking complete control of the affected system. 

See also:
https://securify.nl/en/blog/SFY20151201/there_s-a-party-in-ole_-and-you-are-invited.html