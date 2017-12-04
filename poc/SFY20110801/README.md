# .NET Framework EncoderParameter integer overflow vulnerability
An integer overflow vulnerability has been discovered in the EncoderParameter class of the .NET Framework. Exploiting this vulnerability results in an overflown integer that is used to allocate a buffer on the heap. After the incorrect allocation, one or more user-supplied buffers are copied in the new buffer, resulting in a corruption of the heap.

By exploiting this vulnerability, it is possible for an application running with Partial Trust permissions to break from the CLR sandbox and run arbitrary code with Full Trust permissions.

This issue was resolved with the release of MS12-025. It appears the fix was part of a security push for System.Drawing.dll.

See also:
https://securify.nl/advisory/SFY20110801/_net-framework-encoderparameter-integer-overflow-vulnerability.html

https://securify.nl/en/blog/SFY20150401/tales-from-the-crypt_-exploiting-the-_net-encoderparameter-integer-overflow-vulnerability.html
