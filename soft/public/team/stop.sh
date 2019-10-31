#!/bin/bash

#stop

kill $(ps -ef | grep 'python3 -u gate.py workteam'|awk '{print $2}')
kill $(ps -ef | grep './team team1 ../conf workteam'|awk '{print $2}')
