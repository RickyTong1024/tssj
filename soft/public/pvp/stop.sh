#!/bin/bash

#stop

kill $(ps -ef | grep 'python3 -u gate.py workpvp'|awk '{print $2}')
kill $(ps -ef | grep './pvp pvp1 ../conf workpvp'|awk '{print $2}')
