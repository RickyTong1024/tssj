del .\protocpp\*.pb.h
del .\protocpp\*.pb.cc

.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\acc.sproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\player.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\role.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\pet.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\equip.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\post.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\gtool.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=..\server\src\libgame .\protocol\rpc.proto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\other.proto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\sport.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\sport_list.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_list.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_member.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_event.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_red.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_arrange.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_fight.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_box.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_message.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\guild_mission.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\msg_player.proto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\msg_huodong.proto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\msg_self.proto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\rank.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\social.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\recharge_heitao.sproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\global.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\treasure.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\treasure_list.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\treasure_report.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\treasure_report.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\lottery_list.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\huodong.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\huodong_entry.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\boss.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\huodong_player.eproto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\msg_team.proto
.\etools\protoc.exe -I=.\protocol --cpp_out=.\protocpp .\protocol\rob_t.eproto

pause
