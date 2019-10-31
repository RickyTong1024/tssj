#ifndef __MULTIQUERY_H__
#define __MULTIQUERY_H__

#include "sqlquery.h"

class MultiSqlacc_t : public SqlQuery
{
public:
	MultiSqlacc_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class MultiSqlpost_t : public SqlQuery
{
public:
	MultiSqlpost_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data) {}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class MultiSqlpost_list_t : public SqlQuery
{
public:
	MultiSqlpost_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data){}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
	int post_load_new_query(mysqlpp::Query& query);
};

class MultiSqlsocial_list_t : public SqlQuery
{
public:
	MultiSqlsocial_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data){}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class MultiSqlrecharge_heitao_t : public SqlQuery
{
public:
	MultiSqlrecharge_heitao_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data){}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class MultiSqlhuodong_list_t : public SqlQuery
{
public:
	MultiSqlhuodong_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data){}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class MultiSqlplayer_list_t : public SqlQuery
{
public:
	MultiSqlplayer_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data){}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};


class MultiGuildPvp_list_t : public SqlQuery
{
public:
	MultiGuildPvp_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data){}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

class MultiGuildFight_list_t : public SqlQuery
{
public:
	MultiGuildFight_list_t(uint64_t guid, google::protobuf::Message *data) : SqlQuery(guid, data){}
	virtual int insert(mysqlpp::Query& query);
	virtual int query(mysqlpp::Query& query);
	virtual int update(mysqlpp::Query& query);
	virtual int remove(mysqlpp::Query& query);
};

#endif
