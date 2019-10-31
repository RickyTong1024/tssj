#include "boss_manager.h"
#include "gs_message.h"
#include "boss_config.h"
#include "player_operation.h"
#include "post_operation.h"
#include "social_operation.h"
#include "mission_fight.h"
#include "mission_config.h"
#include "utils.h"
#include "huodong_pool.h"
#include "item_operation.h"

#define BOSS_PERIOD 5000
#define RANK_SIZE 200
#define HIT_MEDAL 200
#define BOSS_DEFAULT_HP 99999999.0
#define BOSS_ID 90000001

BossManager::BossManager()
:timer_(0),
boss_id_(BOSS_ID),
changed_(false)
{
	
}

BossManager::~BossManager()
{
}

int BossManager::init()
{
	if (-1 == sBossConfig->parse())
	{
		return -1;
	}

	boss_load();

	timer_ = game::timer()->schedule(boost::bind(&BossManager::update, this, _1), BOSS_PERIOD, "boss");

	return 0;
}

int BossManager::fini()
{
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}

	boss_save(true);

	return 0;
}

int BossManager::update(const ACE_Time_Value &curr)
{
	dhc::boss_t *boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (boss)
	{
		uint64_t last_time = boss->last_time();
		if (game::timer()->trigger_time(last_time, 12, 0))
		{
			boss->set_last_time(game::timer()->now());
			SocialOperation::gundong_server("t_server_language_text_boss_open", "",  "",  "");
			create_boss();
		}

		if (game::timer()->trigger_time(last_time, 0, 0))
		{
			boss->set_last_time(game::timer()->now());

			reward();
			changed_ = false;
		}

		boss_save(false);
	}

	return 0;
}

void BossManager::boss_load()
{
	uint64_t boss_guid = MAKE_GUID(et_boss, 0);
	Request *req = new Request();
	req->add(opc_query, boss_guid, new dhc::boss_t());
	game::pool()->upcall(req, boost::bind(&BossManager::boss_load_callback, this, _1, boss_guid));
}

void BossManager::boss_load_callback(Request *req, uint64_t boss_guid)
{
	dhc::boss_t *boss = 0;
	if (req->success())
	{
		boss = (dhc::boss_t*)req->release_data();
		POOL_ADD(boss->guid(), boss);

		boss_hit bh;
		for (int i = 0; i < boss->player_guids_size(); ++i)
		{
			bh.guid = boss->player_guids(i);
			bh.template_id = boss->player_templates(i);
			bh.name = boss->player_names(i);
			bh.level = boss->player_levels(i);
			bh.hit = boss->player_damages(i);
			bh.num = boss->player_nums(i);
			bh.top = boss->player_tops(i);
			bh.vip = boss->player_vips(i);
			bh.achieve = boss->player_achieves(i);
			bh.chenchao = boss->player_chenghaos(i);
			bh.nalflag = boss->player_nalflags(i);
			boss_hits_[bh.guid] = bh;
			calrank(bh);
		}

		if (boss->boss_last_num() <= 0)
		{
			boss->set_boss_last_num(get_boss_player_num());
		}
	}
	else
	{
		boss = new dhc::boss_t();
		boss->set_guid(boss_guid);
		boss->set_last_time(game::timer()->now());
		POOL_ADD_NEW(boss_guid, boss);

		create_boss();
	}
}

void BossManager::boss_save(bool release)
{
	dhc::boss_t *boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (boss)
	{
		if (changed_ && game::timer()->hour() >= 12)
		{
			changed_ = false;
			boss->clear_player_guids();
			boss->clear_player_templates();
			boss->clear_player_names();
			boss->clear_player_damages();
			boss->clear_player_nums();
			boss->clear_player_tops();
			boss->clear_player_levels();
			boss->clear_player_vips();
			boss->clear_player_achieves();
			boss->clear_player_chenghaos();
			boss->clear_player_nalflags();

			for (std::map<uint64_t, boss_hit>::const_iterator it = boss_hits_.begin();
				it != boss_hits_.end(); ++it)
			{
				boss->add_player_guids(it->second.guid);
				boss->add_player_templates(it->second.template_id);
				boss->add_player_names(it->second.name);
				boss->add_player_levels(it->second.level);
				boss->add_player_damages(it->second.hit);
				boss->add_player_nums(it->second.num);
				boss->add_player_tops(it->second.top);
				boss->add_player_vips(it->second.vip);
				boss->add_player_achieves(it->second.achieve);
				boss->add_player_chenghaos(it->second.chenchao);
				boss->add_player_nalflags(it->second.nalflag);
			}
		}

		POOL_SAVE(dhc::boss_t, boss, release);
	}
}

void BossManager::create_boss()
{
	dhc::boss_t *boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (boss)
	{
		boss->set_level(1);
		boss->set_boss_last_num(get_boss_player_num());

		double max_hp = get_boss_hp(boss->level()) * boss->boss_last_num();
		boss->set_cur_hp(max_hp);
		boss->set_max_hp(max_hp);
		boss->clear_player_guids();
		boss->clear_player_templates();
		boss->clear_player_names();
		boss->clear_player_levels();
		boss->clear_player_damages();
		boss->clear_player_nums();
		boss->clear_player_tops();
		boss->clear_player_vips();
		boss->clear_player_achieves();
		boss->clear_player_chenghaos();
		boss->clear_player_nalflags();
		boss_hits_.clear();
		total_rank_.clear();
		top_rank_.clear();
		changed_ = true;
	}
}

void BossManager::end_boss()
{
	dhc::boss_t *boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (boss)
	{
		boss->set_level(boss->level() + 1);

		double max_hp = get_boss_hp(boss->level()) * boss->boss_last_num();
		boss->set_cur_hp(max_hp);
		boss->set_max_hp(max_hp);
	}
}

void BossManager::calrank(const boss_hit &bh)
{
	find_.guid = bh.guid;

	find_ite_ = std::find(total_rank_.begin(),
		total_rank_.end(), find_);
	if (find_ite_ == total_rank_.end())
	{
		total_rank_.push_back(boss_pre(bh.guid, bh.name, bh.template_id, bh.level, bh.hit, bh.vip, bh.achieve, bh.chenchao, bh.nalflag));
	}
	else
	{
		find_ite_->hit = bh.hit;
		find_ite_->template_id = bh.template_id;
		find_ite_->level = bh.level;
		find_ite_->vip = bh.vip;
		find_ite_->achieve = bh.achieve;
		find_ite_->chenghao = bh.chenchao;
		find_ite_->nalflag = bh.nalflag;
	}
	std::sort(total_rank_.begin(), total_rank_.end(), std::greater<boss_pre>());
	if (total_rank_.size() > RANK_SIZE)
	{
		total_rank_.pop_back();
	}

	find_ite_ = std::find(top_rank_.begin(),
		top_rank_.end(), find_);
	if (find_ite_ == top_rank_.end())
	{
		top_rank_.push_back(boss_pre(bh.guid, bh.name, bh.template_id, bh.level, bh.top, bh.vip, bh.achieve, bh.chenchao, bh.nalflag));
	}
	else
	{
		find_ite_->hit = bh.top;
		find_ite_->template_id = bh.template_id;
		find_ite_->level = bh.level;
		find_ite_->vip = bh.vip;
		find_ite_->achieve = bh.achieve;
		find_ite_->chenghao = bh.chenchao;
		find_ite_->nalflag = bh.nalflag;
	}
	std::sort(top_rank_.begin(), top_rank_.end(), std::greater<boss_pre>());
	if (top_rank_.size() > RANK_SIZE)
	{
		top_rank_.pop_back();
	}
}

void BossManager::reward()
{
	dhc::boss_t *boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (!boss)
	{
		return;
	}
	boss->clear_player_rank_guids();
	boss->clear_player_ranks();

	/// 最高伤害
	
	for (int i = 0; i < top_rank_.size(); ++i)
	{
		const s_t_boss_reward *t_boss_reward = sBossConfig->get_boss_reward(i + 1);
		if (!t_boss_reward)
		{
			continue;
		}

		int lang_ver = game::channel()->get_channel_lang(top_rank_[i].guid);
		// 玩家在线
		std::string sender;
		std::string title;
		std::string text;
		game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
		game::scheme()->get_server_str(lang_ver, text, "boss_top_text", i + 1);
		game::scheme()->get_server_str(lang_ver, title, "boss_top_title");
		PostOperation::post_create(top_rank_[i].guid, title, text, sender, t_boss_reward->top_rewards);

	}

	/// 累计伤害
	for (int i = 0; i < total_rank_.size(); ++i)
	{
		const s_t_boss_reward *t_boss_reward = sBossConfig->get_boss_reward(i + 1);
		if (!t_boss_reward)
		{
			continue;
		}

		dhc::player_t *player = POOL_GET_PLAYER(total_rank_[i].guid);
		if (player)
		{
			if (player->boss_max_rank() > i + 1)
			{
				player->set_boss_max_rank(i + 1);
			}
		}
		else
		{
			boss->add_player_rank_guids(total_rank_[i].guid);
			boss->add_player_ranks(i + 1);
		}
		std::string sender;
		std::string title;
		std::string text;
		int lang_ver = game::channel()->get_channel_lang(total_rank_[i].guid);
		game::scheme()->get_server_str(lang_ver,title, "boss_title");
		game::scheme()->get_server_str(lang_ver, text, "boss_text", i + 1);
		PostOperation::post_create(total_rank_[i].guid, title, text, sender, t_boss_reward->max_rewards);
	}
}

void BossManager::add_rank(dhc::player_t *player, int64_t hit)
{
	if (hit < 0)
	{
		return;
	}

	changed_ = true;

	int64_t max_damage = hit;
	std::map<uint64_t, boss_hit>::iterator it = boss_hits_.find(player->guid());
	if (it == boss_hits_.end())
	{
		boss_hit bh;
		bh.guid = player->guid();
		bh.template_id = player->template_id();
		bh.name = player->name();
		bh.level = player->level();
		bh.hit = hit;
		bh.num = 1;
		bh.top = hit;
		bh.vip = player->vip();
		bh.achieve = player->dress_achieves_size();
		bh.chenchao = player->chenghao_on();
		bh.nalflag = player->nalflag();
		boss_hits_[player->guid()] = bh;
		calrank(bh);
	}
	else
	{
		it->second.template_id = player->template_id();
		it->second.level = player->level();
		it->second.vip = player->vip();
		it->second.achieve = player->dress_achieves_size();
		it->second.chenchao = player->chenghao_on();
		it->second.nalflag = player->nalflag();
		it->second.hit += hit;
		it->second.num += 1;
		if (hit > it->second.top)
		{
			it->second.top = hit;
		}
		max_damage = it->second.hit;
		calrank(it->second);
	}

	if (player->boss_max_damage() < max_damage)
	{
		player->set_boss_max_damage(max_damage);
	}

	if (game::timer()->run_day(player->birth_time()) <= 7)
	{
		player->set_boss_leiji_damage(player->boss_leiji_damage() + hit);
	}
}

int BossManager::get_rank(dhc::player_t *player, int type)
{
	find_.guid = player->guid();
	if (type == 0)
	{
		find_ite_ = std::find(total_rank_.begin(),
			total_rank_.end(), find_);
		if (find_ite_ == total_rank_.end())
		{
			return 0;
		}
		return std::distance(total_rank_.begin(), find_ite_) + 1;
	}
	else
	{
		find_ite_ = std::find(top_rank_.begin(),
			top_rank_.end(), find_);
		if (find_ite_ == top_rank_.end())
		{
			return 0;
		}
		return std::distance(top_rank_.begin(), find_ite_) + 1;
	}
}

std::pair<int, int> BossManager::get_medal_reward() const
{
	static int rate[4] = { 10, 20, 20, 50 };
	int g = Utils::get_int32(0, 99);
	int gl = 0;
	for (int i = 0; i < 4; ++i)
	{
		gl += rate[i];
		if (gl > g)
		{
			switch (i)
			{
			case 0:
				return std::make_pair(125, 3);
			case 1:
				return std::make_pair(100, 2);
			case 2:
				return std::make_pair(75, 1);
			case 3:
				return std::make_pair(50, 0);
			}
			break;
		}
	}
	return std::make_pair(50, 0);
}

double BossManager::get_boss_hp(int boss_level) const
{
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(MAKE_GUID(et_sport_list, 0));
	if (sport_list)
	{
		int level = 0;
		int num = 10;
		if (num > sport_list->player_level_size())
		{
			num = sport_list->player_level_size();
		}
		if (num <= 0)
		{
			num = 1;
		}
		for (int i = 0; i < num; ++i)
		{
			level += sport_list->player_level(i);
		}
		level = level / num;
		const s_t_boss_hp* t_boss_hp = sBossConfig->get_boss_hp(level);
		if (t_boss_hp)
		{
			return t_boss_hp->hp_base + t_boss_hp->hp_inc * (boss_level - 1);
		}
	}
	return BOSS_DEFAULT_HP;
}

int BossManager::get_boss_player_num() const
{
	int num = game::channel()->get_real_channel_num();
	num = num / 30;
	if (num <= 0)
	{
		num = 1;
	}
	if (num > 40)
	{
		num = 40;
	}

	return num;
}

void BossManager::terminal_boss_look(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::boss_t* boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (!boss)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->trigger_time(player->boss_hit_time(), 12, 0))
	{
		player->clear_boss_active_ids();
		player->clear_boss_active_nums();
		player->clear_boss_active_rewards();
		player->set_boss_hit_time(game::timer()->now());
	}

	set_active(player, 2, boss->level() - 1);

	protocol::game::smsg_boss_look msg1;
	msg1.set_cur_hp(boss->cur_hp());
	msg1.set_max_hp(boss->max_hp());
	msg1.set_myhit(0);
	msg1.set_num(0);
	msg1.set_rank(0);
	msg1.set_tophit(0);
	msg1.set_toprank(0);
	msg1.set_level(boss->level());

	std::map<uint64_t, boss_hit>::const_iterator it = boss_hits_.find(player->guid());
	if (it != boss_hits_.end())
	{
		msg1.set_myhit(it->second.hit);
		msg1.set_num(it->second.num);
		msg1.set_tophit(it->second.top);
		msg1.set_rank(get_rank(player, 0));
		msg1.set_toprank(get_rank(player, 1));

		set_active(player, 1, it->second.hit);
	}
	msg1.set_reward(has_active(player));

	ResMessage::res_boss_look(player, msg1, name, id);
}

void BossManager::terminal_boss_look_ex(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common_ex msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	PCK_CHECK_EX

	dhc::boss_t* boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (!boss)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->trigger_time(player->boss_hit_time(), 12, 0))
	{
		player->clear_boss_active_ids();
		player->clear_boss_active_nums();
		player->clear_boss_active_rewards();
		player->set_boss_hit_time(game::timer()->now());
	}

	set_active(player, 2, boss->level() - 1);

	protocol::game::smsg_boss_look msg1;
	msg1.set_cur_hp(boss->cur_hp());
	msg1.set_max_hp(boss->max_hp());
	msg1.set_myhit(0);
	msg1.set_num(0);
	msg1.set_rank(0);
	msg1.set_tophit(0);
	msg1.set_toprank(0);
	msg1.set_level(boss->level());

	std::map<uint64_t, boss_hit>::const_iterator it = boss_hits_.find(player->guid());
	if (it != boss_hits_.end())
	{
		msg1.set_myhit(it->second.hit);
		msg1.set_num(it->second.num);
		msg1.set_tophit(it->second.top);
		msg1.set_rank(get_rank(player, 0));
		msg1.set_toprank(get_rank(player, 1));

		set_active(player, 1, it->second.hit);
	}
	msg1.set_reward(has_active(player));

	ResMessage::res_boss_look_ex(player, msg1, name, id);
}

void BossManager::terminal_boss_rank(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	protocol::game::smsg_boss_rank smsg;
	int num = 10;
	if (total_rank_.size() < num)
	{
		num = total_rank_.size();
	}
	for (int i = 0; i < num; ++i)
	{
		smsg.add_total_names(total_rank_[i].name);
		smsg.add_total_templates(total_rank_[i].template_id);
		smsg.add_total_hits(total_rank_[i].hit);
		smsg.add_total_levels(total_rank_[i].level);
		smsg.add_total_guids(total_rank_[i].guid);
		smsg.add_total_vips(total_rank_[i].vip);
		smsg.add_total_achieves(total_rank_[i].achieve);
		smsg.add_total_chenghaos(total_rank_[i].chenghao);
		smsg.add_total_nalflags(total_rank_[i].nalflag);
	}

	num = 10;
	if (top_rank_.size() < num)
	{
		num = top_rank_.size();
	}
	for (int i = 0; i < num; ++i)
	{
		smsg.add_top_names(top_rank_[i].name);
		smsg.add_top_templates(top_rank_[i].template_id);
		smsg.add_top_hits(top_rank_[i].hit);
		smsg.add_top_levels(top_rank_[i].level);
		smsg.add_top_guids(top_rank_[i].guid);
		smsg.add_top_vips(top_rank_[i].vip);
		smsg.add_top_achieves(top_rank_[i].achieve);
		smsg.add_top_chenghaos(top_rank_[i].chenghao);
		smsg.add_top_nalflag(top_rank_[i].nalflag);
	}

	ResMessage::res_boss_rank(player, smsg, name, id);
}

void BossManager::terminal_boss_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (!PlayerOperation::check_fight_time(player))
	{
		PERROR(ERROR_FIGHT_TIME);
		return;
	}

	int boss_time = game::timer()->hour();
	if (boss_time < 12)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::boss_t* boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (!boss)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_do_boss(player);
	if (player->boss_num() <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	int btotal_rank = get_rank(player, 0);
	int btop_rank = get_rank(player, 1);

	std::string text;
	double hhp = 0;
	int gj = 1;
	if ((boss_time >= 12 && boss_time < 14) ||
		(boss_time >= 18 && boss_time < 20))
	{
		gj = 2;
	}
	int result = MissionFight::mission_boss(player, boss_id_, boss->cur_hp(), boss->max_hp(), gj, hhp, text);

	boss->set_cur_hp(boss->cur_hp() - hhp);

	int64_t inthpp = static_cast<int64_t>(hhp);
	add_rank(player, inthpp);
	
	std::pair<int,int> medal = get_medal_reward();
	int hit_medal = 0;
	if (boss->cur_hp() <= 0.0)
	{
		hit_medal = HIT_MEDAL;
		end_boss();
	}
	int atotal_rank = get_rank(player, 0);
	int atop_rank = get_rank(player, 1);

	PlayerOperation::player_add_resource(player, resource::MW_MEDAL, medal.first + hit_medal, LOGWAY_BOSS_END);
	PlayerOperation::player_dec_resource(player, resource::MW_COUNT, 1, LOGWAY_BOSS_END);
	PlayerOperation::player_add_active(player, 1000, 1);
	player->set_boss_task_num(player->boss_task_num() + 1);
	sHuodongPool->huodong_active(player, HUODONG_COND_MW_COUNT, 1);

	protocol::game::smsg_boss_fight_end smsg;
	smsg.set_result(result);
	smsg.set_text(text);
	smsg.set_hit(inthpp);
	smsg.set_btop_rank(btop_rank);
	smsg.set_bmax_rank(btotal_rank);
	smsg.set_atop_rank(atop_rank);
	smsg.set_amax_rank(atotal_rank);
	smsg.set_medal(medal.first);
	smsg.set_hit_medal(hit_medal);
	smsg.set_baoji(medal.second);
	ResMessage::res_boss_fight_end(player, smsg, name, id);
}

void BossManager::terminal_boss_active_look(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::boss_t *boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (!boss)
	{
		GLOBAL_ERROR;
		return;
	}


	protocol::game::smsg_boss_active_look smsg;
	smsg.set_level(player->boss_player_level());
	for (int i = 0; i < player->boss_active_ids_size(); ++i)
	{
		smsg.add_ids(player->boss_active_ids(i));
		smsg.add_nums(player->boss_active_nums(i));
		smsg.add_rewards(player->boss_active_rewards(i));
	}

	ResMessage::res_boss_active_look(player, smsg, name, id);

}

void BossManager::terminal_boss_active(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_active_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_boss_dw *t_dw = sBossConfig->get_boss_dw(player->boss_player_level());
	if (!t_dw)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	if (msg.id() == 1 || msg.id() == 2)
	{
		const s_t_boss_active *t_active = 0;
		int64_t total_count = 0;
		for (int i = 0; i < player->boss_active_ids_size(); ++i)
		{
			t_active = sBossConfig->get_boss_active(player->boss_active_ids(i));
			if (t_active && msg.id() == t_active->type)
			{
				total_count = t_active->count;
				if (t_active->type == 1)
				{
					total_count = (int64_t)t_dw->shanghai * t_active->count;
				}
				if (total_count <= player->boss_active_nums(i) &&
					player->boss_active_rewards(i) != 1)
				{
					s_t_reward rd = t_active->reward;
					rd.value2 += t_dw->dw * t_active->jiacheng;
					rds.add_reward(rd);
					player->set_boss_active_rewards(i, 1);
				}
			}
		}
	}
	else
	{
		const s_t_boss_active *t_active = sBossConfig->get_boss_active(msg.id());
		if (!t_active)
		{
			GLOBAL_ERROR;
			return;
		}

		int index = -1;
		for (int i = 0; i < player->boss_active_ids_size(); ++i)
		{
			if (t_active->id == player->boss_active_ids(i))
			{
				index = i;
				break;
			}
		}

		int64_t total_count = t_active->count;
		if (t_active->type == 1)
		{
			total_count = (int64_t)t_dw->shanghai * t_active->count;
		}

		if (index == -1 || player->boss_active_nums(index) < total_count)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}

		if (player->boss_active_rewards(index) == 1)
		{
			PERROR(ERROR_TASK_FINIED);
			return;
		}
		s_t_reward rd = t_active->reward;
		rd.value2 += t_dw->dw * t_active->jiacheng;
		rds.add_reward(rd);
		player->set_boss_active_rewards(index, 1);
	}


	PlayerOperation::player_add_reward(player, rds,  LOGWAY_BOSS_ACTIVE);
	ResMessage::res_active_reward(player, rds, name, id);
}


void BossManager::set_active(dhc::player_t *player, int type, int64_t count)
{
	const s_t_boss_dw *t_dw = sBossConfig->get_boss_dw(player->boss_player_level());
	if (!t_dw)
	{
		return;
	}


	bool has_jieri_huodong = sHuodongPool->has_jieri_huodong(player);
	bool flag = false;
	const std::map<int, s_t_boss_active>& boss_actives = sBossConfig->get_all_boss_active();
	for (std::map<int, s_t_boss_active>::const_iterator it = boss_actives.begin();
		it != boss_actives.end();
		++it)
	{
		const s_t_boss_active &t_active = it->second;
		if (t_active.type == type)
		{
			if (!has_jieri_huodong && t_active.reward.value1 == s_t_rewards::HUODONG_ITEM_ID1)
			{
				continue;
			}

			flag = false;
			for (int i = 0; i < player->boss_active_ids_size(); ++i)
			{
				if (player->boss_active_ids(i) == t_active.id)
				{
					flag = true;
					player->set_boss_active_nums(i, count);
					if (type == 1)
					{
						if (player->boss_active_nums(i) >(int64_t)t_active.count * t_dw->shanghai)
						{
							player->set_boss_active_nums(i, (int64_t)t_active.count * t_dw->shanghai);
						}
					}
					else
					{
						if (player->boss_active_nums(i) > t_active.count)
						{
							player->set_boss_active_nums(i, t_active.count);
						}
					}
					
					break;
				}
			}
			if (!flag)
			{
				player->add_boss_active_ids(t_active.id);
				player->add_boss_active_rewards(0);
				player->add_boss_active_nums(count);
			}
		}
	}
}

bool BossManager::has_active(dhc::player_t *player)
{
	const s_t_boss_dw *t_dw = sBossConfig->get_boss_dw(player->boss_player_level());
	if (!t_dw)
	{
		return false;
	}

	const s_t_boss_active* t_active = 0;
	for (int i = 0; i < player->boss_active_ids_size(); ++i)
	{
		if (player->boss_active_rewards(i) == 0)
		{
			t_active = sBossConfig->get_boss_active(player->boss_active_ids(i));
			if (t_active)
			{
				if (t_active->type == 1)
				{
					if (player->boss_active_nums(i) >= (int64_t)t_active->count * t_dw->shanghai)
					{
						return true;
					}
				}
				else
				{
					if (player->boss_active_nums(i) >= t_active->count)
					{
						return true;
					}
				}
			}
		}
	}

	return false;
}

void BossManager::terminal_boss_change_name(Packet* pck)
{
	protocol::self::self_boss_change_name msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	std::map<uint64_t, boss_hit>::iterator it = boss_hits_.find(msg.player_guid());
	if (it != boss_hits_.end())
	{
		it->second.name = msg.player_name();
	}

	find_.guid = msg.player_guid();
	find_ite_ = std::find(total_rank_.begin(),
		total_rank_.end(), find_);
	if (find_ite_ != total_rank_.end())
	{
		find_ite_->name = msg.player_name();
	}
	find_ite_ = std::find(top_rank_.begin(),
		top_rank_.end(), find_);
	if (find_ite_ != top_rank_.end())
	{
		find_ite_->name = msg.player_name();
	}
}

void BossManager::terminal_boss_saodang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_boss_saodang msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int boss_time = game::timer()->hour();
	if (boss_time < 12)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::boss_t* boss = POOL_GET_BOSS(MAKE_GUID(et_boss, 0));
	if (!boss)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_do_boss(player);
	int item_num = 0;
	int boss_num = player->boss_num();
	if (boss_num < msg.num())
	{
		if (!msg.use_item())
		{
			GLOBAL_ERROR;
			return;
		}

		item_num = msg.num() - boss_num;
		if (ItemOperation::item_num_templete(player, 10010009) < item_num)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}
	else
	{
		boss_num = msg.num();
	}

	int gj = 1;
	if ((boss_time >= 12 && boss_time < 14) ||
		(boss_time >= 18 && boss_time < 20))
	{
		gj = 2;
	}
	std::string text;
	double hhp = 0;
	int medal = 0;
	int64_t inthpp = 0;
	std::pair<int, int> medals;
	int i = 0, j = 0;
	protocol::game::smsg_boss_saodang smsg;
	smsg.set_btop_rank(get_rank(player, 1));
	smsg.set_bmax_rank(get_rank(player, 0));

	do 
	{
		if (i >= boss_num && j >= item_num)
		{
			break;
		}
		if (i < boss_num)
		{
			i++;
		}
		else if (j < item_num)
		{
			j++;
		}
		else
		{
			break;
		}

		smsg.add_level(boss->level());
		smsg.add_use_item((j > 0));

		hhp = 0;
		MissionFight::mission_boss(player, boss_id_, boss->cur_hp(), boss->max_hp(), gj, hhp, text);
		boss->set_cur_hp(boss->cur_hp() - hhp);

		inthpp = static_cast<int64_t>(hhp);
		add_rank(player, inthpp);
		smsg.add_hit(inthpp);

		medals = get_medal_reward();
		medal += medals.first;
		if (boss->cur_hp() <= 0.0)
		{
			medal += HIT_MEDAL;
			end_boss();
			smsg.add_medal(medals.first + HIT_MEDAL);
		}
		else
		{
			smsg.add_medal(medals.first);
		}

	} while (true);

	smsg.set_atop_rank(get_rank(player, 1));
	smsg.set_amax_rank(get_rank(player, 0));

	PlayerOperation::player_add_resource(player, resource::MW_MEDAL, medal, LOGWAY_BOSS_SAODANG);
	PlayerOperation::player_dec_resource(player, resource::MW_COUNT, boss_num, LOGWAY_BOSS_SAODANG);
	ItemOperation::item_destory_templete(player, 10010009, item_num, LOGWAY_BOSS_SAODANG);
	PlayerOperation::player_add_active(player, 1000, msg.num());
	player->set_boss_task_num(player->boss_task_num() + msg.num());
	sHuodongPool->huodong_active(player, HUODONG_COND_MW_COUNT, msg.num());

	ResMessage::res_boss_fight_saodang(player, smsg, name, id);
}