# Internet Explorer iframe sandbox local file name disclosure vulnerability
It was found that Internet Explorer allows the disclosure of local file names. This issue exists due to the fact that Internet Explorer behaves different for file:// URLs pointing to existing and non-existent files. When used in combination with HTML5 sandbox iframes it is possible to use this behavior to find out if a local file exists. This technique only works on Internet Explorer 10 & 11 since these support the HTML5 sandbox. Also it is not possible to do this from a regular website as file:// URLs are blocked all together. The attack must be performed locally (works with Internet zone Mark of the Web) or from a share.

Microsoft released MS16-095 that fixes this vulnerability.

See also:
https://securify.nl/en/advisory/SFY20160301/internet-explorer-iframe-sandbox-local-file-name-disclosure-vulnerability.html