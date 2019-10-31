## 打包 ##

运行pack.bat打包 生成的打包的文件在packet里

新建server文件夹

```
mkdir /root/app/server
```

用winscp将packet文件夹里面的所有东西传到server文件夹下

## 部署 ##

在server目录下 执行

```
chmod -R 777 build_server.sh
./build_server.sh
```

等待执行完毕

## 启动 ##

在tmp目录下 执行

```
./start_server.sh
```

用ps -ef | grep *** 命令查看执行结果