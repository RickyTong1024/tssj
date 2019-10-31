rq=`date +%Y%m%d`
cp /root/app/work0/server/out/nohup.out /root/app/logbak/logdata/0/nohup$rq.out
cat /dev/null > /root/app/work0/server/out/nohup.out
find /root/app/logbak/logdata/0 -mtime +7 -name "*.out" -delete
cp /root/app/work1/server/out/nohup.out /root/app/logbak/logdata/1/nohup$rq.out
cat /dev/null > /root/app/work1/server/out/nohup.out
find /root/app/logbak/logdata/1 -mtime +7 -name "*.out" -delete
cp /root/app/work2/server/out/nohup.out /root/app/logbak/logdata/2/nohup$rq.out
cat /dev/null > /root/app/work2/server/out/nohup.out
find /root/app/logbak/logdata/2 -mtime +7 -name "*.out" -delete
cp /root/app/work3/server/out/nohup.out /root/app/logbak/logdata/3/nohup$rq.out
cat /dev/null > /root/app/work3/server/out/nohup.out
find /root/app/logbak/logdata/3 -mtime +7 -name "*.out" -delete
cp /root/app/work4/server/out/nohup.out /root/app/logbak/logdata/4/nohup$rq.out
cat /dev/null > /root/app/work4/server/out/nohup.out
find /root/app/logbak/logdata/4 -mtime +7 -name "*.out" -delete
cp /root/app/work5/server/out/nohup.out /root/app/logbak/logdata/5/nohup$rq.out
cat /dev/null > /root/app/work5/server/out/nohup.out
find /root/app/logbak/logdata/5 -mtime +7 -name "*.out" -delete
cp /root/app/work6/server/out/nohup.out /root/app/logbak/logdata/6/nohup$rq.out
cat /dev/null > /root/app/work6/server/out/nohup.out
find /root/app/logbak/logdata/6 -mtime +7 -name "*.out" -delete
cp /root/app/work7/server/out/nohup.out /root/app/logbak/logdata/7/nohup$rq.out
cat /dev/null > /root/app/work7/server/out/nohup.out
find /root/app/logbak/logdata/7 -mtime +7 -name "*.out" -delete
