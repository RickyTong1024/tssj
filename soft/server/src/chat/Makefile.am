noinst_PROGRAMS = chat
chat_SOURCES =                \
    chat.cpp                  \
    chat_manager.cpp

INCLUDES = -I$(top_builddir)/src/libgame -I/usr/local/include

chat_LDFLAGS = -lpthread -ldl -lrt -lmysqlpp -lACE -lboost_thread -lboost_date_time -lboost_filesystem -lboost_system -lprotobuf -lzmq -lz

chat_LDADD = $(top_builddir)/src/libgame/libgame.a

