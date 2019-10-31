#!/bin/bash

cur_path=$PWD
deploy_path=/root/app/service

cp My_Logging_Strategy.h /root/app/deps/ACE_wrappers/netsvcs/lib
cp My_Logging_Strategy.cpp /root/app/deps/ACE_wrappers/netsvcs/lib
cp lib.mpc /root/app/deps/ACE_wrappers/netsvcs/lib

export ACE_ROOT=/root/app/deps/ACE_wrappers
export PATH=$PATH:$ACE_ROOT/MPC
export LD_LIBRARY_PATH
echo "ACE_ROOT:" $ACE_ROOT
echo "Path:" $PATH


cd $ACE_ROOT/netsvcs/lib
mpc.pl -type make -include ../../bin/MakeProjectCreator/config lib.mpc
make -f Makefile.netsvcs

cd $ACE_ROOT/netsvcs/servers
mpc.pl -type make -include ../../bin/MakeProjectCreator/config servers.mpc
make -f  Makefile.Netsvcs_server

cd cur_path
cp /root/app/deps/ACE_wrappers/lib/libnetsvcs.so /usr/local/lib
cd /usr/local/lib
cp libnetsvcs.so libnetsvcs.so.6.2.7
ln -s libnetsvcs.so.6.2.7 libnetsvcs.so
ldconfig

cd cur_path


if [ ! -d "$deploy_path/log_server"]; then
    mkdir -p $deploy_path/log_server
    mkdir -p $deploy_path/log_server/server
    mkdir -p $deploy_path/log_server/new_log
    mkdir -p $deploy_path/log_server/old_log
fi

cp svc.conf $deploy_path/log_server/server
cp start_server.sh $deploy_path/log_server/server
cp /root/app/deps/ACE_wrappers/netsvcs/servers/ace_netsvcs $deploy_path/log_server/server
cd $deploy_path/log_server/server
python replace.py svc.conf {{deploy_path}} $deploy_path

cd cur_path

cp -rf parse $deploy_path/log_server
cd $deploy_path/log_server/parse
mysql -uroot -proot <<EOF
create database tslog default charset utf8 collate utf8_general_ci;
EOF
mysql -uroot -proot --default-character-set=utf8 tslog < tslog.sql
python replace.py LogParse.py {{deploy_path}} $deploy_path

cd cur_path


