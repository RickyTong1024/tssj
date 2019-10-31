#!/bin/bash

#stopex

kill -s 9 $(ps -ef | grep './gs gs1 ../conf work{{slot}}'|awk '{print $2}')
kill -s 9 $(ps -ef | grep './chat chat1 ../conf work{{slot}}'|awk '{print $2}')
