#!/bin/bash

#stop

kill $(ps -ef | grep 'python3 -u gate.py work{{slot}}'|awk '{print $2}')
kill $(ps -ef | grep 'python3 -u libao.py work{{slot}}'|awk '{print $2}')
kill $(ps -ef | grep 'python3 -u login.py work{{slot}}'|awk '{print $2}')
kill $(ps -ef | grep 'python3 -u remote.py work{{slot}}'|awk '{print $2}')
kill $(ps -ef | grep './master ../conf work{{slot}}'|awk '{print $2}')

exit 0
