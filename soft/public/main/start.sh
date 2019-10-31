#!/bin/bash

#start

cd gate1
nohup python3 -u gate.py work{{slot}} >/dev/null 2>&1 &
cd ../gate2
nohup python3 -u gate.py work{{slot}} >/dev/null 2>&1 &
cd ../gate3
nohup python3 -u gate.py work{{slot}} >/dev/null 2>&1 &
cd ../gate4
nohup python3 -u gate.py work{{slot}} >/dev/null 2>&1 &
cd ../libao
nohup python3 -u libao.py work{{slot}} >/dev/null 2>&1 &
cd ../login
nohup python3 -u login.py work{{slot}} >/dev/null 2>&1 &
cd ../remote
nohup python3 -u remote.py work{{slot}} > nohup.out 2>&1 &
cd ../server/out
nohup ./master ../conf work{{slot}} > nohup.out 2>&1 &

exit 0
