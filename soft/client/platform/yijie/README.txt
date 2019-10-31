一些渠道中gradle的版本号、包名、签名有所不同
其中包名和签名可以在易接工具中修改
所以分为通用gradle 和 单独gradle
打包时替换掉工程中的gradle

签名 yijie ,版本 1：
魅族 渠道当前gradle  包名 com.nsxqnb.mz 版本 为 1，签名 yijie  别名 yijie  渠道ID：8DD43FECE77A64DE  CHANNEL：meizu
4399渠道当前gradle  包名 com.yymoon.nsxq.m4399 版本 为 1，签名 yijie  别名  yijie 渠道ID：4CB4C42B71641CB3  CHANNEL：m4399
酷派 渠道当前gradle 包名 com.nsxq.coolpad 版本 为 1，签名 yijie  别名 yijie  渠道ID：5F9E9900D9CCC2D0
金立 渠道当前gradle 包名 com.nsxqnb.am 版本 为 1，签名 yijie  别名 yijie  渠道ID：0993AC81D95D4E48  CHANNEL：jinli
联想 渠道当前gradle 包名 com.nsxq.lenovo 版本 为 1，签名 yijie  别名 yijie  渠道ID：81AB24E2162D54BC  CHANNEL：lenovo
应用汇 渠道当前gradle 包名 com.nsxqnb.yyh 版本 为 1，签名 yijie  别名 yijie  渠道ID: ACAA9433B9649EA6  CHANNEL：yyh
安智 渠道当前gradle 包名 com.nsxqnb.anzhi 版本 为 1，签名 yijie  别名 yijie  渠道ID: A2D2F4AED400E281  CHANNEL：anzhi
机锋 渠道当前gradle 包名 com.nsxqnb.gfan 版本 为 1，签名 yijie  别名 yijie  渠道ID: 152E84D3CAB12856  CHANNEL：jifeng
木蚂蚁 渠道当前gradle 包名 com.nsxqnb.mumayi 版本 为 1，签名 yijie  别名 yijie  渠道ID: FC9729AE50FCAD69  CHANNEL：mumayi
聚乐 渠道当前gradle 包名 com.nsxqnb.htc 版本 为 1，签名 yijie  别名 yijie  渠道ID: C6B5708195B3725C  CHANNEL：jvle
夜神 渠道当前gradle 包名 com.nsxqnb.yeshen 版本 为 1，签名 yijie  别名 yijie  渠道ID: 07441320EB983FC6  CHANNEL：yeshen
天橙游玩 渠道当前gradle 包名 com.tcyw.nsxq 版本 为 1，签名 yijie  别名 yijie  渠道ID: A7ADEF1F32E55703  CHANNEL：tcyw
游戏fan 渠道当前gradle 包名 com.nsxq.jh 版本 为 1，签名 yijie  别名 yijie  渠道ID: 6EDC1D7C654416C7  CHANNEL：jh

签名 yijie ,版本 2：
vivo 渠道当前gradle  包名 com.yinyuewangluo.nvshenxingqiu.vivo 版本 为 2，签名 yijie  别名 yijie  渠道ID：5EFCB428547E62B1  CHANNEL：vivo
uc 渠道当前gradle  包名 com.yymoon.nsxq.aligames 版本 为 2，签名 yijie  别名 yijie  渠道ID：F52F35C5A04A1876  CHANNEL：uc
百度 渠道当前gradle  包名 com.yymoon.nsxq.baidu 版本 为 2，签名 yijie  别名 yijie  渠道ID：A15DC579667D6DA6  CHANNEL：baidu

签名 yijie ,版本 11：
360 渠道当前gradle 包名 com.nsxq.yymoon.qihu360 版本 为 10，签名 yijie  别名 yijie  渠道ID：E7FDED8015C8FD56  CHANNEL：360

签名 xiaomi 别名 googlenba ,版本 3：
小米 渠道当前gradle 包名 com.nsxqnb.mi 版本 3，签名 xiaomi 别名 googlenba  渠道ID：DD72FEA8BCEE13F4  CHANNEL：xiaomi

特殊：
鲁大师 
注意：渠道当前gradle  编译版本:27  使用V4:27.1.1工具包 包名 com.nsxq.ludashi 版本 为 1，签名 yijie  别名 yijie  渠道ID：97E532E4E2C9FF14  CHANNEL：ludashi
      
UC  VIVO 为特殊渠道不能批量编译
聚乐渠道内xml一些属性定义与母包冲突，根据error文档删除母包内的属性即可