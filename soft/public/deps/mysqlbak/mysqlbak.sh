rq=`date +%Y%m%d`
mysqldump -uroot -proot tsjh0 > /home/app/mysqlbak/mysqldata/0/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/0 -mtime +7 -name "*.sql" -delete
mysqldump -uroot -proot tsjh1 > /home/app/mysqlbak/mysqldata/1/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/1 -mtime +7 -name "*.sql" -delete
mysqldump -uroot -proot tsjh2 > /home/app/mysqlbak/mysqldata/2/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/2 -mtime +7 -name "*.sql" -delete
mysqldump -uroot -proot tsjh3 > /home/app/mysqlbak/mysqldata/3/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/3 -mtime +7 -name "*.sql" -delete
mysqldump -uroot -proot tsjh4 > /home/app/mysqlbak/mysqldata/4/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/4 -mtime +7 -name "*.sql" -delete
mysqldump -uroot -proot tsjh5 > /home/app/mysqlbak/mysqldata/5/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/5 -mtime +7 -name "*.sql" -delete
mysqldump -uroot -proot tsjh6 > /home/app/mysqlbak/mysqldata/6/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/6 -mtime +7 -name "*.sql" -delete
mysqldump -uroot -proot tsjh7 > /home/app/mysqlbak/mysqldata/7/tsjh$rq.sql
find /home/app/mysqlbak/mysqldata/7 -mtime +7 -name "*.sql" -delete
