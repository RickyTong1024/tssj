@echo off
set center_url=
set /p center_url=center_url:
@echo on

cd main
rd server /s /q
mkdir server
cd server
mkdir conf
mkdir config
mkdir src
cd src
mkdir libgame
mkdir gs
mkdir chat
cd ..
cd ..
xcopy ..\..\..\server\config\*.* .\server\config /e /q /h
xcopy ..\..\..\server\src\libgame .\server\src\libgame /e /q /h
xcopy ..\..\..\server\src\gs .\server\src\gs /e /q /h
xcopy ..\..\..\server\src\chat .\server\src\chat /e /q /h
xcopy ..\..\..\server\src\master .\server\src\master /e /q /h
copy ..\..\..\server\Makefile.am .\server
copy ..\..\..\server\configure.ac .\server
rd common /s /q
mkdir common
cd common
mkdir protocol
mkdir protocpp
cd ..
xcopy ..\..\..\common\protocol\*.* .\common\protocol /e /q /h
xcopy ..\..\..\common\protocpp\*.* .\common\protocpp /e /q /h
xcopy ..\..\..\server\sub_server\common\*.py .\common /e /q /h
rd gate1 /s /q
mkdir gate1
xcopy ..\..\..\server\sub_server\gate\*.py .\gate1 /e /q /h
rd gate2 /s /q
mkdir gate2
xcopy ..\..\..\server\sub_server\gate\*.py .\gate2 /e /q /h
rd gate3 /s /q
mkdir gate3
xcopy ..\..\..\server\sub_server\gate\*.py .\gate3 /e /q /h
rd gate4 /s /q
mkdir gate4
xcopy ..\..\..\server\sub_server\gate\*.py .\gate4 /e /q /h
rd libao /s /q
mkdir libao
xcopy ..\..\..\server\sub_server\libao\*.py .\libao /e /q /h
rd login /s /q
mkdir login
xcopy ..\..\..\server\sub_server\login\*.py .\login /e /q /h
rd remote /s /q
mkdir remote
xcopy ..\..\..\server\sub_server\remote\*.py .\remote /e /q /h
cd ..

copy game.json .\main\server\conf
copy server.json .\main\server\conf
copy gate1.py .\main\gate1\config.py
copy gate2.py .\main\gate2\config.py
copy gate3.py .\main\gate3\config.py
copy gate4.py .\main\gate4\config.py
copy libao.py .\main\libao\config.py
python replace.py .\main\libao\config.py {{center_url}} %center_url%
copy login.py .\main\login\config.py
python replace.py .\main\login\config.py {{center_url}} %center_url%
copy remote.py .\main\remote\config.py
python replace.py .\main\remote\config.py {{center_url}} %center_url%
copy replace.py .\main\replace.py
copy make.sh .\main\server
copy makeex.sh .\main\server
copy cp.sh .\main\server
copy setff.sh .\main
copy start.sh .\main
copy stop.sh .\main
copy stopex.sh .\main
copy updatebak.sh .\main

pause
