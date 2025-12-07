

# Generate Certificate

Download OpenSSL: [slproweb.com](https://slproweb.com/products/Win32OpenSSL.html)

Open PowerShell as Administrator to generate ROOT certificate:

use information below:
```
Country Name (2 letter code) [AU]:CA
State or Province Name (full name) [Some-State]:ONTARIO
Locality Name (eg, city) []:TORONTO
Organization Name (eg, company) [Internet Widgits Pty Ltd]:TWH
Organizational Unit Name (eg, section) []:TWH
Common Name (e.g. server FQDN or YOUR name) []:TWH
Email Address []:twh@twh.com
```
Run scripts below
```
openssl genrsa -out TWH.key -aes256
```
```
openssl req -x509 -new -extensions v3_ca -key TWH.key -days 1000 -out TWH.crt
```
```
openssl req -new -nodes -newkey rsa:2048 -keyout user.key -out user.csr
```
```
openssl x509 -CA TWH.crt -CAkey TWH.key -CAcreateserial -req -days=1000 -in user.csr -out user.crt
```
```
cmd /c copy user.key + user.crt user.pem
```
Verify certificate: 
```
openssl verify -verbose -CAfile TWH.crt user.pem
```
Verify .pem file:
```
openssl x509 -in user.pem -inform PEM -subject -nameopt RFC2253
```
Create admin user to local mongo:
```
db.getSiblingDB("$external").runCommand(
  {
    createUser: "emailAddress=twhuser@twh.com,CN=127.0.0.1,OU=TWH,O=TWH,L=TORONTO,ST=ONTARIO,C=CA",
    roles: [
             { role: 'readWrite', db: 'TWHDB' },
             { role: 'userAdminAnyDatabase', db: 'admin' }
           ],
    writeConcern: { w: "majority" , wtimeout: 5000 }
  }
)
```
Now create .pfx 
```
	certutil -mergepfx user.crt TWH.pfx
```
Copy certificate (TWH.pfx) in root directory: "c:\\" 
