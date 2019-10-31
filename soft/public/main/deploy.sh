#!/bin/sh

place=ceshi
version=121
tar xvzf work.tar.gz

cd server
chmod -R 777 make.sh
# 创建版本号文件夹
cd /home/app
mkdir deploy
cd deploy
mkdir place
cd place
mkdir version
cd version
mkdir work
cd work
# 移动
cp -R /home/app/temp/.  /home/app/deploy/place/version/work