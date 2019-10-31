# 合服

## 合服步骤

单独的合服分四步：

1. 合并数据库

   使用如下命令：

   ```
   fab -f hequ_one.py go_merge_db
   ```

2.  合并serverlist.xml或者config_yymoon.xml

   使用如下命令：

   ```
   fab -f hequ_one.py go_merge_oss
   ```

3. 删除work目录

   使用如下命令:

   ```
   fab -f hequ_one.py go_merge_remove_work
   ```

4. 合并gtool中的server

   使用如下命令：

   ```
   fab -f hequ_one.py go_merge_gtool
   ```

5. 对于长尾和爱丽游渠道 其他渠道不需要的  

   额外需多运行一个命令:

   ```
   fab -f hequ_one.py go_merge_recharge
   ```


6. 对于有礼包服的区服 合并礼包服

**<u>最终合服请使用如下命令</u>**：

```
fab -f hequ_one.py go
```

依次键入需要的命令:

1. db 合并数据库
2. oss 合并serverlist.xml或者config_yymoon.xml
3. work 删除work目录
4. gtool 合并gtool中的server
5. recharge 对于长尾和爱丽游渠道

注意必须按照顺寻来操作，操作完一组操作接着操作下一组.

其中合并数据库时间可能会很长

## 合服配置

这个很重要，不能弄错

* hequ_servers  填写合并的区服服务器id,可写填写多组

  比如服务器id为308到310合并为一组，服务器id为406到409合并为一组，则填写

  <u>hequ_servers=[(308,309,310), (406,407,408,409)],**服务器id不能填错，填重，这个很重要，不然区将合错,**</u>

  <u>**务必仔细检查**。</u>

* oss_path  oss或者ftp地址(长尾和爱丽游是使用ftp)

* oss_user oss用户名或ftp用户名

* oss_password oss密码或ftp密码

* oss_file 服务器列表(长尾和爱丽游是多个)

* oss_block 如果使用oss，则代表oss bucket

* oss_type 使用ftp还是oss

* oss_dowload_path serverlist下载路径 

* serverlist_type serverlist类型（现阶段国内苹果安卓官网渠道使用hebing,其他渠道都是normal)

* web_host 渠道web后台地址

* web_root 渠道web后台用户名

* web_password 渠道web后台密码

* web_db 渠道web后台数据库

* recharge_host 充值web后台地址(长尾和爱丽游使用)

* rechare_need （长尾和爱丽游渠道填True，其他渠道填false)