noinst_PROGRAMS = gs
gs_SOURCES =                \
    gs.cpp                  \
    gs_manager.cpp          \
    gs_message.cpp          \
    role_config.cpp         \
    role_manager.cpp        \
    role_operation.cpp      \
    player_config.cpp       \
    player_manager.cpp      \
    player_operation.cpp    \
    player_load.cpp         \
    player_pool.cpp         \
    equip_config.cpp        \
    equip_manager.cpp       \
    equip_operation.cpp     \
    mission_config.cpp      \
    mission_manager.cpp     \
    mission_pool.cpp        \
    mission_operation.cpp   \
    mission_fight.cpp       \
    item_config.cpp         \
    item_manager.cpp        \
    item_operation.cpp      \
    post_manager.cpp        \
    post_operation.cpp      \
    post_pool.cpp           \
    sport_manager.cpp       \
    sport_config.cpp        \
    sport_operation.cpp     \
    sport_pool.cpp          \
    social_manager.cpp      \
    social_operation.cpp    \
    social_pool.cpp         \
    social_config.cpp       \
    boss_manager.cpp        \
    boss_config.cpp         \
    guild_manager.cpp       \
    guild_operation.cpp     \
    guild_pool.cpp          \
    guild_config.cpp        \
    rank_manager.cpp        \
    rank_operation.cpp      \
    rank_pool.cpp           \
    huodong_manager.cpp     \
    huodong_operation.cpp   \
    huodong_config.cpp      \
    treasure_list.cpp       \
    treasure_config.cpp     \
    treasure_manager.cpp    \
    treasure_operation.cpp  \
    huodong_pool.cpp        \
    pvp_config.cpp          \
	pvp_manager.cpp         \
    pvp_operation.cpp       \
    global_pool.cpp

INCLUDES = -I$(top_builddir)/src/libgame -I/usr/local/include -I/usr/local/include/mysql++ -I/usr/include/mysql

gs_LDFLAGS = -lpthread -ldl -lrt -lmysqlpp -lACE -lboost_thread -lboost_date_time -lboost_filesystem -lboost_system -lprotobuf -lzmq -lz

gs_LDADD = $(top_builddir)/src/libgame/libgame.a

