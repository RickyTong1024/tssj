
using UnityEngine;
using System.Collections;

public class icon_manager : MonoBehaviour {
	
	public static icon_manager _instance;

	void Awake()
	{
		_instance = this;
	}
	
	void Update() {
	}

	public GameObject create_card_icon(int class_id, int glevel, int jlevel, int level)
	{		
		GameObject _icon = game_data._instance.ins_object_res("ui/card_icon");
		_icon.GetComponent<card_icon>().init ();
		_icon.GetComponent<card_icon>().m_class = game_data._instance.get_t_class(class_id);
		_icon.GetComponent<card_icon>().m_glevel = glevel;
		_icon.GetComponent<card_icon>().m_jlevel = jlevel;
		_icon.GetComponent<card_icon>().m_level = level;
		_icon.GetComponent<card_icon>().reset ();
		
		return _icon;
	}
    public GameObject create_chenghao_icon(int chenhaog_id,int type = 1)
    {
        GameObject _icon = game_data._instance.ins_object_res("ui/chenhao_icon");
        _icon.GetComponent<chenghao_icon>().reset(chenhaog_id, type);
        return _icon;
 
    }
	public GameObject create_player_icon(int class_id, int count, int vip,int gq = 0)
	{		
		GameObject _icon = game_data._instance.ins_object_res("ui/player_icon");
		_icon.GetComponent<player_icon>().init ();
		_icon.GetComponent<player_icon>().id = class_id;
		_icon.GetComponent<player_icon>().count = count;
		_icon.GetComponent<player_icon>().vip = vip;
        _icon.GetComponent<player_icon>().gq_id = gq;
		_icon.GetComponent<player_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_card_icon_ex(int class_id, int glevel, int jlevel, int level)
	{		
		GameObject _icon = game_data._instance.ins_object_res("ui/card_icon");
		_icon.GetComponent<card_icon>().init ();
		_icon.GetComponent<card_icon>().m_class = game_data._instance.get_t_class(class_id);
		_icon.GetComponent<card_icon>().m_glevel = glevel;
		_icon.GetComponent<card_icon>().m_jlevel = jlevel;
		_icon.GetComponent<card_icon>().m_level = level;
		_icon.GetComponent<card_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_card_icon(ccard card)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/card_icon");
		_icon.GetComponent<card_icon>().init ();
		_icon.GetComponent<card_icon>().m_card = card;
		_icon.GetComponent<card_icon>().m_star = false;
		_icon.GetComponent<card_icon>().reset ();
		
		//_icon.transform.Find("kuang").gameObject.SetActive(false);
		_icon.transform.Find("lv").gameObject.SetActive(false);
		_icon.transform.Find("glevel").gameObject.SetActive(false);
		_icon.transform.Find("jlevel").gameObject.SetActive(false);

		return _icon;
	}
	

	public GameObject create_card_icon_ex(ccard card)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/card_icon");
		_icon.GetComponent<card_icon>().init ();
		_icon.GetComponent<card_icon>().m_card = card;
		_icon.GetComponent<card_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_card_icon(ulong guid)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/card_icon");
		_icon.GetComponent<card_icon>().init ();
		_icon.GetComponent<card_icon>().m_guid_id = guid;
		_icon.GetComponent<card_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_card_icon_ex(ulong guid)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/card_icon");
		_icon.GetComponent<card_icon>().init ();
		_icon.GetComponent<card_icon>().m_guid_id = guid;
		_icon.GetComponent<card_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_pet_icon(int pet_id, int star, int jlevel, int level)
	{		
		GameObject _icon = game_data._instance.ins_object_res("ui/pet_icon");
		_icon.GetComponent<pet_icon>().init ();
		_icon.GetComponent<pet_icon>().m_t_pet = game_data._instance.get_t_pet(pet_id);
		_icon.GetComponent<pet_icon>().m_star = star;
		_icon.GetComponent<pet_icon>().m_jlevel = jlevel;
		_icon.GetComponent<pet_icon>().m_level = level;
		_icon.GetComponent<pet_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_pet_icon(ulong guid)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/pet_icon");
		_icon.GetComponent<pet_icon>().init ();
		_icon.GetComponent<pet_icon>().m_guid_id = guid;
		_icon.GetComponent<pet_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_pet_icon_ex(ulong guid)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/pet_icon");
		_icon.GetComponent<pet_icon>().init ();
		_icon.GetComponent<pet_icon>().m_guid_id = guid;
		_icon.GetComponent<pet_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_pet_icon_ex(int pet_id, int star, int jlevel, int level)
	{		
		GameObject _icon = game_data._instance.ins_object_res("ui/pet_icon");
		_icon.GetComponent<pet_icon>().init ();
		_icon.GetComponent<pet_icon>().m_t_pet = game_data._instance.get_t_pet(pet_id);
		_icon.GetComponent<pet_icon>().m_star = star;
		_icon.GetComponent<pet_icon>().m_jlevel = jlevel;
		_icon.GetComponent<pet_icon>().m_level = level;
		_icon.GetComponent<pet_icon>().reset ();
		
		return _icon;
	}


	public GameObject create_item_icon(int id, bool type = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/item_icon");
		_icon.GetComponent<item_icon>().init ();
		_icon.GetComponent<item_icon>().m_item_id = id;
		_icon.GetComponent<item_icon>().type = type;
		_icon.GetComponent<item_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_item_icon_ex(int id, bool type = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/item_icon");
		_icon.GetComponent<item_icon>().init ();
		_icon.GetComponent<item_icon>().m_item_id = id;
		_icon.GetComponent<item_icon>().type = type;
		_icon.GetComponent<item_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_item_icon(int id, int num, bool type = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/item_icon");
		_icon.GetComponent<item_icon>().init ();
		_icon.GetComponent<item_icon>().m_item_id = id;
		_icon.GetComponent<item_icon>().m_item_num = num;
		_icon.GetComponent<item_icon>().type = type;
		_icon.GetComponent<item_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_item_icon_ex(int id, int num, bool type = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/item_icon");
		_icon.GetComponent<item_icon>().init ();
		_icon.GetComponent<item_icon>().m_item_id = id;
		_icon.GetComponent<item_icon>().m_item_num = num;
		_icon.GetComponent<item_icon>().type = type;
		_icon.GetComponent<item_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_item_icon(int id, int num, int max, bool type = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/item_icon");
		_icon.GetComponent<item_icon>().init ();
		_icon.GetComponent<item_icon>().m_item_id = id;
		_icon.GetComponent<item_icon>().m_item_num = num;
		_icon.GetComponent<item_icon>().m_max = max;
		_icon.GetComponent<item_icon>().type = type;
		_icon.GetComponent<item_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_item_icon_ex(int id, int num, int max, bool type = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/item_icon");
		_icon.GetComponent<item_icon>().init ();
		_icon.GetComponent<item_icon>().m_item_id = id;
		_icon.GetComponent<item_icon>().m_item_num = num;
		_icon.GetComponent<item_icon>().m_max = max;
		_icon.GetComponent<item_icon>().type = type;
		_icon.GetComponent<item_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_equip_icon(ulong guid)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_guid = guid;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_equip_icon_ex(ulong guid)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_guid = guid;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_equip_icon(int id)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_id = id;
		_icon.GetComponent<equip_icon>().m_show_enhance = false;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_equip_icon_ex(int id)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_id = id;
		_icon.GetComponent<equip_icon>().m_show_enhance = false;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_equip_icon(int id, int enhance)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_id = id;
		_icon.GetComponent<equip_icon>().m_enhance = enhance;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_equip_icon_ex(int id, int enhance)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_id = id;
		_icon.GetComponent<equip_icon>().m_enhance = enhance;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_equip_icon(int id, int enhance, int jl)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_id = id;
		_icon.GetComponent<equip_icon>().m_enhance = enhance;
		_icon.GetComponent<equip_icon>().m_jl = jl;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_equip_icon_ex(int id, int enhance, int jl)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip_id = id;
		_icon.GetComponent<equip_icon>().m_enhance = enhance;
		_icon.GetComponent<equip_icon>().m_jl = jl;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_equip_icon(dhc.equip_t equip)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().m_equip = equip;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_equip_icon_ex(dhc.equip_t equip, bool flag = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/equip_icon");
		_icon.GetComponent<equip_icon>().init ();
		_icon.GetComponent<equip_icon>().flag = flag;
		_icon.GetComponent<equip_icon>().m_equip = equip;
		_icon.GetComponent<equip_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_resource_icon(int type, int num)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/resource_icon");
		_icon.GetComponent<resource_icon>().init ();
		_icon.GetComponent<resource_icon>().m_type = type;
		_icon.GetComponent<resource_icon>().m_num = num;
		_icon.GetComponent<resource_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_resource_icon_ex(int type, int num)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/resource_icon");
		_icon.GetComponent<resource_icon>().init ();
		_icon.GetComponent<resource_icon>().m_type = type;
		_icon.GetComponent<resource_icon>().m_num = num;
		_icon.GetComponent<resource_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_resource_icon(int type, int num, int max)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/resource_icon");
		_icon.GetComponent<resource_icon>().init ();
		_icon.GetComponent<resource_icon>().m_type = type;
		_icon.GetComponent<resource_icon>().m_num = num;
		_icon.GetComponent<resource_icon>().m_max = max;
		_icon.GetComponent<resource_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_resource_icon_ex(int type, int num, int max)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/resource_icon");
		_icon.GetComponent<resource_icon>().init ();
		_icon.GetComponent<resource_icon>().m_type = type;
		_icon.GetComponent<resource_icon>().m_num = num;
		_icon.GetComponent<resource_icon>().m_max = max;
		_icon.GetComponent<resource_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_dressrole_icon(int id)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/dressrole_icon");
		_icon.GetComponent<dressrole_icon>().init ();
		_icon.GetComponent<dressrole_icon>().m_id = id;
		_icon.GetComponent<dressrole_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_dressrole_icon_ex(int id)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/dressrole_icon");
		_icon.GetComponent<dressrole_icon>().init ();
		_icon.GetComponent<dressrole_icon>().m_id = id;
		_icon.GetComponent<dressrole_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_dress_icon_ex(int id,int type)
	{
		if(game_data._instance.get_t_dress (id) == null)
		{
			return null;
		}
		GameObject _icon = game_data._instance.ins_object_res("ui/dress_icon");
		if(type == 1)
		{
			_icon.transform.Find("Sprite").GetComponent<UISprite>().spriteName  = "people_card01";
		}
		else
		{
			_icon.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "nszjm_szblz04";
		}
		_icon.GetComponent<dress_icon>().init ();
		_icon.GetComponent<dress_icon>().m_id = id;
		_icon.GetComponent<dress_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon(ulong guid,bool flag = true)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_show_sx = flag;
		_icon.GetComponent<treasure_icon>().m_treasure_guid = guid;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_treasure_icon_ex(ulong guid)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure_guid = guid;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon_ex(dhc.treasure_t treasure, bool flag = false)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().flag = flag;
		_icon.GetComponent<treasure_icon>().m_treasure = treasure;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon(int id)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure_id = id;
		_icon.GetComponent<treasure_icon>().m_show_enhance = false;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	
	public GameObject create_treasure_icon_ex(int id)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure_id = id;
		_icon.GetComponent<treasure_icon>().m_show_enhance = false;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon(dhc.treasure_t treasure)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure = treasure;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon_ex(dhc.treasure_t treasure)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure = treasure;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon(int id, int num, int max, bool flag)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure_id = id;
		_icon.GetComponent<treasure_icon>().m_num = num;
		_icon.GetComponent<treasure_icon>().m_max = max;
		_icon.GetComponent<treasure_icon>().m_show_enhance = flag;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}
	
	public GameObject create_treasure_icon_ex(int id, int num, int max, bool flag)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure_id = id;
		_icon.GetComponent<treasure_icon>().m_num = num;
		_icon.GetComponent<treasure_icon>().m_max = max;
		_icon.GetComponent<treasure_icon>().m_show_enhance = flag;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon(int id, int enhance, int jinglian)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure_id = id;
		_icon.GetComponent<treasure_icon>().m_enhance = enhance;
		_icon.GetComponent<treasure_icon>().m_jl = jinglian;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_treasure_icon_ex(int id, int enhance, int jinglian)
	{
		GameObject _icon = game_data._instance.ins_object_res("ui/treasure_icon");
		_icon.GetComponent<treasure_icon>().init ();
		_icon.GetComponent<treasure_icon>().m_treasure_id = id;
		_icon.GetComponent<treasure_icon>().m_enhance = enhance;
		_icon.GetComponent<treasure_icon>().m_jl = jinglian;
		_icon.GetComponent<treasure_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_reward_icon(int type, int value1, int value2, int value3)
	{
		if (type == 1)
		{
			return create_resource_icon(value1, value2 + value3 * sys._instance.m_self.m_t_player.level);
		}
		else if (type == 2)
		{
			return create_item_icon(value1, value2);
		}
		else if (type == 3)
		{
			return create_card_icon(value1, value3, value3, value2);
		}
		else if (type == 4)
		{
			return create_equip_icon(value1);
		}
		else if (type == 5)
		{
			return create_dressrole_icon(value1);
		}
		else if (type == 6)
		{
			return create_treasure_icon(value1);
		}
		else if(type == 7)
		{
			return create_guanghuan_icon(value1,value2);
		}
        else if (type == 8)
        {
            return create_chenghao_icon(value1,value3);
        }
		return null;
	}

	public GameObject create_reward_icon(int type, int value1, int value2, int value3,int num)
	{
		if (type == 1)
		{
			return create_resource_icon(value1, num,value2 + value3 * sys._instance.m_self.m_t_player.level);
		}
		else if (type == 2)
		{
			return create_item_icon(value1,num,value2);
		}
		return null;
	}

	public GameObject create_skill_icon(int id)
	{
		s_t_skill _skill = game_data._instance.get_t_skill(id);

		if(_skill == null)
		{
			return null;
		}

		GameObject _icon = game_data._instance.ins_object_res("ui/skill_icon");

		_icon.GetComponent<skill_icon>().init ();
		_icon.GetComponent<skill_icon>().m_skill = _skill;
		_icon.GetComponent<skill_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_skill_icon(role_skill skill)
	{
		s_t_skill _skill = game_data._instance.get_t_skill(skill.m_t_skill.id);
		
		if(_skill == null)
		{
			return null;
		}
		
		GameObject _icon = game_data._instance.ins_object_res("ui/skill_icon");
		
		_icon.GetComponent<skill_icon>().init ();
		_icon.GetComponent<skill_icon>().m_skill = _skill;
		_icon.GetComponent<skill_icon>().m_role_skill = skill;
		_icon.GetComponent<skill_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_guanghuan_icon(int id,int level)
	{
		s_t_guanghuan _guanghuan = game_data._instance.get_t_guanghuan(id);
		
		if(_guanghuan == null)
		{
			return null;
		}
		
		GameObject _icon = game_data._instance.ins_object_res("ui/guanghuan_icon");
		
		_icon.GetComponent<guanghuan_icon>().init ();
		_icon.GetComponent<guanghuan_icon>().level = level;
		_icon.GetComponent<guanghuan_icon>().m_guanghuan = _guanghuan;
		_icon.GetComponent<guanghuan_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_guanghuan_icon_ex(int id)
	{
		s_t_guanghuan _guanghuan = game_data._instance.get_t_guanghuan(id);
		
		if(_guanghuan == null)
		{
			return null;
		}
		
		GameObject _icon = game_data._instance.ins_object_res("ui/guanghuan_icon");
		
		_icon.GetComponent<guanghuan_icon>().init ();
		_icon.GetComponent<guanghuan_icon>().m_guanghuan = _guanghuan;
		_icon.GetComponent<guanghuan_icon>().reset ();
		
		return _icon;
	}

	public GameObject create_reward_icon_ex_update(int type, int value1, int value2, int value3)
	{
		if (type == 1)
		{
			return create_resource_icon_ex(value1, value2 + value3 * 1);
		}
		else if (type == 2)
		{
			if (value3 == 0)
			{
				return create_item_icon_ex(value1, value2);
			}
			else
			{
				return create_item_icon_ex(value1, value2, true);
			}
		}
		else if (type == 3)
		{
			return create_card_icon_ex(value1, value3, value3, value2);
		}
		else if (type == 4)
		{
			return create_equip_icon_ex(value1);
		}
		else if (type == 5)
		{
			return create_dressrole_icon_ex(value1);
		}
		else if (type == 6)
		{
			return create_treasure_icon_ex(value1);
		}
		else if (type == 7)
		{
			return create_guanghuan_icon_ex(value1);
		}
		else if (type == 9)
		{
			return create_dress_icon_ex(value1,1);
		}
		else if (type == 11)
		{
			return create_pet_icon_ex(value1, value3, value3, value2);
		}
		return null;
	}

	public GameObject create_reward_icon_ex(int type, int value1, int value2, int value3)
	{
		if (type == 1)
		{
			return create_resource_icon_ex(value1, value2 + value3 * sys._instance.m_self.m_t_player.level);
		}
		else if (type == 2)
		{
			if (value3 == 0)
			{
				return create_item_icon_ex(value1, value2);
			}
			else
			{
				return create_item_icon_ex(value1, value2, true);
			}
		}
		else if (type == 3)
		{
			return create_card_icon_ex(value1, value3, value3, value2);
		}
		else if (type == 4)
		{
			return create_equip_icon_ex(value1);
		}
		else if (type == 5)
		{
			return create_dressrole_icon_ex(value1);
		}
		else if (type == 6)
		{
			return create_treasure_icon_ex(value1);
		}
		else if (type == 7)
		{
			return create_guanghuan_icon_ex(value1);
		}
		else if (type == 9)
		{
			return create_dress_icon_ex(value1,1);
		}
		else if (type == 11)
		{
			return create_pet_icon_ex(value1, value3, value3, value2);
		}
		return null;
	}
}
