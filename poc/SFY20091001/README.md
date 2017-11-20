# Outlook PR_ATTACH_METHOD file execution vulnerability
It has been discovered that certain e-mail message cause Outlook to create Windows shortcut-like attachments or messages within Outlook. Through specially crafted TNEF streams with certain MAPI attachment properties, it is possible to set a path name to files to be executed. When a user double clicks on such an attachment or message, Outlook will proceed to execute the file that is set by the path name value. These files can be local files, but also file stored remotely for example on a file share. Exploitation is limited by the fact that its is not possible for attackers to supply command line options.

Microsoft released MS10-045 that blocks unsafe use of the PR_ATTACH_METHOD property in e-mail messages.

See also:
https://securify.nl/advisory/SFY20091001/outlook-pr-attach-method-file-execution-vulnerability.html