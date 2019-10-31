运行system.bat将打包日志服和礼包服。生成的打包的文件在packet里，部署的时候将packet拖到目标机器上。

## 礼包服

libao_server打包文件中有个build_libao_server.sh，在目标机器上运行它，进行礼包服部署。默认的部署目录在

```
/root/app/service/libao_server
```

部署完毕之后，运行部署目录中start_libao.sh开启礼包服。默认的监听端口是:

```
40100
```



## 日志服

log_server打包文件中有个build_log_server.sh文件，在目标机器上运行它，进行日志服部署。默认的部署目录在

```
/root/app/service/log_server
```

部署完毕之后，运行部署server目录中start_server.sh开启日志服。

将部署目录server/parse里的parse.sh文件添加到一个每10分钟运行的定时器中:

```
*/10 * * * * cd /root/app/service/log_server/server/parse && ./parse.sh
```

nohup ./ace_netsvcs -f svc.conf >/dev/null 2>&1 &