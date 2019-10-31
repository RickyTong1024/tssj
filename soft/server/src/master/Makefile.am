noinst_PROGRAMS = master
master_SOURCES = master.cpp

INCLUDES = -I$(top_builddir)/src/libgame -I/usr/local/include

master_LDFLAGS = -lpthread -ldl -lrt -lmysqlpp -lACE -lboost_thread -lboost_date_time -lboost_filesystem -lboost_system -lprotobuf -lzmq -lz

master_LDADD = $(top_builddir)/src/libgame/libgame.a

