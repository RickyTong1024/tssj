#include "dhc.h"
#include "sqlquery.h"
#include "multiquery.h"
#include "game_interface.h"
#include <boost/filesystem.hpp>

Dhc::Dhc()
: conn_(0)
{

}

Dhc::~Dhc()
{

}

int Dhc::init()
{
	conn_ = new mysqlpp::Connection;
	conn_->disable_exceptions();
	conn_->set_option(new mysqlpp::ReconnectOption(true));
	conn_->set_option(new mysqlpp::SetCharsetNameOption("utf8"));
	int port = boost::lexical_cast<int>(game::env()->get_db_value("port"));
	int ok = conn_->connect(game::env()->get_db_value("db").c_str(), game::env()->get_db_value("host").c_str(), game::env()->get_db_value("username").c_str(), game::env()->get_db_value("password").c_str(), port);
	if (!ok)
	{
		printf("Failed to connect database.\n");
		return -1;
	}

	return 0;
}

int Dhc::fini()
{
	conn_->disconnect();
	delete conn_;
	conn_ = 0;
	return 0;
}

int Dhc::do_request(Request *req)
{
	conn_->ping();
	int res = -1;
	switch(req->op())
	{
	case opc_insert:
		res = do_insert(req->guid(), req->data());
		break;
	case opc_query:
		res = do_query(req->guid(), req->data());
		if (req->data())
		{
			req->data()->clear_changed();
		}
		break;
	case opc_update:
		res = do_update(req->guid(), req->data());
		break;
	case opc_remove:
		res = do_remove(req->guid(), req->data());
		break;
	}

	if (-1 != res)
	{
		req->set_success(true);
	}
	return res;
}

int Dhc::do_check()
{
	/// 枚举proto
	std::list<std::string> filelist;
	std::string path = game::env()->get_game_value("proto_path");
	boost::filesystem::directory_iterator iter(path), end_iter;
	while (iter != end_iter)
	{
		if (!boost::filesystem::is_directory(iter->status()))
		{
			std::string ext = boost::filesystem::extension(*iter);
			if (ext == ".sproto" || ext == ".eproto")
			{
				filelist.push_back((*iter).path().generic_string());
			}
		}
		++iter;
	}

	/// 解析proto
	std::vector<Files> files_vec;
	std::map<std::string, std::vector<std::string> > field_map;
	for (std::list<std::string>::iterator it = filelist.begin(); it != filelist.end(); ++it)
	{
		Files files;
		files.use_auto_increment = false;
		files.use_datetime = false;
		std::string fs = *it;
		FILE *fp = fopen(fs.c_str(), "r");
		if (!fp)
		{
			return -1;
		}
		//add
		std::vector<std::string> field_name;
		char tmpc[100];
		while (fscanf(fp, "%s", &tmpc) != EOF)
		{
			std::string tmps(tmpc);
			if (tmps == "message")
			{
				char c[100];
				fscanf(fp, "%s", &c);
				files.cname = c;
			}
			else if (tmps == "int64" || tmps == "uint64" || tmps == "int32" || tmps == "uint32" || tmps == "bytes" || tmps == "string" || tmps == "dt" || tmps == "float" || tmps == "double")
			{
				Itstr it;
				it.disc = 0;
				it.type = tmps;
				char c[100];
				fscanf(fp, "%s", &c);
				it.name = c;
				//add
				field_name.push_back(it.name);
				files.itstr_vec.push_back(it);
			}
			else if (tmps == "repeated")
			{
				Itstr it;
				it.disc = 1;
				char c[100];
				fscanf(fp, "%s", &c);
				it.type = c;
				fscanf(fp, "%s", &c);
				it.name = c;
				//add
				field_name.push_back(it.name);
				files.itstr_vec.push_back(it);
			}
			else if (tmps == "//using")
			{
				char c[100];
				fscanf(fp, "%s", &c);
				std::string ccc(c);
				if (ccc == "autoinc")
				{
					files.use_auto_increment = true;
				}
				else if (ccc == "datetime")
				{
					files.use_datetime = true;
				}
				else if (ccc == "unique")
				{
					fscanf(fp, "%s", &c);
					std::string in1(c);
					fscanf(fp, "%s", &c);
					std::string in2(c);
					Indexstr ist;
					ist.index_name = in1;
					ist.index_len = boost::lexical_cast<int>(in2);
					files.uniques.push_back(ist);
				}
				else if (ccc == "index")
				{
					fscanf(fp, "%s", &c);
					std::string in1(c);
					fscanf(fp, "%s", &c);
					std::string in2(c);
					Indexstr ist;
					ist.index_name = in1;
					ist.index_len = boost::lexical_cast<int>(in2);
					files.indexes.push_back(ist);
				}
			}
		}

		fclose(fp);

		if (files.use_datetime)
		{
			Itstr it;
			it.disc = 0;
			it.type = "dt";
			it.name = "dt";
			//add
			field_name.push_back(it.name);
			files.itstr_vec.push_back(it);
		}

		files_vec.push_back(files);
		field_map[files.cname] = field_name;
	}

	for (int i = 0; i < files_vec.size(); ++i)
	{
		std::string table_name = files_vec[i].cname;
		std::string select_name = "select * from " + table_name + " limit 1";
		mysqlpp::Query query = conn_->query(select_name);
		mysqlpp::StoreQueryResult res = query.store();
		if (res)
		{
			/// 数据库中多余的列
			for (int k = 0; k < res.num_fields(); ++k)
			{
				std::vector<std::string> & names = field_map[table_name];
				if (std::find(names.begin(), names.end(), res.field_name(k)) == names.end())
				{
					return -1;
				}
			}

			for (int j = 0; j < files_vec[i].itstr_vec.size(); ++j)
			{
				std::string query_name = "select DATA_TYPE from information_schema.COLUMNS where TABLE_SCHEMA='" + game::env()->get_db_value("db") + "' and TABLE_NAME='" + table_name + "' and column_name = '" + files_vec[i].itstr_vec[j].name + "'";
				mysqlpp::Query query2 = conn_->query(query_name);
				mysqlpp::StoreQueryResult res = query2.store();
				if (res && res.num_rows() == 1)
				{
					if (res[0][0] == "mediumblob" && files_vec[i].itstr_vec[j].disc == 1)
					{
						continue;
					}
					else if (res[0][0] == "bigint" && files_vec[i].itstr_vec[j].type == "int64")
					{
						continue;
					}
					else if (res[0][0] == "bigint" && files_vec[i].itstr_vec[j].type == "uint64")
					{
						continue;
					}
					else if (res[0][0] == "int" && files_vec[i].itstr_vec[j].type == "int32")
					{
						continue;
					}
					else if (res[0][0] == "int" && files_vec[i].itstr_vec[j].type == "uint32")
					{
						continue;
					}
					else if (res[0][0] == "mediumblob" && files_vec[i].itstr_vec[j].type == "bytes")
					{
						continue;
					}
					else if (res[0][0] == "text" && files_vec[i].itstr_vec[j].type == "string")
					{
						continue;
					}
					else if (res[0][0] == "timestamp" && files_vec[i].itstr_vec[j].type == "dt")
					{
						continue;
					}
					else if (res[0][0] == "double" && files_vec[i].itstr_vec[j].type == "float")
					{
						continue;
					}
					else if (res[0][0] == "double" && files_vec[i].itstr_vec[j].type == "double")
					{
						continue;
					}
					return -1;
				}
				else
				{
					return -1;
				}
			}
		}
		else
		{
			return -1;
		}
	}
	return 0;
}

int Dhc::do_insert(uint64_t guid, google::protobuf::Message *data)
{
	mysqlpp::Query query = conn_->query();

	if (IS_PLAYER_GUID(guid))
	{
		Sqlplayer_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_ACC_GUID(guid))
	{
		MultiSqlacc_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GTOOL_GUID(guid))
	{
		Sqlgtool_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_ROLE_GUID(guid))
	{
		Sqlrole_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_EQUIP_GUID(guid))
	{
		Sqlequip_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_POST_GUID(guid))
	{
		MultiSqlpost_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_POST_LIST_GUID(guid))
	{
		MultiSqlpost_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_SPORT_GUID(guid))
	{
		Sqlsport_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_SPORT_LIST_GUID(guid))
	{
		Sqlsport_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_RANK_GUID(guid))
	{
		Sqlrank_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_LIST_GUID(guid))
	{
		Sqlguild_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_GUID(guid))
	{
		Sqlguild_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_MEMBER_GUID(guid))
	{
		Sqlguild_member_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_EVENT_GUID(guid))
	{
		Sqlguild_event_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_SOCIAL_GUID(guid))
	{
		Sqlsocial_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_SOCIAL_LIST_GUID(guid))
	{
		MultiSqlsocial_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_RECHARGE_HEITAO_GUID(guid))
	{
		MultiSqlrecharge_heitao_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GLOBAL_GUID(guid))
	{
		Sqlglobal_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_TREASURE_GUID(guid))
	{
		Sqltreasure_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_TREASURE_LIST_GUID(guid))
	{
		Sqltreasure_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_TREASURE_REPORT_GUID(guid))
	{
		Sqltreasure_report_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_LOTTERY_LIST_GUID(guid))
	{
		Sqlpvp_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_HUODONG_GUID(guid))
	{
		Sqlhuodong_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_HUODONG_ENTRY_GUID(guid))
	{
		Sqlhuodong_entry_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_HUODONG_LIST_GUID(guid))
	{
		MultiSqlhuodong_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_MISSION_GUID(guid))
	{
		Sqlguild_mission_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_MESSAGE_GUID(guid))
	{
		Sqlguild_message_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_BOSS_GUID(guid))
	{
		Sqlboss_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_HUODONG_PLAYER_GUID(guid))
	{
		Sqlhuodong_player_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_RED_GUID(guid))
	{
		Sqlguild_red_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_BOX_GUID(guid))
	{
		Sqlguild_box_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_PET_GUID(guid))
	{
		Sqlpet_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_ARRANGE_GUID(guid))
	{
		Sqlguild_arrange_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_ARRANGE_LIST_GUID(guid))
	{
		MultiGuildPvp_list_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_FIGHT_GUID(guid))
	{
		Sqlguild_fight_t s(guid, data);
		return s.insert(query);
	}
	else if (IS_GUILD_FIGHT_LIST_GUID(guid))
	{
		MultiGuildFight_list_t s(guid, data);
		return s.insert(query);
	}
	return -1;
}

int Dhc::do_query(uint64_t guid, google::protobuf::Message *data)
{
	mysqlpp::Query query = conn_->query();

	if (IS_PLAYER_GUID(guid))
	{
		Sqlplayer_t s(guid, data);
		return s.query(query);
	}
	else if (IS_ACC_GUID(guid))
	{
		MultiSqlacc_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GTOOL_GUID(guid))
	{
		Sqlgtool_t s(guid, data);
		return s.query(query);
	}
	else if (IS_ROLE_GUID(guid))
	{
		Sqlrole_t s(guid, data);
		return s.query(query);
	}
	else if (IS_EQUIP_GUID(guid))
	{
		Sqlequip_t s(guid, data);
		return s.query(query);
	}
	else if (IS_POST_GUID(guid))
	{
		Sqlpost_t s(guid, data);
		return s.query(query);
	}
	else if (IS_POST_LIST_GUID(guid))
	{
		MultiSqlpost_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_SPORT_GUID(guid))
	{
		Sqlsport_t s(guid, data);
		return s.query(query);
	}
	else if (IS_SPORT_LIST_GUID(guid))
	{
		Sqlsport_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_RANK_GUID(guid))
	{
		Sqlrank_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_LIST_GUID(guid))
	{
		Sqlguild_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_GUID(guid))
	{
		Sqlguild_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_MEMBER_GUID(guid))
	{
		Sqlguild_member_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_EVENT_GUID(guid))
	{
		Sqlguild_event_t s(guid, data);
		return s.query(query);
	}
	else if (IS_SOCIAL_GUID(guid))
	{
		Sqlsocial_t s(guid, data);
		return s.query(query);
	}
	else if (IS_SOCIAL_LIST_GUID(guid))
	{
		MultiSqlsocial_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_RECHARGE_HEITAO_GUID(guid))
	{
		MultiSqlrecharge_heitao_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GLOBAL_GUID(guid))
	{
		Sqlglobal_t s(guid, data);
		return s.query(query);
	}
	else if (IS_TREASURE_GUID(guid))
	{
		Sqltreasure_t s(guid, data);
		return s.query(query);
	}
	else if (IS_TREASURE_LIST_GUID(guid))
	{
		Sqltreasure_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_TREASURE_REPORT_GUID(guid))
	{
		Sqltreasure_report_t s(guid, data);
		return s.query(query);
	}
	else if (IS_LOTTERY_LIST_GUID(guid))
	{
		Sqlpvp_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_HUODONG_GUID(guid))
	{
		Sqlhuodong_t s(guid, data);
		return s.query(query);
	}
	else if (IS_HUODONG_ENTRY_GUID(guid))
	{
		Sqlhuodong_entry_t s(guid, data);
		return s.query(query);
	}
	else if (IS_HUODONG_LIST_GUID(guid))
	{
		MultiSqlhuodong_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_MISSION_GUID(guid))
	{
		Sqlguild_mission_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_MESSAGE_GUID(guid))
	{
		Sqlguild_message_t s(guid, data);
		return s.query(query);
	}
	else if (IS_BOSS_GUID(guid))
	{
		Sqlboss_t s(guid, data);
		return s.query(query);
	}
	else if (IS_PLAYER_LIST_GUID(guid))
	{
		MultiSqlplayer_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_HUODONG_PLAYER_GUID(guid))
	{
		Sqlhuodong_player_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_RED_GUID(guid))
	{
		Sqlguild_red_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_BOX_GUID(guid))
	{
		Sqlguild_box_t s(guid, data);
		return s.query(query);
	}
	else if (IS_PET_GUID(guid))
	{
		Sqlpet_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_ARRANGE_GUID(guid))
	{
		Sqlguild_arrange_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_ARRANGE_LIST_GUID(guid))
	{
		MultiGuildPvp_list_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_FIGHT_GUID(guid))
	{
		Sqlguild_fight_t s(guid, data);
		return s.query(query);
	}
	else if (IS_GUILD_FIGHT_LIST_GUID(guid))
	{
		MultiGuildFight_list_t s(guid, data);
		return s.query(query);
	}
	return -1;
}

int Dhc::do_update(uint64_t guid, google::protobuf::Message *data)
{
	mysqlpp::Query query = conn_->query();

	if (IS_PLAYER_GUID(guid))
	{
		Sqlplayer_t s(guid, data);
		return s.update(query);
	}
	else if (IS_ACC_GUID(guid))
	{
		MultiSqlacc_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GTOOL_GUID(guid))
	{
		Sqlgtool_t s(guid, data);
		return s.update(query);
	}
	else if (IS_ROLE_GUID(guid))
	{
		Sqlrole_t s(guid, data);
		return s.update(query);
	}
	else if (IS_EQUIP_GUID(guid))
	{
		Sqlequip_t s(guid, data);
		return s.update(query);
	}
	else if (IS_POST_GUID(guid))
	{
		MultiSqlpost_t s(guid, data);
		return s.update(query);
	}
	else if (IS_POST_LIST_GUID(guid))
	{
		MultiSqlpost_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_SPORT_GUID(guid))
	{
		Sqlsport_t s(guid, data);
		return s.update(query);
	}
	else if (IS_SPORT_LIST_GUID(guid))
	{
		Sqlsport_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_RANK_GUID(guid))
	{
		Sqlrank_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_LIST_GUID(guid))
	{
		Sqlguild_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_GUID(guid))
	{
		Sqlguild_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_MEMBER_GUID(guid))
	{
		Sqlguild_member_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_EVENT_GUID(guid))
	{
		Sqlguild_event_t s(guid, data);
		return s.update(query);
	}
	else if (IS_SOCIAL_GUID(guid))
	{
		Sqlsocial_t s(guid, data);
		return s.update(query);
	}
	else if (IS_SOCIAL_LIST_GUID(guid))
	{
		MultiSqlsocial_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_RECHARGE_HEITAO_GUID(guid))
	{
		MultiSqlrecharge_heitao_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GLOBAL_GUID(guid))
	{
		Sqlglobal_t s(guid, data);
		return s.update(query);
	}
	else if (IS_TREASURE_GUID(guid))
	{
		Sqltreasure_t s(guid, data);
		return s.update(query);
	}
	else if (IS_TREASURE_LIST_GUID(guid))
	{
		Sqltreasure_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_TREASURE_REPORT_GUID(guid))
	{
		Sqltreasure_report_t s(guid, data);
		return s.update(query);
	}
	else if (IS_LOTTERY_LIST_GUID(guid))
	{
		Sqlpvp_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_HUODONG_GUID(guid))
	{
		Sqlhuodong_t s(guid, data);
		return s.update(query);
	}
	else if (IS_HUODONG_ENTRY_GUID(guid))
	{
		Sqlhuodong_entry_t s(guid, data);
		return s.update(query);
	}
	else if (IS_HUODONG_LIST_GUID(guid))
	{
		MultiSqlhuodong_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_MISSION_GUID(guid))
	{
		Sqlguild_mission_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_MESSAGE_GUID(guid))
	{
		Sqlguild_message_t s(guid, data);
		return s.update(query);
	}
	else if (IS_BOSS_GUID(guid))
	{
		Sqlboss_t s(guid, data);
		return s.update(query);
	}
	else if (IS_HUODONG_PLAYER_GUID(guid))
	{
		Sqlhuodong_player_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_RED_GUID(guid))
	{
		Sqlguild_red_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_BOX_GUID(guid))
	{
		Sqlguild_box_t s(guid, data);
		return s.update(query);
	}
	else if (IS_PET_GUID(guid))
	{
		Sqlpet_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_ARRANGE_GUID(guid))
	{
		Sqlguild_arrange_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_ARRANGE_LIST_GUID(guid))
	{
		MultiGuildPvp_list_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_FIGHT_GUID(guid))
	{
		Sqlguild_fight_t s(guid, data);
		return s.update(query);
	}
	else if (IS_GUILD_FIGHT_LIST_GUID(guid))
	{
		MultiGuildFight_list_t s(guid, data);
		return s.update(query);
	}
	return -1;
}

int Dhc::do_remove(uint64_t guid, google::protobuf::Message *data)
{
	mysqlpp::Query query = conn_->query();

	if (IS_PLAYER_GUID(guid))
	{
		Sqlplayer_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_ACC_GUID(guid))
	{
		MultiSqlacc_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GTOOL_GUID(guid))
	{
		Sqlgtool_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_ROLE_GUID(guid))
	{
		Sqlrole_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_EQUIP_GUID(guid))
	{
		Sqlequip_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_POST_GUID(guid))
	{
		Sqlpost_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_POST_LIST_GUID(guid))
	{
		MultiSqlpost_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_SPORT_GUID(guid))
	{
		Sqlsport_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_SPORT_LIST_GUID(guid))
	{
		Sqlsport_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_RANK_GUID(guid))
	{
		Sqlrank_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_LIST_GUID(guid))
	{
		Sqlguild_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_GUID(guid))
	{
		Sqlguild_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_MEMBER_GUID(guid))
	{
		Sqlguild_member_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_EVENT_GUID(guid))
	{
		Sqlguild_event_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_SOCIAL_GUID(guid))
	{
		Sqlsocial_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_SOCIAL_LIST_GUID(guid))
	{
		MultiSqlsocial_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_RECHARGE_HEITAO_GUID(guid))
	{
		MultiSqlrecharge_heitao_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GLOBAL_GUID(guid))
	{
		Sqlglobal_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_TREASURE_GUID(guid))
	{
		Sqltreasure_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_TREASURE_LIST_GUID(guid))
	{
		Sqltreasure_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_TREASURE_REPORT_GUID(guid))
	{
		Sqltreasure_report_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_LOTTERY_LIST_GUID(guid))
	{
		Sqlpvp_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_HUODONG_GUID(guid))
	{
		Sqlhuodong_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_HUODONG_ENTRY_GUID(guid))
	{
		Sqlhuodong_entry_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_HUODONG_LIST_GUID(guid))
	{
		MultiSqlhuodong_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_MISSION_GUID(guid))
	{
		Sqlguild_mission_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_MESSAGE_GUID(guid))
	{
		Sqlguild_message_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_BOSS_GUID(guid))
	{
		Sqlboss_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_HUODONG_PLAYER_GUID(guid))
	{
		Sqlhuodong_player_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_RED_GUID(guid))
	{
		Sqlguild_red_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_BOX_GUID(guid))
	{
		Sqlguild_box_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_PET_GUID(guid))
	{
		Sqlpet_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_ARRANGE_GUID(guid))
	{
		Sqlguild_arrange_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_ARRANGE_LIST_GUID(guid))
	{
		MultiGuildPvp_list_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_FIGHT_GUID(guid))
	{
		Sqlguild_fight_t s(guid, data);
		return s.remove(query);
	}
	else if (IS_GUILD_FIGHT_LIST_GUID(guid))
	{
		MultiGuildFight_list_t s(guid, data);
		return s.remove(query);
	}
	return -1;
}
