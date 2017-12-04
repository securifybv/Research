# SyntaxHighlight MediaWiki extension allows injection of arbitrary Pygments options
A vulnerability was found in the SyntaxHighlight MediaWiki extension. Using this vulnerability it is possible for an anonymous attacker to pass arbitrary options to the Pygments library. By specifying specially crafted options, it is possible for an attacker to trigger a (stored) Cross-Site Scripting condition. In addition, it allows the creating of arbitrary files containing user-controllable data. Depending on the server configuration, this can be used by an anonymous attacker to execute arbitrary PHP code.

A fix for this issue is included in MediaWiki version 1.28.2 and version 1.27.3.

See also:
https://www.securify.nl/en/advisory/SFY20170201/syntaxhighlight-mediawiki-extension-allows-injection-of-arbitrary-pygments-options.html
