#!/bin/bash

#start

cd gate1
nohup python3 -u gate.py workteam >/dev/null 2>&1 &
cd ../gate2
nohup python3 -u gate.py workteam >/dev/null 2>&1 &
cd ../gate3
nohup python3 -u gate.py workteam >/dev/null 2>&1 &
cd ../gate4
nohup python3 -u gate.py workteam >/dev/null 2>&1 &
cd ../server/out
nohup ./team team1 ../conf workteam > nohup.out 2>&1 &
