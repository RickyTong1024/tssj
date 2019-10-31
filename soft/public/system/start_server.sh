#!/bin/bash

# 开启账号服务器
cd account
nohup python3 -u account.py >nohup.out 2>&1 &
cd ..

# 开启充值服务器
cd pay
nohup python3 -u pay.py >nohup.out 2>&1 &
cd ..

# 开启礼包服务器
cd libao_server
nohup python3 -u libao_server.py >nohup.out 2>&1 &
cd ..

# 开启储存服务器
cd storage_server
nohup python3 -u storage.py >nohup.out 2>&1 &
cd ..

# 开启pvp服务器
cd pvp/server
chmod -R 777 make.sh
chmod -R 777 cp.sh
./make.sh
cd ..
chmod -R 777 start.sh
chmod -R 777 stop.sh
chmod -R 777 steff.sh
./start.sh
cd ..

# 开启team服务器
cd team/server
chmod -R 777 make.sh
chmod -R 777 cp.sh
./make.sh
cd ..
chmod -R 777 start.sh
chmod -R 777 stop.sh
chmod -R 777 steff.sh
./start.sh
cd ..