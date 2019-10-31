# 服务器部署

## 后台搭建

1. 安装python环境，需要安装以下python库：

   ```shell
   pip install django==1.6.0
   pip install django-bootstrap-toolkit
   pip install uwsgi
   pip install fabric==1.13.1
   pip install django-crontab==0.6.0
   pip install lxml
   pip install httplib
   ```

   

2. 将gtool目录拖到服务器上

3. 修改gtool目录下settings.py文件，DEBUG=True改为DEBUG=Flase。如下所示：

   ```python
   DEBUG = False
   ```

4. 修改gttool目录下settings.py文件，修改MySQL配置，如下所示示例：

   ```python
   DATABASES = {
          'default': {
              'ENGINE': 'django.db.backends.mysql',
              'NAME': 'gttool',
              'USER': 'root',
              'PASSWORD': 'root',
              'HOST': '127.0.0.1',
          }
      }
   ```

5. 新建一个数据库名叫gttool

6. 在libao_web(gttool)目录下运行下面的命令，生成数据库表

   ```shell
    python manager.py syncdb
   ```

7. 修改django.xml表中的socket端口（可以任意设置）和chdir目录为gttool所在的目录

   ```xml
   <uwsgi>
          <socket>:9090</socket>           <!-- socket端口 --> 
          <chdir>/root/app/gttool</chdir>  <!-- chdir目录 --> 
          <module>django_wsgi</module>
          <processes>2</processes> <!-- 进程数 --> 
          <daemonize>uwsgi.log</daemonize> <!-- 日志 --> 
   </uwsgi>
   ```

8. 修改nginx(/etc/nginx/nginx.conf)配置，其中添加

   ```json
   server {
              listen 80;
              location /static/ {
                  alias /root/app/gttool/static/;
              }
              location / {
                  uwsgi_pass 127.0.0.1:9090;
                  include uwsgi_params;
              }
          }
   ```

   其中uwsgi_pass转发地址为django.xml表中socket端口号对应

9. 重启nginx

   ```shell
   nginx -t        // 检查nginx配置是否正确
   nginx -s reload // 重启nginx
   ```

10. 进入gtool所在目录启动服务器

    ```shell
    uwsgi -x django.xml
    ```

11. 浏览器中启动ip来访问

12. 配置定时器

    1. 配置cron.py文件中的tongji_normal和tongji_liucun,tongji_qudao_normal,tongji_qudao_liucun函数中日志服地址，如下：

       ```python
       logdb = torndb.Connection(host = "127.0.0.1", database = "tslog", user = "root", password = "root", time_zone = "+8:00")
       ```

    2. 启动定时器

       ```shell
       python manager.py crontab add
       ```

13. 配置后台deploy

    1. 在/root/app文件下建立一个文件夹deploy

    2. 进入到本地public目录中，修改deploy.py中的hosts，project_dev_root为正确地址和打包目录

       ```python
       api.env.user = "root"
       api.env.hosts = ["127.0.0.1"]         # 需要部署地址
       api.env.password = "yymoon@39299911"
       api.env.project_dev_root = "E:\\zhanliang\\soft\\public"
       api.env.project_dev_root = "E:\\zhanliang\\soft\\public\\version"
       ```

    3. 运行deploy.bat,会提示输入版本ID（随便填，不相同），平台（ios,apple等等），礼包服地址（国内都为121.43.107.164），pvp服地址（搭建的PVP和team所在服务器外网地址）

    4. 在网页后台server lists中配置开服参数

       | 平台               | 填写andorid或者ios                                    |
       | ------------------ | ----------------------------------------------------- |
       | 渠道               | 渠道名                                                |
       | 当前版本           | 运行deploy.bat中输入的版本ID                          |
       | 当前服务器ID       | 该渠道第一个服务器的服务器ID，以后部署会自动增长      |
       | 部署源代码路径     | /root/app/deploy/平台(运行deploy.bat中输入的平台)     |
       | oss服务器列表路径  | 比如config/config_yymoon.xml                          |
       | 活动地址           | 填写127.0.0.1                                         |
       | 充值地址           | 填写121.43.107.164                                    |
       | 是否使用合并配置表 | 如果勾选，将使用config_yymoon.xml否则是serverlist.xml |
       | 是否需要配置服务器 | 如果有多个渠道，第二到第N个渠道不用勾选，否则要勾选   |
       | 分类平台           | 随便填，如果有多个子渠道，填写一样的                  |

    5. 在后台Open servers中配置开服所需要的服务器

    6. 在Gonggao platforms中配置公告oss地址



## 日志服搭建

1. 将tools/netsvcs/lib文件中的**lib.mpc**,**My_Logging_Strategy.h**和**My_Logging_Strategy.cpp**拖到服务器下面的目录中

   ```shell
   /root/app/deps/ACE_wrappers/netsvcs/lib
   ```

2. 将/root/app/deps/文件中的build_svc.sh拷贝到/root/app/deps/ACE_wrappers文件夹中，运行./build_svc.sh编译日志服

3. 将/root/app/deps/ACE_wrappers/lib中编译的libnetsvcs.so拷贝到/usr/local/lib文件中，然后运行

   ```shell
   ldconfig
   ```

4. 在root/app文件加中建立一个文件夹log_server，将/root/app/deps/ACE_wrappers/netsvcs/servers中的ace_netsvcs拷贝到log_server中

5. 将本机tools/netsvcs/文件中的start.sh和svc.conf拷贝到/root/app/log_server中

6. 在log_server中建立两个文件夹new_log和old_log

7. 修改svc.conf中第一行配置中日志目录文件位置，如下所示：

   ```shell
   dynamic Logger Service_Object * netsvcs:_make_My_Logging_Strategy() "-s /root/app/log_server/new_log/log -i 60 -m 1024 -f OSTREAM|VERBOSE"
   ```

8. 修改start.sh中的client.conf改为svc.conf，如下所示：

   ```shell
   nohup ./ace_netsvcs -f svc.conf >/dev/null 2>&1 &
   ```

9. 启动日志服,运行

   ```shell
   ./start.sh
   ```

10. 检查new_log中是否生成log文件，cat log日志，查看是否正确启动

11. 配置日志解析

    1. 新建一个数据库tslog,导入tslog的表

    2. 将tools\LogParse\LogParse文件夹拖到服务器/root/app/log_server中

    3. 修改/root/app/log_server/LogParse/LogParse.py中的src_dir和target_dir，如下所示:

       ```python
       src_dir = "/root/app/log_server/new_log"
       target_dir = "/root/app/log_server/old_log"
       ```

    4. 修改/root/app/log_server/LogParse/LogParse.py中sqlbackupformat，如下所示：

       ```python
       sqlbackupformat="mysqldump -uroot -proot tslog > /root/app/log_server/old_log/tslog%s.sql" 
       ```

    5. 运行./parse.sh看是否运行正常(old_log 中多了一个2018-06-21的文件)

    6. 添加定时器，如下所示:

       ```shell
       */10 * * * * cd /root/app/log_server/LogParse && ./parse.sh
       ```

    7. 在游戏服运行一段时间以后，查看tslog中是否有数据，检查是否正确解析日志



## 日志代理搭建

每台游戏服需要搭建一个日志代理，该日志代理会连接到上面所配置的日志服上。

1. 在/root/app文件夹下建立一个文件夹log_proxy

2. 将/root/app/deps/ACE_wrappers/netsvcs/servers/ace_netsvcs拷贝到log_proxy中

3. 将本机tools/netsvcs/文件中的start.sh和client.conf拷贝到/root/app/log_proxy中

4. 修改client.conf中日志服地址，如下所示: 将host改为日志服所在的内网ip地址

   ```shell
   dynamic Client_Logging_Service Service_Object * netsvcs:_make_ACE_Client_Logging_Acceptor() active "-p 20009 -h host"
   ```

5. 进入/usr/local/lib,添加软链接

   ```shell
   ln -s libnetsvcs.so.6.2.7 libnetsvcs.so
   ldconfig
   ```

   

6. 运行./start.sh启动日志代理服务器

7. 在游戏服运行一段时间以后，查看tslog中是否有数据，检查是否有该服的日志



## 礼包服搭建

如果需要开启礼包功能，则需要配置礼包服。

1. 将本机的tools/libao_server目录拖到服务器上

2. 新建一个数据库tslibao,将tslibao.sql导入

3. 开启礼包服务器

   ```shell
   nohup python -u libao_server.py&
   ```

礼包服默认监听端口是40100

4. 修改后台gtool中forms.py的ClibaoForm，DlibaoForm，LibaoItems类的礼包服数据库地址，默认是后台跟礼包服是同一台服务器。如果是在同一台机器上，则无需修改,形如以下：

   ```python
   db = MySQLdb.connect(user='root', passwd='root', db='tslibao', host='127.0.0.1', charset='utf8')
   ```

5. 修改了gtool，则需要重启gttool后台

6. 游戏中测试礼包功能