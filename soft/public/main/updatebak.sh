#!/bin/bash

#updatebak

rq=`date +%Y%m%d`
mysqldump -uroot -proot tsjh{{slot}} > /root/app/updatebak/tsjh{{slot}}$rq.sql
