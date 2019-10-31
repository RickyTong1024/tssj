#!/bin/bash


deploy_path=/root/app/service

if [ ! -d "$deploy_path/libao_server"]; then
    mkdir -p $deploy_path/libao_server
fi


cp libao_server.py  $deploy_path/libao_server
cp start_libao.sh $deploy_path/libao_server

mysql -uroot -proot <<EOF
create database tslibao default charset utf8 collate utf8_general_ci;
EOF
mysql -uroot -proot --default-character-set=utf8 tslibao < tslibao.sql




