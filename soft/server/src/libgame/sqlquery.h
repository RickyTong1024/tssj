#ifndef __SQLQUERY_H__
#define __SQLQUERY_H__

#include "gameinc.h"
#include "mysql++.h"

class SqlQuery
{
public:
	SqlQuery(uint64_t guid, google::protobuf::Message *data) : guid_(guid), data_(data) {}
	virtual int insert(mysqlpp::Query& query) = 0;
	virtual int query(mysqlpp::Query& query) = 0;
	virtual int update(mysqlpp::Query& query) = 0;
	virtual int remove(mysqlpp::Query& query) = 0;

protected:
	google::protobuf::Message *data_;
	uint64_t guid_;
};

class Sqlboss_t : public SqlQuery
{
public:
	Sqlboss_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlequip_t : public SqlQuery
{
public:
	Sqlequip_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlglobal_t : public SqlQuery
{
public:
	Sqlglobal_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlgtool_t : public SqlQuery
{
public:
	Sqlgtool_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_t : public SqlQuery
{
public:
	Sqlguild_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_arrange_t : public SqlQuery
{
public:
	Sqlguild_arrange_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_box_t : public SqlQuery
{
public:
	Sqlguild_box_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_event_t : public SqlQuery
{
public:
	Sqlguild_event_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_fight_t : public SqlQuery
{
public:
	Sqlguild_fight_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_list_t : public SqlQuery
{
public:
	Sqlguild_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_member_t : public SqlQuery
{
public:
	Sqlguild_member_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_message_t : public SqlQuery
{
public:
	Sqlguild_message_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_mission_t : public SqlQuery
{
public:
	Sqlguild_mission_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlguild_red_t : public SqlQuery
{
public:
	Sqlguild_red_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlhuodong_t : public SqlQuery
{
public:
	Sqlhuodong_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlhuodong_entry_t : public SqlQuery
{
public:
	Sqlhuodong_entry_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlhuodong_player_t : public SqlQuery
{
public:
	Sqlhuodong_player_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlpvp_list_t : public SqlQuery
{
public:
	Sqlpvp_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlpet_t : public SqlQuery
{
public:
	Sqlpet_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlplayer_t : public SqlQuery
{
public:
	Sqlplayer_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlpost_t : public SqlQuery
{
public:
	Sqlpost_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlrank_t : public SqlQuery
{
public:
	Sqlrank_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlrob_t : public SqlQuery
{
public:
	Sqlrob_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlrole_t : public SqlQuery
{
public:
	Sqlrole_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlsocial_t : public SqlQuery
{
public:
	Sqlsocial_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlsport_t : public SqlQuery
{
public:
	Sqlsport_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqlsport_list_t : public SqlQuery
{
public:
	Sqlsport_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqltreasure_t : public SqlQuery
{
public:
	Sqltreasure_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqltreasure_list_t : public SqlQuery
{
public:
	Sqltreasure_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class Sqltreasure_report_t : public SqlQuery
{
public:
	Sqltreasure_report_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

#endif
