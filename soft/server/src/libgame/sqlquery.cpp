#include "sqlquery.h"
#include <sstream>

int Sqlboss_t::insert(mysqlpp::Query& query)
{
	dhc::boss_t *obj = (dhc::boss_t *)data_;
	query << "INSERT INTO boss_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "cur_hp=" << boost::lexical_cast<std::string>(obj->cur_hp());
	query << ",";
	query << "max_hp=" << boost::lexical_cast<std::string>(obj->max_hp());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_damages_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->player_damages(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "player_damages=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_tops_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->player_tops(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "player_tops=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghaos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghaos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghaos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_time=" << boost::lexical_cast<std::string>(obj->last_time());
	query << ",";
	{
		uint32_t size = obj->player_rank_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_rank_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_rank_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_ranks_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_ranks(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_ranks=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "boss_last_num=" << boost::lexical_cast<std::string>(obj->boss_last_num());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlboss_t::query(mysqlpp::Query& query)
{
	dhc::boss_t *obj = (dhc::boss_t *)data_;
	query << "SELECT * FROM boss_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_level(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_cur_hp(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_max_hp(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		std::string temp(res.at(0).at(4).data(), res.at(0).at(4).length());
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
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_templates(v);
		}
	}
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
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
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
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
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_player_damages(v);
		}
	}
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_nums(v);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_player_tops(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_vips(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_achieves(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_chenghaos(v);
		}
	}
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_nalflags(v);
		}
	}
	if (!res.at(0).at(15).is_null())
	{
		obj->set_last_time(res.at(0).at(15));
	}
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_player_rank_guids(v);
		}
	}
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_ranks(v);
		}
	}
	if (!res.at(0).at(18).is_null())
	{
		obj->set_boss_last_num(res.at(0).at(18));
	}
	return 0;
}

int Sqlboss_t::update(mysqlpp::Query& query)
{
	dhc::boss_t *obj = (dhc::boss_t *)data_;
	query << "UPDATE boss_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "cur_hp=" << boost::lexical_cast<std::string>(obj->cur_hp());
	query << ",";
	query << "max_hp=" << boost::lexical_cast<std::string>(obj->max_hp());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_damages_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->player_damages(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "player_damages=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_tops_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->player_tops(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "player_tops=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghaos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghaos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghaos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_time=" << boost::lexical_cast<std::string>(obj->last_time());
	query << ",";
	{
		uint32_t size = obj->player_rank_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_rank_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_rank_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_ranks_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_ranks(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_ranks=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "boss_last_num=" << boost::lexical_cast<std::string>(obj->boss_last_num());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlboss_t::remove(mysqlpp::Query& query)
{
	dhc::boss_t *obj = (dhc::boss_t *)data_;
	query << "DELETE FROM boss_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlequip_t::insert(mysqlpp::Query& query)
{
	dhc::equip_t *obj = (dhc::equip_t *)data_;
	query << "INSERT INTO equip_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "role_guid=" << boost::lexical_cast<std::string>(obj->role_guid());
	query << ",";
	query << "enhance=" << boost::lexical_cast<std::string>(obj->enhance());
	query << ",";
	query << "locked=" << boost::lexical_cast<std::string>(obj->locked());
	query << ",";
	{
		uint32_t size = obj->rand_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rand_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rand_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->rand_values_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rand_values(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rand_values=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->stone_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->stone(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "stone=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "jilian=" << boost::lexical_cast<std::string>(obj->jilian());
	query << ",";
	query << "star=" << boost::lexical_cast<std::string>(obj->star());
	query << ",";
	query << "gaizao_counts=" << boost::lexical_cast<std::string>(obj->gaizao_counts());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlequip_t::query(mysqlpp::Query& query)
{
	dhc::equip_t *obj = (dhc::equip_t *)data_;
	query << "SELECT * FROM equip_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_player_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_template_id(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_role_guid(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_enhance(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_locked(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_rand_ids(v);
		}
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_rand_values(v);
		}
	}
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_stone(v);
		}
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_jilian(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_star(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_gaizao_counts(res.at(0).at(11));
	}
	return 0;
}

int Sqlequip_t::update(mysqlpp::Query& query)
{
	dhc::equip_t *obj = (dhc::equip_t *)data_;
	query << "UPDATE equip_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "role_guid=" << boost::lexical_cast<std::string>(obj->role_guid());
	query << ",";
	query << "enhance=" << boost::lexical_cast<std::string>(obj->enhance());
	query << ",";
	query << "locked=" << boost::lexical_cast<std::string>(obj->locked());
	query << ",";
	{
		uint32_t size = obj->rand_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rand_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rand_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->rand_values_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rand_values(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rand_values=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->stone_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->stone(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "stone=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "jilian=" << boost::lexical_cast<std::string>(obj->jilian());
	query << ",";
	query << "star=" << boost::lexical_cast<std::string>(obj->star());
	query << ",";
	query << "gaizao_counts=" << boost::lexical_cast<std::string>(obj->gaizao_counts());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlequip_t::remove(mysqlpp::Query& query)
{
	dhc::equip_t *obj = (dhc::equip_t *)data_;
	query << "DELETE FROM equip_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlglobal_t::insert(mysqlpp::Query& query)
{
	dhc::global_t *obj = (dhc::global_t *)data_;
	query << "INSERT INTO global_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->huodong_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_start_times_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->huodong_start_times(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "huodong_start_times=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_end_times_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->huodong_end_times(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "huodong_end_times=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_count=" << boost::lexical_cast<std::string>(obj->huodong_count());
	query << ",";
	{
		uint32_t size = obj->pttq_vip_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->pttq_vip_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "pttq_vip_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->pttq_player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->pttq_player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "pttq_player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->kaifu_xg_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->kaifu_xg_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "kaifu_xg_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->kaifu_xg_count_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->kaifu_xg_count(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "kaifu_xg_count=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->zzs_rank_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->zzs_rank_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "zzs_rank_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->zzs_rank_jewel_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->zzs_rank_jewel(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "zzs_rank_jewel=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "ore_rank_time=" << boost::lexical_cast<std::string>(obj->ore_rank_time());
	query << ",";
	query << "guild_refresh_time=" << boost::lexical_cast<std::string>(obj->guild_refresh_time());
	query << ",";
	query << "czjh_count=" << boost::lexical_cast<std::string>(obj->czjh_count());
	query << ",";
	query << "guild_pvp_zhou=" << boost::lexical_cast<std::string>(obj->guild_pvp_zhou());
	query << ",";
	query << "guild_pvp_suc=" << boost::lexical_cast<std::string>(obj->guild_pvp_suc());
	query << ",";
	{
		uint32_t size = obj->guild_pvp_ranks_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_pvp_ranks(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_pvp_ranks=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlglobal_t::query(mysqlpp::Query& query)
{
	dhc::global_t *obj = (dhc::global_t *)data_;
	query << "SELECT * FROM global_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		std::string temp(res.at(0).at(1).data(), res.at(0).at(1).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_ids(v);
		}
	}
	if (!res.at(0).at(2).is_null())
	{
		std::string temp(res.at(0).at(2).data(), res.at(0).at(2).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_huodong_start_times(v);
		}
	}
	if (!res.at(0).at(3).is_null())
	{
		std::string temp(res.at(0).at(3).data(), res.at(0).at(3).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_huodong_end_times(v);
		}
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_huodong_count(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_pttq_vip_id(v);
		}
	}
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_pttq_player_name(buf.get(), len);
		}
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_kaifu_xg_id(v);
		}
	}
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_kaifu_xg_count(v);
		}
	}
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_zzs_rank_names(buf.get(), len);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_zzs_rank_jewel(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_ore_rank_time(res.at(0).at(11));
	}
	if (!res.at(0).at(12).is_null())
	{
		obj->set_guild_refresh_time(res.at(0).at(12));
	}
	if (!res.at(0).at(13).is_null())
	{
		obj->set_czjh_count(res.at(0).at(13));
	}
	if (!res.at(0).at(14).is_null())
	{
		obj->set_guild_pvp_zhou(res.at(0).at(14));
	}
	if (!res.at(0).at(15).is_null())
	{
		obj->set_guild_pvp_suc(res.at(0).at(15));
	}
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_guild_pvp_ranks(v);
		}
	}
	return 0;
}

int Sqlglobal_t::update(mysqlpp::Query& query)
{
	dhc::global_t *obj = (dhc::global_t *)data_;
	query << "UPDATE global_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->huodong_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_start_times_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->huodong_start_times(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "huodong_start_times=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_end_times_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->huodong_end_times(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "huodong_end_times=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_count=" << boost::lexical_cast<std::string>(obj->huodong_count());
	query << ",";
	{
		uint32_t size = obj->pttq_vip_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->pttq_vip_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "pttq_vip_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->pttq_player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->pttq_player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "pttq_player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->kaifu_xg_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->kaifu_xg_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "kaifu_xg_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->kaifu_xg_count_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->kaifu_xg_count(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "kaifu_xg_count=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->zzs_rank_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->zzs_rank_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "zzs_rank_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->zzs_rank_jewel_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->zzs_rank_jewel(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "zzs_rank_jewel=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "ore_rank_time=" << boost::lexical_cast<std::string>(obj->ore_rank_time());
	query << ",";
	query << "guild_refresh_time=" << boost::lexical_cast<std::string>(obj->guild_refresh_time());
	query << ",";
	query << "czjh_count=" << boost::lexical_cast<std::string>(obj->czjh_count());
	query << ",";
	query << "guild_pvp_zhou=" << boost::lexical_cast<std::string>(obj->guild_pvp_zhou());
	query << ",";
	query << "guild_pvp_suc=" << boost::lexical_cast<std::string>(obj->guild_pvp_suc());
	query << ",";
	{
		uint32_t size = obj->guild_pvp_ranks_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_pvp_ranks(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_pvp_ranks=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlglobal_t::remove(mysqlpp::Query& query)
{
	dhc::global_t *obj = (dhc::global_t *)data_;
	query << "DELETE FROM global_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlgtool_t::insert(mysqlpp::Query& query)
{
	dhc::gtool_t *obj = (dhc::gtool_t *)data_;
	query << "INSERT INTO gtool_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "num=" << boost::lexical_cast<std::string>(obj->num());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlgtool_t::query(mysqlpp::Query& query)
{
	dhc::gtool_t *obj = (dhc::gtool_t *)data_;
	query << "SELECT * FROM gtool_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_num(res.at(0).at(1));
	}
	return 0;
}

int Sqlgtool_t::update(mysqlpp::Query& query)
{
	dhc::gtool_t *obj = (dhc::gtool_t *)data_;
	query << "UPDATE gtool_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "num=" << boost::lexical_cast<std::string>(obj->num());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlgtool_t::remove(mysqlpp::Query& query)
{
	dhc::gtool_t *obj = (dhc::gtool_t *)data_;
	query << "DELETE FROM gtool_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_t::insert(mysqlpp::Query& query)
{
	dhc::guild_t *obj = (dhc::guild_t *)data_;
	query << "INSERT INTO guild_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "icon=" << boost::lexical_cast<std::string>(obj->icon());
	query << ",";
	query << "gonggao=" << mysqlpp::quote << obj->gonggao();
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "exp=" << boost::lexical_cast<std::string>(obj->exp());
	query << ",";
	query << "bftj=" << boost::lexical_cast<std::string>(obj->bftj());
	query << ",";
	{
		uint32_t size = obj->member_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->member_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "member_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->event_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->event_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "event_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_boss_time=" << boost::lexical_cast<std::string>(obj->last_boss_time());
	query << ",";
	query << "last_daily_time=" << boost::lexical_cast<std::string>(obj->last_daily_time());
	query << ",";
	query << "last_week_time=" << boost::lexical_cast<std::string>(obj->last_week_time());
	query << ",";
	query << "leader_name=" << mysqlpp::quote << obj->leader_name();
	query << ",";
	query << "honor=" << boost::lexical_cast<std::string>(obj->honor());
	query << ",";
	query << "mission=" << boost::lexical_cast<std::string>(obj->mission());
	query << ",";
	{
		uint32_t size = obj->message_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->message_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "message_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "mobai_num=" << boost::lexical_cast<std::string>(obj->mobai_num());
	query << ",";
	{
		uint32_t size = obj->skill_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->skill_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "skill_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->skill_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->skill_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "skill_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->apply_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "apply_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->apply_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "apply_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_bf_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_bf(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_bf=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_level=" << boost::lexical_cast<std::string>(obj->last_level());
	query << ",";
	{
		uint32_t size = obj->red_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->red_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "red_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->box_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->box_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "box_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "change_name=" << boost::lexical_cast<std::string>(obj->change_name());
	query << ",";
	query << "juntuan_apply=" << boost::lexical_cast<std::string>(obj->juntuan_apply());
	query << ",";
	{
		uint32_t size = obj->pvp_guilds_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->pvp_guilds(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "pvp_guilds=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pvp_guild=" << boost::lexical_cast<std::string>(obj->pvp_guild());
	query << ",";
	{
		uint32_t size = obj->apply_nalfags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_nalfags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_nalfags=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_t::query(mysqlpp::Query& query)
{
	dhc::guild_t *obj = (dhc::guild_t *)data_;
	query << "SELECT * FROM guild_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_name((std::string)res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_icon(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_gonggao((std::string)res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_level(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_exp(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_bftj(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_member_guids(v);
		}
	}
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_event_guids(v);
		}
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_last_boss_time(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_last_daily_time(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_last_week_time(res.at(0).at(11));
	}
	if (!res.at(0).at(12).is_null())
	{
		obj->set_leader_name((std::string)res.at(0).at(12));
	}
	if (!res.at(0).at(13).is_null())
	{
		obj->set_honor(res.at(0).at(13));
	}
	if (!res.at(0).at(14).is_null())
	{
		obj->set_mission(res.at(0).at(14));
	}
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_message_guids(v);
		}
	}
	if (!res.at(0).at(16).is_null())
	{
		obj->set_mobai_num(res.at(0).at(16));
	}
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_skill_ids(v);
		}
	}
	if (!res.at(0).at(18).is_null())
	{
		std::string temp(res.at(0).at(18).data(), res.at(0).at(18).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_skill_levels(v);
		}
	}
	if (!res.at(0).at(19).is_null())
	{
		std::string temp(res.at(0).at(19).data(), res.at(0).at(19).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_apply_guids(v);
		}
	}
	if (!res.at(0).at(20).is_null())
	{
		std::string temp(res.at(0).at(20).data(), res.at(0).at(20).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_apply_names(buf.get(), len);
		}
	}
	if (!res.at(0).at(21).is_null())
	{
		std::string temp(res.at(0).at(21).data(), res.at(0).at(21).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_apply_template(v);
		}
	}
	if (!res.at(0).at(22).is_null())
	{
		std::string temp(res.at(0).at(22).data(), res.at(0).at(22).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_apply_level(v);
		}
	}
	if (!res.at(0).at(23).is_null())
	{
		std::string temp(res.at(0).at(23).data(), res.at(0).at(23).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_apply_bf(v);
		}
	}
	if (!res.at(0).at(24).is_null())
	{
		std::string temp(res.at(0).at(24).data(), res.at(0).at(24).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_apply_vip(v);
		}
	}
	if (!res.at(0).at(25).is_null())
	{
		std::string temp(res.at(0).at(25).data(), res.at(0).at(25).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_apply_achieve(v);
		}
	}
	if (!res.at(0).at(26).is_null())
	{
		std::string temp(res.at(0).at(26).data(), res.at(0).at(26).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop_ids(v);
		}
	}
	if (!res.at(0).at(27).is_null())
	{
		std::string temp(res.at(0).at(27).data(), res.at(0).at(27).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop_nums(v);
		}
	}
	if (!res.at(0).at(28).is_null())
	{
		obj->set_last_level(res.at(0).at(28));
	}
	if (!res.at(0).at(29).is_null())
	{
		std::string temp(res.at(0).at(29).data(), res.at(0).at(29).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_red_guids(v);
		}
	}
	if (!res.at(0).at(30).is_null())
	{
		std::string temp(res.at(0).at(30).data(), res.at(0).at(30).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_box_guids(v);
		}
	}
	if (!res.at(0).at(31).is_null())
	{
		obj->set_change_name(res.at(0).at(31));
	}
	if (!res.at(0).at(32).is_null())
	{
		obj->set_juntuan_apply(res.at(0).at(32));
	}
	if (!res.at(0).at(33).is_null())
	{
		std::string temp(res.at(0).at(33).data(), res.at(0).at(33).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_pvp_guilds(v);
		}
	}
	if (!res.at(0).at(34).is_null())
	{
		obj->set_pvp_guild(res.at(0).at(34));
	}
	if (!res.at(0).at(35).is_null())
	{
		std::string temp(res.at(0).at(35).data(), res.at(0).at(35).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_apply_nalfags(v);
		}
	}
	return 0;
}

int Sqlguild_t::update(mysqlpp::Query& query)
{
	dhc::guild_t *obj = (dhc::guild_t *)data_;
	query << "UPDATE guild_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "icon=" << boost::lexical_cast<std::string>(obj->icon());
	query << ",";
	query << "gonggao=" << mysqlpp::quote << obj->gonggao();
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "exp=" << boost::lexical_cast<std::string>(obj->exp());
	query << ",";
	query << "bftj=" << boost::lexical_cast<std::string>(obj->bftj());
	query << ",";
	{
		uint32_t size = obj->member_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->member_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "member_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->event_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->event_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "event_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_boss_time=" << boost::lexical_cast<std::string>(obj->last_boss_time());
	query << ",";
	query << "last_daily_time=" << boost::lexical_cast<std::string>(obj->last_daily_time());
	query << ",";
	query << "last_week_time=" << boost::lexical_cast<std::string>(obj->last_week_time());
	query << ",";
	query << "leader_name=" << mysqlpp::quote << obj->leader_name();
	query << ",";
	query << "honor=" << boost::lexical_cast<std::string>(obj->honor());
	query << ",";
	query << "mission=" << boost::lexical_cast<std::string>(obj->mission());
	query << ",";
	{
		uint32_t size = obj->message_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->message_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "message_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "mobai_num=" << boost::lexical_cast<std::string>(obj->mobai_num());
	query << ",";
	{
		uint32_t size = obj->skill_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->skill_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "skill_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->skill_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->skill_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "skill_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->apply_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "apply_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->apply_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "apply_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_bf_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_bf(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_bf=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->apply_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_level=" << boost::lexical_cast<std::string>(obj->last_level());
	query << ",";
	{
		uint32_t size = obj->red_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->red_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "red_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->box_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->box_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "box_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "change_name=" << boost::lexical_cast<std::string>(obj->change_name());
	query << ",";
	query << "juntuan_apply=" << boost::lexical_cast<std::string>(obj->juntuan_apply());
	query << ",";
	{
		uint32_t size = obj->pvp_guilds_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->pvp_guilds(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "pvp_guilds=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pvp_guild=" << boost::lexical_cast<std::string>(obj->pvp_guild());
	query << ",";
	{
		uint32_t size = obj->apply_nalfags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->apply_nalfags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "apply_nalfags=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_t::remove(mysqlpp::Query& query)
{
	dhc::guild_t *obj = (dhc::guild_t *)data_;
	query << "DELETE FROM guild_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_arrange_t::insert(mysqlpp::Query& query)
{
	dhc::guild_arrange_t *obj = (dhc::guild_arrange_t *)data_;
	query << "INSERT INTO guild_arrange_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_server=" << boost::lexical_cast<std::string>(obj->guild_server());
	query << ",";
	query << "guild_name=" << mysqlpp::quote << obj->guild_name();
	query << ",";
	query << "guild_zhanji=" << boost::lexical_cast<std::string>(obj->guild_zhanji());
	query << ",";
	query << "guild_total_zhanji=" << boost::lexical_cast<std::string>(obj->guild_total_zhanji());
	query << ",";
	query << "guild_icon=" << boost::lexical_cast<std::string>(obj->guild_icon());
	query << ",";
	query << "guild_ai=" << boost::lexical_cast<std::string>(obj->guild_ai());
	query << ",";
	query << "guild_exp=" << boost::lexical_cast<std::string>(obj->guild_exp());
	query << ",";
	query << "guild_level=" << boost::lexical_cast<std::string>(obj->guild_level());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bat_eff_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bat_eff(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bat_eff=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_map_star_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_map_star(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_map_star=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mplayer_nalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mplayer_nalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mplayer_nalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zguids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_zguids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_zguids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zhanjis_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zhanjis(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zhanjis=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_total_zhanjis_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_total_zhanjis(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_total_zhanjis=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_znames_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_znames(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_znames=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_ztemplate_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_ztemplate(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_ztemplate=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zlevel_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zlevel(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zlevel=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zbat_eff_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zbat_eff(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zbat_eff=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zvip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zvip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zvip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zachieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zachieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zachieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_znalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_znalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_znalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_fights_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_fights(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_fights=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_arrange_t::query(mysqlpp::Query& query)
{
	dhc::guild_arrange_t *obj = (dhc::guild_arrange_t *)data_;
	query << "SELECT * FROM guild_arrange_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_server(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_guild_name((std::string)res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_guild_zhanji(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_guild_total_zhanji(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_guild_icon(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_guild_ai(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_guild_exp(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_guild_level(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
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
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
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
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
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
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
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
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
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
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
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
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
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
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
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
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
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
	if (!res.at(0).at(18).is_null())
	{
		std::string temp(res.at(0).at(18).data(), res.at(0).at(18).length());
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
	if (!res.at(0).at(19).is_null())
	{
		std::string temp(res.at(0).at(19).data(), res.at(0).at(19).length());
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
	if (!res.at(0).at(20).is_null())
	{
		std::string temp(res.at(0).at(20).data(), res.at(0).at(20).length());
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
	if (!res.at(0).at(21).is_null())
	{
		std::string temp(res.at(0).at(21).data(), res.at(0).at(21).length());
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
	if (!res.at(0).at(22).is_null())
	{
		std::string temp(res.at(0).at(22).data(), res.at(0).at(22).length());
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
	if (!res.at(0).at(23).is_null())
	{
		std::string temp(res.at(0).at(23).data(), res.at(0).at(23).length());
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
	if (!res.at(0).at(24).is_null())
	{
		std::string temp(res.at(0).at(24).data(), res.at(0).at(24).length());
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
	if (!res.at(0).at(25).is_null())
	{
		std::string temp(res.at(0).at(25).data(), res.at(0).at(25).length());
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
	if (!res.at(0).at(26).is_null())
	{
		std::string temp(res.at(0).at(26).data(), res.at(0).at(26).length());
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
	if (!res.at(0).at(27).is_null())
	{
		std::string temp(res.at(0).at(27).data(), res.at(0).at(27).length());
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
	if (!res.at(0).at(28).is_null())
	{
		std::string temp(res.at(0).at(28).data(), res.at(0).at(28).length());
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
	return 0;
}

int Sqlguild_arrange_t::update(mysqlpp::Query& query)
{
	dhc::guild_arrange_t *obj = (dhc::guild_arrange_t *)data_;
	query << "UPDATE guild_arrange_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_server=" << boost::lexical_cast<std::string>(obj->guild_server());
	query << ",";
	query << "guild_name=" << mysqlpp::quote << obj->guild_name();
	query << ",";
	query << "guild_zhanji=" << boost::lexical_cast<std::string>(obj->guild_zhanji());
	query << ",";
	query << "guild_total_zhanji=" << boost::lexical_cast<std::string>(obj->guild_total_zhanji());
	query << ",";
	query << "guild_icon=" << boost::lexical_cast<std::string>(obj->guild_icon());
	query << ",";
	query << "guild_ai=" << boost::lexical_cast<std::string>(obj->guild_ai());
	query << ",";
	query << "guild_exp=" << boost::lexical_cast<std::string>(obj->guild_exp());
	query << ",";
	query << "guild_level=" << boost::lexical_cast<std::string>(obj->guild_level());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bat_eff_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bat_eff(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bat_eff=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_map_star_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_map_star(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_map_star=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mplayer_nalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mplayer_nalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mplayer_nalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zguids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_zguids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_zguids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zhanjis_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zhanjis(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zhanjis=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_total_zhanjis_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_total_zhanjis(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_total_zhanjis=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_znames_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_znames(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_znames=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_ztemplate_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_ztemplate(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_ztemplate=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zlevel_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zlevel(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zlevel=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zbat_eff_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zbat_eff(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zbat_eff=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zvip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zvip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zvip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_zachieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_zachieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_zachieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_znalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_znalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_znalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_fights_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_fights(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_fights=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_arrange_t::remove(mysqlpp::Query& query)
{
	dhc::guild_arrange_t *obj = (dhc::guild_arrange_t *)data_;
	query << "DELETE FROM guild_arrange_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_box_t::insert(mysqlpp::Query& query)
{
	dhc::guild_box_t *obj = (dhc::guild_box_t *)data_;
	query << "INSERT INTO guild_box_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "mceng=" << boost::lexical_cast<std::string>(obj->mceng());
	query << ",";
	query << "mindex=" << boost::lexical_cast<std::string>(obj->mindex());
	query << ",";
	{
		uint32_t size = obj->reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->reward_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->reward_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "reward_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->reward_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->reward_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "reward_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->reward_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->reward_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "reward_achieves=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_box_t::query(mysqlpp::Query& query)
{
	dhc::guild_box_t *obj = (dhc::guild_box_t *)data_;
	query << "SELECT * FROM guild_box_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_mceng(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_mindex(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		std::string temp(res.at(0).at(4).data(), res.at(0).at(4).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_reward_ids(v);
		}
	}
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_reward_guids(v);
		}
	}
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_reward_names(buf.get(), len);
		}
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_reward_achieves(v);
		}
	}
	return 0;
}

int Sqlguild_box_t::update(mysqlpp::Query& query)
{
	dhc::guild_box_t *obj = (dhc::guild_box_t *)data_;
	query << "UPDATE guild_box_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "mceng=" << boost::lexical_cast<std::string>(obj->mceng());
	query << ",";
	query << "mindex=" << boost::lexical_cast<std::string>(obj->mindex());
	query << ",";
	{
		uint32_t size = obj->reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->reward_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->reward_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "reward_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->reward_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->reward_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "reward_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->reward_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->reward_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "reward_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_box_t::remove(mysqlpp::Query& query)
{
	dhc::guild_box_t *obj = (dhc::guild_box_t *)data_;
	query << "DELETE FROM guild_box_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_event_t::insert(mysqlpp::Query& query)
{
	dhc::guild_event_t *obj = (dhc::guild_event_t *)data_;
	query << "INSERT INTO guild_event_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "create_name=" << mysqlpp::quote << obj->create_name();
	query << ",";
	query << "player_name=" << mysqlpp::quote << obj->player_name();
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "value=" << boost::lexical_cast<std::string>(obj->value());
	query << ",";
	query << "value1=" << boost::lexical_cast<std::string>(obj->value1());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_event_t::query(mysqlpp::Query& query)
{
	dhc::guild_event_t *obj = (dhc::guild_event_t *)data_;
	query << "SELECT * FROM guild_event_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_create_name((std::string)res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_player_name((std::string)res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_type(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_value(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_value1(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_time(res.at(0).at(7));
	}
	return 0;
}

int Sqlguild_event_t::update(mysqlpp::Query& query)
{
	dhc::guild_event_t *obj = (dhc::guild_event_t *)data_;
	query << "UPDATE guild_event_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "create_name=" << mysqlpp::quote << obj->create_name();
	query << ",";
	query << "player_name=" << mysqlpp::quote << obj->player_name();
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "value=" << boost::lexical_cast<std::string>(obj->value());
	query << ",";
	query << "value1=" << boost::lexical_cast<std::string>(obj->value1());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_event_t::remove(mysqlpp::Query& query)
{
	dhc::guild_event_t *obj = (dhc::guild_event_t *)data_;
	query << "DELETE FROM guild_event_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_fight_t::insert(mysqlpp::Query& query)
{
	dhc::guild_fight_t *obj = (dhc::guild_fight_t *)data_;
	query << "INSERT INTO guild_fight_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_server=" << boost::lexical_cast<std::string>(obj->guild_server());
	query << ",";
	query << "guild_name=" << mysqlpp::quote << obj->guild_name();
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "target_guild_guid=" << boost::lexical_cast<std::string>(obj->target_guild_guid());
	query << ",";
	query << "guild_icon=" << boost::lexical_cast<std::string>(obj->guild_icon());
	query << ",";
	query << "guild_level=" << boost::lexical_cast<std::string>(obj->guild_level());
	query << ",";
	{
		uint32_t size = obj->target_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->target_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "target_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->target_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "target_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_bat_effs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_bat_effs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_bat_effs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_defense_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_defense_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_defense_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guard_points_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guard_points(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guard_points=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guard_gongpuo_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guard_gongpuo(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guard_gongpuo=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->win_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->win_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "win_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->lose_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->lose_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "lose_nums=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_fight_t::query(mysqlpp::Query& query)
{
	dhc::guild_fight_t *obj = (dhc::guild_fight_t *)data_;
	query << "SELECT * FROM guild_fight_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_server(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_guild_name((std::string)res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_guild_guid(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_target_guild_guid(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_guild_icon(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_guild_level(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
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
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
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
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
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
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
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
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
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
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
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
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
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
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
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
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
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
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
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
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
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
	if (!res.at(0).at(18).is_null())
	{
		std::string temp(res.at(0).at(18).data(), res.at(0).at(18).length());
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
	return 0;
}

int Sqlguild_fight_t::update(mysqlpp::Query& query)
{
	dhc::guild_fight_t *obj = (dhc::guild_fight_t *)data_;
	query << "UPDATE guild_fight_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_server=" << boost::lexical_cast<std::string>(obj->guild_server());
	query << ",";
	query << "guild_name=" << mysqlpp::quote << obj->guild_name();
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "target_guild_guid=" << boost::lexical_cast<std::string>(obj->target_guild_guid());
	query << ",";
	query << "guild_icon=" << boost::lexical_cast<std::string>(obj->guild_icon());
	query << ",";
	query << "guild_level=" << boost::lexical_cast<std::string>(obj->guild_level());
	query << ",";
	{
		uint32_t size = obj->target_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->target_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "target_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->target_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "target_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_bat_effs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_bat_effs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_bat_effs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->target_defense_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->target_defense_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "target_defense_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guard_points_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guard_points(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guard_points=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guard_gongpuo_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guard_gongpuo(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guard_gongpuo=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->win_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->win_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "win_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->lose_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->lose_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "lose_nums=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_fight_t::remove(mysqlpp::Query& query)
{
	dhc::guild_fight_t *obj = (dhc::guild_fight_t *)data_;
	query << "DELETE FROM guild_fight_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_list_t::insert(mysqlpp::Query& query)
{
	dhc::guild_list_t *obj = (dhc::guild_list_t *)data_;
	query << "INSERT INTO guild_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->guild_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_guids=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_list_t::query(mysqlpp::Query& query)
{
	dhc::guild_list_t *obj = (dhc::guild_list_t *)data_;
	query << "SELECT * FROM guild_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		std::string temp(res.at(0).at(1).data(), res.at(0).at(1).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_guild_guids(v);
		}
	}
	return 0;
}

int Sqlguild_list_t::update(mysqlpp::Query& query)
{
	dhc::guild_list_t *obj = (dhc::guild_list_t *)data_;
	query << "UPDATE guild_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->guild_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_guids=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_list_t::remove(mysqlpp::Query& query)
{
	dhc::guild_list_t *obj = (dhc::guild_list_t *)data_;
	query << "DELETE FROM guild_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_member_t::insert(mysqlpp::Query& query)
{
	dhc::guild_member_t *obj = (dhc::guild_member_t *)data_;
	query << "INSERT INTO guild_member_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "player_iocn_id=" << boost::lexical_cast<std::string>(obj->player_iocn_id());
	query << ",";
	query << "player_name=" << mysqlpp::quote << obj->player_name();
	query << ",";
	query << "player_level=" << boost::lexical_cast<std::string>(obj->player_level());
	query << ",";
	query << "bf=" << boost::lexical_cast<std::string>(obj->bf());
	query << ",";
	query << "zhiwu=" << boost::lexical_cast<std::string>(obj->zhiwu());
	query << ",";
	query << "sign_flag=" << boost::lexical_cast<std::string>(obj->sign_flag());
	query << ",";
	query << "last_sign_time=" << boost::lexical_cast<std::string>(obj->last_sign_time());
	query << ",";
	query << "join_time=" << boost::lexical_cast<std::string>(obj->join_time());
	query << ",";
	query << "contribution=" << boost::lexical_cast<std::string>(obj->contribution());
	query << ",";
	{
		uint32_t size = obj->honors_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->honors(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "honors=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "mnum=" << boost::lexical_cast<std::string>(obj->mnum());
	query << ",";
	query << "mbnum=" << boost::lexical_cast<std::string>(obj->mbnum());
	query << ",";
	query << "last_mbtime=" << boost::lexical_cast<std::string>(obj->last_mbtime());
	query << ",";
	{
		uint32_t size = obj->mission_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "msg_count=" << boost::lexical_cast<std::string>(obj->msg_count());
	query << ",";
	query << "player_vip=" << boost::lexical_cast<std::string>(obj->player_vip());
	query << ",";
	query << "player_achieve=" << boost::lexical_cast<std::string>(obj->player_achieve());
	query << ",";
	query << "offline_time=" << boost::lexical_cast<std::string>(obj->offline_time());
	query << ",";
	{
		uint32_t size = obj->gongpo_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->gongpo_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "gongpo_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "map_star=" << boost::lexical_cast<std::string>(obj->map_star());
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_member_t::query(mysqlpp::Query& query)
{
	dhc::guild_member_t *obj = (dhc::guild_member_t *)data_;
	query << "SELECT * FROM guild_member_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_player_guid(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_player_iocn_id(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_player_name((std::string)res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_player_level(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_bf(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_zhiwu(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_sign_flag(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_last_sign_time(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_join_time(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_contribution(res.at(0).at(11));
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_honors(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		obj->set_mnum(res.at(0).at(13));
	}
	if (!res.at(0).at(14).is_null())
	{
		obj->set_mbnum(res.at(0).at(14));
	}
	if (!res.at(0).at(15).is_null())
	{
		obj->set_last_mbtime(res.at(0).at(15));
	}
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_rewards(v);
		}
	}
	if (!res.at(0).at(17).is_null())
	{
		obj->set_msg_count(res.at(0).at(17));
	}
	if (!res.at(0).at(18).is_null())
	{
		obj->set_player_vip(res.at(0).at(18));
	}
	if (!res.at(0).at(19).is_null())
	{
		obj->set_player_achieve(res.at(0).at(19));
	}
	if (!res.at(0).at(20).is_null())
	{
		obj->set_offline_time(res.at(0).at(20));
	}
	if (!res.at(0).at(21).is_null())
	{
		std::string temp(res.at(0).at(21).data(), res.at(0).at(21).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_gongpo_rewards(v);
		}
	}
	if (!res.at(0).at(22).is_null())
	{
		obj->set_map_star(res.at(0).at(22));
	}
	if (!res.at(0).at(23).is_null())
	{
		obj->set_nalflag(res.at(0).at(23));
	}
	return 0;
}

int Sqlguild_member_t::update(mysqlpp::Query& query)
{
	dhc::guild_member_t *obj = (dhc::guild_member_t *)data_;
	query << "UPDATE guild_member_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "player_iocn_id=" << boost::lexical_cast<std::string>(obj->player_iocn_id());
	query << ",";
	query << "player_name=" << mysqlpp::quote << obj->player_name();
	query << ",";
	query << "player_level=" << boost::lexical_cast<std::string>(obj->player_level());
	query << ",";
	query << "bf=" << boost::lexical_cast<std::string>(obj->bf());
	query << ",";
	query << "zhiwu=" << boost::lexical_cast<std::string>(obj->zhiwu());
	query << ",";
	query << "sign_flag=" << boost::lexical_cast<std::string>(obj->sign_flag());
	query << ",";
	query << "last_sign_time=" << boost::lexical_cast<std::string>(obj->last_sign_time());
	query << ",";
	query << "join_time=" << boost::lexical_cast<std::string>(obj->join_time());
	query << ",";
	query << "contribution=" << boost::lexical_cast<std::string>(obj->contribution());
	query << ",";
	{
		uint32_t size = obj->honors_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->honors(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "honors=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "mnum=" << boost::lexical_cast<std::string>(obj->mnum());
	query << ",";
	query << "mbnum=" << boost::lexical_cast<std::string>(obj->mbnum());
	query << ",";
	query << "last_mbtime=" << boost::lexical_cast<std::string>(obj->last_mbtime());
	query << ",";
	{
		uint32_t size = obj->mission_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "msg_count=" << boost::lexical_cast<std::string>(obj->msg_count());
	query << ",";
	query << "player_vip=" << boost::lexical_cast<std::string>(obj->player_vip());
	query << ",";
	query << "player_achieve=" << boost::lexical_cast<std::string>(obj->player_achieve());
	query << ",";
	query << "offline_time=" << boost::lexical_cast<std::string>(obj->offline_time());
	query << ",";
	{
		uint32_t size = obj->gongpo_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->gongpo_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "gongpo_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "map_star=" << boost::lexical_cast<std::string>(obj->map_star());
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_member_t::remove(mysqlpp::Query& query)
{
	dhc::guild_member_t *obj = (dhc::guild_member_t *)data_;
	query << "DELETE FROM guild_member_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_message_t::insert(mysqlpp::Query& query)
{
	dhc::guild_message_t *obj = (dhc::guild_message_t *)data_;
	query << "INSERT INTO guild_message_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "zhiwu=" << boost::lexical_cast<std::string>(obj->zhiwu());
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "otype=" << boost::lexical_cast<std::string>(obj->otype());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_message_t::query(mysqlpp::Query& query)
{
	dhc::guild_message_t *obj = (dhc::guild_message_t *)data_;
	query << "SELECT * FROM guild_message_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_name((std::string)res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_zhiwu(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_text((std::string)res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_time(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_otype(res.at(0).at(6));
	}
	return 0;
}

int Sqlguild_message_t::update(mysqlpp::Query& query)
{
	dhc::guild_message_t *obj = (dhc::guild_message_t *)data_;
	query << "UPDATE guild_message_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "zhiwu=" << boost::lexical_cast<std::string>(obj->zhiwu());
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "otype=" << boost::lexical_cast<std::string>(obj->otype());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_message_t::remove(mysqlpp::Query& query)
{
	dhc::guild_message_t *obj = (dhc::guild_message_t *)data_;
	query << "DELETE FROM guild_message_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_mission_t::insert(mysqlpp::Query& query)
{
	dhc::guild_mission_t *obj = (dhc::guild_mission_t *)data_;
	query << "INSERT INTO guild_mission_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "guild_ceng=" << boost::lexical_cast<std::string>(obj->guild_ceng());
	query << ",";
	query << "guild_max_ceng=" << boost::lexical_cast<std::string>(obj->guild_max_ceng());
	query << ",";
	query << "guild_last_ceng=" << boost::lexical_cast<std::string>(obj->guild_last_ceng());
	query << ",";
	{
		uint32_t size = obj->guild_monsters_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_monsters(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_monsters=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_max_hps_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->guild_max_hps(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "guild_max_hps=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_cur_hps_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->guild_cur_hps(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "guild_cur_hps=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_damages_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->player_damages(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "player_damages=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_counts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_counts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_counts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->mission_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "mission_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghaos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghaos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghaos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "nalflag=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_mission_t::query(mysqlpp::Query& query)
{
	dhc::guild_mission_t *obj = (dhc::guild_mission_t *)data_;
	query << "SELECT * FROM guild_mission_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_guild_ceng(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_guild_max_ceng(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_guild_last_ceng(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_monsters(v);
		}
	}
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_guild_max_hps(v);
		}
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_guild_cur_hps(v);
		}
	}
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
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
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_templates(v);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
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
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_player_damages(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_counts(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_vips(v);
		}
	}
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_achieves(v);
		}
	}
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_rewards(v);
		}
	}
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_mission_names(buf.get(), len);
		}
	}
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_chenghaos(v);
		}
	}
	if (!res.at(0).at(18).is_null())
	{
		std::string temp(res.at(0).at(18).data(), res.at(0).at(18).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_nalflag(v);
		}
	}
	return 0;
}

int Sqlguild_mission_t::update(mysqlpp::Query& query)
{
	dhc::guild_mission_t *obj = (dhc::guild_mission_t *)data_;
	query << "UPDATE guild_mission_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "guild_ceng=" << boost::lexical_cast<std::string>(obj->guild_ceng());
	query << ",";
	query << "guild_max_ceng=" << boost::lexical_cast<std::string>(obj->guild_max_ceng());
	query << ",";
	query << "guild_last_ceng=" << boost::lexical_cast<std::string>(obj->guild_last_ceng());
	query << ",";
	{
		uint32_t size = obj->guild_monsters_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_monsters(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_monsters=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_max_hps_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->guild_max_hps(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "guild_max_hps=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_cur_hps_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->guild_cur_hps(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "guild_cur_hps=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_damages_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->player_damages(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "player_damages=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_counts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_counts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_counts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->mission_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "mission_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghaos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghaos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghaos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "nalflag=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_mission_t::remove(mysqlpp::Query& query)
{
	dhc::guild_mission_t *obj = (dhc::guild_mission_t *)data_;
	query << "DELETE FROM guild_mission_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlguild_red_t::insert(mysqlpp::Query& query)
{
	dhc::guild_red_t *obj = (dhc::guild_red_t *)data_;
	query << "INSERT INTO guild_red_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "create_name=" << mysqlpp::quote << obj->create_name();
	query << ",";
	query << "create_id=" << boost::lexical_cast<std::string>(obj->create_id());
	query << ",";
	query << "create_vip=" << boost::lexical_cast<std::string>(obj->create_vip());
	query << ",";
	query << "create_achieve=" << boost::lexical_cast<std::string>(obj->create_achieve());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();
	query << ",";
	query << "remain=" << boost::lexical_cast<std::string>(obj->remain());
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_jewel_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_jewel(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_jewel=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflag=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_red_t::query(mysqlpp::Query& query)
{
	dhc::guild_red_t *obj = (dhc::guild_red_t *)data_;
	query << "SELECT * FROM guild_red_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guild_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_create_name((std::string)res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_create_id(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_create_vip(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_create_achieve(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_template_id(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_time(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_text((std::string)res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_remain(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_nalflag(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_player_guid(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_ids(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
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
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
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
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
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
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_jewel(v);
		}
	}
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_nalflag(v);
		}
	}
	return 0;
}

int Sqlguild_red_t::update(mysqlpp::Query& query)
{
	dhc::guild_red_t *obj = (dhc::guild_red_t *)data_;
	query << "UPDATE guild_red_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "guild_guid=" << boost::lexical_cast<std::string>(obj->guild_guid());
	query << ",";
	query << "create_name=" << mysqlpp::quote << obj->create_name();
	query << ",";
	query << "create_id=" << boost::lexical_cast<std::string>(obj->create_id());
	query << ",";
	query << "create_vip=" << boost::lexical_cast<std::string>(obj->create_vip());
	query << ",";
	query << "create_achieve=" << boost::lexical_cast<std::string>(obj->create_achieve());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();
	query << ",";
	query << "remain=" << boost::lexical_cast<std::string>(obj->remain());
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_jewel_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_jewel(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_jewel=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflag=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlguild_red_t::remove(mysqlpp::Query& query)
{
	dhc::guild_red_t *obj = (dhc::guild_red_t *)data_;
	query << "DELETE FROM guild_red_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlhuodong_t::insert(mysqlpp::Query& query)
{
	dhc::huodong_t *obj = (dhc::huodong_t *)data_;
	query << "INSERT INTO huodong_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "id=" << boost::lexical_cast<std::string>(obj->id());
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "subtype=" << boost::lexical_cast<std::string>(obj->subtype());
	query << ",";
	query << "start=" << boost::lexical_cast<std::string>(obj->start());
	query << ",";
	query << "end=" << boost::lexical_cast<std::string>(obj->end());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "kai_start=" << boost::lexical_cast<std::string>(obj->kai_start());
	query << ",";
	query << "kai_end=" << boost::lexical_cast<std::string>(obj->kai_end());
	query << ",";
	query << "group_name=" << mysqlpp::quote << obj->group_name();
	query << ",";
	query << "show_time=" << boost::lexical_cast<std::string>(obj->show_time());
	query << ",";
	query << "noshow_time=" << boost::lexical_cast<std::string>(obj->noshow_time());
	query << ",";
	{
		uint32_t size = obj->entrys_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->entrys(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "entrys=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "jieri_time=" << boost::lexical_cast<std::string>(obj->jieri_time());
	query << ",";
	query << "kaikai_start=" << boost::lexical_cast<std::string>(obj->kaikai_start());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_players_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_players(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_players=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_subs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_subs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_subs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "rank_time=" << boost::lexical_cast<std::string>(obj->rank_time());
	query << ",";
	query << "item_name1=" << mysqlpp::quote << obj->item_name1();
	query << ",";
	query << "item_des1=" << mysqlpp::quote << obj->item_des1();
	query << ",";
	query << "item_name2=" << mysqlpp::quote << obj->item_name2();
	query << ",";
	query << "item_des2=" << mysqlpp::quote << obj->item_des2();
	query << ",";
	query << "extra_data=" << boost::lexical_cast<std::string>(obj->extra_data());
	query << ",";
	query << "extra_data1=" << boost::lexical_cast<std::string>(obj->extra_data1());
	query << ",";
	query << "extra_data2=" << boost::lexical_cast<std::string>(obj->extra_data2());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlhuodong_t::query(mysqlpp::Query& query)
{
	dhc::huodong_t *obj = (dhc::huodong_t *)data_;
	query << "SELECT * FROM huodong_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_id(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_type(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_subtype(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_start(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_end(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_name((std::string)res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_kai_start(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_kai_end(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_group_name((std::string)res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_show_time(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_noshow_time(res.at(0).at(11));
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
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
	if (!res.at(0).at(13).is_null())
	{
		obj->set_jieri_time(res.at(0).at(13));
	}
	if (!res.at(0).at(14).is_null())
	{
		obj->set_kaikai_start(res.at(0).at(14));
	}
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
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
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
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
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
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
	if (!res.at(0).at(18).is_null())
	{
		std::string temp(res.at(0).at(18).data(), res.at(0).at(18).length());
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
	if (!res.at(0).at(19).is_null())
	{
		obj->set_rank_time(res.at(0).at(19));
	}
	if (!res.at(0).at(20).is_null())
	{
		obj->set_item_name1((std::string)res.at(0).at(20));
	}
	if (!res.at(0).at(21).is_null())
	{
		obj->set_item_des1((std::string)res.at(0).at(21));
	}
	if (!res.at(0).at(22).is_null())
	{
		obj->set_item_name2((std::string)res.at(0).at(22));
	}
	if (!res.at(0).at(23).is_null())
	{
		obj->set_item_des2((std::string)res.at(0).at(23));
	}
	if (!res.at(0).at(24).is_null())
	{
		obj->set_extra_data(res.at(0).at(24));
	}
	if (!res.at(0).at(25).is_null())
	{
		obj->set_extra_data1(res.at(0).at(25));
	}
	if (!res.at(0).at(26).is_null())
	{
		obj->set_extra_data2(res.at(0).at(26));
	}
	return 0;
}

int Sqlhuodong_t::update(mysqlpp::Query& query)
{
	dhc::huodong_t *obj = (dhc::huodong_t *)data_;
	query << "UPDATE huodong_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "id=" << boost::lexical_cast<std::string>(obj->id());
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "subtype=" << boost::lexical_cast<std::string>(obj->subtype());
	query << ",";
	query << "start=" << boost::lexical_cast<std::string>(obj->start());
	query << ",";
	query << "end=" << boost::lexical_cast<std::string>(obj->end());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "kai_start=" << boost::lexical_cast<std::string>(obj->kai_start());
	query << ",";
	query << "kai_end=" << boost::lexical_cast<std::string>(obj->kai_end());
	query << ",";
	query << "group_name=" << mysqlpp::quote << obj->group_name();
	query << ",";
	query << "show_time=" << boost::lexical_cast<std::string>(obj->show_time());
	query << ",";
	query << "noshow_time=" << boost::lexical_cast<std::string>(obj->noshow_time());
	query << ",";
	{
		uint32_t size = obj->entrys_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->entrys(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "entrys=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "jieri_time=" << boost::lexical_cast<std::string>(obj->jieri_time());
	query << ",";
	query << "kaikai_start=" << boost::lexical_cast<std::string>(obj->kaikai_start());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_players_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_players(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_players=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_subs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_subs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_subs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "rank_time=" << boost::lexical_cast<std::string>(obj->rank_time());
	query << ",";
	query << "item_name1=" << mysqlpp::quote << obj->item_name1();
	query << ",";
	query << "item_des1=" << mysqlpp::quote << obj->item_des1();
	query << ",";
	query << "item_name2=" << mysqlpp::quote << obj->item_name2();
	query << ",";
	query << "item_des2=" << mysqlpp::quote << obj->item_des2();
	query << ",";
	query << "extra_data=" << boost::lexical_cast<std::string>(obj->extra_data());
	query << ",";
	query << "extra_data1=" << boost::lexical_cast<std::string>(obj->extra_data1());
	query << ",";
	query << "extra_data2=" << boost::lexical_cast<std::string>(obj->extra_data2());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlhuodong_t::remove(mysqlpp::Query& query)
{
	dhc::huodong_t *obj = (dhc::huodong_t *)data_;
	query << "DELETE FROM huodong_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlhuodong_entry_t::insert(mysqlpp::Query& query)
{
	dhc::huodong_entry_t *obj = (dhc::huodong_entry_t *)data_;
	query << "INSERT INTO huodong_entry_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "huodong_guid=" << boost::lexical_cast<std::string>(obj->huodong_guid());
	query << ",";
	query << "id=" << boost::lexical_cast<std::string>(obj->id());
	query << ",";
	query << "cond=" << boost::lexical_cast<std::string>(obj->cond());
	query << ",";
	query << "arg1=" << boost::lexical_cast<std::string>(obj->arg1());
	query << ",";
	query << "arg2=" << boost::lexical_cast<std::string>(obj->arg2());
	query << ",";
	query << "arg3=" << boost::lexical_cast<std::string>(obj->arg3());
	query << ",";
	query << "arg4=" << boost::lexical_cast<std::string>(obj->arg4());
	query << ",";
	query << "arg5=" << boost::lexical_cast<std::string>(obj->arg5());
	query << ",";
	{
		uint32_t size = obj->arg6_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->arg6(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "arg6=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->arg7_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->arg7(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "arg7=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->arg8_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->arg8(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "arg8=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->types_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->types(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "types=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value1s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value1s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value1s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value2s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value2s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value2s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value3s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value3s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value3s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "show_time=" << boost::lexical_cast<std::string>(obj->show_time());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_arg1s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_arg1s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_arg1s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_arg2s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_arg2s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_arg2s=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlhuodong_entry_t::query(mysqlpp::Query& query)
{
	dhc::huodong_entry_t *obj = (dhc::huodong_entry_t *)data_;
	query << "SELECT * FROM huodong_entry_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_huodong_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_id(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_cond(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_arg1(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_arg2(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_arg3(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_arg4(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_arg5(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_arg6(v);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_arg7(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_arg8(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_types(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_value1s(v);
		}
	}
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_value2s(v);
		}
	}
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_value3s(v);
		}
	}
	if (!res.at(0).at(16).is_null())
	{
		obj->set_show_time(res.at(0).at(16));
	}
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
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
	if (!res.at(0).at(18).is_null())
	{
		std::string temp(res.at(0).at(18).data(), res.at(0).at(18).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_arg1s(v);
		}
	}
	if (!res.at(0).at(19).is_null())
	{
		std::string temp(res.at(0).at(19).data(), res.at(0).at(19).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_arg2s(v);
		}
	}
	return 0;
}

int Sqlhuodong_entry_t::update(mysqlpp::Query& query)
{
	dhc::huodong_entry_t *obj = (dhc::huodong_entry_t *)data_;
	query << "UPDATE huodong_entry_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "huodong_guid=" << boost::lexical_cast<std::string>(obj->huodong_guid());
	query << ",";
	query << "id=" << boost::lexical_cast<std::string>(obj->id());
	query << ",";
	query << "cond=" << boost::lexical_cast<std::string>(obj->cond());
	query << ",";
	query << "arg1=" << boost::lexical_cast<std::string>(obj->arg1());
	query << ",";
	query << "arg2=" << boost::lexical_cast<std::string>(obj->arg2());
	query << ",";
	query << "arg3=" << boost::lexical_cast<std::string>(obj->arg3());
	query << ",";
	query << "arg4=" << boost::lexical_cast<std::string>(obj->arg4());
	query << ",";
	query << "arg5=" << boost::lexical_cast<std::string>(obj->arg5());
	query << ",";
	{
		uint32_t size = obj->arg6_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->arg6(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "arg6=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->arg7_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->arg7(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "arg7=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->arg8_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->arg8(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "arg8=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->types_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->types(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "types=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value1s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value1s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value1s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value2s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value2s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value2s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value3s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value3s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value3s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "show_time=" << boost::lexical_cast<std::string>(obj->show_time());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_arg1s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_arg1s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_arg1s=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_arg2s_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_arg2s(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_arg2s=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlhuodong_entry_t::remove(mysqlpp::Query& query)
{
	dhc::huodong_entry_t *obj = (dhc::huodong_entry_t *)data_;
	query << "DELETE FROM huodong_entry_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlhuodong_player_t::insert(mysqlpp::Query& query)
{
	dhc::huodong_player_t *obj = (dhc::huodong_player_t *)data_;
	query << "INSERT INTO huodong_player_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "huodong_guid=" << boost::lexical_cast<std::string>(obj->huodong_guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "arg1=" << boost::lexical_cast<std::string>(obj->arg1());
	query << ",";
	query << "arg2=" << boost::lexical_cast<std::string>(obj->arg2());
	query << ",";
	query << "arg3=" << boost::lexical_cast<std::string>(obj->arg3());
	query << ",";
	query << "arg4=" << boost::lexical_cast<std::string>(obj->arg4());
	query << ",";
	query << "arg5=" << boost::lexical_cast<std::string>(obj->arg5());
	query << ",";
	query << "arg6=" << boost::lexical_cast<std::string>(obj->arg6());
	query << ",";
	query << "arg7=" << boost::lexical_cast<std::string>(obj->arg7());
	query << ",";
	{
		uint32_t size = obj->args1_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args1(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args1=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args2_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args2(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args2=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args3_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args3(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args3=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args4_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args4(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args4=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args5_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args5(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args5=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args6_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->args6(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "args6=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args7_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->args7(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "args7=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args8_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->args8(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "args8=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args9_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args9(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args9=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->times_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->times(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "times=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "start=" << boost::lexical_cast<std::string>(obj->start());
	query << ",";
	query << "end=" << boost::lexical_cast<std::string>(obj->end());
	query << ",";
	{
		uint32_t size = obj->extra_data1_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data1(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data1=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data2_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data2(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data2=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data3_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data3(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data3=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data4_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data4(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data4=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data5_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data5(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data5=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data6_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data6(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data6=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlhuodong_player_t::query(mysqlpp::Query& query)
{
	dhc::huodong_player_t *obj = (dhc::huodong_player_t *)data_;
	query << "SELECT * FROM huodong_player_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_huodong_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_player_guid(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_arg1(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_arg2(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_arg3(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_arg4(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_arg5(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_arg6(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_arg7(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_args1(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_args2(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_args3(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_args4(v);
		}
	}
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_args5(v);
		}
	}
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_args6(v);
		}
	}
	if (!res.at(0).at(16).is_null())
	{
		std::string temp(res.at(0).at(16).data(), res.at(0).at(16).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_args7(v);
		}
	}
	if (!res.at(0).at(17).is_null())
	{
		std::string temp(res.at(0).at(17).data(), res.at(0).at(17).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_args8(v);
		}
	}
	if (!res.at(0).at(18).is_null())
	{
		std::string temp(res.at(0).at(18).data(), res.at(0).at(18).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_args9(v);
		}
	}
	if (!res.at(0).at(19).is_null())
	{
		std::string temp(res.at(0).at(19).data(), res.at(0).at(19).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_times(v);
		}
	}
	if (!res.at(0).at(20).is_null())
	{
		obj->set_start(res.at(0).at(20));
	}
	if (!res.at(0).at(21).is_null())
	{
		obj->set_end(res.at(0).at(21));
	}
	if (!res.at(0).at(22).is_null())
	{
		std::string temp(res.at(0).at(22).data(), res.at(0).at(22).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_extra_data1(v);
		}
	}
	if (!res.at(0).at(23).is_null())
	{
		std::string temp(res.at(0).at(23).data(), res.at(0).at(23).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_extra_data2(v);
		}
	}
	if (!res.at(0).at(24).is_null())
	{
		std::string temp(res.at(0).at(24).data(), res.at(0).at(24).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_extra_data3(v);
		}
	}
	if (!res.at(0).at(25).is_null())
	{
		std::string temp(res.at(0).at(25).data(), res.at(0).at(25).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_extra_data4(v);
		}
	}
	if (!res.at(0).at(26).is_null())
	{
		std::string temp(res.at(0).at(26).data(), res.at(0).at(26).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_extra_data5(v);
		}
	}
	if (!res.at(0).at(27).is_null())
	{
		std::string temp(res.at(0).at(27).data(), res.at(0).at(27).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_extra_data6(v);
		}
	}
	return 0;
}

int Sqlhuodong_player_t::update(mysqlpp::Query& query)
{
	dhc::huodong_player_t *obj = (dhc::huodong_player_t *)data_;
	query << "UPDATE huodong_player_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "huodong_guid=" << boost::lexical_cast<std::string>(obj->huodong_guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "arg1=" << boost::lexical_cast<std::string>(obj->arg1());
	query << ",";
	query << "arg2=" << boost::lexical_cast<std::string>(obj->arg2());
	query << ",";
	query << "arg3=" << boost::lexical_cast<std::string>(obj->arg3());
	query << ",";
	query << "arg4=" << boost::lexical_cast<std::string>(obj->arg4());
	query << ",";
	query << "arg5=" << boost::lexical_cast<std::string>(obj->arg5());
	query << ",";
	query << "arg6=" << boost::lexical_cast<std::string>(obj->arg6());
	query << ",";
	query << "arg7=" << boost::lexical_cast<std::string>(obj->arg7());
	query << ",";
	{
		uint32_t size = obj->args1_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args1(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args1=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args2_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args2(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args2=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args3_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args3(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args3=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args4_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args4(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args4=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args5_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args5(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args5=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args6_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->args6(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "args6=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args7_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->args7(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "args7=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args8_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->args8(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "args8=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->args9_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->args9(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "args9=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->times_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->times(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "times=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "start=" << boost::lexical_cast<std::string>(obj->start());
	query << ",";
	query << "end=" << boost::lexical_cast<std::string>(obj->end());
	query << ",";
	{
		uint32_t size = obj->extra_data1_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data1(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data1=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data2_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data2(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data2=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data3_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data3(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data3=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data4_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data4(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data4=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data5_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data5(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data5=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->extra_data6_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->extra_data6(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "extra_data6=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlhuodong_player_t::remove(mysqlpp::Query& query)
{
	dhc::huodong_player_t *obj = (dhc::huodong_player_t *)data_;
	query << "DELETE FROM huodong_player_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlpvp_list_t::insert(mysqlpp::Query& query)
{
	dhc::pvp_list_t *obj = (dhc::pvp_list_t *)data_;
	query << "INSERT INTO pvp_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_targets_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_targets(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_targets=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_servers_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_servers(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_servers=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bfs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bfs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bfs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_pvps_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_pvps(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_pvps=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_wins_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_wins(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_wins=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_guanghuans_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_guanghuans(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_guanghuans=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_dress_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_dress(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_dress=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghaos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghaos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghaos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "refresh_time=" << boost::lexical_cast<std::string>(obj->refresh_time());
	query << ",";
	query << "sync_flag=" << boost::lexical_cast<std::string>(obj->sync_flag());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlpvp_list_t::query(mysqlpp::Query& query)
{
	dhc::pvp_list_t *obj = (dhc::pvp_list_t *)data_;
	query << "SELECT * FROM pvp_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		std::string temp(res.at(0).at(1).data(), res.at(0).at(1).length());
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
	if (!res.at(0).at(2).is_null())
	{
		std::string temp(res.at(0).at(2).data(), res.at(0).at(2).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_player_targets(v);
		}
	}
	if (!res.at(0).at(3).is_null())
	{
		std::string temp(res.at(0).at(3).data(), res.at(0).at(3).length());
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
	if (!res.at(0).at(4).is_null())
	{
		std::string temp(res.at(0).at(4).data(), res.at(0).at(4).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_templates(v);
		}
	}
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_servers(v);
		}
	}
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_bfs(v);
		}
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_pvps(v);
		}
	}
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_wins(v);
		}
	}
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_vips(v);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_achieves(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_guanghuans(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_dress(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_chenghaos(v);
		}
	}
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_nalflags(v);
		}
	}
	if (!res.at(0).at(15).is_null())
	{
		obj->set_refresh_time(res.at(0).at(15));
	}
	if (!res.at(0).at(16).is_null())
	{
		obj->set_sync_flag(res.at(0).at(16));
	}
	return 0;
}

int Sqlpvp_list_t::update(mysqlpp::Query& query)
{
	dhc::pvp_list_t *obj = (dhc::pvp_list_t *)data_;
	query << "UPDATE pvp_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->player_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_targets_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_targets(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_targets=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_names_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_names(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_names=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_templates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_templates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_templates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_servers_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_servers(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_servers=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bfs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bfs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bfs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_pvps_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_pvps(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_pvps=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_wins_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_wins(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_wins=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_guanghuans_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_guanghuans(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_guanghuans=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_dress_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_dress(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_dress=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghaos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghaos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghaos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "refresh_time=" << boost::lexical_cast<std::string>(obj->refresh_time());
	query << ",";
	query << "sync_flag=" << boost::lexical_cast<std::string>(obj->sync_flag());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlpvp_list_t::remove(mysqlpp::Query& query)
{
	dhc::pvp_list_t *obj = (dhc::pvp_list_t *)data_;
	query << "DELETE FROM pvp_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlpet_t::insert(mysqlpp::Query& query)
{
	dhc::pet_t *obj = (dhc::pet_t *)data_;
	query << "INSERT INTO pet_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "role_guid=" << boost::lexical_cast<std::string>(obj->role_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "jinjie=" << boost::lexical_cast<std::string>(obj->jinjie());
	query << ",";
	query << "star=" << boost::lexical_cast<std::string>(obj->star());
	query << ",";
	query << "exp=" << boost::lexical_cast<std::string>(obj->exp());
	query << ",";
	{
		uint32_t size = obj->jinjie_slot_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->jinjie_slot(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "jinjie_slot=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlpet_t::query(mysqlpp::Query& query)
{
	dhc::pet_t *obj = (dhc::pet_t *)data_;
	query << "SELECT * FROM pet_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_player_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_role_guid(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_template_id(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_level(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_jinjie(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_star(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_exp(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_jinjie_slot(v);
		}
	}
	return 0;
}

int Sqlpet_t::update(mysqlpp::Query& query)
{
	dhc::pet_t *obj = (dhc::pet_t *)data_;
	query << "UPDATE pet_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "role_guid=" << boost::lexical_cast<std::string>(obj->role_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "jinjie=" << boost::lexical_cast<std::string>(obj->jinjie());
	query << ",";
	query << "star=" << boost::lexical_cast<std::string>(obj->star());
	query << ",";
	query << "exp=" << boost::lexical_cast<std::string>(obj->exp());
	query << ",";
	{
		uint32_t size = obj->jinjie_slot_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->jinjie_slot(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "jinjie_slot=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlpet_t::remove(mysqlpp::Query& query)
{
	dhc::pet_t *obj = (dhc::pet_t *)data_;
	query << "DELETE FROM pet_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlplayer_t::insert(mysqlpp::Query& query)
{
	dhc::player_t *obj = (dhc::player_t *)data_;
	query << "INSERT INTO player_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "serverid=" << mysqlpp::quote << obj->serverid();
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "birth_time=" << boost::lexical_cast<std::string>(obj->birth_time());
	query << ",";
	query << "last_daily_time=" << boost::lexical_cast<std::string>(obj->last_daily_time());
	query << ",";
	query << "last_week_time=" << boost::lexical_cast<std::string>(obj->last_week_time());
	query << ",";
	query << "last_month_time=" << boost::lexical_cast<std::string>(obj->last_month_time());
	query << ",";
	query << "last_tili_time=" << boost::lexical_cast<std::string>(obj->last_tili_time());
	query << ",";
	query << "last_energy_time=" << boost::lexical_cast<std::string>(obj->last_energy_time());
	query << ",";
	query << "zsname=" << mysqlpp::quote << obj->zsname();
	query << ",";
	query << "last_login_time=" << boost::lexical_cast<std::string>(obj->last_login_time());
	query << ",";
	query << "last_check_time=" << boost::lexical_cast<std::string>(obj->last_check_time());
	query << ",";
	query << "last_fight_time=" << boost::lexical_cast<std::string>(obj->last_fight_time());
	query << ",";
	query << "change_name_num=" << boost::lexical_cast<std::string>(obj->change_name_num());
	query << ",";
	query << "change_nalflag_num=" << boost::lexical_cast<std::string>(obj->change_nalflag_num());
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());
	query << ",";
	query << "gold=" << boost::lexical_cast<std::string>(obj->gold());
	query << ",";
	query << "jewel=" << boost::lexical_cast<std::string>(obj->jewel());
	query << ",";
	query << "tili=" << boost::lexical_cast<std::string>(obj->tili());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "exp=" << boost::lexical_cast<std::string>(obj->exp());
	query << ",";
	query << "jjc_point=" << boost::lexical_cast<std::string>(obj->jjc_point());
	query << ",";
	query << "mw_point=" << boost::lexical_cast<std::string>(obj->mw_point());
	query << ",";
	query << "equip_kc_num=" << boost::lexical_cast<std::string>(obj->equip_kc_num());
	query << ",";
	query << "bf=" << boost::lexical_cast<std::string>(obj->bf());
	query << ",";
	query << "yuanli=" << boost::lexical_cast<std::string>(obj->yuanli());
	query << ",";
	query << "contribution=" << boost::lexical_cast<std::string>(obj->contribution());
	query << ",";
	query << "powder=" << boost::lexical_cast<std::string>(obj->powder());
	query << ",";
	query << "energy=" << boost::lexical_cast<std::string>(obj->energy());
	query << ",";
	query << "medal_point=" << boost::lexical_cast<std::string>(obj->medal_point());
	query << ",";
	query << "luck_point=" << boost::lexical_cast<std::string>(obj->luck_point());
	query << ",";
	query << "huiyi_point=" << boost::lexical_cast<std::string>(obj->huiyi_point());
	query << ",";
	query << "lieren_point=" << boost::lexical_cast<std::string>(obj->lieren_point());
	query << ",";
	query << "bf_max=" << boost::lexical_cast<std::string>(obj->bf_max());
	query << ",";
	query << "bingjing=" << boost::lexical_cast<std::string>(obj->bingjing());
	query << ",";
	query << "xinpian=" << boost::lexical_cast<std::string>(obj->xinpian());
	query << ",";
	query << "youqingdian=" << boost::lexical_cast<std::string>(obj->youqingdian());
	query << ",";
	{
		uint32_t size = obj->roles_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->roles(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "roles=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->equips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->equips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "equips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->item_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->item_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "item_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->item_amount_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->item_amount(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "item_amount=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->role_template_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->role_template_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "role_template_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->treasures_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->treasures(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "treasures=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "treasure_kc_num=" << boost::lexical_cast<std::string>(obj->treasure_kc_num());
	query << ",";
	{
		uint32_t size = obj->chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "chenghao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->chengchao_time_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->chengchao_time(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "chengchao_time=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "chenghao_on=" << boost::lexical_cast<std::string>(obj->chenghao_on());
	query << ",";
	{
		uint32_t size = obj->pets_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->pets(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "pets=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pet_on=" << boost::lexical_cast<std::string>(obj->pet_on());
	query << ",";
	query << "mission=" << boost::lexical_cast<std::string>(obj->mission());
	query << ",";
	query << "mission_jy=" << boost::lexical_cast<std::string>(obj->mission_jy());
	query << ",";
	{
		uint32_t size = obj->mission_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_star_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_star(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_star=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_cishu_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_cishu_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_cishu_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_cishu_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_cishu(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_cishu=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_goumai_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_goumai_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_goumai_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_goumai_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_goumai(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_goumai=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->map_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->map_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "map_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->map_star_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->map_star(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "map_star=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->map_reward_get_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->map_reward_get(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "map_reward_get=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "yb_finish_num=" << boost::lexical_cast<std::string>(obj->yb_finish_num());
	query << ",";
	{
		uint32_t size = obj->ybq_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->ybq_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "ybq_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "yb_type=" << boost::lexical_cast<std::string>(obj->yb_type());
	query << ",";
	query << "yb_level=" << boost::lexical_cast<std::string>(obj->yb_level());
	query << ",";
	query << "yb_start_time=" << boost::lexical_cast<std::string>(obj->yb_start_time());
	query << ",";
	query << "yb_jiasu_time=" << boost::lexical_cast<std::string>(obj->yb_jiasu_time());
	query << ",";
	query << "yb_refresh_num=" << boost::lexical_cast<std::string>(obj->yb_refresh_num());
	query << ",";
	query << "yb_gw_type=" << boost::lexical_cast<std::string>(obj->yb_gw_type());
	query << ",";
	query << "yb_byb_num=" << boost::lexical_cast<std::string>(obj->yb_byb_num());
	query << ",";
	query << "yb_per=" << boost::lexical_cast<std::string>(obj->yb_per());
	query << ",";
	query << "ybq_finish_num=" << boost::lexical_cast<std::string>(obj->ybq_finish_num());
	query << ",";
	query << "ybq_last_time=" << boost::lexical_cast<std::string>(obj->ybq_last_time());
	query << ",";
	query << "ore_finish_num=" << boost::lexical_cast<std::string>(obj->ore_finish_num());
	query << ",";
	query << "ore_nindex=" << boost::lexical_cast<std::string>(obj->ore_nindex());
	query << ",";
	query << "ore_last_time=" << boost::lexical_cast<std::string>(obj->ore_last_time());
	query << ",";
	{
		uint32_t size = obj->mission_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->zhenxing_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->zhenxing(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "zhenxing=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->duixing_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->duixing(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "duixing=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "duixing_id=" << boost::lexical_cast<std::string>(obj->duixing_id());
	query << ",";
	query << "duixing_level=" << boost::lexical_cast<std::string>(obj->duixing_level());
	query << ",";
	{
		uint32_t size = obj->houyuan_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->houyuan(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "houyuan=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guanghuan_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guanghuan(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guanghuan=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guanghuan_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guanghuan_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guanghuan_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "guanghuan_id=" << boost::lexical_cast<std::string>(obj->guanghuan_id());
	query << ",";
	query << "ck2_free_time=" << boost::lexical_cast<std::string>(obj->ck2_free_time());
	query << ",";
	{
		uint32_t size = obj->ck_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ck_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ck_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->finished_tasks_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->finished_tasks(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "finished_tasks=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pt_task_num=" << boost::lexical_cast<std::string>(obj->pt_task_num());
	query << ",";
	query << "jy_task_num=" << boost::lexical_cast<std::string>(obj->jy_task_num());
	query << ",";
	query << "sj_task_num=" << boost::lexical_cast<std::string>(obj->sj_task_num());
	query << ",";
	query << "jjie_task_num=" << boost::lexical_cast<std::string>(obj->jjie_task_num());
	query << ",";
	query << "qh_task_num=" << boost::lexical_cast<std::string>(obj->qh_task_num());
	query << ",";
	query << "jj_task_num=" << boost::lexical_cast<std::string>(obj->jj_task_num());
	query << ",";
	query << "boss_task_num=" << boost::lexical_cast<std::string>(obj->boss_task_num());
	query << ",";
	query << "hs_task_num=" << boost::lexical_cast<std::string>(obj->hs_task_num());
	query << ",";
	query << "ttt_task_num=" << boost::lexical_cast<std::string>(obj->ttt_task_num());
	query << ",";
	query << "bweh_task_num=" << boost::lexical_cast<std::string>(obj->bweh_task_num());
	query << ",";
	query << "bwjl_task_num=" << boost::lexical_cast<std::string>(obj->bwjl_task_num());
	query << ",";
	query << "jjcs_task_num=" << boost::lexical_cast<std::string>(obj->jjcs_task_num());
	query << ",";
	query << "yb_task_num=" << boost::lexical_cast<std::string>(obj->yb_task_num());
	query << ",";
	{
		uint32_t size = obj->shoppet_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shoppet_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shoppet_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shoppet_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shoppet_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shoppet_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shoppet_refresh_num=" << boost::lexical_cast<std::string>(obj->shoppet_refresh_num());
	query << ",";
	query << "shoppet_last_time=" << boost::lexical_cast<std::string>(obj->shoppet_last_time());
	query << ",";
	query << "shoppet_num=" << boost::lexical_cast<std::string>(obj->shoppet_num());
	query << ",";
	query << "shop1_refresh_num=" << boost::lexical_cast<std::string>(obj->shop1_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop1_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop1_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop1_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop1_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop1_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop1_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop2_refresh_num=" << boost::lexical_cast<std::string>(obj->shop2_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop2_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop2_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop2_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop2_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop2_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop2_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop3_refresh_num=" << boost::lexical_cast<std::string>(obj->shop3_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop3_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop3_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop3_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop3_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop3_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop3_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop_last_time=" << boost::lexical_cast<std::string>(obj->shop_last_time());
	query << ",";
	{
		uint32_t size = obj->shop_xg_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_xg_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_xg_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop_xg_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_xg_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_xg_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop_refresh_num=" << boost::lexical_cast<std::string>(obj->shop_refresh_num());
	query << ",";
	query << "shop_buy_num=" << boost::lexical_cast<std::string>(obj->shop_buy_num());
	query << ",";
	query << "shop4_refresh_num=" << boost::lexical_cast<std::string>(obj->shop4_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop4_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop4_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop4_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop4_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop4_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop4_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_shop_last_time=" << boost::lexical_cast<std::string>(obj->huiyi_shop_last_time());
	query << ",";
	{
		uint32_t size = obj->active_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->active_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->active_reward_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_reward(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_reward=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "active_score=" << boost::lexical_cast<std::string>(obj->active_score());
	query << ",";
	{
		uint32_t size = obj->active_score_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_score_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_score_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->sports_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->sports(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "sports=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "max_rank=" << boost::lexical_cast<std::string>(obj->max_rank());
	query << ",";
	query << "zhouka_time=" << boost::lexical_cast<std::string>(obj->zhouka_time());
	query << ",";
	query << "yueka_time=" << boost::lexical_cast<std::string>(obj->yueka_time());
	query << ",";
	query << "total_recharge=" << boost::lexical_cast<std::string>(obj->total_recharge());
	query << ",";
	query << "vip=" << boost::lexical_cast<std::string>(obj->vip());
	query << ",";
	query << "vip_exp=" << boost::lexical_cast<std::string>(obj->vip_exp());
	query << ",";
	{
		uint32_t size = obj->recharge_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->recharge_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "recharge_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->vip_reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->vip_reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "vip_reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dj_num=" << boost::lexical_cast<std::string>(obj->dj_num());
	query << ",";
	query << "tili_reward=" << boost::lexical_cast<std::string>(obj->tili_reward());
	query << ",";
	query << "first_reward=" << boost::lexical_cast<std::string>(obj->first_reward());
	query << ",";
	query << "online_reward_index=" << boost::lexical_cast<std::string>(obj->online_reward_index());
	query << ",";
	query << "online_reward_time=" << boost::lexical_cast<std::string>(obj->online_reward_time());
	query << ",";
	query << "daily_sign_index=" << boost::lexical_cast<std::string>(obj->daily_sign_index());
	query << ",";
	query << "daily_sign_reward=" << boost::lexical_cast<std::string>(obj->daily_sign_reward());
	query << ",";
	query << "daily_sign_num=" << boost::lexical_cast<std::string>(obj->daily_sign_num());
	query << ",";
	{
		uint32_t size = obj->libao_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->libao_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "libao_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->rdz_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rdz_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rdz_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->rdz_counts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rdz_counts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rdz_counts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "guest=" << boost::lexical_cast<std::string>(obj->guest());
	query << ",";
	query << "total_spend=" << boost::lexical_cast<std::string>(obj->total_spend());
	query << ",";
	query << "daily_sign_flag=" << boost::lexical_cast<std::string>(obj->daily_sign_flag());
	query << ",";
	query << "huiyi_chou_num=" << boost::lexical_cast<std::string>(obj->huiyi_chou_num());
	query << ",";
	query << "boss_leiji_damage=" << boost::lexical_cast<std::string>(obj->boss_leiji_damage());
	query << ",";
	query << "boss_max_damage=" << boost::lexical_cast<std::string>(obj->boss_max_damage());
	query << ",";
	query << "boss_max_rank=" << boost::lexical_cast<std::string>(obj->boss_max_rank());
	query << ",";
	{
		uint32_t size = obj->boss_active_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->boss_active_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "boss_active_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->boss_active_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->boss_active_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "boss_active_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->boss_active_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->boss_active_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "boss_active_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "boss_num=" << boost::lexical_cast<std::string>(obj->boss_num());
	query << ",";
	query << "boss_last_time=" << boost::lexical_cast<std::string>(obj->boss_last_time());
	query << ",";
	query << "boss_hit_time=" << boost::lexical_cast<std::string>(obj->boss_hit_time());
	query << ",";
	query << "boss_player_level=" << boost::lexical_cast<std::string>(obj->boss_player_level());
	query << ",";
	query << "hbb_refresh_num=" << boost::lexical_cast<std::string>(obj->hbb_refresh_num());
	query << ",";
	query << "hbb_num=" << boost::lexical_cast<std::string>(obj->hbb_num());
	query << ",";
	query << "hbb_finish_num=" << boost::lexical_cast<std::string>(obj->hbb_finish_num());
	query << ",";
	{
		uint32_t size = obj->hbb_class_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->hbb_class_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "hbb_class_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "ttt_dead=" << boost::lexical_cast<std::string>(obj->ttt_dead());
	query << ",";
	query << "ttt_star=" << boost::lexical_cast<std::string>(obj->ttt_star());
	query << ",";
	{
		uint32_t size = obj->ttt_reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_cur_stars_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_cur_stars(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_cur_stars=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_last_stars_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_last_stars(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_last_stars=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_cur_reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_cur_reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_cur_reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "ttt_can_reward=" << boost::lexical_cast<std::string>(obj->ttt_can_reward());
	query << ",";
	query << "ttt_cz_num=" << boost::lexical_cast<std::string>(obj->ttt_cz_num());
	query << ",";
	query << "ttt_mibao=" << boost::lexical_cast<std::string>(obj->ttt_mibao());
	query << ",";
	query << "random_event_num=" << boost::lexical_cast<std::string>(obj->random_event_num());
	query << ",";
	query << "random_event_time=" << boost::lexical_cast<std::string>(obj->random_event_time());
	query << ",";
	query << "random_event_id=" << boost::lexical_cast<std::string>(obj->random_event_id());
	query << ",";
	query << "guild=" << boost::lexical_cast<std::string>(obj->guild());
	query << ",";
	query << "last_leave_guild_time=" << boost::lexical_cast<std::string>(obj->last_leave_guild_time());
	query << ",";
	{
		uint32_t size = obj->guild_skill_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_skill_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_skill_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_skill_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_skill_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_skill_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_applys_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_applys(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_applys=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_shilians_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_shilians(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_shilians=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "guild_num=" << boost::lexical_cast<std::string>(obj->guild_num());
	query << ",";
	query << "guild_buy_num=" << boost::lexical_cast<std::string>(obj->guild_buy_num());
	query << ",";
	{
		uint32_t size = obj->guild_honors_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_honors(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_honors=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "social_shou_num=" << boost::lexical_cast<std::string>(obj->social_shou_num());
	query << ",";
	query << "guild_sign_time=" << boost::lexical_cast<std::string>(obj->guild_sign_time());
	query << ",";
	query << "guild_sign_flag=" << boost::lexical_cast<std::string>(obj->guild_sign_flag());
	query << ",";
	query << "guild_red_num=" << boost::lexical_cast<std::string>(obj->guild_red_num());
	query << ",";
	query << "guild_red_num1=" << boost::lexical_cast<std::string>(obj->guild_red_num1());
	query << ",";
	query << "guild_deliver_jewel=" << boost::lexical_cast<std::string>(obj->guild_deliver_jewel());
	query << ",";
	query << "guild_rob_jewel=" << boost::lexical_cast<std::string>(obj->guild_rob_jewel());
	query << ",";
	query << "guild_lnum=" << boost::lexical_cast<std::string>(obj->guild_lnum());
	query << ",";
	query << "guild_ltime=" << boost::lexical_cast<std::string>(obj->guild_ltime());
	query << ",";
	query << "guild_pvp_num=" << boost::lexical_cast<std::string>(obj->guild_pvp_num());
	query << ",";
	query << "guild_pvp_buy_num=" << boost::lexical_cast<std::string>(obj->guild_pvp_buy_num());
	query << ",";
	{
		uint32_t size = obj->dress_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->dress_on_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_on_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_on_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->dress_id_bags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_id_bags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_id_bags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dress_tuzhi=" << boost::lexical_cast<std::string>(obj->dress_tuzhi());
	query << ",";
	{
		uint32_t size = obj->dress_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dress_flag=" << boost::lexical_cast<std::string>(obj->dress_flag());
	query << ",";
	{
		uint32_t size = obj->huiyi_jihuos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_jihuos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_jihuos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_shoujidu=" << boost::lexical_cast<std::string>(obj->huiyi_shoujidu());
	query << ",";
	{
		uint32_t size = obj->huiyi_zhanpus_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_zhanpus(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_zhanpus=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_zhanpu_flag=" << boost::lexical_cast<std::string>(obj->huiyi_zhanpu_flag());
	query << ",";
	query << "huiyi_zhanpu_num=" << boost::lexical_cast<std::string>(obj->huiyi_zhanpu_num());
	query << ",";
	query << "huiyi_gaiyun_num=" << boost::lexical_cast<std::string>(obj->huiyi_gaiyun_num());
	query << ",";
	{
		uint32_t size = obj->huiyi_shop_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_shop_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_shop_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huiyi_shop_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_shop_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_shop_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_fan_num=" << boost::lexical_cast<std::string>(obj->huiyi_fan_num());
	query << ",";
	{
		uint32_t size = obj->huiyi_jihuo_starts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_jihuo_starts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_jihuo_starts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_shoujidu_top=" << boost::lexical_cast<std::string>(obj->huiyi_shoujidu_top());
	query << ",";
	query << "treasure_protect_next_time=" << boost::lexical_cast<std::string>(obj->treasure_protect_next_time());
	query << ",";
	query << "treasure_protect_cd_time=" << boost::lexical_cast<std::string>(obj->treasure_protect_cd_time());
	query << ",";
	{
		uint32_t size = obj->treasure_reports_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->treasure_reports(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "treasure_reports=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "treasure_first=" << boost::lexical_cast<std::string>(obj->treasure_first());
	query << ",";
	{
		uint32_t size = obj->treasure_hechengs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->treasure_hechengs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "treasure_hechengs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->yh_roles_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->yh_roles(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "yh_roles=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "yh_time=" << boost::lexical_cast<std::string>(obj->yh_time());
	query << ",";
	query << "yh_hour=" << boost::lexical_cast<std::string>(obj->yh_hour());
	query << ",";
	query << "yh_update_time=" << boost::lexical_cast<std::string>(obj->yh_update_time());
	query << ",";
	{
		uint32_t size = obj->qiyu_mission_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->qiyu_mission(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "qiyu_mission=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->qiyu_hard_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->qiyu_hard(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "qiyu_hard=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->qiyu_suc_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->qiyu_suc(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "qiyu_suc=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "qiyu_last_time=" << boost::lexical_cast<std::string>(obj->qiyu_last_time());
	query << ",";
	{
		uint32_t size = obj->sport_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->sport_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "sport_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mw_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mw_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mw_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_shop_ex_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_shop_ex_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_shop_ex_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_red_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_red_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_red_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pvp_num=" << boost::lexical_cast<std::string>(obj->pvp_num());
	query << ",";
	query << "pvp_refresh_num=" << boost::lexical_cast<std::string>(obj->pvp_refresh_num());
	query << ",";
	query << "pvp_buy_num=" << boost::lexical_cast<std::string>(obj->pvp_buy_num());
	query << ",";
	query << "pvp_total=" << boost::lexical_cast<std::string>(obj->pvp_total());
	query << ",";
	query << "pvp_hit=" << boost::lexical_cast<std::string>(obj->pvp_hit());
	query << ",";
	{
		uint32_t size = obj->pvp_hit_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->pvp_hit_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "pvp_hit_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->by_shops_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->by_shops(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "by_shops=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->by_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->by_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "by_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->by_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->by_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "by_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "by_reward_num=" << boost::lexical_cast<std::string>(obj->by_reward_num());
	query << ",";
	query << "by_reward_buy=" << boost::lexical_cast<std::string>(obj->by_reward_buy());
	query << ",";
	query << "by_point=" << boost::lexical_cast<std::string>(obj->by_point());
	query << ",";
	query << "ds_reward_buy=" << boost::lexical_cast<std::string>(obj->ds_reward_buy());
	query << ",";
	query << "ds_reward_num=" << boost::lexical_cast<std::string>(obj->ds_reward_num());
	query << ",";
	query << "ds_point=" << boost::lexical_cast<std::string>(obj->ds_point());
	query << ",";
	query << "ds_duanwei=" << boost::lexical_cast<std::string>(obj->ds_duanwei());
	query << ",";
	query << "ds_hit=" << boost::lexical_cast<std::string>(obj->ds_hit());
	query << ",";
	{
		uint32_t size = obj->ds_hit_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ds_hit_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ds_hit_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_pttq_reward_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_pttq_reward(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_pttq_reward=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_kaifu_finish_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_kaifu_finish_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_kaifu_finish_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_czjh_buy=" << boost::lexical_cast<std::string>(obj->huodong_czjh_buy());
	query << ",";
	query << "huodong_czjh_index=" << boost::lexical_cast<std::string>(obj->huodong_czjh_index());
	query << ",";
	query << "huodong_vip_libao=" << boost::lexical_cast<std::string>(obj->huodong_vip_libao());
	query << ",";
	{
		uint32_t size = obj->huodong_week_libao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_week_libao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_week_libao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_kaifu_ptfb=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_ptfb());
	query << ",";
	query << "huodong_kaifu_jyfb=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_jyfb());
	query << ",";
	query << "huodong_kaifu_dbfb=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_dbfb());
	query << ",";
	query << "huodong_kaifu_jjc=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_jjc());
	query << ",";
	{
		uint32_t size = obj->huodong_kaifu_dbcz_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_kaifu_dbcz(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_kaifu_dbcz=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_czjh_index1=" << boost::lexical_cast<std::string>(obj->huodong_czjh_index1());
	query << ",";
	query << "huodong_yxhg_tap=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_tap());
	query << ",";
	query << "huodong_yxhg_rmb=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_rmb());
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_buzhu_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_buzhu_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_buzhu_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_yxhg_buzhu_day=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_buzhu_day());
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_fuli_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_fuli_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_fuli_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_fuli_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_fuli_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_fuli_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_haoli_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_haoli_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_haoli_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_yxhg_end_time=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_end_time());
	query << ",";
	query << "huodong_yxhg_time=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_time());
	query << ",";
	query << "gag_time=" << boost::lexical_cast<std::string>(obj->gag_time());
	query << ",";
	query << "google_stat=" << boost::lexical_cast<std::string>(obj->google_stat());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlplayer_t::query(mysqlpp::Query& query)
{
	dhc::player_t *obj = (dhc::player_t *)data_;
	query << "SELECT * FROM player_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_serverid((std::string)res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_name((std::string)res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_template_id(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_birth_time(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_last_daily_time(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_last_week_time(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_last_month_time(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_last_tili_time(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_last_energy_time(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_zsname((std::string)res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_last_login_time(res.at(0).at(11));
	}
	if (!res.at(0).at(12).is_null())
	{
		obj->set_last_check_time(res.at(0).at(12));
	}
	if (!res.at(0).at(13).is_null())
	{
		obj->set_last_fight_time(res.at(0).at(13));
	}
	if (!res.at(0).at(14).is_null())
	{
		obj->set_change_name_num(res.at(0).at(14));
	}
	if (!res.at(0).at(15).is_null())
	{
		obj->set_change_nalflag_num(res.at(0).at(15));
	}
	if (!res.at(0).at(16).is_null())
	{
		obj->set_nalflag(res.at(0).at(16));
	}
	if (!res.at(0).at(17).is_null())
	{
		obj->set_gold(res.at(0).at(17));
	}
	if (!res.at(0).at(18).is_null())
	{
		obj->set_jewel(res.at(0).at(18));
	}
	if (!res.at(0).at(19).is_null())
	{
		obj->set_tili(res.at(0).at(19));
	}
	if (!res.at(0).at(20).is_null())
	{
		obj->set_level(res.at(0).at(20));
	}
	if (!res.at(0).at(21).is_null())
	{
		obj->set_exp(res.at(0).at(21));
	}
	if (!res.at(0).at(22).is_null())
	{
		obj->set_jjc_point(res.at(0).at(22));
	}
	if (!res.at(0).at(23).is_null())
	{
		obj->set_mw_point(res.at(0).at(23));
	}
	if (!res.at(0).at(24).is_null())
	{
		obj->set_equip_kc_num(res.at(0).at(24));
	}
	if (!res.at(0).at(25).is_null())
	{
		obj->set_bf(res.at(0).at(25));
	}
	if (!res.at(0).at(26).is_null())
	{
		obj->set_yuanli(res.at(0).at(26));
	}
	if (!res.at(0).at(27).is_null())
	{
		obj->set_contribution(res.at(0).at(27));
	}
	if (!res.at(0).at(28).is_null())
	{
		obj->set_powder(res.at(0).at(28));
	}
	if (!res.at(0).at(29).is_null())
	{
		obj->set_energy(res.at(0).at(29));
	}
	if (!res.at(0).at(30).is_null())
	{
		obj->set_medal_point(res.at(0).at(30));
	}
	if (!res.at(0).at(31).is_null())
	{
		obj->set_luck_point(res.at(0).at(31));
	}
	if (!res.at(0).at(32).is_null())
	{
		obj->set_huiyi_point(res.at(0).at(32));
	}
	if (!res.at(0).at(33).is_null())
	{
		obj->set_lieren_point(res.at(0).at(33));
	}
	if (!res.at(0).at(34).is_null())
	{
		obj->set_bf_max(res.at(0).at(34));
	}
	if (!res.at(0).at(35).is_null())
	{
		obj->set_bingjing(res.at(0).at(35));
	}
	if (!res.at(0).at(36).is_null())
	{
		obj->set_xinpian(res.at(0).at(36));
	}
	if (!res.at(0).at(37).is_null())
	{
		obj->set_youqingdian(res.at(0).at(37));
	}
	if (!res.at(0).at(38).is_null())
	{
		std::string temp(res.at(0).at(38).data(), res.at(0).at(38).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_roles(v);
		}
	}
	if (!res.at(0).at(39).is_null())
	{
		std::string temp(res.at(0).at(39).data(), res.at(0).at(39).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_equips(v);
		}
	}
	if (!res.at(0).at(40).is_null())
	{
		std::string temp(res.at(0).at(40).data(), res.at(0).at(40).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_item_ids(v);
		}
	}
	if (!res.at(0).at(41).is_null())
	{
		std::string temp(res.at(0).at(41).data(), res.at(0).at(41).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_item_amount(v);
		}
	}
	if (!res.at(0).at(42).is_null())
	{
		std::string temp(res.at(0).at(42).data(), res.at(0).at(42).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_role_template_ids(v);
		}
	}
	if (!res.at(0).at(43).is_null())
	{
		std::string temp(res.at(0).at(43).data(), res.at(0).at(43).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_treasures(v);
		}
	}
	if (!res.at(0).at(44).is_null())
	{
		obj->set_treasure_kc_num(res.at(0).at(44));
	}
	if (!res.at(0).at(45).is_null())
	{
		std::string temp(res.at(0).at(45).data(), res.at(0).at(45).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_chenghao(v);
		}
	}
	if (!res.at(0).at(46).is_null())
	{
		std::string temp(res.at(0).at(46).data(), res.at(0).at(46).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_chengchao_time(v);
		}
	}
	if (!res.at(0).at(47).is_null())
	{
		obj->set_chenghao_on(res.at(0).at(47));
	}
	if (!res.at(0).at(48).is_null())
	{
		std::string temp(res.at(0).at(48).data(), res.at(0).at(48).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_pets(v);
		}
	}
	if (!res.at(0).at(49).is_null())
	{
		obj->set_pet_on(res.at(0).at(49));
	}
	if (!res.at(0).at(50).is_null())
	{
		obj->set_mission(res.at(0).at(50));
	}
	if (!res.at(0).at(51).is_null())
	{
		obj->set_mission_jy(res.at(0).at(51));
	}
	if (!res.at(0).at(52).is_null())
	{
		std::string temp(res.at(0).at(52).data(), res.at(0).at(52).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_ids(v);
		}
	}
	if (!res.at(0).at(53).is_null())
	{
		std::string temp(res.at(0).at(53).data(), res.at(0).at(53).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_star(v);
		}
	}
	if (!res.at(0).at(54).is_null())
	{
		std::string temp(res.at(0).at(54).data(), res.at(0).at(54).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_cishu_ids(v);
		}
	}
	if (!res.at(0).at(55).is_null())
	{
		std::string temp(res.at(0).at(55).data(), res.at(0).at(55).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_cishu(v);
		}
	}
	if (!res.at(0).at(56).is_null())
	{
		std::string temp(res.at(0).at(56).data(), res.at(0).at(56).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_goumai_ids(v);
		}
	}
	if (!res.at(0).at(57).is_null())
	{
		std::string temp(res.at(0).at(57).data(), res.at(0).at(57).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_goumai(v);
		}
	}
	if (!res.at(0).at(58).is_null())
	{
		std::string temp(res.at(0).at(58).data(), res.at(0).at(58).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_map_ids(v);
		}
	}
	if (!res.at(0).at(59).is_null())
	{
		std::string temp(res.at(0).at(59).data(), res.at(0).at(59).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_map_star(v);
		}
	}
	if (!res.at(0).at(60).is_null())
	{
		std::string temp(res.at(0).at(60).data(), res.at(0).at(60).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_map_reward_get(v);
		}
	}
	if (!res.at(0).at(61).is_null())
	{
		obj->set_yb_finish_num(res.at(0).at(61));
	}
	if (!res.at(0).at(62).is_null())
	{
		std::string temp(res.at(0).at(62).data(), res.at(0).at(62).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_ybq_guids(v);
		}
	}
	if (!res.at(0).at(63).is_null())
	{
		obj->set_yb_type(res.at(0).at(63));
	}
	if (!res.at(0).at(64).is_null())
	{
		obj->set_yb_level(res.at(0).at(64));
	}
	if (!res.at(0).at(65).is_null())
	{
		obj->set_yb_start_time(res.at(0).at(65));
	}
	if (!res.at(0).at(66).is_null())
	{
		obj->set_yb_jiasu_time(res.at(0).at(66));
	}
	if (!res.at(0).at(67).is_null())
	{
		obj->set_yb_refresh_num(res.at(0).at(67));
	}
	if (!res.at(0).at(68).is_null())
	{
		obj->set_yb_gw_type(res.at(0).at(68));
	}
	if (!res.at(0).at(69).is_null())
	{
		obj->set_yb_byb_num(res.at(0).at(69));
	}
	if (!res.at(0).at(70).is_null())
	{
		obj->set_yb_per(res.at(0).at(70));
	}
	if (!res.at(0).at(71).is_null())
	{
		obj->set_ybq_finish_num(res.at(0).at(71));
	}
	if (!res.at(0).at(72).is_null())
	{
		obj->set_ybq_last_time(res.at(0).at(72));
	}
	if (!res.at(0).at(73).is_null())
	{
		obj->set_ore_finish_num(res.at(0).at(73));
	}
	if (!res.at(0).at(74).is_null())
	{
		obj->set_ore_nindex(res.at(0).at(74));
	}
	if (!res.at(0).at(75).is_null())
	{
		obj->set_ore_last_time(res.at(0).at(75));
	}
	if (!res.at(0).at(76).is_null())
	{
		std::string temp(res.at(0).at(76).data(), res.at(0).at(76).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mission_rewards(v);
		}
	}
	if (!res.at(0).at(77).is_null())
	{
		std::string temp(res.at(0).at(77).data(), res.at(0).at(77).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_zhenxing(v);
		}
	}
	if (!res.at(0).at(78).is_null())
	{
		std::string temp(res.at(0).at(78).data(), res.at(0).at(78).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_duixing(v);
		}
	}
	if (!res.at(0).at(79).is_null())
	{
		obj->set_duixing_id(res.at(0).at(79));
	}
	if (!res.at(0).at(80).is_null())
	{
		obj->set_duixing_level(res.at(0).at(80));
	}
	if (!res.at(0).at(81).is_null())
	{
		std::string temp(res.at(0).at(81).data(), res.at(0).at(81).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_houyuan(v);
		}
	}
	if (!res.at(0).at(82).is_null())
	{
		std::string temp(res.at(0).at(82).data(), res.at(0).at(82).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guanghuan(v);
		}
	}
	if (!res.at(0).at(83).is_null())
	{
		std::string temp(res.at(0).at(83).data(), res.at(0).at(83).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guanghuan_level(v);
		}
	}
	if (!res.at(0).at(84).is_null())
	{
		obj->set_guanghuan_id(res.at(0).at(84));
	}
	if (!res.at(0).at(85).is_null())
	{
		obj->set_ck2_free_time(res.at(0).at(85));
	}
	if (!res.at(0).at(86).is_null())
	{
		std::string temp(res.at(0).at(86).data(), res.at(0).at(86).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_ck_num(v);
		}
	}
	if (!res.at(0).at(87).is_null())
	{
		std::string temp(res.at(0).at(87).data(), res.at(0).at(87).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_finished_tasks(v);
		}
	}
	if (!res.at(0).at(88).is_null())
	{
		obj->set_pt_task_num(res.at(0).at(88));
	}
	if (!res.at(0).at(89).is_null())
	{
		obj->set_jy_task_num(res.at(0).at(89));
	}
	if (!res.at(0).at(90).is_null())
	{
		obj->set_sj_task_num(res.at(0).at(90));
	}
	if (!res.at(0).at(91).is_null())
	{
		obj->set_jjie_task_num(res.at(0).at(91));
	}
	if (!res.at(0).at(92).is_null())
	{
		obj->set_qh_task_num(res.at(0).at(92));
	}
	if (!res.at(0).at(93).is_null())
	{
		obj->set_jj_task_num(res.at(0).at(93));
	}
	if (!res.at(0).at(94).is_null())
	{
		obj->set_boss_task_num(res.at(0).at(94));
	}
	if (!res.at(0).at(95).is_null())
	{
		obj->set_hs_task_num(res.at(0).at(95));
	}
	if (!res.at(0).at(96).is_null())
	{
		obj->set_ttt_task_num(res.at(0).at(96));
	}
	if (!res.at(0).at(97).is_null())
	{
		obj->set_bweh_task_num(res.at(0).at(97));
	}
	if (!res.at(0).at(98).is_null())
	{
		obj->set_bwjl_task_num(res.at(0).at(98));
	}
	if (!res.at(0).at(99).is_null())
	{
		obj->set_jjcs_task_num(res.at(0).at(99));
	}
	if (!res.at(0).at(100).is_null())
	{
		obj->set_yb_task_num(res.at(0).at(100));
	}
	if (!res.at(0).at(101).is_null())
	{
		std::string temp(res.at(0).at(101).data(), res.at(0).at(101).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_shoppet_ids(v);
		}
	}
	if (!res.at(0).at(102).is_null())
	{
		std::string temp(res.at(0).at(102).data(), res.at(0).at(102).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shoppet_sell(v);
		}
	}
	if (!res.at(0).at(103).is_null())
	{
		obj->set_shoppet_refresh_num(res.at(0).at(103));
	}
	if (!res.at(0).at(104).is_null())
	{
		obj->set_shoppet_last_time(res.at(0).at(104));
	}
	if (!res.at(0).at(105).is_null())
	{
		obj->set_shoppet_num(res.at(0).at(105));
	}
	if (!res.at(0).at(106).is_null())
	{
		obj->set_shop1_refresh_num(res.at(0).at(106));
	}
	if (!res.at(0).at(107).is_null())
	{
		std::string temp(res.at(0).at(107).data(), res.at(0).at(107).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_shop1_ids(v);
		}
	}
	if (!res.at(0).at(108).is_null())
	{
		std::string temp(res.at(0).at(108).data(), res.at(0).at(108).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop1_sell(v);
		}
	}
	if (!res.at(0).at(109).is_null())
	{
		obj->set_shop2_refresh_num(res.at(0).at(109));
	}
	if (!res.at(0).at(110).is_null())
	{
		std::string temp(res.at(0).at(110).data(), res.at(0).at(110).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_shop2_ids(v);
		}
	}
	if (!res.at(0).at(111).is_null())
	{
		std::string temp(res.at(0).at(111).data(), res.at(0).at(111).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop2_sell(v);
		}
	}
	if (!res.at(0).at(112).is_null())
	{
		obj->set_shop3_refresh_num(res.at(0).at(112));
	}
	if (!res.at(0).at(113).is_null())
	{
		std::string temp(res.at(0).at(113).data(), res.at(0).at(113).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_shop3_ids(v);
		}
	}
	if (!res.at(0).at(114).is_null())
	{
		std::string temp(res.at(0).at(114).data(), res.at(0).at(114).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop3_sell(v);
		}
	}
	if (!res.at(0).at(115).is_null())
	{
		obj->set_shop_last_time(res.at(0).at(115));
	}
	if (!res.at(0).at(116).is_null())
	{
		std::string temp(res.at(0).at(116).data(), res.at(0).at(116).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop_xg_ids(v);
		}
	}
	if (!res.at(0).at(117).is_null())
	{
		std::string temp(res.at(0).at(117).data(), res.at(0).at(117).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop_xg_nums(v);
		}
	}
	if (!res.at(0).at(118).is_null())
	{
		obj->set_shop_refresh_num(res.at(0).at(118));
	}
	if (!res.at(0).at(119).is_null())
	{
		obj->set_shop_buy_num(res.at(0).at(119));
	}
	if (!res.at(0).at(120).is_null())
	{
		obj->set_shop4_refresh_num(res.at(0).at(120));
	}
	if (!res.at(0).at(121).is_null())
	{
		std::string temp(res.at(0).at(121).data(), res.at(0).at(121).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint32_t));
			obj->add_shop4_ids(v);
		}
	}
	if (!res.at(0).at(122).is_null())
	{
		std::string temp(res.at(0).at(122).data(), res.at(0).at(122).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_shop4_sell(v);
		}
	}
	if (!res.at(0).at(123).is_null())
	{
		obj->set_huiyi_shop_last_time(res.at(0).at(123));
	}
	if (!res.at(0).at(124).is_null())
	{
		std::string temp(res.at(0).at(124).data(), res.at(0).at(124).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_active_id(v);
		}
	}
	if (!res.at(0).at(125).is_null())
	{
		std::string temp(res.at(0).at(125).data(), res.at(0).at(125).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_active_num(v);
		}
	}
	if (!res.at(0).at(126).is_null())
	{
		std::string temp(res.at(0).at(126).data(), res.at(0).at(126).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_active_reward(v);
		}
	}
	if (!res.at(0).at(127).is_null())
	{
		obj->set_active_score(res.at(0).at(127));
	}
	if (!res.at(0).at(128).is_null())
	{
		std::string temp(res.at(0).at(128).data(), res.at(0).at(128).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_active_score_id(v);
		}
	}
	if (!res.at(0).at(129).is_null())
	{
		std::string temp(res.at(0).at(129).data(), res.at(0).at(129).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_sports(v);
		}
	}
	if (!res.at(0).at(130).is_null())
	{
		obj->set_max_rank(res.at(0).at(130));
	}
	if (!res.at(0).at(131).is_null())
	{
		obj->set_zhouka_time(res.at(0).at(131));
	}
	if (!res.at(0).at(132).is_null())
	{
		obj->set_yueka_time(res.at(0).at(132));
	}
	if (!res.at(0).at(133).is_null())
	{
		obj->set_total_recharge(res.at(0).at(133));
	}
	if (!res.at(0).at(134).is_null())
	{
		obj->set_vip(res.at(0).at(134));
	}
	if (!res.at(0).at(135).is_null())
	{
		obj->set_vip_exp(res.at(0).at(135));
	}
	if (!res.at(0).at(136).is_null())
	{
		std::string temp(res.at(0).at(136).data(), res.at(0).at(136).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_recharge_ids(v);
		}
	}
	if (!res.at(0).at(137).is_null())
	{
		std::string temp(res.at(0).at(137).data(), res.at(0).at(137).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_vip_reward_ids(v);
		}
	}
	if (!res.at(0).at(138).is_null())
	{
		obj->set_dj_num(res.at(0).at(138));
	}
	if (!res.at(0).at(139).is_null())
	{
		obj->set_tili_reward(res.at(0).at(139));
	}
	if (!res.at(0).at(140).is_null())
	{
		obj->set_first_reward(res.at(0).at(140));
	}
	if (!res.at(0).at(141).is_null())
	{
		obj->set_online_reward_index(res.at(0).at(141));
	}
	if (!res.at(0).at(142).is_null())
	{
		obj->set_online_reward_time(res.at(0).at(142));
	}
	if (!res.at(0).at(143).is_null())
	{
		obj->set_daily_sign_index(res.at(0).at(143));
	}
	if (!res.at(0).at(144).is_null())
	{
		obj->set_daily_sign_reward(res.at(0).at(144));
	}
	if (!res.at(0).at(145).is_null())
	{
		obj->set_daily_sign_num(res.at(0).at(145));
	}
	if (!res.at(0).at(146).is_null())
	{
		std::string temp(res.at(0).at(146).data(), res.at(0).at(146).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_libao_nums(v);
		}
	}
	if (!res.at(0).at(147).is_null())
	{
		std::string temp(res.at(0).at(147).data(), res.at(0).at(147).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_rdz_ids(v);
		}
	}
	if (!res.at(0).at(148).is_null())
	{
		std::string temp(res.at(0).at(148).data(), res.at(0).at(148).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_rdz_counts(v);
		}
	}
	if (!res.at(0).at(149).is_null())
	{
		obj->set_guest(res.at(0).at(149));
	}
	if (!res.at(0).at(150).is_null())
	{
		obj->set_total_spend(res.at(0).at(150));
	}
	if (!res.at(0).at(151).is_null())
	{
		obj->set_daily_sign_flag(res.at(0).at(151));
	}
	if (!res.at(0).at(152).is_null())
	{
		obj->set_huiyi_chou_num(res.at(0).at(152));
	}
	if (!res.at(0).at(153).is_null())
	{
		obj->set_boss_leiji_damage(res.at(0).at(153));
	}
	if (!res.at(0).at(154).is_null())
	{
		obj->set_boss_max_damage(res.at(0).at(154));
	}
	if (!res.at(0).at(155).is_null())
	{
		obj->set_boss_max_rank(res.at(0).at(155));
	}
	if (!res.at(0).at(156).is_null())
	{
		std::string temp(res.at(0).at(156).data(), res.at(0).at(156).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_boss_active_ids(v);
		}
	}
	if (!res.at(0).at(157).is_null())
	{
		std::string temp(res.at(0).at(157).data(), res.at(0).at(157).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int64_t));
			obj->add_boss_active_nums(v);
		}
	}
	if (!res.at(0).at(158).is_null())
	{
		std::string temp(res.at(0).at(158).data(), res.at(0).at(158).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_boss_active_rewards(v);
		}
	}
	if (!res.at(0).at(159).is_null())
	{
		obj->set_boss_num(res.at(0).at(159));
	}
	if (!res.at(0).at(160).is_null())
	{
		obj->set_boss_last_time(res.at(0).at(160));
	}
	if (!res.at(0).at(161).is_null())
	{
		obj->set_boss_hit_time(res.at(0).at(161));
	}
	if (!res.at(0).at(162).is_null())
	{
		obj->set_boss_player_level(res.at(0).at(162));
	}
	if (!res.at(0).at(163).is_null())
	{
		obj->set_hbb_refresh_num(res.at(0).at(163));
	}
	if (!res.at(0).at(164).is_null())
	{
		obj->set_hbb_num(res.at(0).at(164));
	}
	if (!res.at(0).at(165).is_null())
	{
		obj->set_hbb_finish_num(res.at(0).at(165));
	}
	if (!res.at(0).at(166).is_null())
	{
		std::string temp(res.at(0).at(166).data(), res.at(0).at(166).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_hbb_class_ids(v);
		}
	}
	if (!res.at(0).at(167).is_null())
	{
		obj->set_ttt_dead(res.at(0).at(167));
	}
	if (!res.at(0).at(168).is_null())
	{
		obj->set_ttt_star(res.at(0).at(168));
	}
	if (!res.at(0).at(169).is_null())
	{
		std::string temp(res.at(0).at(169).data(), res.at(0).at(169).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_ttt_reward_ids(v);
		}
	}
	if (!res.at(0).at(170).is_null())
	{
		std::string temp(res.at(0).at(170).data(), res.at(0).at(170).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_ttt_cur_stars(v);
		}
	}
	if (!res.at(0).at(171).is_null())
	{
		std::string temp(res.at(0).at(171).data(), res.at(0).at(171).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_ttt_last_stars(v);
		}
	}
	if (!res.at(0).at(172).is_null())
	{
		std::string temp(res.at(0).at(172).data(), res.at(0).at(172).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_ttt_cur_reward_ids(v);
		}
	}
	if (!res.at(0).at(173).is_null())
	{
		obj->set_ttt_can_reward(res.at(0).at(173));
	}
	if (!res.at(0).at(174).is_null())
	{
		obj->set_ttt_cz_num(res.at(0).at(174));
	}
	if (!res.at(0).at(175).is_null())
	{
		obj->set_ttt_mibao(res.at(0).at(175));
	}
	if (!res.at(0).at(176).is_null())
	{
		obj->set_random_event_num(res.at(0).at(176));
	}
	if (!res.at(0).at(177).is_null())
	{
		obj->set_random_event_time(res.at(0).at(177));
	}
	if (!res.at(0).at(178).is_null())
	{
		obj->set_random_event_id(res.at(0).at(178));
	}
	if (!res.at(0).at(179).is_null())
	{
		obj->set_guild(res.at(0).at(179));
	}
	if (!res.at(0).at(180).is_null())
	{
		obj->set_last_leave_guild_time(res.at(0).at(180));
	}
	if (!res.at(0).at(181).is_null())
	{
		std::string temp(res.at(0).at(181).data(), res.at(0).at(181).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_skill_ids(v);
		}
	}
	if (!res.at(0).at(182).is_null())
	{
		std::string temp(res.at(0).at(182).data(), res.at(0).at(182).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_skill_levels(v);
		}
	}
	if (!res.at(0).at(183).is_null())
	{
		std::string temp(res.at(0).at(183).data(), res.at(0).at(183).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_guild_applys(v);
		}
	}
	if (!res.at(0).at(184).is_null())
	{
		std::string temp(res.at(0).at(184).data(), res.at(0).at(184).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_rewards(v);
		}
	}
	if (!res.at(0).at(185).is_null())
	{
		std::string temp(res.at(0).at(185).data(), res.at(0).at(185).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_shilians(v);
		}
	}
	if (!res.at(0).at(186).is_null())
	{
		obj->set_guild_num(res.at(0).at(186));
	}
	if (!res.at(0).at(187).is_null())
	{
		obj->set_guild_buy_num(res.at(0).at(187));
	}
	if (!res.at(0).at(188).is_null())
	{
		std::string temp(res.at(0).at(188).data(), res.at(0).at(188).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_honors(v);
		}
	}
	if (!res.at(0).at(189).is_null())
	{
		obj->set_social_shou_num(res.at(0).at(189));
	}
	if (!res.at(0).at(190).is_null())
	{
		obj->set_guild_sign_time(res.at(0).at(190));
	}
	if (!res.at(0).at(191).is_null())
	{
		obj->set_guild_sign_flag(res.at(0).at(191));
	}
	if (!res.at(0).at(192).is_null())
	{
		obj->set_guild_red_num(res.at(0).at(192));
	}
	if (!res.at(0).at(193).is_null())
	{
		obj->set_guild_red_num1(res.at(0).at(193));
	}
	if (!res.at(0).at(194).is_null())
	{
		obj->set_guild_deliver_jewel(res.at(0).at(194));
	}
	if (!res.at(0).at(195).is_null())
	{
		obj->set_guild_rob_jewel(res.at(0).at(195));
	}
	if (!res.at(0).at(196).is_null())
	{
		obj->set_guild_lnum(res.at(0).at(196));
	}
	if (!res.at(0).at(197).is_null())
	{
		obj->set_guild_ltime(res.at(0).at(197));
	}
	if (!res.at(0).at(198).is_null())
	{
		obj->set_guild_pvp_num(res.at(0).at(198));
	}
	if (!res.at(0).at(199).is_null())
	{
		obj->set_guild_pvp_buy_num(res.at(0).at(199));
	}
	if (!res.at(0).at(200).is_null())
	{
		std::string temp(res.at(0).at(200).data(), res.at(0).at(200).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_dress_ids(v);
		}
	}
	if (!res.at(0).at(201).is_null())
	{
		std::string temp(res.at(0).at(201).data(), res.at(0).at(201).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_dress_on_ids(v);
		}
	}
	if (!res.at(0).at(202).is_null())
	{
		std::string temp(res.at(0).at(202).data(), res.at(0).at(202).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_dress_id_bags(v);
		}
	}
	if (!res.at(0).at(203).is_null())
	{
		obj->set_dress_tuzhi(res.at(0).at(203));
	}
	if (!res.at(0).at(204).is_null())
	{
		std::string temp(res.at(0).at(204).data(), res.at(0).at(204).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_dress_achieves(v);
		}
	}
	if (!res.at(0).at(205).is_null())
	{
		obj->set_dress_flag(res.at(0).at(205));
	}
	if (!res.at(0).at(206).is_null())
	{
		std::string temp(res.at(0).at(206).data(), res.at(0).at(206).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huiyi_jihuos(v);
		}
	}
	if (!res.at(0).at(207).is_null())
	{
		obj->set_huiyi_shoujidu(res.at(0).at(207));
	}
	if (!res.at(0).at(208).is_null())
	{
		std::string temp(res.at(0).at(208).data(), res.at(0).at(208).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huiyi_zhanpus(v);
		}
	}
	if (!res.at(0).at(209).is_null())
	{
		obj->set_huiyi_zhanpu_flag(res.at(0).at(209));
	}
	if (!res.at(0).at(210).is_null())
	{
		obj->set_huiyi_zhanpu_num(res.at(0).at(210));
	}
	if (!res.at(0).at(211).is_null())
	{
		obj->set_huiyi_gaiyun_num(res.at(0).at(211));
	}
	if (!res.at(0).at(212).is_null())
	{
		std::string temp(res.at(0).at(212).data(), res.at(0).at(212).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huiyi_shop_ids(v);
		}
	}
	if (!res.at(0).at(213).is_null())
	{
		std::string temp(res.at(0).at(213).data(), res.at(0).at(213).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huiyi_shop_nums(v);
		}
	}
	if (!res.at(0).at(214).is_null())
	{
		obj->set_huiyi_fan_num(res.at(0).at(214));
	}
	if (!res.at(0).at(215).is_null())
	{
		std::string temp(res.at(0).at(215).data(), res.at(0).at(215).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huiyi_jihuo_starts(v);
		}
	}
	if (!res.at(0).at(216).is_null())
	{
		obj->set_huiyi_shoujidu_top(res.at(0).at(216));
	}
	if (!res.at(0).at(217).is_null())
	{
		obj->set_treasure_protect_next_time(res.at(0).at(217));
	}
	if (!res.at(0).at(218).is_null())
	{
		obj->set_treasure_protect_cd_time(res.at(0).at(218));
	}
	if (!res.at(0).at(219).is_null())
	{
		std::string temp(res.at(0).at(219).data(), res.at(0).at(219).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_treasure_reports(v);
		}
	}
	if (!res.at(0).at(220).is_null())
	{
		obj->set_treasure_first(res.at(0).at(220));
	}
	if (!res.at(0).at(221).is_null())
	{
		std::string temp(res.at(0).at(221).data(), res.at(0).at(221).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_treasure_hechengs(v);
		}
	}
	if (!res.at(0).at(222).is_null())
	{
		std::string temp(res.at(0).at(222).data(), res.at(0).at(222).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_yh_roles(v);
		}
	}
	if (!res.at(0).at(223).is_null())
	{
		obj->set_yh_time(res.at(0).at(223));
	}
	if (!res.at(0).at(224).is_null())
	{
		obj->set_yh_hour(res.at(0).at(224));
	}
	if (!res.at(0).at(225).is_null())
	{
		obj->set_yh_update_time(res.at(0).at(225));
	}
	if (!res.at(0).at(226).is_null())
	{
		std::string temp(res.at(0).at(226).data(), res.at(0).at(226).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_qiyu_mission(v);
		}
	}
	if (!res.at(0).at(227).is_null())
	{
		std::string temp(res.at(0).at(227).data(), res.at(0).at(227).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_qiyu_hard(v);
		}
	}
	if (!res.at(0).at(228).is_null())
	{
		std::string temp(res.at(0).at(228).data(), res.at(0).at(228).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_qiyu_suc(v);
		}
	}
	if (!res.at(0).at(229).is_null())
	{
		obj->set_qiyu_last_time(res.at(0).at(229));
	}
	if (!res.at(0).at(230).is_null())
	{
		std::string temp(res.at(0).at(230).data(), res.at(0).at(230).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_sport_shop_rewards(v);
		}
	}
	if (!res.at(0).at(231).is_null())
	{
		std::string temp(res.at(0).at(231).data(), res.at(0).at(231).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_ttt_shop_rewards(v);
		}
	}
	if (!res.at(0).at(232).is_null())
	{
		std::string temp(res.at(0).at(232).data(), res.at(0).at(232).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_mw_shop_rewards(v);
		}
	}
	if (!res.at(0).at(233).is_null())
	{
		std::string temp(res.at(0).at(233).data(), res.at(0).at(233).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_shop_rewards(v);
		}
	}
	if (!res.at(0).at(234).is_null())
	{
		std::string temp(res.at(0).at(234).data(), res.at(0).at(234).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_shop_ex_rewards(v);
		}
	}
	if (!res.at(0).at(235).is_null())
	{
		std::string temp(res.at(0).at(235).data(), res.at(0).at(235).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_guild_red_rewards(v);
		}
	}
	if (!res.at(0).at(236).is_null())
	{
		obj->set_pvp_num(res.at(0).at(236));
	}
	if (!res.at(0).at(237).is_null())
	{
		obj->set_pvp_refresh_num(res.at(0).at(237));
	}
	if (!res.at(0).at(238).is_null())
	{
		obj->set_pvp_buy_num(res.at(0).at(238));
	}
	if (!res.at(0).at(239).is_null())
	{
		obj->set_pvp_total(res.at(0).at(239));
	}
	if (!res.at(0).at(240).is_null())
	{
		obj->set_pvp_hit(res.at(0).at(240));
	}
	if (!res.at(0).at(241).is_null())
	{
		std::string temp(res.at(0).at(241).data(), res.at(0).at(241).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_pvp_hit_ids(v);
		}
	}
	if (!res.at(0).at(242).is_null())
	{
		std::string temp(res.at(0).at(242).data(), res.at(0).at(242).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_by_shops(v);
		}
	}
	if (!res.at(0).at(243).is_null())
	{
		std::string temp(res.at(0).at(243).data(), res.at(0).at(243).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_by_nums(v);
		}
	}
	if (!res.at(0).at(244).is_null())
	{
		std::string temp(res.at(0).at(244).data(), res.at(0).at(244).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_by_rewards(v);
		}
	}
	if (!res.at(0).at(245).is_null())
	{
		obj->set_by_reward_num(res.at(0).at(245));
	}
	if (!res.at(0).at(246).is_null())
	{
		obj->set_by_reward_buy(res.at(0).at(246));
	}
	if (!res.at(0).at(247).is_null())
	{
		obj->set_by_point(res.at(0).at(247));
	}
	if (!res.at(0).at(248).is_null())
	{
		obj->set_ds_reward_buy(res.at(0).at(248));
	}
	if (!res.at(0).at(249).is_null())
	{
		obj->set_ds_reward_num(res.at(0).at(249));
	}
	if (!res.at(0).at(250).is_null())
	{
		obj->set_ds_point(res.at(0).at(250));
	}
	if (!res.at(0).at(251).is_null())
	{
		obj->set_ds_duanwei(res.at(0).at(251));
	}
	if (!res.at(0).at(252).is_null())
	{
		obj->set_ds_hit(res.at(0).at(252));
	}
	if (!res.at(0).at(253).is_null())
	{
		std::string temp(res.at(0).at(253).data(), res.at(0).at(253).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_ds_hit_ids(v);
		}
	}
	if (!res.at(0).at(254).is_null())
	{
		std::string temp(res.at(0).at(254).data(), res.at(0).at(254).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_pttq_reward(v);
		}
	}
	if (!res.at(0).at(255).is_null())
	{
		std::string temp(res.at(0).at(255).data(), res.at(0).at(255).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_kaifu_finish_ids(v);
		}
	}
	if (!res.at(0).at(256).is_null())
	{
		obj->set_huodong_czjh_buy(res.at(0).at(256));
	}
	if (!res.at(0).at(257).is_null())
	{
		obj->set_huodong_czjh_index(res.at(0).at(257));
	}
	if (!res.at(0).at(258).is_null())
	{
		obj->set_huodong_vip_libao(res.at(0).at(258));
	}
	if (!res.at(0).at(259).is_null())
	{
		std::string temp(res.at(0).at(259).data(), res.at(0).at(259).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_week_libao(v);
		}
	}
	if (!res.at(0).at(260).is_null())
	{
		obj->set_huodong_kaifu_ptfb(res.at(0).at(260));
	}
	if (!res.at(0).at(261).is_null())
	{
		obj->set_huodong_kaifu_jyfb(res.at(0).at(261));
	}
	if (!res.at(0).at(262).is_null())
	{
		obj->set_huodong_kaifu_dbfb(res.at(0).at(262));
	}
	if (!res.at(0).at(263).is_null())
	{
		obj->set_huodong_kaifu_jjc(res.at(0).at(263));
	}
	if (!res.at(0).at(264).is_null())
	{
		std::string temp(res.at(0).at(264).data(), res.at(0).at(264).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_kaifu_dbcz(v);
		}
	}
	if (!res.at(0).at(265).is_null())
	{
		obj->set_huodong_czjh_index1(res.at(0).at(265));
	}
	if (!res.at(0).at(266).is_null())
	{
		obj->set_huodong_yxhg_tap(res.at(0).at(266));
	}
	if (!res.at(0).at(267).is_null())
	{
		obj->set_huodong_yxhg_rmb(res.at(0).at(267));
	}
	if (!res.at(0).at(268).is_null())
	{
		std::string temp(res.at(0).at(268).data(), res.at(0).at(268).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_yxhg_buzhu_id(v);
		}
	}
	if (!res.at(0).at(269).is_null())
	{
		obj->set_huodong_yxhg_buzhu_day(res.at(0).at(269));
	}
	if (!res.at(0).at(270).is_null())
	{
		std::string temp(res.at(0).at(270).data(), res.at(0).at(270).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_yxhg_fuli_id(v);
		}
	}
	if (!res.at(0).at(271).is_null())
	{
		std::string temp(res.at(0).at(271).data(), res.at(0).at(271).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_yxhg_fuli_num(v);
		}
	}
	if (!res.at(0).at(272).is_null())
	{
		std::string temp(res.at(0).at(272).data(), res.at(0).at(272).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_huodong_yxhg_haoli_id(v);
		}
	}
	if (!res.at(0).at(273).is_null())
	{
		obj->set_huodong_yxhg_end_time(res.at(0).at(273));
	}
	if (!res.at(0).at(274).is_null())
	{
		obj->set_huodong_yxhg_time(res.at(0).at(274));
	}
	if (!res.at(0).at(275).is_null())
	{
		obj->set_gag_time(res.at(0).at(275));
	}
	if (!res.at(0).at(276).is_null())
	{
		obj->set_google_stat(res.at(0).at(276));
	}
	return 0;
}

int Sqlplayer_t::update(mysqlpp::Query& query)
{
	dhc::player_t *obj = (dhc::player_t *)data_;
	query << "UPDATE player_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "serverid=" << mysqlpp::quote << obj->serverid();
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "birth_time=" << boost::lexical_cast<std::string>(obj->birth_time());
	query << ",";
	query << "last_daily_time=" << boost::lexical_cast<std::string>(obj->last_daily_time());
	query << ",";
	query << "last_week_time=" << boost::lexical_cast<std::string>(obj->last_week_time());
	query << ",";
	query << "last_month_time=" << boost::lexical_cast<std::string>(obj->last_month_time());
	query << ",";
	query << "last_tili_time=" << boost::lexical_cast<std::string>(obj->last_tili_time());
	query << ",";
	query << "last_energy_time=" << boost::lexical_cast<std::string>(obj->last_energy_time());
	query << ",";
	query << "zsname=" << mysqlpp::quote << obj->zsname();
	query << ",";
	query << "last_login_time=" << boost::lexical_cast<std::string>(obj->last_login_time());
	query << ",";
	query << "last_check_time=" << boost::lexical_cast<std::string>(obj->last_check_time());
	query << ",";
	query << "last_fight_time=" << boost::lexical_cast<std::string>(obj->last_fight_time());
	query << ",";
	query << "change_name_num=" << boost::lexical_cast<std::string>(obj->change_name_num());
	query << ",";
	query << "change_nalflag_num=" << boost::lexical_cast<std::string>(obj->change_nalflag_num());
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());
	query << ",";
	query << "gold=" << boost::lexical_cast<std::string>(obj->gold());
	query << ",";
	query << "jewel=" << boost::lexical_cast<std::string>(obj->jewel());
	query << ",";
	query << "tili=" << boost::lexical_cast<std::string>(obj->tili());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "exp=" << boost::lexical_cast<std::string>(obj->exp());
	query << ",";
	query << "jjc_point=" << boost::lexical_cast<std::string>(obj->jjc_point());
	query << ",";
	query << "mw_point=" << boost::lexical_cast<std::string>(obj->mw_point());
	query << ",";
	query << "equip_kc_num=" << boost::lexical_cast<std::string>(obj->equip_kc_num());
	query << ",";
	query << "bf=" << boost::lexical_cast<std::string>(obj->bf());
	query << ",";
	query << "yuanli=" << boost::lexical_cast<std::string>(obj->yuanli());
	query << ",";
	query << "contribution=" << boost::lexical_cast<std::string>(obj->contribution());
	query << ",";
	query << "powder=" << boost::lexical_cast<std::string>(obj->powder());
	query << ",";
	query << "energy=" << boost::lexical_cast<std::string>(obj->energy());
	query << ",";
	query << "medal_point=" << boost::lexical_cast<std::string>(obj->medal_point());
	query << ",";
	query << "luck_point=" << boost::lexical_cast<std::string>(obj->luck_point());
	query << ",";
	query << "huiyi_point=" << boost::lexical_cast<std::string>(obj->huiyi_point());
	query << ",";
	query << "lieren_point=" << boost::lexical_cast<std::string>(obj->lieren_point());
	query << ",";
	query << "bf_max=" << boost::lexical_cast<std::string>(obj->bf_max());
	query << ",";
	query << "bingjing=" << boost::lexical_cast<std::string>(obj->bingjing());
	query << ",";
	query << "xinpian=" << boost::lexical_cast<std::string>(obj->xinpian());
	query << ",";
	query << "youqingdian=" << boost::lexical_cast<std::string>(obj->youqingdian());
	query << ",";
	{
		uint32_t size = obj->roles_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->roles(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "roles=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->equips_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->equips(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "equips=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->item_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->item_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "item_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->item_amount_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->item_amount(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "item_amount=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->role_template_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->role_template_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "role_template_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->treasures_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->treasures(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "treasures=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "treasure_kc_num=" << boost::lexical_cast<std::string>(obj->treasure_kc_num());
	query << ",";
	{
		uint32_t size = obj->chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "chenghao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->chengchao_time_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->chengchao_time(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "chengchao_time=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "chenghao_on=" << boost::lexical_cast<std::string>(obj->chenghao_on());
	query << ",";
	{
		uint32_t size = obj->pets_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->pets(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "pets=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pet_on=" << boost::lexical_cast<std::string>(obj->pet_on());
	query << ",";
	query << "mission=" << boost::lexical_cast<std::string>(obj->mission());
	query << ",";
	query << "mission_jy=" << boost::lexical_cast<std::string>(obj->mission_jy());
	query << ",";
	{
		uint32_t size = obj->mission_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_star_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_star(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_star=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_cishu_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_cishu_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_cishu_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_cishu_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_cishu(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_cishu=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_goumai_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_goumai_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_goumai_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mission_goumai_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_goumai(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_goumai=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->map_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->map_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "map_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->map_star_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->map_star(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "map_star=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->map_reward_get_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->map_reward_get(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "map_reward_get=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "yb_finish_num=" << boost::lexical_cast<std::string>(obj->yb_finish_num());
	query << ",";
	{
		uint32_t size = obj->ybq_guids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->ybq_guids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "ybq_guids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "yb_type=" << boost::lexical_cast<std::string>(obj->yb_type());
	query << ",";
	query << "yb_level=" << boost::lexical_cast<std::string>(obj->yb_level());
	query << ",";
	query << "yb_start_time=" << boost::lexical_cast<std::string>(obj->yb_start_time());
	query << ",";
	query << "yb_jiasu_time=" << boost::lexical_cast<std::string>(obj->yb_jiasu_time());
	query << ",";
	query << "yb_refresh_num=" << boost::lexical_cast<std::string>(obj->yb_refresh_num());
	query << ",";
	query << "yb_gw_type=" << boost::lexical_cast<std::string>(obj->yb_gw_type());
	query << ",";
	query << "yb_byb_num=" << boost::lexical_cast<std::string>(obj->yb_byb_num());
	query << ",";
	query << "yb_per=" << boost::lexical_cast<std::string>(obj->yb_per());
	query << ",";
	query << "ybq_finish_num=" << boost::lexical_cast<std::string>(obj->ybq_finish_num());
	query << ",";
	query << "ybq_last_time=" << boost::lexical_cast<std::string>(obj->ybq_last_time());
	query << ",";
	query << "ore_finish_num=" << boost::lexical_cast<std::string>(obj->ore_finish_num());
	query << ",";
	query << "ore_nindex=" << boost::lexical_cast<std::string>(obj->ore_nindex());
	query << ",";
	query << "ore_last_time=" << boost::lexical_cast<std::string>(obj->ore_last_time());
	query << ",";
	{
		uint32_t size = obj->mission_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mission_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mission_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->zhenxing_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->zhenxing(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "zhenxing=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->duixing_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->duixing(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "duixing=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "duixing_id=" << boost::lexical_cast<std::string>(obj->duixing_id());
	query << ",";
	query << "duixing_level=" << boost::lexical_cast<std::string>(obj->duixing_level());
	query << ",";
	{
		uint32_t size = obj->houyuan_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->houyuan(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "houyuan=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guanghuan_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guanghuan(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guanghuan=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guanghuan_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guanghuan_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guanghuan_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "guanghuan_id=" << boost::lexical_cast<std::string>(obj->guanghuan_id());
	query << ",";
	query << "ck2_free_time=" << boost::lexical_cast<std::string>(obj->ck2_free_time());
	query << ",";
	{
		uint32_t size = obj->ck_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ck_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ck_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->finished_tasks_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->finished_tasks(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "finished_tasks=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pt_task_num=" << boost::lexical_cast<std::string>(obj->pt_task_num());
	query << ",";
	query << "jy_task_num=" << boost::lexical_cast<std::string>(obj->jy_task_num());
	query << ",";
	query << "sj_task_num=" << boost::lexical_cast<std::string>(obj->sj_task_num());
	query << ",";
	query << "jjie_task_num=" << boost::lexical_cast<std::string>(obj->jjie_task_num());
	query << ",";
	query << "qh_task_num=" << boost::lexical_cast<std::string>(obj->qh_task_num());
	query << ",";
	query << "jj_task_num=" << boost::lexical_cast<std::string>(obj->jj_task_num());
	query << ",";
	query << "boss_task_num=" << boost::lexical_cast<std::string>(obj->boss_task_num());
	query << ",";
	query << "hs_task_num=" << boost::lexical_cast<std::string>(obj->hs_task_num());
	query << ",";
	query << "ttt_task_num=" << boost::lexical_cast<std::string>(obj->ttt_task_num());
	query << ",";
	query << "bweh_task_num=" << boost::lexical_cast<std::string>(obj->bweh_task_num());
	query << ",";
	query << "bwjl_task_num=" << boost::lexical_cast<std::string>(obj->bwjl_task_num());
	query << ",";
	query << "jjcs_task_num=" << boost::lexical_cast<std::string>(obj->jjcs_task_num());
	query << ",";
	query << "yb_task_num=" << boost::lexical_cast<std::string>(obj->yb_task_num());
	query << ",";
	{
		uint32_t size = obj->shoppet_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shoppet_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shoppet_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shoppet_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shoppet_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shoppet_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shoppet_refresh_num=" << boost::lexical_cast<std::string>(obj->shoppet_refresh_num());
	query << ",";
	query << "shoppet_last_time=" << boost::lexical_cast<std::string>(obj->shoppet_last_time());
	query << ",";
	query << "shoppet_num=" << boost::lexical_cast<std::string>(obj->shoppet_num());
	query << ",";
	query << "shop1_refresh_num=" << boost::lexical_cast<std::string>(obj->shop1_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop1_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop1_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop1_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop1_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop1_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop1_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop2_refresh_num=" << boost::lexical_cast<std::string>(obj->shop2_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop2_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop2_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop2_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop2_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop2_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop2_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop3_refresh_num=" << boost::lexical_cast<std::string>(obj->shop3_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop3_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop3_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop3_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop3_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop3_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop3_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop_last_time=" << boost::lexical_cast<std::string>(obj->shop_last_time());
	query << ",";
	{
		uint32_t size = obj->shop_xg_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_xg_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_xg_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop_xg_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop_xg_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop_xg_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "shop_refresh_num=" << boost::lexical_cast<std::string>(obj->shop_refresh_num());
	query << ",";
	query << "shop_buy_num=" << boost::lexical_cast<std::string>(obj->shop_buy_num());
	query << ",";
	query << "shop4_refresh_num=" << boost::lexical_cast<std::string>(obj->shop4_refresh_num());
	query << ",";
	{
		uint32_t size = obj->shop4_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint32_t v = obj->shop4_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint32_t));
		}
		query << "shop4_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->shop4_sell_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->shop4_sell(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "shop4_sell=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_shop_last_time=" << boost::lexical_cast<std::string>(obj->huiyi_shop_last_time());
	query << ",";
	{
		uint32_t size = obj->active_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->active_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->active_reward_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_reward(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_reward=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "active_score=" << boost::lexical_cast<std::string>(obj->active_score());
	query << ",";
	{
		uint32_t size = obj->active_score_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->active_score_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "active_score_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->sports_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->sports(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "sports=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "max_rank=" << boost::lexical_cast<std::string>(obj->max_rank());
	query << ",";
	query << "zhouka_time=" << boost::lexical_cast<std::string>(obj->zhouka_time());
	query << ",";
	query << "yueka_time=" << boost::lexical_cast<std::string>(obj->yueka_time());
	query << ",";
	query << "total_recharge=" << boost::lexical_cast<std::string>(obj->total_recharge());
	query << ",";
	query << "vip=" << boost::lexical_cast<std::string>(obj->vip());
	query << ",";
	query << "vip_exp=" << boost::lexical_cast<std::string>(obj->vip_exp());
	query << ",";
	{
		uint32_t size = obj->recharge_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->recharge_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "recharge_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->vip_reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->vip_reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "vip_reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dj_num=" << boost::lexical_cast<std::string>(obj->dj_num());
	query << ",";
	query << "tili_reward=" << boost::lexical_cast<std::string>(obj->tili_reward());
	query << ",";
	query << "first_reward=" << boost::lexical_cast<std::string>(obj->first_reward());
	query << ",";
	query << "online_reward_index=" << boost::lexical_cast<std::string>(obj->online_reward_index());
	query << ",";
	query << "online_reward_time=" << boost::lexical_cast<std::string>(obj->online_reward_time());
	query << ",";
	query << "daily_sign_index=" << boost::lexical_cast<std::string>(obj->daily_sign_index());
	query << ",";
	query << "daily_sign_reward=" << boost::lexical_cast<std::string>(obj->daily_sign_reward());
	query << ",";
	query << "daily_sign_num=" << boost::lexical_cast<std::string>(obj->daily_sign_num());
	query << ",";
	{
		uint32_t size = obj->libao_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->libao_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "libao_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->rdz_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rdz_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rdz_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->rdz_counts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->rdz_counts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "rdz_counts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "guest=" << boost::lexical_cast<std::string>(obj->guest());
	query << ",";
	query << "total_spend=" << boost::lexical_cast<std::string>(obj->total_spend());
	query << ",";
	query << "daily_sign_flag=" << boost::lexical_cast<std::string>(obj->daily_sign_flag());
	query << ",";
	query << "huiyi_chou_num=" << boost::lexical_cast<std::string>(obj->huiyi_chou_num());
	query << ",";
	query << "boss_leiji_damage=" << boost::lexical_cast<std::string>(obj->boss_leiji_damage());
	query << ",";
	query << "boss_max_damage=" << boost::lexical_cast<std::string>(obj->boss_max_damage());
	query << ",";
	query << "boss_max_rank=" << boost::lexical_cast<std::string>(obj->boss_max_rank());
	query << ",";
	{
		uint32_t size = obj->boss_active_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->boss_active_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "boss_active_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->boss_active_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int64_t v = obj->boss_active_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int64_t));
		}
		query << "boss_active_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->boss_active_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->boss_active_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "boss_active_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "boss_num=" << boost::lexical_cast<std::string>(obj->boss_num());
	query << ",";
	query << "boss_last_time=" << boost::lexical_cast<std::string>(obj->boss_last_time());
	query << ",";
	query << "boss_hit_time=" << boost::lexical_cast<std::string>(obj->boss_hit_time());
	query << ",";
	query << "boss_player_level=" << boost::lexical_cast<std::string>(obj->boss_player_level());
	query << ",";
	query << "hbb_refresh_num=" << boost::lexical_cast<std::string>(obj->hbb_refresh_num());
	query << ",";
	query << "hbb_num=" << boost::lexical_cast<std::string>(obj->hbb_num());
	query << ",";
	query << "hbb_finish_num=" << boost::lexical_cast<std::string>(obj->hbb_finish_num());
	query << ",";
	{
		uint32_t size = obj->hbb_class_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->hbb_class_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "hbb_class_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "ttt_dead=" << boost::lexical_cast<std::string>(obj->ttt_dead());
	query << ",";
	query << "ttt_star=" << boost::lexical_cast<std::string>(obj->ttt_star());
	query << ",";
	{
		uint32_t size = obj->ttt_reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_cur_stars_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_cur_stars(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_cur_stars=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_last_stars_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_last_stars(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_last_stars=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_cur_reward_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_cur_reward_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_cur_reward_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "ttt_can_reward=" << boost::lexical_cast<std::string>(obj->ttt_can_reward());
	query << ",";
	query << "ttt_cz_num=" << boost::lexical_cast<std::string>(obj->ttt_cz_num());
	query << ",";
	query << "ttt_mibao=" << boost::lexical_cast<std::string>(obj->ttt_mibao());
	query << ",";
	query << "random_event_num=" << boost::lexical_cast<std::string>(obj->random_event_num());
	query << ",";
	query << "random_event_time=" << boost::lexical_cast<std::string>(obj->random_event_time());
	query << ",";
	query << "random_event_id=" << boost::lexical_cast<std::string>(obj->random_event_id());
	query << ",";
	query << "guild=" << boost::lexical_cast<std::string>(obj->guild());
	query << ",";
	query << "last_leave_guild_time=" << boost::lexical_cast<std::string>(obj->last_leave_guild_time());
	query << ",";
	{
		uint32_t size = obj->guild_skill_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_skill_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_skill_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_skill_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_skill_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_skill_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_applys_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->guild_applys(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "guild_applys=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_shilians_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_shilians(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_shilians=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "guild_num=" << boost::lexical_cast<std::string>(obj->guild_num());
	query << ",";
	query << "guild_buy_num=" << boost::lexical_cast<std::string>(obj->guild_buy_num());
	query << ",";
	{
		uint32_t size = obj->guild_honors_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_honors(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_honors=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "social_shou_num=" << boost::lexical_cast<std::string>(obj->social_shou_num());
	query << ",";
	query << "guild_sign_time=" << boost::lexical_cast<std::string>(obj->guild_sign_time());
	query << ",";
	query << "guild_sign_flag=" << boost::lexical_cast<std::string>(obj->guild_sign_flag());
	query << ",";
	query << "guild_red_num=" << boost::lexical_cast<std::string>(obj->guild_red_num());
	query << ",";
	query << "guild_red_num1=" << boost::lexical_cast<std::string>(obj->guild_red_num1());
	query << ",";
	query << "guild_deliver_jewel=" << boost::lexical_cast<std::string>(obj->guild_deliver_jewel());
	query << ",";
	query << "guild_rob_jewel=" << boost::lexical_cast<std::string>(obj->guild_rob_jewel());
	query << ",";
	query << "guild_lnum=" << boost::lexical_cast<std::string>(obj->guild_lnum());
	query << ",";
	query << "guild_ltime=" << boost::lexical_cast<std::string>(obj->guild_ltime());
	query << ",";
	query << "guild_pvp_num=" << boost::lexical_cast<std::string>(obj->guild_pvp_num());
	query << ",";
	query << "guild_pvp_buy_num=" << boost::lexical_cast<std::string>(obj->guild_pvp_buy_num());
	query << ",";
	{
		uint32_t size = obj->dress_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->dress_on_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_on_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_on_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->dress_id_bags_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_id_bags(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_id_bags=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dress_tuzhi=" << boost::lexical_cast<std::string>(obj->dress_tuzhi());
	query << ",";
	{
		uint32_t size = obj->dress_achieves_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_achieves(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_achieves=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dress_flag=" << boost::lexical_cast<std::string>(obj->dress_flag());
	query << ",";
	{
		uint32_t size = obj->huiyi_jihuos_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_jihuos(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_jihuos=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_shoujidu=" << boost::lexical_cast<std::string>(obj->huiyi_shoujidu());
	query << ",";
	{
		uint32_t size = obj->huiyi_zhanpus_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_zhanpus(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_zhanpus=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_zhanpu_flag=" << boost::lexical_cast<std::string>(obj->huiyi_zhanpu_flag());
	query << ",";
	query << "huiyi_zhanpu_num=" << boost::lexical_cast<std::string>(obj->huiyi_zhanpu_num());
	query << ",";
	query << "huiyi_gaiyun_num=" << boost::lexical_cast<std::string>(obj->huiyi_gaiyun_num());
	query << ",";
	{
		uint32_t size = obj->huiyi_shop_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_shop_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_shop_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huiyi_shop_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_shop_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_shop_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_fan_num=" << boost::lexical_cast<std::string>(obj->huiyi_fan_num());
	query << ",";
	{
		uint32_t size = obj->huiyi_jihuo_starts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huiyi_jihuo_starts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huiyi_jihuo_starts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huiyi_shoujidu_top=" << boost::lexical_cast<std::string>(obj->huiyi_shoujidu_top());
	query << ",";
	query << "treasure_protect_next_time=" << boost::lexical_cast<std::string>(obj->treasure_protect_next_time());
	query << ",";
	query << "treasure_protect_cd_time=" << boost::lexical_cast<std::string>(obj->treasure_protect_cd_time());
	query << ",";
	{
		uint32_t size = obj->treasure_reports_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->treasure_reports(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "treasure_reports=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "treasure_first=" << boost::lexical_cast<std::string>(obj->treasure_first());
	query << ",";
	{
		uint32_t size = obj->treasure_hechengs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->treasure_hechengs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "treasure_hechengs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->yh_roles_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->yh_roles(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "yh_roles=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "yh_time=" << boost::lexical_cast<std::string>(obj->yh_time());
	query << ",";
	query << "yh_hour=" << boost::lexical_cast<std::string>(obj->yh_hour());
	query << ",";
	query << "yh_update_time=" << boost::lexical_cast<std::string>(obj->yh_update_time());
	query << ",";
	{
		uint32_t size = obj->qiyu_mission_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->qiyu_mission(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "qiyu_mission=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->qiyu_hard_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->qiyu_hard(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "qiyu_hard=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->qiyu_suc_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->qiyu_suc(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "qiyu_suc=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "qiyu_last_time=" << boost::lexical_cast<std::string>(obj->qiyu_last_time());
	query << ",";
	{
		uint32_t size = obj->sport_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->sport_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "sport_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->ttt_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ttt_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ttt_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->mw_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->mw_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "mw_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_shop_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_shop_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_shop_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_shop_ex_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_shop_ex_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_shop_ex_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->guild_red_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->guild_red_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "guild_red_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pvp_num=" << boost::lexical_cast<std::string>(obj->pvp_num());
	query << ",";
	query << "pvp_refresh_num=" << boost::lexical_cast<std::string>(obj->pvp_refresh_num());
	query << ",";
	query << "pvp_buy_num=" << boost::lexical_cast<std::string>(obj->pvp_buy_num());
	query << ",";
	query << "pvp_total=" << boost::lexical_cast<std::string>(obj->pvp_total());
	query << ",";
	query << "pvp_hit=" << boost::lexical_cast<std::string>(obj->pvp_hit());
	query << ",";
	{
		uint32_t size = obj->pvp_hit_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->pvp_hit_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "pvp_hit_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->by_shops_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->by_shops(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "by_shops=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->by_nums_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->by_nums(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "by_nums=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->by_rewards_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->by_rewards(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "by_rewards=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "by_reward_num=" << boost::lexical_cast<std::string>(obj->by_reward_num());
	query << ",";
	query << "by_reward_buy=" << boost::lexical_cast<std::string>(obj->by_reward_buy());
	query << ",";
	query << "by_point=" << boost::lexical_cast<std::string>(obj->by_point());
	query << ",";
	query << "ds_reward_buy=" << boost::lexical_cast<std::string>(obj->ds_reward_buy());
	query << ",";
	query << "ds_reward_num=" << boost::lexical_cast<std::string>(obj->ds_reward_num());
	query << ",";
	query << "ds_point=" << boost::lexical_cast<std::string>(obj->ds_point());
	query << ",";
	query << "ds_duanwei=" << boost::lexical_cast<std::string>(obj->ds_duanwei());
	query << ",";
	query << "ds_hit=" << boost::lexical_cast<std::string>(obj->ds_hit());
	query << ",";
	{
		uint32_t size = obj->ds_hit_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->ds_hit_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "ds_hit_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_pttq_reward_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_pttq_reward(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_pttq_reward=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_kaifu_finish_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_kaifu_finish_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_kaifu_finish_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_czjh_buy=" << boost::lexical_cast<std::string>(obj->huodong_czjh_buy());
	query << ",";
	query << "huodong_czjh_index=" << boost::lexical_cast<std::string>(obj->huodong_czjh_index());
	query << ",";
	query << "huodong_vip_libao=" << boost::lexical_cast<std::string>(obj->huodong_vip_libao());
	query << ",";
	{
		uint32_t size = obj->huodong_week_libao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_week_libao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_week_libao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_kaifu_ptfb=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_ptfb());
	query << ",";
	query << "huodong_kaifu_jyfb=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_jyfb());
	query << ",";
	query << "huodong_kaifu_dbfb=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_dbfb());
	query << ",";
	query << "huodong_kaifu_jjc=" << boost::lexical_cast<std::string>(obj->huodong_kaifu_jjc());
	query << ",";
	{
		uint32_t size = obj->huodong_kaifu_dbcz_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_kaifu_dbcz(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_kaifu_dbcz=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_czjh_index1=" << boost::lexical_cast<std::string>(obj->huodong_czjh_index1());
	query << ",";
	query << "huodong_yxhg_tap=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_tap());
	query << ",";
	query << "huodong_yxhg_rmb=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_rmb());
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_buzhu_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_buzhu_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_buzhu_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_yxhg_buzhu_day=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_buzhu_day());
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_fuli_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_fuli_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_fuli_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_fuli_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_fuli_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_fuli_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->huodong_yxhg_haoli_id_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->huodong_yxhg_haoli_id(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "huodong_yxhg_haoli_id=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "huodong_yxhg_end_time=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_end_time());
	query << ",";
	query << "huodong_yxhg_time=" << boost::lexical_cast<std::string>(obj->huodong_yxhg_time());
	query << ",";
	query << "gag_time=" << boost::lexical_cast<std::string>(obj->gag_time());
	query << ",";
	query << "google_stat=" << boost::lexical_cast<std::string>(obj->google_stat());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlplayer_t::remove(mysqlpp::Query& query)
{
	dhc::player_t *obj = (dhc::player_t *)data_;
	query << "DELETE FROM player_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlpost_t::insert(mysqlpp::Query& query)
{
	dhc::post_t *obj = (dhc::post_t *)data_;
	query << "INSERT INTO post_t SET ";
	query << "pid=" << boost::lexical_cast<std::string>(obj->pid());
	query << ",";
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

int Sqlpost_t::query(mysqlpp::Query& query)
{
	dhc::post_t *obj = (dhc::post_t *)data_;
	query << "SELECT * FROM post_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_pid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_receiver_guid(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_sender_date(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_title((std::string)res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_text((std::string)res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_sender_name((std::string)res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
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
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
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
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
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
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
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
	return 0;
}

int Sqlpost_t::update(mysqlpp::Query& query)
{
	dhc::post_t *obj = (dhc::post_t *)data_;
	query << "UPDATE post_t SET ";
	query << "pid=" << boost::lexical_cast<std::string>(obj->pid());
	query << ",";
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
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlpost_t::remove(mysqlpp::Query& query)
{
	dhc::post_t *obj = (dhc::post_t *)data_;
	query << "DELETE FROM post_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlrank_t::insert(mysqlpp::Query& query)
{
	dhc::rank_t *obj = (dhc::rank_t *)data_;
	query << "INSERT INTO rank_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bf_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bf(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bf=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_huiyi_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_huiyi(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_huiyi=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflag=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "reward_flag=" << boost::lexical_cast<std::string>(obj->reward_flag());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlrank_t::query(mysqlpp::Query& query)
{
	dhc::rank_t *obj = (dhc::rank_t *)data_;
	query << "SELECT * FROM rank_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		std::string temp(res.at(0).at(1).data(), res.at(0).at(1).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_player_guid(v);
		}
	}
	if (!res.at(0).at(2).is_null())
	{
		std::string temp(res.at(0).at(2).data(), res.at(0).at(2).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_player_name(buf.get(), len);
		}
	}
	if (!res.at(0).at(3).is_null())
	{
		std::string temp(res.at(0).at(3).data(), res.at(0).at(3).length());
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
	if (!res.at(0).at(4).is_null())
	{
		std::string temp(res.at(0).at(4).data(), res.at(0).at(4).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_bf(v);
		}
	}
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_value(v);
		}
	}
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
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
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
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
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
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
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_huiyi(v);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_chenghao(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_nalflag(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		obj->set_reward_flag(res.at(0).at(12));
	}
	return 0;
}

int Sqlrank_t::update(mysqlpp::Query& query)
{
	dhc::rank_t *obj = (dhc::rank_t *)data_;
	query << "UPDATE rank_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bf_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bf(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bf=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->value_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->value(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "value=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_huiyi_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_huiyi(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_huiyi=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_nalflag=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "reward_flag=" << boost::lexical_cast<std::string>(obj->reward_flag());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlrank_t::remove(mysqlpp::Query& query)
{
	dhc::rank_t *obj = (dhc::rank_t *)data_;
	query << "DELETE FROM rank_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlrob_t::insert(mysqlpp::Query& query)
{
	dhc::rob_t *obj = (dhc::rob_t *)data_;
	query << "INSERT INTO rob_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "username=" << mysqlpp::quote << obj->username();
	query << ",";
	query << "serverid=" << mysqlpp::quote << obj->serverid();
	query << ",";
	query << "extra=" << mysqlpp::quote << obj->extra();
	query << ",";
	query << "gm_level=" << boost::lexical_cast<std::string>(obj->gm_level());
	query << ",";
	query << "fenghao_time=" << boost::lexical_cast<std::string>(obj->fenghao_time());
	query << ",";
	query << "device=" << mysqlpp::quote << obj->device();
	query << ",";
	query << "version=" << boost::lexical_cast<std::string>(obj->version());
	query << ",";
	query << "last_device=" << mysqlpp::quote << obj->last_device();
	query << ",";
	query << "last_time=" << boost::lexical_cast<std::string>(obj->last_time());
	query << ",";
	query << "gag_time=" << boost::lexical_cast<std::string>(obj->gag_time());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlrob_t::query(mysqlpp::Query& query)
{
	dhc::rob_t *obj = (dhc::rob_t *)data_;
	query << "SELECT * FROM rob_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_type(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_username((std::string)res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_serverid((std::string)res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_extra((std::string)res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_gm_level(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_fenghao_time(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_device((std::string)res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_version(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_last_device((std::string)res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_last_time(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_gag_time(res.at(0).at(11));
	}
	return 0;
}

int Sqlrob_t::update(mysqlpp::Query& query)
{
	dhc::rob_t *obj = (dhc::rob_t *)data_;
	query << "UPDATE rob_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "username=" << mysqlpp::quote << obj->username();
	query << ",";
	query << "serverid=" << mysqlpp::quote << obj->serverid();
	query << ",";
	query << "extra=" << mysqlpp::quote << obj->extra();
	query << ",";
	query << "gm_level=" << boost::lexical_cast<std::string>(obj->gm_level());
	query << ",";
	query << "fenghao_time=" << boost::lexical_cast<std::string>(obj->fenghao_time());
	query << ",";
	query << "device=" << mysqlpp::quote << obj->device();
	query << ",";
	query << "version=" << boost::lexical_cast<std::string>(obj->version());
	query << ",";
	query << "last_device=" << mysqlpp::quote << obj->last_device();
	query << ",";
	query << "last_time=" << boost::lexical_cast<std::string>(obj->last_time());
	query << ",";
	query << "gag_time=" << boost::lexical_cast<std::string>(obj->gag_time());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlrob_t::remove(mysqlpp::Query& query)
{
	dhc::rob_t *obj = (dhc::rob_t *)data_;
	query << "DELETE FROM rob_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlrole_t::insert(mysqlpp::Query& query)
{
	dhc::role_t *obj = (dhc::role_t *)data_;
	query << "INSERT INTO role_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "jlevel=" << boost::lexical_cast<std::string>(obj->jlevel());
	query << ",";
	query << "glevel=" << boost::lexical_cast<std::string>(obj->glevel());
	query << ",";
	query << "pinzhi=" << boost::lexical_cast<std::string>(obj->pinzhi());
	query << ",";
	{
		uint32_t size = obj->jskill_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->jskill_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "jskill_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->dress_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dress_on_id=" << boost::lexical_cast<std::string>(obj->dress_on_id());
	query << ",";
	{
		uint32_t size = obj->zhuangbeis_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->zhuangbeis(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "zhuangbeis=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->treasures_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->treasures(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "treasures=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "xq=" << boost::lexical_cast<std::string>(obj->xq());
	query << ",";
	query << "xq_time=" << boost::lexical_cast<std::string>(obj->xq_time());
	query << ",";
	query << "bskill_level=" << boost::lexical_cast<std::string>(obj->bskill_level());
	query << ",";
	{
		uint32_t size = obj->bskill_counts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->bskill_counts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "bskill_counts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pet=" << boost::lexical_cast<std::string>(obj->pet());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlrole_t::query(mysqlpp::Query& query)
{
	dhc::role_t *obj = (dhc::role_t *)data_;
	query << "SELECT * FROM role_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_player_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_template_id(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_level(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_jlevel(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_glevel(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_pinzhi(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_jskill_level(v);
		}
	}
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_dress_ids(v);
		}
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_dress_on_id(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_zhuangbeis(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_treasures(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		obj->set_xq(res.at(0).at(12));
	}
	if (!res.at(0).at(13).is_null())
	{
		obj->set_xq_time(res.at(0).at(13));
	}
	if (!res.at(0).at(14).is_null())
	{
		obj->set_bskill_level(res.at(0).at(14));
	}
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_bskill_counts(v);
		}
	}
	if (!res.at(0).at(16).is_null())
	{
		obj->set_pet(res.at(0).at(16));
	}
	return 0;
}

int Sqlrole_t::update(mysqlpp::Query& query)
{
	dhc::role_t *obj = (dhc::role_t *)data_;
	query << "UPDATE role_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "jlevel=" << boost::lexical_cast<std::string>(obj->jlevel());
	query << ",";
	query << "glevel=" << boost::lexical_cast<std::string>(obj->glevel());
	query << ",";
	query << "pinzhi=" << boost::lexical_cast<std::string>(obj->pinzhi());
	query << ",";
	{
		uint32_t size = obj->jskill_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->jskill_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "jskill_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->dress_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->dress_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "dress_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "dress_on_id=" << boost::lexical_cast<std::string>(obj->dress_on_id());
	query << ",";
	{
		uint32_t size = obj->zhuangbeis_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->zhuangbeis(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "zhuangbeis=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->treasures_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->treasures(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "treasures=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "xq=" << boost::lexical_cast<std::string>(obj->xq());
	query << ",";
	query << "xq_time=" << boost::lexical_cast<std::string>(obj->xq_time());
	query << ",";
	query << "bskill_level=" << boost::lexical_cast<std::string>(obj->bskill_level());
	query << ",";
	{
		uint32_t size = obj->bskill_counts_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->bskill_counts(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "bskill_counts=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "pet=" << boost::lexical_cast<std::string>(obj->pet());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlrole_t::remove(mysqlpp::Query& query)
{
	dhc::role_t *obj = (dhc::role_t *)data_;
	query << "DELETE FROM role_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlsocial_t::insert(mysqlpp::Query& query)
{
	dhc::social_t *obj = (dhc::social_t *)data_;
	query << "INSERT INTO social_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "target_guid=" << boost::lexical_cast<std::string>(obj->target_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "bf=" << boost::lexical_cast<std::string>(obj->bf());
	query << ",";
	query << "can_shou=" << boost::lexical_cast<std::string>(obj->can_shou());
	query << ",";
	query << "last_song_time=" << boost::lexical_cast<std::string>(obj->last_song_time());
	query << ",";
	query << "vip=" << boost::lexical_cast<std::string>(obj->vip());
	query << ",";
	query << "achieve=" << boost::lexical_cast<std::string>(obj->achieve());
	query << ",";
	query << "offline_time=" << boost::lexical_cast<std::string>(obj->offline_time());
	query << ",";
	query << "chengchao=" << boost::lexical_cast<std::string>(obj->chengchao());
	query << ",";
	{
		uint32_t size = obj->invite_players_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->invite_players(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "invite_players=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->invite_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->invite_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "invite_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->invite_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->invite_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "invite_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlsocial_t::query(mysqlpp::Query& query)
{
	dhc::social_t *obj = (dhc::social_t *)data_;
	query << "SELECT * FROM social_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_player_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_target_guid(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_template_id(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_name((std::string)res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_level(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_bf(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_can_shou(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_last_song_time(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_vip(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_achieve(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_offline_time(res.at(0).at(11));
	}
	if (!res.at(0).at(12).is_null())
	{
		obj->set_chengchao(res.at(0).at(12));
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
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
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
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
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
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
	if (!res.at(0).at(16).is_null())
	{
		obj->set_nalflag(res.at(0).at(16));
	}
	return 0;
}

int Sqlsocial_t::update(mysqlpp::Query& query)
{
	dhc::social_t *obj = (dhc::social_t *)data_;
	query << "UPDATE social_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "target_guid=" << boost::lexical_cast<std::string>(obj->target_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "name=" << mysqlpp::quote << obj->name();
	query << ",";
	query << "level=" << boost::lexical_cast<std::string>(obj->level());
	query << ",";
	query << "bf=" << boost::lexical_cast<std::string>(obj->bf());
	query << ",";
	query << "can_shou=" << boost::lexical_cast<std::string>(obj->can_shou());
	query << ",";
	query << "last_song_time=" << boost::lexical_cast<std::string>(obj->last_song_time());
	query << ",";
	query << "vip=" << boost::lexical_cast<std::string>(obj->vip());
	query << ",";
	query << "achieve=" << boost::lexical_cast<std::string>(obj->achieve());
	query << ",";
	query << "offline_time=" << boost::lexical_cast<std::string>(obj->offline_time());
	query << ",";
	query << "chengchao=" << boost::lexical_cast<std::string>(obj->chengchao());
	query << ",";
	{
		uint32_t size = obj->invite_players_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->invite_players(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "invite_players=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->invite_levels_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->invite_levels(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "invite_levels=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->invite_ids_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->invite_ids(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "invite_ids=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "nalflag=" << boost::lexical_cast<std::string>(obj->nalflag());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlsocial_t::remove(mysqlpp::Query& query)
{
	dhc::social_t *obj = (dhc::social_t *)data_;
	query << "DELETE FROM social_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlsport_t::insert(mysqlpp::Query& query)
{
	dhc::sport_t *obj = (dhc::sport_t *)data_;
	query << "INSERT INTO sport_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "other_guid=" << boost::lexical_cast<std::string>(obj->other_guid());
	query << ",";
	query << "other_name=" << mysqlpp::quote << obj->other_name();
	query << ",";
	query << "win=" << boost::lexical_cast<std::string>(obj->win());
	query << ",";
	query << "rank=" << boost::lexical_cast<std::string>(obj->rank());
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlsport_t::query(mysqlpp::Query& query)
{
	dhc::sport_t *obj = (dhc::sport_t *)data_;
	query << "SELECT * FROM sport_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_player_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_time(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_type(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_other_guid(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_other_name((std::string)res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_win(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_rank(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_text((std::string)res.at(0).at(8));
	}
	return 0;
}

int Sqlsport_t::update(mysqlpp::Query& query)
{
	dhc::sport_t *obj = (dhc::sport_t *)data_;
	query << "UPDATE sport_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "type=" << boost::lexical_cast<std::string>(obj->type());
	query << ",";
	query << "other_guid=" << boost::lexical_cast<std::string>(obj->other_guid());
	query << ",";
	query << "other_name=" << mysqlpp::quote << obj->other_name();
	query << ",";
	query << "win=" << boost::lexical_cast<std::string>(obj->win());
	query << ",";
	query << "rank=" << boost::lexical_cast<std::string>(obj->rank());
	query << ",";
	query << "text=" << mysqlpp::quote << obj->text();
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlsport_t::remove(mysqlpp::Query& query)
{
	dhc::sport_t *obj = (dhc::sport_t *)data_;
	query << "DELETE FROM sport_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqlsport_list_t::insert(mysqlpp::Query& query)
{
	dhc::sport_list_t *obj = (dhc::sport_list_t *)data_;
	query << "INSERT INTO sport_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bat_eff_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bat_eff(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bat_eff=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_isnpc_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_isnpc(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_isnpc=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_time=" << boost::lexical_cast<std::string>(obj->last_time());
	query << ",";
	{
		uint32_t size = obj->nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "nalflag=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlsport_list_t::query(mysqlpp::Query& query)
{
	dhc::sport_list_t *obj = (dhc::sport_list_t *)data_;
	query << "SELECT * FROM sport_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		std::string temp(res.at(0).at(1).data(), res.at(0).at(1).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_player_guid(v);
		}
	}
	if (!res.at(0).at(2).is_null())
	{
		std::string temp(res.at(0).at(2).data(), res.at(0).at(2).length());
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
	if (!res.at(0).at(3).is_null())
	{
		std::string temp(res.at(0).at(3).data(), res.at(0).at(3).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_player_name(buf.get(), len);
		}
	}
	if (!res.at(0).at(4).is_null())
	{
		std::string temp(res.at(0).at(4).data(), res.at(0).at(4).length());
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
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
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
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_isnpc(v);
		}
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
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
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
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
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_chenghao(v);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_last_time(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_nalflag(v);
		}
	}
	return 0;
}

int Sqlsport_list_t::update(mysqlpp::Query& query)
{
	dhc::sport_list_t *obj = (dhc::sport_list_t *)data_;
	query << "UPDATE sport_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bat_eff_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bat_eff(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bat_eff=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_isnpc_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_isnpc(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_isnpc=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghao=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "last_time=" << boost::lexical_cast<std::string>(obj->last_time());
	query << ",";
	{
		uint32_t size = obj->nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "nalflag=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqlsport_list_t::remove(mysqlpp::Query& query)
{
	dhc::sport_list_t *obj = (dhc::sport_list_t *)data_;
	query << "DELETE FROM sport_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqltreasure_t::insert(mysqlpp::Query& query)
{
	dhc::treasure_t *obj = (dhc::treasure_t *)data_;
	query << "INSERT INTO treasure_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "role_guid=" << boost::lexical_cast<std::string>(obj->role_guid());
	query << ",";
	query << "enhance=" << boost::lexical_cast<std::string>(obj->enhance());
	query << ",";
	query << "enhance_exp=" << boost::lexical_cast<std::string>(obj->enhance_exp());
	query << ",";
	query << "jilian=" << boost::lexical_cast<std::string>(obj->jilian());
	query << ",";
	query << "locked=" << boost::lexical_cast<std::string>(obj->locked());
	query << ",";
	query << "enhance_counts=" << boost::lexical_cast<std::string>(obj->enhance_counts());
	query << ",";
	query << "star=" << boost::lexical_cast<std::string>(obj->star());
	query << ",";
	query << "star_exp=" << boost::lexical_cast<std::string>(obj->star_exp());
	query << ",";
	query << "star_gold=" << boost::lexical_cast<std::string>(obj->star_gold());
	query << ",";
	query << "star_jewel=" << boost::lexical_cast<std::string>(obj->star_jewel());
	query << ",";
	query << "star_var=" << boost::lexical_cast<std::string>(obj->star_var());
	query << ",";
	{
		uint32_t size = obj->star_rates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->star_rates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "star_rates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->star_bjs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->star_bjs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "star_bjs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "star_luck=" << boost::lexical_cast<std::string>(obj->star_luck());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqltreasure_t::query(mysqlpp::Query& query)
{
	dhc::treasure_t *obj = (dhc::treasure_t *)data_;
	query << "SELECT * FROM treasure_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_player_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_template_id(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_role_guid(res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_enhance(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_enhance_exp(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_jilian(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_locked(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_enhance_counts(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_star(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_star_exp(res.at(0).at(10));
	}
	if (!res.at(0).at(11).is_null())
	{
		obj->set_star_gold(res.at(0).at(11));
	}
	if (!res.at(0).at(12).is_null())
	{
		obj->set_star_jewel(res.at(0).at(12));
	}
	if (!res.at(0).at(13).is_null())
	{
		obj->set_star_var(res.at(0).at(13));
	}
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_star_rates(v);
		}
	}
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_star_bjs(v);
		}
	}
	if (!res.at(0).at(16).is_null())
	{
		obj->set_star_luck(res.at(0).at(16));
	}
	return 0;
}

int Sqltreasure_t::update(mysqlpp::Query& query)
{
	dhc::treasure_t *obj = (dhc::treasure_t *)data_;
	query << "UPDATE treasure_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "role_guid=" << boost::lexical_cast<std::string>(obj->role_guid());
	query << ",";
	query << "enhance=" << boost::lexical_cast<std::string>(obj->enhance());
	query << ",";
	query << "enhance_exp=" << boost::lexical_cast<std::string>(obj->enhance_exp());
	query << ",";
	query << "jilian=" << boost::lexical_cast<std::string>(obj->jilian());
	query << ",";
	query << "locked=" << boost::lexical_cast<std::string>(obj->locked());
	query << ",";
	query << "enhance_counts=" << boost::lexical_cast<std::string>(obj->enhance_counts());
	query << ",";
	query << "star=" << boost::lexical_cast<std::string>(obj->star());
	query << ",";
	query << "star_exp=" << boost::lexical_cast<std::string>(obj->star_exp());
	query << ",";
	query << "star_gold=" << boost::lexical_cast<std::string>(obj->star_gold());
	query << ",";
	query << "star_jewel=" << boost::lexical_cast<std::string>(obj->star_jewel());
	query << ",";
	query << "star_var=" << boost::lexical_cast<std::string>(obj->star_var());
	query << ",";
	{
		uint32_t size = obj->star_rates_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->star_rates(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "star_rates=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->star_bjs_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->star_bjs(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "star_bjs=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	query << "star_luck=" << boost::lexical_cast<std::string>(obj->star_luck());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqltreasure_t::remove(mysqlpp::Query& query)
{
	dhc::treasure_t *obj = (dhc::treasure_t *)data_;
	query << "DELETE FROM treasure_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqltreasure_list_t::insert(mysqlpp::Query& query)
{
	dhc::treasure_list_t *obj = (dhc::treasure_list_t *)data_;
	query << "INSERT INTO treasure_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "amount=" << boost::lexical_cast<std::string>(obj->amount());
	query << ",";
	{
		uint32_t size = obj->nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "nalflag=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bt_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bt(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bt=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_total_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_total(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_total=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_time_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_time(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_time=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_first_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_first(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_first=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghao=" << mysqlpp::quote << ssm.str();
	}

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqltreasure_list_t::query(mysqlpp::Query& query)
{
	dhc::treasure_list_t *obj = (dhc::treasure_list_t *)data_;
	query << "SELECT * FROM treasure_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_template_id(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_amount(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		std::string temp(res.at(0).at(3).data(), res.at(0).at(3).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_nalflag(v);
		}
	}
	if (!res.at(0).at(4).is_null())
	{
		std::string temp(res.at(0).at(4).data(), res.at(0).at(4).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_player_guid(v);
		}
	}
	if (!res.at(0).at(5).is_null())
	{
		std::string temp(res.at(0).at(5).data(), res.at(0).at(5).length());
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
	if (!res.at(0).at(6).is_null())
	{
		std::string temp(res.at(0).at(6).data(), res.at(0).at(6).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint32_t len = 0;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			boost::scoped_array<char> buf(new char[len]);
			ssm.read(buf.get(), len);
			obj->add_player_name(buf.get(), len);
		}
	}
	if (!res.at(0).at(7).is_null())
	{
		std::string temp(res.at(0).at(7).data(), res.at(0).at(7).length());
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
	if (!res.at(0).at(8).is_null())
	{
		std::string temp(res.at(0).at(8).data(), res.at(0).at(8).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_bt(v);
		}
	}
	if (!res.at(0).at(9).is_null())
	{
		std::string temp(res.at(0).at(9).data(), res.at(0).at(9).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_num(v);
		}
	}
	if (!res.at(0).at(10).is_null())
	{
		std::string temp(res.at(0).at(10).data(), res.at(0).at(10).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_total(v);
		}
	}
	if (!res.at(0).at(11).is_null())
	{
		std::string temp(res.at(0).at(11).data(), res.at(0).at(11).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		uint64_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(uint64_t));
			obj->add_player_time(v);
		}
	}
	if (!res.at(0).at(12).is_null())
	{
		std::string temp(res.at(0).at(12).data(), res.at(0).at(12).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_first(v);
		}
	}
	if (!res.at(0).at(13).is_null())
	{
		std::string temp(res.at(0).at(13).data(), res.at(0).at(13).length());
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
	if (!res.at(0).at(14).is_null())
	{
		std::string temp(res.at(0).at(14).data(), res.at(0).at(14).length());
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
	if (!res.at(0).at(15).is_null())
	{
		std::string temp(res.at(0).at(15).data(), res.at(0).at(15).length());
		std::stringstream ssm(temp);
		uint32_t size = 0;
		ssm.read(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		int32_t v;
		for (uint32_t i = 0; i < size; i++)
		{
			ssm.read(reinterpret_cast<char*>(&v), sizeof(int32_t));
			obj->add_player_chenghao(v);
		}
	}
	return 0;
}

int Sqltreasure_list_t::update(mysqlpp::Query& query)
{
	dhc::treasure_list_t *obj = (dhc::treasure_list_t *)data_;
	query << "UPDATE treasure_list_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "template_id=" << boost::lexical_cast<std::string>(obj->template_id());
	query << ",";
	query << "amount=" << boost::lexical_cast<std::string>(obj->amount());
	query << ",";
	{
		uint32_t size = obj->nalflag_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->nalflag(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "nalflag=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_guid_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_guid(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_guid=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_template_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_template(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_template=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_name_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			std::string v = obj->player_name(i);
			uint32_t len = v.size() + 1;
			ssm.write(reinterpret_cast<char*>(&len), sizeof(uint32_t));
			ssm.write(v.data(), len);
		}
		query << "player_name=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_level_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_level(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_level=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_bt_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_bt(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_bt=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_num_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_num(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_num=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_total_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_total(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_total=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_time_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			uint64_t v = obj->player_time(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(uint64_t));
		}
		query << "player_time=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_first_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_first(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_first=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_vip_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_vip(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_vip=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_achieve_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_achieve(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_achieve=" << mysqlpp::quote << ssm.str();
	}
	query << ",";
	{
		uint32_t size = obj->player_chenghao_size();
		std::stringstream ssm;
		ssm.write(reinterpret_cast<char*>(&size), sizeof(uint32_t));
		for (uint32_t i = 0; i < size; i++)
		{
			int32_t v = obj->player_chenghao(i);
			ssm.write(reinterpret_cast<char*>(&v), sizeof(int32_t));
		}
		query << "player_chenghao=" << mysqlpp::quote << ssm.str();
	}
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqltreasure_list_t::remove(mysqlpp::Query& query)
{
	dhc::treasure_list_t *obj = (dhc::treasure_list_t *)data_;
	query << "DELETE FROM treasure_list_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

//////////////////////////////////////////////////////////////////////////

int Sqltreasure_report_t::insert(mysqlpp::Query& query)
{
	dhc::treasure_report_t *obj = (dhc::treasure_report_t *)data_;
	query << "INSERT INTO treasure_report_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "other_guid=" << boost::lexical_cast<std::string>(obj->other_guid());
	query << ",";
	query << "other_name=" << mysqlpp::quote << obj->other_name();
	query << ",";
	query << "other_template=" << boost::lexical_cast<std::string>(obj->other_template());
	query << ",";
	query << "other_level=" << boost::lexical_cast<std::string>(obj->other_level());
	query << ",";
	query << "other_bf=" << boost::lexical_cast<std::string>(obj->other_bf());
	query << ",";
	query << "win=" << boost::lexical_cast<std::string>(obj->win());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "suipian_id=" << boost::lexical_cast<std::string>(obj->suipian_id());
	query << ",";
	query << "new_report=" << boost::lexical_cast<std::string>(obj->new_report());

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqltreasure_report_t::query(mysqlpp::Query& query)
{
	dhc::treasure_report_t *obj = (dhc::treasure_report_t *)data_;
	query << "SELECT * FROM treasure_report_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::StoreQueryResult res = query.store();

	if (!res || res.num_rows() != 1)
	{
		return -1;
	}

	if (!res.at(0).at(0).is_null())
	{
		obj->set_guid(res.at(0).at(0));
	}
	if (!res.at(0).at(1).is_null())
	{
		obj->set_player_guid(res.at(0).at(1));
	}
	if (!res.at(0).at(2).is_null())
	{
		obj->set_other_guid(res.at(0).at(2));
	}
	if (!res.at(0).at(3).is_null())
	{
		obj->set_other_name((std::string)res.at(0).at(3));
	}
	if (!res.at(0).at(4).is_null())
	{
		obj->set_other_template(res.at(0).at(4));
	}
	if (!res.at(0).at(5).is_null())
	{
		obj->set_other_level(res.at(0).at(5));
	}
	if (!res.at(0).at(6).is_null())
	{
		obj->set_other_bf(res.at(0).at(6));
	}
	if (!res.at(0).at(7).is_null())
	{
		obj->set_win(res.at(0).at(7));
	}
	if (!res.at(0).at(8).is_null())
	{
		obj->set_time(res.at(0).at(8));
	}
	if (!res.at(0).at(9).is_null())
	{
		obj->set_suipian_id(res.at(0).at(9));
	}
	if (!res.at(0).at(10).is_null())
	{
		obj->set_new_report(res.at(0).at(10));
	}
	return 0;
}

int Sqltreasure_report_t::update(mysqlpp::Query& query)
{
	dhc::treasure_report_t *obj = (dhc::treasure_report_t *)data_;
	query << "UPDATE treasure_report_t SET ";
	query << "guid=" << boost::lexical_cast<std::string>(obj->guid());
	query << ",";
	query << "player_guid=" << boost::lexical_cast<std::string>(obj->player_guid());
	query << ",";
	query << "other_guid=" << boost::lexical_cast<std::string>(obj->other_guid());
	query << ",";
	query << "other_name=" << mysqlpp::quote << obj->other_name();
	query << ",";
	query << "other_template=" << boost::lexical_cast<std::string>(obj->other_template());
	query << ",";
	query << "other_level=" << boost::lexical_cast<std::string>(obj->other_level());
	query << ",";
	query << "other_bf=" << boost::lexical_cast<std::string>(obj->other_bf());
	query << ",";
	query << "win=" << boost::lexical_cast<std::string>(obj->win());
	query << ",";
	query << "time=" << boost::lexical_cast<std::string>(obj->time());
	query << ",";
	query << "suipian_id=" << boost::lexical_cast<std::string>(obj->suipian_id());
	query << ",";
	query << "new_report=" << boost::lexical_cast<std::string>(obj->new_report());
	query << " WHERE guid=" << boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}

int Sqltreasure_report_t::remove(mysqlpp::Query& query)
{
	dhc::treasure_report_t *obj = (dhc::treasure_report_t *)data_;
	query << "DELETE FROM treasure_report_t WHERE guid="
		<< boost::lexical_cast<std::string>(guid_);

	mysqlpp::SimpleResult res = query.execute();

	if (!res)
	{
		game::log()->error(query.error());
		return -1;
	}
	return 0;
}
