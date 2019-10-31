cd packet
rd log_server /s /q
mkdir log_server
cd log_server
mkdir parse
cd ..
xcopy ..\..\..\server_python\netsvcs\lib\My_Logging_Strategy.h .\log_server\ /e /q /h
xcopy ..\..\..\server_python\netsvcs\lib\My_Logging_Strategy.cpp .\log_server\ /e /q /h
xcopy ..\..\..\server_python\netsvcs\lib\lib.mpc .\log_server\ /e /q /h
cd ..
copy svc.conf .\packet\log_server\ 
copy start_server.sh .\packet\log_server\ 
copy build_log_server.sh .\packet\log_server\ 
copy replace.py .\packet\log_server\ 
cd packet
xcopy ..\..\..\server_python\LogParse\LogParse\LogParse.py .\log_server\parse /e /q /h
xcopy ..\..\..\server_python\LogParse\LogParse\parse.sh .\log_server\parse /e /q /h
xcopy ..\..\..\server_python\LogParse\LogParse\tslog.sql .\log_server\parse /e /q /h
cd ..
xcopy replace.py .\packet\log_server\parse 


cd packet
rd libao_server /s /q
mkdir libao_server
xcopy ..\..\..\server_python\libao_server\libao_server.py .\libao_server\ /e /q /h
xcopy ..\..\..\server_python\libao_server\tslibao.sql .\libao_server\ /e /q /h
cd ..
copy start_libao.sh .\packet\libao_server\ 
copy build_libao_server.sh .\packet\libao_server\ 

pause
