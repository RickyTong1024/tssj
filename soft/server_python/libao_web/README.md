# libao_web部署

## python环境

需要安装django和django-bootstrap-toolkit和uwsgi

```shell
pip3 install django==2.1.7
pip3 install django-bootstrap3
pip3 install django_crontab
pip3 install uwsgi
ln -s /usr/local/python3/bin/uwsgi /usr/bin/uwsgi3
```



## 部署

1. 将libao_web拖到服务器上

2. 修改gttool目录下settings.py文件，修改MySQL配置

   ```python
   DATABASES = {
       'default': {
           'ENGINE': 'django.db.backends.mysql',
           'NAME': 'tslibao_web',
           'USER': 'root',
           'PASSWORD': 'root',
           'HOST': '127.0.0.1',
       }
   }
   ```

3. 修改settings文件中的DEBUG=True改为DEBUG=False

   ```python
   # SECURITY WARNING: don't run with debug turned on in production!
   DEBUG = False
   ```

4. 建立一个数据库名叫上述配置‘NAME’中的名字

5. 在libao_web目录下运行下面的shell，生成表

   ```shell
   python3 manage.py makemigrations
   python3 manage.py migrate
   ```

6. 在libao_web目录下运行下面的shell，生成超级用户

   ```
   python3 manage.py createsuperuser
   ```

7. 修改django.xml表中的socket端口（可以任意设置）和chdir目录为libao_web所在的目录

   ```xml
   <uwsgi>
       <socket>:7020</socket>
       <chdir>/home/app/libao_web</chdir>
       <module>django_wsgi</module>
       <processes>2</processes> <!-- 进程数 --> 
       <daemonize>uwsgi.log</daemonize>
   </uwsgi>
   ```

8. 修改nginx(/etc/nginx/nginx.conf)配置添加

   ```json
     server {
           listen 7010;
           location /static/ {
               alias /home/app/libao_web/static/;
           }
           location / {
               uwsgi_pass 127.0.0.1:7020;
               include uwsgi_params;
           }
       }
   ```

   其中uwsgi_pass转发地址为django.xml表中socket端口号对应

9. 重启nginx

   ```shell
   nginx -t  // 检查nginx配置是否正确
   nginx -s reload // 重启nginx
   ```

10. 进入libao_web目录启动服务器

   ```shell
   uwsgi3 -x django.xml
   ```

11. 浏览器中尝试ip+端口来访问，其中ip+端口/admin为后台地址

12. 在后台Libao_servers中添加礼包服地址和端口，在server中添加需要发送礼包的游戏服务器

13. 测试发送礼包整个流程，正式上线



## 开发

1. 如果要部署海外版本的话，其中LibaoForm 中有个函数:

```python
def get_username(self, username):
        return username
```

你可以重写这个方法来获取正确的老帐号





2. 如果觉得每次开新服都需要在后台server中添加新服的配置，可以在服务器自动开服中添加一个步骤将新服插入到改server中。



3. 保证高安全的情况下，可以在每次发放一个充值礼包码之后将礼包字段weixin设为1，在兑换的时候向礼包服请求检查该字段（exchange_libao函数中），如果该字段为0，说名该礼包码不是通过官方渠道发码，需要注意该玩家，甚至可以封号。