# 运维说明

## 爱丽游CDN

自动开服的时候，上传服务器列表地方，爱丽游从ftp改到了百度云。百度云SDK只支持python2.7版本，我们使用的python2.6版本无法使用它的sdk。不过百度云提供了cmd工具，所以可以从python来运行可执行文件来上传服务器列表到百度云。现在依赖爱丽游CDN的有爱丽游渠道（已修改），长尾渠道（已修改）和黑桃渠道。使用该工具说明如下：

1. 下载 https://sdk.bce.baidu.com/console-sdk/linux-bcecmd-0.2.1.zip
2. 解压并拷贝bcecmd到/usr/bin
3. 运行bcecmd -c 来设置一些参数，其中Access Key 为83ea88d865e040eb84451ddf23489af2，Secret Key为9c9296c15fa94f7b8efd678da25ae30f，Region Name 为gz,Domain为gz.bcebos.com
4. 然后拷贝gtool文件下aliyou文件夹中爱丽游配置，这里要注意cron.py脚本中的配置参数要改为你自己平台配置（千万注意）
5. 修改后台Files addresss中oss配置全部改为百度云配置，其中oss地址改为：gz.bcebos.com，oss用户名和密码该为上面的的Access Key和Secret key,oss目录为yymoon
6. 修改gonggao platforms同步方式为2，serverlists中同步方式为2