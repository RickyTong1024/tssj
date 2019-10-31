#include "social_config.h"
#include "dbc.h"
#include "utils.h"

SocialConfig::SocialConfig()
{

}

SocialConfig::~SocialConfig()
{

}

int SocialConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_gundong.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_gundong gd;
		gd.start = dbfile->Get(i, 0)->iValue;
		gd.time = dbfile->Get(i, 1)->iValue;
		gd.text = dbfile->Get(i, 2)->pString;
		gd.gtime = 0;
		t_gundong_.push_back(gd);
	}

	dbfile = game::scheme()->get_dbc("t_target.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		if (dbfile->Get(i, 4)->iValue == 4)
		{
			t_social_active_[dbfile->Get(i, 7)->iValue].push_back(std::make_pair(dbfile->Get(i, 0)->iValue,
				dbfile->Get(i, 8)->iValue));
		}
	}

	return 0;
}

std::vector<s_t_gundong> & SocialConfig::get_gundong()
{
	return t_gundong_;
}
