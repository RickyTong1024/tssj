#!/bin/bash

#部署账号数据库
cd account
mysql -uroot -proot <<EOF
creat database tsuser dafault charset utf8 collate utf8_general_ci;
EOF
mysql -uroot -proot --default-character-set=utf8 tsuser < tsuser.sql
cd ..

#部署充值数据库
cd pay
mysql -uroot -proot <<EOF
create database tsjhpay default charset utf8 collate utf8_general_ci;
EOF
mysql -uroot -proot --default-character-set=utf8 tsjhpay < pay.sql
cd ..

#部署礼包数据库
cd libao_server
mysql -uroot -proot <<EOF
create database tslibao default charset utf8 collate utf8_general_ci;
EOF
mysql -uroot -proot --default-character-set=utf8 tslibao < tslibao.sql
cd ..

#部署储存数据库
cd storage_server
mysql -uroot -proot <<EOF
create database tsstorage default charset utf8 collate utf8_general_ci;
EOF
mysql -uroot -proot --default-character-set=utf8 tsstorage < storage.sql
cd ..

#提升start.sh权限
chmod -R 777 start_server.sh