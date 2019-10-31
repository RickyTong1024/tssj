@echo off
@echo on

rem pvp打包
cd packet
rd pvp /s /q
mkdir pvp
cd pvp
rd server /s /q
mkdir server
cd server
mkdir conf
mkdir config
mkdir src
cd src
mkdir libgame
mkdir pvp
cd ..
cd ..
xcopy ..\..\..\..\server\config\*.* .\server\config /e /q /h
xcopy ..\..\..\..\server\src\libgame .\server\src\libgame /e /q /h
xcopy ..\..\..\..\server\src\pvp .\server\src\pvp /e /q /h
rd common /s /q
mkdir common
cd common
mkdir protocol
mkdir protocpp
cd ..
xcopy ..\..\..\..\common\protocol\*.* .\common\protocol /e /q /h
xcopy ..\..\..\..\common\protocpp\*.* .\common\protocpp /e /q /h
xcopy ..\..\..\..\server\sub_server\common\*.py .\common /e /q /h
rd gate1 /s /q
mkdir gate1
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate1 /e /q /h
rd gate2 /s /q
mkdir gate2
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate2 /e /q /h
rd gate3 /s /q
mkdir gate3
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate3 /e /q /h
rd gate4 /s /q
mkdir gate4
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate4 /e /q /h
cd ..
cd ..

echo f | xcopy ..\pvp\Makefile.am .\packet\pvp\server\Makefile.am /e /q /h /y
echo f | xcopy ..\pvp\configure.ac .\packet\pvp\server\configure.ac  /e /q /h
echo f | xcopy ..\pvp\game.json .\packet\pvp\server\conf\game.json /e /q /h
echo f | xcopy ..\pvp\server.json .\packet\pvp\server\conf\server.json /e /q /h /y
python replace.py .\packet\pvp\server\conf\server.json {{ip}} %ip%
echo f | xcopy ..\pvp\gate1.py .\packet\pvp\gate1\config.py /e /q /h /y
python replace.py .\packet\pvp\gate1\config.py {{ip}} %ip%
echo f | xcopy ..\pvp\gate2.py .\packet\pvp\gate2\config.py /e /q /h /y
python replace.py .\packet\pvp\gate2\config.py {{ip}} %ip%
echo f | xcopy ..\pvp\gate3.py .\packet\pvp\gate3\config.py /e /q /h /y
python replace.py .\packet\pvp\gate3\config.py {{ip}} %ip%
echo f | xcopy ..\pvp\gate4.py .\packet\pvp\gate4\config.py /e /q /h /y
python replace.py .\packet\pvp\gate4\config.py {{ip}} %ip%
echo f | xcopy ..\pvp\make.sh .\packet\pvp\server\make.sh /e /q /h /y
echo f | xcopy ..\pvp\cp.sh .\packet\pvp\server\cp.sh /e /q /h /y
echo f | xcopy ..\pvp\setff.sh .\packet\pvp\setff.sh /e /q /h /y
echo f | xcopy ..\pvp\start.sh .\packet\pvp\start.sh /e /q /h /y
echo f | xcopy ..\pvp\stop.sh .\packet\pvp\stop.sh /e /q /h /y


rem team打包
cd packet
rd team /s /q
mkdir team
cd team
rd server /s /q
mkdir server
cd server
mkdir conf
mkdir config
mkdir src
cd src
mkdir libgame
mkdir team
cd ..
cd ..

xcopy ..\..\..\..\server\config\*.* .\server\config /e /q /h
xcopy ..\..\..\..\server\src\libgame .\server\src\libgame /e /q /h
xcopy ..\..\..\..\server\src\team .\server\src\team /e /q /h
rd common /s /q
mkdir common
cd common
mkdir protocol
mkdir protocpp
cd ..
xcopy ..\..\..\..\common\protocol\*.* .\common\protocol /e /q /h
xcopy ..\..\..\..\common\protocpp\*.* .\common\protocpp /e /q /h
xcopy ..\..\..\..\server\sub_server\common\*.py .\common /e /q /h
rd gate1 /s /q
mkdir gate1
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate1 /e /q /h
rd gate2 /s /q
mkdir gate2
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate2 /e /q /h
rd gate3 /s /q
mkdir gate3
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate3 /e /q /h
rd gate4 /s /q
mkdir gate4
xcopy ..\..\..\..\server\sub_server\gate\*.py .\gate4 /e /q /h
cd ..
cd ..

echo f | xcopy ..\team\Makefile.am .\packet\team\server\Makefile.am /e /q /h /y
echo f | xcopy ..\team\configure.ac .\packet\team\server\configure.ac /e /q /h /y
echo f | xcopy ..\team\game.json .\packet\team\server\conf\game.json /e /q /h /y
echo f | xcopy ..\team\server.json .\packet\team\server\conf\server.json /e /q /h /y
echo f | xcopy ..\team\gate1.py .\packet\team\gate1\config.py /e /q /h /y
echo f | xcopy ..\team\gate2.py .\packet\team\gate2\config.py /e /q /h /y
echo f | xcopy ..\team\gate3.py .\packet\team\gate3\config.py /e /q /h /y
echo f | xcopy ..\team\gate4.py .\packet\team\gate4\config.py /e /q /h /y
echo f | xcopy ..\team\make.sh .\packet\team\server\make.sh /e /q /h /y
echo f | xcopy ..\team\cp.sh .\packet\team\server\cp.sh /e /q /h /y
echo f | xcopy ..\team\setff.sh .\packet\team\setff.sh /e /q /h /y
echo f | xcopy ..\team\start.sh .\packet\team\start.sh /e /q /h /y
echo f | xcopy ..\team\stop.sh .\packet\team\stop.sh /e /q /h /y

rem 账号服打包
cd packet
rd account /s /q
mkdir account
xcopy ..\..\..\server_python\account_server\*.* .\account /e /q /h
cd ..

rem 充值服打包
cd packet
rd pay /s /q
mkdir pay
xcopy ..\..\..\server_python\pay_server\pay\*.* .\pay /e /q /h
cd ..

rem 礼包服打包
cd packet
rd libao_server /s /q
mkdir libao_server
xcopy ..\..\..\server_python\libao_server\*.* .\libao_server /e /q /h
cd ..

rem 储存服打包
cd packet
rd storage_server /s /q
mkdir storage_server
xcopy ..\..\..\server_python\storage_server\*.* .\storage_server /e /q /h
cd ..

rem 部署工具和一键运行
copy start_server.sh .\packet\
copy build_server.sh .\packet\

rem 日志服打包
rem 暂时没你啥事

rem 监控服打包
rem 需要重新写一个新的 你先在这边待定吧

pause
