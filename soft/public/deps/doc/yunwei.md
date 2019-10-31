## 服务器部署：(开服)



1.oss上传新服公告，登录gttool后台(黑桃 不需要 公告)，查看服务器时间，页面下方创建服务器，依次填写完成后创建，弹出创建成功后点击生成的时间线的部署，等待部署完成，
  部署成功后会显示开服按钮，如果需要立即开服可以点击开服，服务器则不需要在等到开服时间才启动。
  服务器时间线显示的服务器id为当前的服务器id，随开服依次递增，服务器位置为所在地址的物理位置(服务器位置为0则为物理位置为work0)，服务器版本为deploy下的代码版本。
2.登录linux远程工具CRT查看是否生成新的服务器。
  例如：新服的物理位置为5，会生成work5目录，命令行ll查看生成时间是否有误。cd到work5/server，执行make.sh，检查是否编译成功。
3.到开服时间后，查看同步情况，如果是失败则服务器列表没有添加新的服务器列，检查cron.py文件的serverlist上传路径是否和gttool后台serverlist的Oss服务器列表路径一样。
4.服务器已开，CRT下执行 ps -ef | grep workx 命令查看是否正常。



## 服务器停服



1.如果是单服需要临时停一下，到workx下执行stop.sh，然后ps -ef |grep workx 检查是否已停服
2.如果全区停服，到本地 server\tools\yunwei 目录下，使用 IDLE (Python GUI) 打开ssh.py文件,找到 def make_server() 接口，检查需要停服的大区后台地址是否和接口参数相同，(不相同就进行修改)
然后登陆gttool后台的admin/engine/server/查看需要停服的服务器id，修改ssh.py文件14行为 serverids = [] + range(i,j+1),其中i为起始服务器id，j为末尾服务器id。
  例如：serverids = [] + range(1,14+1)，停服范围为serverid为1到14的服务器。main函数修改下为thread_do(stop, ())，其余的注释(make_server 是一直 开启的)，然后run module F5执行。stop命令需要执行2-3次以确保成功，可以到CRT下检查。
3.CRT下分别cd 到pvp和team下执行stop.sh，检查是否关闭。
4.main函数下修改为thread_do(updatebak, ()) 并执行，保存数据。



代码更新：

1.先执行停服所有操作，oss上传停服公告和维护公告，停服完成后执行thread_do(upload, ("D:/xxx/", "/yyy/",)) ,其中xxx表示本地需要上传的文件夹，yyy表示线上服务器要被替换的文件夹，
  例如：thread_do(upload, ("D:/workspace/game_english/soft/server/src/gs/", "/server/src/gs/",)) 上传新版本的gs代码
​		thread_do(upload, ("D:/workspace/game_english/soft/server/configs/", "/server/config/",)) 上传新版本的配置表
2.新版代码或文件上传成功后，执行 thread_do(make, ()) 编译代码，如果编译过程卡顿时间过长，重新执行run module F5，直至编译完成。
3.编译成功后，执行thread_do(cp, ()) 将编译后的可执行文件拷贝。
4.使用winscp上传替换pvp和temp服代码和文件，然后编译并启动。
5.使用winscp上传替换deploy文件下的代码和文件。
6.如果数据库条目有修改或增删，notepad++打开本地 soft\common 目录下的create_db.bat文件，修改为.\etools\create_db.exe .\protocol xxx yyy root pswd 1  其中xxx为服务器的数据库名，yyy为服务器地址，pswd为数据库密码
  例如：.\etools\create_db.exe .\protocol tsjh5 47.52.120.238 root 1qaz2wsx@39299911 1  表示更新work5的数据库。所有更新的服务器都要执行此操作，也就是（range(i,j+1)）包含的服务器。执行完成后打开本地navicat mysql执行tsjhx数据传输，
 （只要是更新后的tsjh都可以）保存后替换deploy文件夹下的tsjh.sql (如果有protocol的更新 ，需要上传到haiWai 的150 地址中，拖动上传)
7.main函数下修改为thread_do(start, ()) 并执行,CRT下检查服务器是否成功启动。



## 合服



1.先执行停服操作，只需要停需要合服的服务器，然后到本地 server\tools\hequ 目录下，拷贝hequ.py文件到需要合服的后台服务器地址的 app/temp 目录下。如果没有就创建一个temp。
2.vi hequ.py 检查get_server() 接口的MySQLdb.connect 参数，修改为服务器gttool后台地址。
3.修改hequ.py中 he = [x, y],其中x为 合并后的服务器id，y为合并的第一个服务器id，执行完成后y修改为下一个要合并的服务器id，依次执行， 运行hequ.py脚本
  例如：需要合并1001-1010区，则合并后的服务器id为1001，将这10个区的数据都合并到1001区，依次修改并执行 he = [1001,1002], he = [1001,1003] 。。。直至 he = [1001,1010].
4.合并完成后，在有空闲的服务器(合并后空下来的)所在app下创建temp文件夹，然后将空闲服务器mv到temp下。
  例如：1001-1010合服后，需要将1002-1010都cp到各自所在app的temp下，需要注意 1001是合并后的主服务器，不可cp。
  例如：1002服地址为120.13.3.168，物理位置为work2，则登录CRT地址为120.13.3.168，在app下执行cp work2 temp/work2。
5.将空闲区服的服务器地址修改为合并后主服务器的地址，物理位置改为主服务器的物理位置，合并区服改为主服务器的服务器id。如果充值地址不是gttool后台地址，则将充值地址的engine_server表也同样修改。
  例如：1001-1010合服后，1001为主服务器，它的服务器地址为 120.131.3.168，物理位置为3，则将1002-1010的服务器地址都改为 120.131.3.168，物理位置都改为3，合并区服都改为1001，注意1001的合并区服不需要改变。
  注意：使用mysql工具修改时要先备份engine_server表。
6.登录gttool后台 admin/engine/openserver/,依次打开空闲服务器所在地址，将该服务器物理位置对应site改为0。
  例如：work2是和服后空闲下来的服务器，地址为120.131.3.168，则在admin/engine/openserver/点击120.131.3.168，将site2置为0。
7.oss下载服务器列表，notepad++打开，修改空闲服务器所在条目，将列表中的 http, tcp, port 分别改为主服务器的http, tcp, port，其余不变，然后保存上传。修改时要注意，只修改合并后空闲的服务器。
8.数据库检查合并无误后，登录CRT，将主服务器开启，注意：合并后空闲的服务器不再开启。