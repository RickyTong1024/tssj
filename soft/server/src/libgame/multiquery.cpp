#include "multiquery.h"
#include "gameinc.h"

int MultiSqlacc_t::insert(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlacc_t::query(mysqlpp::Query& query)
{
	dhc::acc_t *obj = (dhc::acc_t *)data_;
	uint64_t max = obj->guid();

	{
		query << "SELECT guid, gm_level, fenghao_time, device, version, last_device, last_time FROM acc_t WHERE "
			<< "username=" << mysqlpp::quote << obj->username()
			<< " and "
			<< "serverid=" << mysqlpp::quote << obj->serverid();

		mysqlpp::StoreQueryResult res = query.store();

		if (res && res.num_rows() == 1)
		{
			obj->set_guid(res.at(0).at(0));
			obj->set_gm_level(res.at(0).at(1));
			obj->set_fenghao_time(res.at(0).at(2));
			obj->set_device(std::string(res.at(0).at(3)));
			obj->set_version(res.at(0).at(4));
			obj->set_last_device(std::string(res.at(0).at(5)));
			obj->set_last_time(res.at(0).at(6));
			return 0;
		}
	}

	if (obj->guid() == 0)
	{
		return -1;
	}

	{
		query << "SELECT max(guid) FROM acc_t where "
			<< "serverid=" << mysqlpp::quote << obj->serverid();

		mysqlpp::StoreQueryResult res = query.store();

		if (res && res.num_rows() == 1)
		{
			if (!res.at(0).at(0).is_null())
			{
				max = res.at(0).at(0);
				max += 1;
			}
		}
		else
		{
			return -1;
		}

	}

	{
		query << "INSERT INTO acc_t SET "
			<< "guid=" << boost::lexical_cast<std::string>(max)
			<< ","
			<< "gm_level=0"
			<< ","
			<< "username=" << mysqlpp::quote << obj->username()
			<< ","
			<< "serverid=" << mysqlpp::quote << obj->serverid()
			<< ","
			<< "extra=" << mysqlpp::quote << obj->extra()
			<< ","
			<< "fenghao_time=0"
			<< ","
			<< "device=" << mysqlpp::quote << obj->device()
			<< ","
			<< "version=" << boost::lexical_cast<std::string>(obj->version())
			<< ","
			<< "last_device=" << mysqlpp::quote << obj->device()
			<< ","
			<< "last_time=0"
			<< ","
			<< "dt=now()";

		mysqlpp::SimpleResult res = query.execute();
		if (!res)
		{
			game::log()->error(query.error());
			return -1;
		}
		obj->set_guid(max);
		obj->set_gm_level(0);
	}

	return 0;
}

int MultiSqlacc_t::update(mysqlpp::Query& query) 
{
	dhc::acc_t *obj = (dhc::acc_t *)data_;
	query << "UPDATE acc_t SET ";
	query << "device=" << mysqlpp::quote << obj->device();
	query << ",";
	query << "version=" << boost::lexical_cast<std::string>(obj->version());
	query << ",";
	query << "last_device=" << mysqlpp::quote << obj->last_device();
	query << ",";
	query << "last_time=" << boost::lexical_cast<std::string>(obj->last_time());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(obj->guid());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int MultiSqlacc_t::remove(mysqlpp::Query& query) 
{
	return -1;
}

//////////////////////////////////////////////////////////////////////////

int MultiSqlpost_t::insert(mysqlpp::Query& query)
{
	dhc::post_t *obj = (dhc::post_t *)data_;
	query << "INSERT INTO post_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "receiver_guid=" << boost::lexical_cast<std::string>(obj->receiver_guid());
	query << ",";
	query << "sender_date=" << boost::lexical_cast<std::string>(obj->sender_date());
	query << ",";
	query << "title=" << mysqlpp::quote << obj->title();
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();
	query << ",";
	query << "sender_name=" << mysqlpp::quote << obj->sender_name();
	query << ",";
	{
		uint32_t size = obj->type_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->type(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "type=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value1_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value1(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value1=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value2_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value2(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value2=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value3_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value3(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value3=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int MultiSqlpost_t::query(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlpost_t::update(mysqlpp::Query& query) 
{
	dhc::post_t *obj = (dhc::post_t *)data_;
	query << "UPDATE post_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "receiver_guid=" << boost::lexical_cast<std::string>(obj->receiver_guid());
	query << ",";
	query << "sender_date=" << boost::lexical_cast<std::string>(obj->sender_date());
	query << ",";
	query << "title=" << mysqlpp::quote << obj->title();
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();
	query << ",";
	query << "sender_name=" << mysqlpp::quote << obj->sender_name();
	query << ",";
	{
		uint32_t size = obj->type_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->type(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "type=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value1_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value1(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value1=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value2_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value2(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value2=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value3_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value3(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value3=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE pid=" << boost::lexical_cast<std::string>(obj->pid());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int MultiSqlpost_t::remove(mysqlpp::Query& query) 
{
	return -1;
}

//////////////////////////////////////////////////////////////////////////

int MultiSqlpost_list_t::insert(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlpost_list_t::query(mysqlpp::Query& query)
{
	if (GUID_COUNTER(guid_) == 1)
	{
		return post_load_new_query(query);
	}
	protocol::game::post_list_t *obj_list = (protocol::game::post_list_t *)data_;
	query << "SELECT pid,guid,receiver_guid,sender_date,title,text,sender_name,type,value1,value2,value3 FROM post_t";

	mysqlpp::StoreQueryResult res = query.store();

	if (!res)
	{
		return -1;
	}

	for (int i = 0; i < res.num_rows(); ++i)
	{
		dhc::post_t* obj = obj_list->add_post_record();
		if (!res.at(i).at(0).is_null())
		{
			obj->set_pid(res.at(i).at(0));
		}
		if (!res.at(i).at(1).is_null())
		{
			obj->set_guid(res.at(i).at(1));
		}
		if (!res.at(i).at(2).is_null())
		{
			obj->set_receiver_guid(res.at(i).at(2));
		}
		if (!res.at(i).at(3).is_null())
		{
			obj->set_sender_date(res.at(i).at(3));
		}
		if (!res.at(i).at(4).is_null())
		{
			obj->set_title((std::string)res.at(i).at(4));
		}
		if (!res.at(i).at(5).is_null())
		{
			obj->set_text((std::string)res.at(i).at(5));
		}
		if (!res.at(i).at(6).is_null())
		{
			obj->set_sender_name((std::string)res.at(i).at(6));
		}
		if (!res.at(i).at(7).is_null())
		{
			std::string temp(res.at(i).at(7).data(), res.at(i).at(7).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_type(v);
			}
		}
		if (!res.at(i).at(8).is_null())
		{
			std::string temp(res.at(i).at(8).data(), res.at(i).at(8).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_value1(v);
			}
		}
		if (!res.at(i).at(9).is_null())
		{
			std::string temp(res.at(i).at(9).data(), res.at(i).at(9).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_value2(v);
			}
		}
		if (!res.at(i).at(10).is_null())
		{
			std::string temp(res.at(i).at(10).data(), res.at(i).at(10).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_value3(v);
			}
		}
	}
	return 0;
}

int MultiSqlpost_list_t::post_load_new_query(mysqlpp::Query& query)
{
	protocol::game::post_list_t *obj_list = (protocol::game::post_list_t *)data_;
	query << "SELECT pid,guid,receiver_guid,sender_date,title,text,sender_name,type,value1,value2,value3 FROM post_t where guid = 0";

	mysqlpp::StoreQueryResult res = query.store();

	if (!res)
	{
		return -1;
	}

	for (int i = 0; i < res.num_rows(); ++i)
	{
		dhc::post_t* obj = obj_list->add_post_record();
		if (!res.at(i).at(0).is_null())
		{
			obj->set_pid(res.at(i).at(0));
		}
		if (!res.at(i).at(1).is_null())
		{
			obj->set_guid(res.at(i).at(1));
		}
		if (!res.at(i).at(2).is_null())
		{
			obj->set_receiver_guid(res.at(i).at(2));
		}
		if (!res.at(i).at(3).is_null())
		{
			obj->set_sender_date(res.at(i).at(3));
		}
		if (!res.at(i).at(4).is_null())
		{
			obj->set_title((std::string)res.at(i).at(4));
		}
		if (!res.at(i).at(5).is_null())
		{
			obj->set_text((std::string)res.at(i).at(5));
		}
		if (!res.at(i).at(6).is_null())
		{
			obj->set_sender_name((std::string)res.at(i).at(6));
		}
		if (!res.at(i).at(7).is_null())
		{
			std::string temp(res.at(i).at(7).data(), res.at(i).at(7).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_type(v);
			}
		}
		if (!res.at(i).at(8).is_null())
		{
			std::string temp(res.at(i).at(8).data(), res.at(i).at(8).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_value1(v);
			}
		}
		if (!res.at(i).at(9).is_null())
		{
			std::string temp(res.at(i).at(9).data(), res.at(i).at(9).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_value2(v);
			}
		}
		if (!res.at(i).at(10).is_null())
		{
			std::string temp(res.at(i).at(10).data(), res.at(i).at(10).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_value3(v);
			}
		}
	}
	return 0;
}

int MultiSqlpost_list_t::update(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlpost_list_t::remove(mysqlpp::Query& query)
{
	return -1;
}


//////////////////////////////////////////////////////////////////////////

int MultiSqlsocial_list_t::insert(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlsocial_list_t::query(mysqlpp::Query& query)
{
	protocol::game::social_list_t *obj_list = (protocol::game::social_list_t *)data_;
	query << "SELECT * FROM social_t";

	mysqlpp::StoreQueryResult res = query.store();

	if (!res)
	{
		return -1;
	}

	for (int i = 0; i < res.num_rows(); ++i)
	{
		dhc::social_t* obj = obj_list->add_socials();
		if (!res.at(i).at(0).is_null())
		{
			obj->set_guid(res.at(i).at(0));
		}
		if (!res.at(i).at(1).is_null())
		{
			obj->set_player_guid(res.at(i).at(1));
		}
		if (!res.at(i).at(2).is_null())
		{
			obj->set_target_guid(res.at(i).at(2));
		}
		if (!res.at(i).at(3).is_null())
		{
			obj->set_template_id(res.at(i).at(3));
		}
		if (!res.at(i).at(4).is_null())
		{
			obj->set_name((std::string)res.at(i).at(4));
		}
		if (!res.at(i).at(5).is_null())
		{
			obj->set_level(res.at(i).at(5));
		}
		if (!res.at(i).at(6).is_null())
		{
			obj->set_bf(res.at(i).at(6));
		}
		if (!res.at(i).at(7).is_null())
		{
			obj->set_can_shou(res.at(i).at(7));
		}
		if (!res.at(i).at(8).is_null())
		{
			obj->set_last_song_time(res.at(i).at(8));
		}
		if (!res.at(i).at(9).is_null())
		{
			obj->set_vip(res.at(i).at(9));
		}
		if (!res.at(i).at(10).is_null())
		{
			obj->set_achieve(res.at(i).at(10));
		}
		if (!res.at(i).at(11).is_null())
		{
			obj->set_offline_time(res.at(i).at(11));
		}
		if (!res.at(i).at(12).is_null())
		{
			obj->set_chengchao(res.at(i).at(12));
		}
		if (!res.at(i).at(13).is_null())
		{
			std::string temp(res.at(i).at(13).data(), res.at(i).at(13).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_invite_players(v);
			}
		}
		if (!res.at(i).at(14).is_null())
		{
			std::string temp(res.at(i).at(14).data(), res.at(i).at(14).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_invite_levels(v);
			}
		}
		if (!res.at(i).at(15).is_null())
		{
			std::string temp(res.at(i).at(15).data(), res.at(i).at(15).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_invite_ids(v);
			}
		}
	}
	return 0;
}

int MultiSqlsocial_list_t::update(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlsocial_list_t::remove(mysqlpp::Query& query)
{
	return -1;
}

//////////////////////////////////////////////////////////////////////////

int MultiSqlrecharge_heitao_t::insert(mysqlpp::Query& query)
{
	dhc::recharge_heitao_t *obj = (dhc::recharge_heitao_t *)data_;

	{
		query << "SELECT * FROM recharge_heitao_t WHERE "
			<< "orderno=" << mysqlpp::quote << obj->orderno();

		mysqlpp::StoreQueryResult res = query.store();

		if (res && res.num_rows() >= 1)
		{
			return -1;
		}
	}

	{
		query << "INSERT INTO recharge_heitao_t SET "
			<< "orderno=" << mysqlpp::quote << obj->orderno()
			<< ","
			<< "rid=" << boost::lexical_cast<std::string>(obj->rid())
			<< ","
			<< "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid())
			<< ","
			<< "dt=now()";

		mysqlpp::SimpleResult res = query.execute();
		if (!res)
		{
			game::log()->error(query.error());
			return -1;
		}
	}
	return 0;
}

int MultiSqlrecharge_heitao_t::query(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlrecharge_heitao_t::update(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlrecharge_heitao_t::remove(mysqlpp::Query& query)
{
	return -1;
}


int MultiSqlhuodong_list_t::insert(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlhuodong_list_t::query(mysqlpp::Query& query)
{
	protocol::game::huodong_list_t *obj_list = (protocol::game::huodong_list_t *)data_;
	query << "SELECT guid,id,type,subtype,start,end,name,kai_start,kai_end,group_name,show_time,noshow_time,item_name1,item_des1,item_name2,item_des2,jieri_time,kaikai_start,entrys,player_guids,player_levels,player_players,player_subs,rank_time,extra_data,extra_data1,extra_data2 FROM huodong_t";

	mysqlpp::StoreQueryResult res = query.store();

	if (!res)
	{
		return -1;
	}

	for (int i = 0; i < res.num_rows(); ++i)
	{
		dhc::huodong_t* obj = obj_list->add_huodongs();
		if (!res.at(i).at(0).is_null())
		{
			obj->set_guid(res.at(i).at(0));
		}
		if (!res.at(i).at(1).is_null())
		{
			obj->set_id(res.at(i).at(1));
		}
		if (!res.at(i).at(2).is_null())
		{
			obj->set_type(res.at(i).at(2));
		}
		if (!res.at(i).at(3).is_null())
		{
			obj->set_subtype(res.at(i).at(3));
		}
		if (!res.at(i).at(4).is_null())
		{
			obj->set_start(res.at(i).at(4));
		}
		if (!res.at(i).at(5).is_null())
		{
			obj->set_end(res.at(i).at(5));
		}
		if (!res.at(i).at(6).is_null())
		{
			obj->set_name((std::string)res.at(i).at(6));
		}
		if (!res.at(i).at(7).is_null())
		{
			obj->set_kai_start(res.at(i).at(7));
		}
		if (!res.at(i).at(8).is_null())
		{
			obj->set_kai_end(res.at(i).at(8));
		}
		if (!res.at(i).at(9).is_null())
		{
			obj->set_group_name((std::string)res.at(i).at(9));
		}
		if (!res.at(i).at(10).is_null())
		{
			obj->set_show_time(res.at(i).at(10));
		}
		if (!res.at(i).at(11).is_null())
		{
			obj->set_noshow_time(res.at(i).at(11));
		}
		if (!res.at(i).at(12).is_null())
		{
			obj->set_item_name1((std::string)res.at(i).at(12));
		}
		if (!res.at(i).at(13).is_null())
		{
			obj->set_item_des1((std::string)res.at(i).at(13));
		}
		if (!res.at(i).at(14).is_null())
		{
			obj->set_item_name2((std::string)res.at(i).at(14));
		}
		if (!res.at(i).at(15).is_null())
		{
			obj->set_item_des2((std::string)res.at(i).at(15));
		}
		if (!res.at(i).at(16).is_null())
		{
			obj->set_jieri_time(res.at(i).at(16));
		}
		if (!res.at(i).at(17).is_null())
		{
			obj->set_kaikai_start(res.at(i).at(17));
		}
		if (!res.at(i).at(18).is_null())
		{
			std::string temp(res.at(i).at(18).data(), res.at(i).at(18).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_entrys(v);
			}
		}
		if (!res.at(i).at(19).is_null())
		{
			std::string temp(res.at(i).at(19).data(), res.at(i).at(19).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_player_guids(v);
			}
		}
		if (!res.at(i).at(20).is_null())
		{
			std::string temp(res.at(i).at(20).data(), res.at(i).at(20).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_levels(v);
			}
		}
		if (!res.at(i).at(21).is_null())
		{
			std::string temp(res.at(i).at(21).data(), res.at(i).at(21).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_player_players(v);
			}
		}
		if (!res.at(i).at(22).is_null())
		{
			std::string temp(res.at(i).at(22).data(), res.at(i).at(22).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_player_subs(v);
			}
		}
		if (!res.at(i).at(23).is_null())
		{
			obj->set_rank_time(res.at(i).at(23));
		}
		if (!res.at(i).at(24).is_null())
		{
			obj->set_extra_data(res.at(i).at(24));
		}
		if (!res.at(i).at(25).is_null())
		{
			obj->set_extra_data1(res.at(i).at(25));
		}
		if (!res.at(i).at(26).is_null())
		{
			obj->set_extra_data2(res.at(i).at(26));
		}
	}
	return 0;
}

int MultiSqlhuodong_list_t::update(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlhuodong_list_t::remove(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlplayer_list_t::insert(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlplayer_list_t::query(mysqlpp::Query& query)
{
	protocol::game::player_guids_list_t *obj_list = (protocol::game::player_guids_list_t *)data_;
	query << "SELECT guid FROM player_t";

	mysqlpp::StoreQueryResult res = query.store();

	if (!res)
	{
		return -1;
	}

	for (int i = 0; i < res.num_rows(); ++i)
	{
		if (!res.at(i).at(0).is_null())
		{
			obj_list->add_player_guids(res.at(i).at(0));
		}
	}
	
	return 0;
}

int MultiSqlplayer_list_t::update(mysqlpp::Query& query)
{
	return -1;
}

int MultiSqlplayer_list_t::remove(mysqlpp::Query& query)
{
	return -1;
}


int MultiGuildPvp_list_t::insert(mysqlpp::Query& query)
{
	return -1;
}

int MultiGuildPvp_list_t::query(mysqlpp::Query& query)
{
	protocol::game::tmsg_guild_pvp_load *obj_list = (protocol::game::tmsg_guild_pvp_load *)data_;
	query << "SELECT * FROM guild_arrange_t";

	mysqlpp::StoreQueryResult res = query.store();

	if (!res)
	{
		return -1;
	}

	for (int i = 0; i < res.num_rows(); ++i)
	{
		dhc::guild_arrange_t *obj = obj_list->add_ars();
		if (!res.at(i).at(0).is_null())
		{
			obj->set_guid(res.at(i).at(0));
		}
		if (!res.at(i).at(1).is_null())
		{
			obj->set_guild_server(res.at(i).at(1));
		}
		if (!res.at(i).at(2).is_null())
		{
			obj->set_guild_name((std::string)res.at(i).at(2));
		}
		if (!res.at(i).at(3).is_null())
		{
			obj->set_guild_zhanji(res.at(i).at(3));
		}
		if (!res.at(i).at(4).is_null())
		{
			obj->set_guild_total_zhanji(res.at(i).at(4));
		}
		if (!res.at(i).at(5).is_null())
		{
			obj->set_guild_icon(res.at(i).at(5));
		}
		if (!res.at(i).at(6).is_null())
		{
			obj->set_guild_ai(res.at(i).at(6));
		}
		if (!res.at(i).at(7).is_null())
		{
			obj->set_guild_exp(res.at(i).at(7));
		}
		if (!res.at(i).at(8).is_null())
		{
			obj->set_guild_level(res.at(i).at(8));
		}
		if (!res.at(i).at(9).is_null())
		{
			std::string temp(res.at(i).at(9).data(), res.at(i).at(9).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_player_guids(v);
			}
		}
		if (!res.at(i).at(10).is_null())
		{
			std::string temp(res.at(i).at(10).data(), res.at(i).at(10).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint32_t len = 0;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
				boost::scoped_array<char> buf(new char[len]);
				ssm.read(buf.get(), len);
				obj->add_player_names(buf.get(), len);
			}
		}
		if (!res.at(i).at(11).is_null())
		{
			std::string temp(res.at(i).at(11).data(), res.at(i).at(11).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_template(v);
			}
		}
		if (!res.at(i).at(12).is_null())
		{
			std::string temp(res.at(i).at(12).data(), res.at(i).at(12).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_level(v);
			}
		}
		if (!res.at(i).at(13).is_null())
		{
			std::string temp(res.at(i).at(13).data(), res.at(0).at(13).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_bat_eff(v);
			}
		}
		if (!res.at(i).at(14).is_null())
		{
			std::string temp(res.at(i).at(14).data(), res.at(i).at(14).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_vip(v);
			}
		}
		if (!res.at(i).at(15).is_null())
		{
			std::string temp(res.at(i).at(15).data(), res.at(i).at(15).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_achieve(v);
			}
		}
		if (!res.at(i).at(16).is_null())
		{
			std::string temp(res.at(i).at(16).data(), res.at(i).at(16).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_map_star(v);
			}
		}
		if (!res.at(i).at(17).is_null())
		{
			std::string temp(res.at(i).at(17).data(), res.at(i).at(17).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_mplayer_nalflags(v);
			}
		}
		if (!res.at(i).at(18).is_null())
		{
			std::string temp(res.at(i).at(18).data(), res.at(i).at(18).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_player_zguids(v);
			}
		}
		if (!res.at(i).at(19).is_null())
		{
			std::string temp(res.at(i).at(19).data(), res.at(i).at(19).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_zhanjis(v);
			}
		}
		if (!res.at(i).at(20).is_null())
		{
			std::string temp(res.at(i).at(20).data(), res.at(i).at(20).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_total_zhanjis(v);
			}
		}
		if (!res.at(i).at(21).is_null())
		{
			std::string temp(res.at(i).at(21).data(), res.at(i).at(21).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint32_t len = 0;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
				boost::scoped_array<char> buf(new char[len]);
				ssm.read(buf.get(), len);
				obj->add_player_znames(buf.get(), len);
			}
		}
		if (!res.at(i).at(22).is_null())
		{
			std::string temp(res.at(i).at(22).data(), res.at(i).at(22).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_ztemplate(v);
			}
		}
		if (!res.at(i).at(23).is_null())
		{
			std::string temp(res.at(i).at(23).data(), res.at(i).at(23).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_zlevel(v);
			}
		}
		if (!res.at(i).at(24).is_null())
		{
			std::string temp(res.at(i).at(24).data(), res.at(i).at(24).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_zbat_eff(v);
			}
		}
		if (!res.at(i).at(25).is_null())
		{
			std::string temp(res.at(i).at(25).data(), res.at(i).at(25).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_zvip(v);
			}
		}
		if (!res.at(i).at(26).is_null())
		{
			std::string temp(res.at(i).at(26).data(), res.at(i).at(26).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_zachieve(v);
			}
		}
		if (!res.at(i).at(27).is_null())
		{
			std::string temp(res.at(i).at(27).data(), res.at(i).at(27).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_player_znalflags(v);
			}
		}
		if (!res.at(i).at(28).is_null())
		{
			std::string temp(res.at(i).at(28).data(), res.at(i).at(28).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_guild_fights(v);
			}
		}
	}
	return 0;
}

int MultiGuildPvp_list_t::update(mysqlpp::Query& query)
{
	return -1;
}

int MultiGuildPvp_list_t::remove(mysqlpp::Query& query)
{
	return -1;
}

int MultiGuildFight_list_t::insert(mysqlpp::Query& query)
{
	return -1;
}

int MultiGuildFight_list_t::query(mysqlpp::Query& query)
{
	protocol::game::tmsg_guild_pvp_load_fight *obj_list = (protocol::game::tmsg_guild_pvp_load_fight *)data_;
	query << "SELECT * FROM guild_fight_t";

	mysqlpp::StoreQueryResult res = query.store();

	if (!res)
	{
		return -1;
	}

	for (int i = 0; i < res.num_rows(); ++i)
	{
		dhc::guild_fight_t* obj = obj_list->add_ars();
		if (!res.at(i).at(0).is_null())
		{
			obj->set_guid(res.at(i).at(0));
		}
		if (!res.at(i).at(1).is_null())
		{
			obj->set_guild_server(res.at(i).at(1));
		}
		if (!res.at(i).at(2).is_null())
		{
			obj->set_guild_name(std::string(res.at(i).at(2)));
		}
		if (!res.at(i).at(3).is_null())
		{
			obj->set_guild_guid((res.at(i).at(3)));
		}
		if (!res.at(i).at(4).is_null())
		{
			obj->set_target_guild_guid((res.at(i).at(4)));
		}
		if (!res.at(i).at(5).is_null())
		{
			obj->set_guild_icon((res.at(i).at(5)));
		}
		if (!res.at(i).at(6).is_null())
		{
			obj->set_guild_level((res.at(i).at(6)));
		}
		if (!res.at(i).at(7).is_null())
		{
			std::string temp(res.at(i).at(7).data(), res.at(i).at(7).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint64_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
				obj->add_target_guids(v);
			}
		}
		if (!res.at(i).at(8).is_null())
		{
			std::string temp(res.at(i).at(8).data(), res.at(i).at(8).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			uint32_t len = 0;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
				boost::scoped_array<char> buf(new char[len]);
				ssm.read(buf.get(), len);
				obj->add_target_names(buf.get(), len);
			}
		}
		if (!res.at(i).at(9).is_null())
		{
			std::string temp(res.at(i).at(9).data(), res.at(i).at(9).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_target_templates(v);
			}
		}
		if (!res.at(i).at(10).is_null())
		{
			std::string temp(res.at(i).at(10).data(), res.at(i).at(10).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_target_levels(v);
			}
		}
		if (!res.at(i).at(11).is_null())
		{
			std::string temp(res.at(i).at(11).data(), res.at(i).at(11).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_target_bat_effs(v);
			}
		}
		if (!res.at(i).at(12).is_null())
		{
			std::string temp(res.at(i).at(12).data(), res.at(i).at(12).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_target_vips(v);
			}
		}
		if (!res.at(i).at(13).is_null())
		{
			std::string temp(res.at(i).at(13).data(), res.at(i).at(13).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_target_achieves(v);
			}
		}
		if (!res.at(i).at(14).is_null())
		{
			std::string temp(res.at(i).at(14).data(), res.at(i).at(14).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_target_defense_nums(v);
			}
		}
		if (!res.at(i).at(15).is_null())
		{
			std::string temp(res.at(i).at(15).data(), res.at(i).at(15).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_guard_points(v);
			}
		}
		if (!res.at(i).at(16).is_null())
		{
			std::string temp(res.at(i).at(16).data(), res.at(i).at(16).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_guard_gongpuo(v);
			}
		}
		if (!res.at(i).at(17).is_null())
		{
			std::string temp(res.at(i).at(17).data(), res.at(i).at(17).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_win_nums(v);
			}
		}
		if (!res.at(i).at(18).is_null())
		{
			std::string temp(res.at(i).at(18).data(), res.at(i).at(18).length());
			std::stringstream ssm(temp);
			uint32_t size = 0;
			ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
			int32_t v;
			for (uint32_t i = 0; i < size; i++)
			{
				ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
				obj->add_lose_nums(v);
			}
		}
	}
	return 0;
}

int MultiGuildFight_list_t::update(mysqlpp::Query& query)
{
	return -1;
}

int MultiGuildFight_list_t::remove(mysqlpp::Query& query)
{
	return -1;
}
