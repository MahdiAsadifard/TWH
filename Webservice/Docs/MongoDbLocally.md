
# Install Mongodb community
### Learn More: [Install MongoDB](https://www.mongodb.com/docs/manual/installation/)

- Download from mongodb official website [MongoDB Community Server Download](https://www.mongodb.com/try/download/community)
- Go to directory below and add the path to your system environment path
```
C:\Program Files\MongoDB\Server\<INSTALLED_VERSION>\bin
``` 
- Open cmd to verify insalltation completed
```
mongod -version
```
- here is the result sample if installed correctly
```
db version v8.0.3
Build Info: {
    "version": "8.0.3",
    "gitVersion": "",
    "modules": [],
    "allocator": "tcmalloc-gperf",
    "environment": {
        "distmod": "windows",
        "distarch": "x86_64",
        "target_arch": "x86_64"
    }
}
```
# 
### NoSQLBooster for Mongodb
- Download `NoSQLBooster for Mongodb` to access to the local database: [here](https://nosqlbooster.com/downloads)
- Click on `Connect`
- `New Connection`
- Basic tab
    - Server: locallhost
    - Port: 27017
- Save