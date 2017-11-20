# PulseAudio local race condition privilege escalation vulnerability
The PulseAudio binary is affected by a local race condition. If the binary is installed as SUID root, it is possible to exploit this vulnerability to gain root privileges. This attack requires that a local attacker can create hard links on the same hard disk partition on which PulseAudio is installed (i.e. /usr/bin and /tmp reside on the same partition).

A patch for PulseAudio was released that addresses this issue. This patch can be obtained from the following location:
http://git.0pointer.de/?p=pulseaudio.git;a=commit;h=84200b423ebfa7e2dad9b1b65f64eac7bf3d2114

This proof of concept can be used to exploit this issue. The proof of concept tries to exploit this issue by creating hard links in the /tmp directory.

See also:
https://securify.nl/advisory/SFY20090602/pulseaudio-local-race-privilege-escalation-vulnerability.html