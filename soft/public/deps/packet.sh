#!/bin/bash

set -e

\cp CentOS-Base.repo /etc/yum.repos.d
\cp epel.repo /etc/yum.repos.d
systemctl disable firewalld
yum remove firewalld -y
yum install iptables-services -y
systemctl disable iptables

#sys
yum update -y
yum install gcc gcc-c++ bzip2 bzip2-devel bzip2-libs -y
yum install python-devel -y
yum install patch -y
yum install -y automake

#python3
tar -zxvf Python-3.6.5.tgz
yum install -y zlib*
yum -y install zlib-devel bzip2-devel openssl-devel ncurses-devel sqlite-devel readline-devel tk-devel gdbm-devel db4-devel libpcap-devel xz-devel
chmod -R 777 Python-3.6.5
cd Python-3.6.5/
./configure --prefix=/usr/local/python3 --with-ssl
make
make install
ln -s /usr/local/python3/bin/python3 /usr/bin/python3
ln -s /usr/local/python3/bin/pip3 /usr/bin/pip3
cd ..

#mysql change
rpm -ivh mysql57-community-release-el7-11.noarch.rpm
yum install mysql-community-server
yum install mysql-community-devel
cp my.cnf /etc
mkdir /home/app/mysqldata
chmod -R 777 /home/app/mysqldata
systemctl start mysqld
#获取初始密码以及修改初始密码
grep 'temporary password' /var/log/mysqld.log
set global validate_password_length=0;
set global validate_password_policy=0;
set password for 'root'@'localhost' = password('root');
systemctl enable mysqld
mysql -uroot -proot << EOF
grant all privileges on *.* to 'root'@'127.0.0.1' identified by 'root';
grant all privileges on *.* to 'root'@'localhost' identified by 'root';
grant all privileges on *.* to 'root'@'%' identified by '1qaz2wsx@39299911';
flush privileges;
create database tsjh0 default charset utf8mb4 collate utf8mb4_general_ci;
create database tsjh1 default charset utf8mb4 collate utf8mb4_general_ci;
create database tsjh2 default charset utf8mb4 collate utf8mb4_general_ci;
create database tsjh3 default charset utf8mb4 collate utf8mb4_general_ci;
create database tsjh4 default charset utf8mb4 collate utf8mb4_general_ci;
create database tsjh5 default charset utf8mb4 collate utf8mb4_general_ci;
create database tsjh6 default charset utf8mb4 collate utf8mb4_general_ci;
create database tsjh7 default charset utf8mb4 collate utf8mb4_general_ci;
EOF

cp -r mysqlbak /home/app
chmod 777 -R /home/app/mysqlbak

cp -r logbak /home/app
chmod 777 -R /home/app/logbak

cp -r updatebak /home/app
chmod 777 -R /home/app/updatebak

#cp crontab /etc/crontab
#/etc/rc.d/init.d/crond restart
# 这个不知道怎么写 再想一下
systemctl start crond
crontab -e
#以下两行在vi中输入
01 3 * * * root /home/app/mysqlbak/mysqlbak.sh
01 3 * * * root /home/app/logbak/logbak.sh
#执行crontab -l -u root查看
/bin/systemctl reload crond.service

#nginx change

vi /etc/yum.repos.d/nginx.repo
#输入
[nginx]
name=nginx repo
baseurl=http://nginx.org/packages/centos/$releasever/$basearch/
gpgcheck=0
enabled=1

#保存
yum install nginx -y
cp nginx.conf /etc/nginx
mkdir /usr/share/nginx/logs
mkdir /etc/nginx/logs
sudo /usr/sbin/nginx -c /etc/nginx/nginx.conf
systemctl enable nginx
/usr/sbin/nginx
# service nginx start

#boost
tar xvzf boost_1_55_0.tar.gz
chmod 777 -R boost_1_55_0
cd boost_1_55_0
./bootstrap.sh
./b2
./b2 install --prefix=/usr/local
cd ..

#mysql++
tar xvzf mysql++-3.2.1.tar.gz
chmod 777 -R mysql++-3.2.1
cd mysql++-3.2.1
./configure
make
make install
cd ..

#zmq
tar xvzf zeromq-4.0.4.tar.gz
chmod 777 -R zeromq-4.0.4
cd zeromq-4.0.4
./configure
make
make install
cd ..
cp zmq.hpp /usr/local/include

#ACE
tar xvzf ACE-6.2.7.tar.gz
chmod 777 -R ACE_wrappers
cd ACE_wrappers/ace
cp ../../config-linux.h config-linux.h
ln -s config-linux.h config.h
cd ../include/makeinclude
ln -s platform_linux.GNU platform_macros.GNU
cd ../..
export ACE_ROOT=/home/app/deps/ACE_wrappers
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$ACE_ROOT/ace
make
make install INSTALL_PREFIX=/usr/local
cd ..

#protobuf
tar xvzf protobuf-3.6.1.tar.gz
chmod 777 -R protobuf-3.6.1
cd protobuf-3.6.1
./configure
make
make install
cd ..

#python
pip3 install tornado==5.1.1
pip3 install zmq==0.0.0
pip3 install PyMySQL==0.9.2
pip3 install pyDes==2.0.1
pip3 install protobuf==3.6.1
pip3 install oss2
pip3 install rsa
pip3 install xmltodict
pip3 install fabric3

#system
cp .bash_profile /home
cp common.conf /etc/ld.so.conf.d

#ldconfig
ldconfig

#reboot
reboot