#include "team_config.h"
#include "dbc.h"


int TeamConfig::parse()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_master_duanwei.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_duanwei dw;
		dw.id = dbfile->Get(i, 0)->iValue;
		dw.rank = dbfile->Get(i, 10)->iValue;
		dw.point = dbfile->Get(i, 9)->iValue;
		t_duanweis_.push_back(dw);
	}

	return 0;
}

int TeamConfig::get_duanwei(int point, int rank, int old_duanwei) const
{
	int new_duanwei = 0;
	for (int i = 0; i < t_duanweis_.size(); ++i)
	{
		const s_t_duanwei &dw = t_duanweis_[i];
		if (point >= dw.point)
		{
			if (dw.rank == 0 || rank <= dw.rank)
			{
				new_duanwei = dw.id;
				break;
			}
		}
	}
	if (new_duanwei > old_duanwei)
	{
		return new_duanwei;
	}
	return old_duanwei;
}

int TeamConfig::get_duanwei_point(int duanwei) const
{
	for (int i = 0; i < t_duanweis_.size(); ++i)
	{
		const s_t_duanwei &dw = t_duanweis_[i];
		if (duanwei >= dw.id)
		{
			return dw.point;
		}
	}
	return 0;
}