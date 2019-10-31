noinst_PROGRAMS = team
team_SOURCES =               \
    equip_config.cpp         \
    equip_operation.cpp      \
    guild_config.cpp         \
    item_config.cpp          \
    item_operation.cpp       \
    mission_config.cpp       \
    mission_fight.cpp        \
    mission_operation.cpp    \
    player_config.cpp        \
    pvp_config.cpp           \
    pool.cpp                 \
    role_config.cpp          \
    role_operation.cpp       \
    sport_config.cpp         \
    treasure_config.cpp      \
	treasure_list.cpp        \
	team.cpp                 \
    single.cpp               \
    client.cpp               \
	team_service.cpp         \
    team_config.cpp          \
    main.cpp
	
INCLUDES = -I$(top_builddir)/src/libgame -I/usr/local/include -I/usr/local/include/mysql++ -I/usr/include/mysql

team_LDFLAGS = -lpthread -ldl -lrt -lmysqlpp -lACE -lboost_thread -lboost_date_time -lboost_filesystem -lboost_system -lprotobuf -lzmq -lz

team_LDADD = $(top_builddir)/src/libgame/libgame.a

