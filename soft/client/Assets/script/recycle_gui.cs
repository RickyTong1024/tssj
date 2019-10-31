
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class recycle_gui : MonoBehaviour,IMessage{

	public GameObject m_equip_page_gui;
	public GameObject m_card_page_gui;
    public GameObject m_treasure_page_gui ;
	private GameObject m_pet_page_gui;
	public GameObject m_effect;
	public GameObject m_gold;
	public GameObject m_mw;
	public GameObject m_shop_panel;
	public GameObject m_zhanhun_panel;
	public UILabel m_zhanhun;
	public GameObject m_jiegu;
	public GameObject m_fenjie;
	private List<ulong> m_shells = new List<ulong>();
	private static List<ulong> m_jiyins = new List<ulong>();
	private static List<ulong> m_cards = new List<ulong>();
	private int m_add_gold = 0;
	private int m_add_zhanhun = 0;
	private int m_add_mw = 0;
	private int m_item_id;
	private ccard m_current_card;
	public GameObject m_equip_huishou;
    public ccard m_card;
	public pet m_pet;
    public GameObject m_obj = null;
    public UILabel m_desc;
	public UILabel m_huoban_l;
	public UILabel m_huobanl2;
	public UILabel m_jiyin_l;
	public UILabel m_jiyin_l2;
	public UILabel m_zhuangbei_l;
	public UILabel m_zhuangbei_l2;
	public UILabel m_jiugu_l;
	public UILabel m_jiugu_l0;
	public UILabel m_jiegu_l2;
	public UILabel m_yijian_fenjie;
	public UILabel m_fenjie_l;
	public UILabel m_jiegu_l3;
	public UILabel m_fenjie3;
	public UILabel m_fenjie4;
	public UILabel m_info;
	
	// Use this for initialization
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{
        equip_fenjie();
	}
	
	public void init()
	{
		m_equip_page_gui = game_data._instance.ins_object_res("ui/equip_page_gui");
		m_equip_page_gui.transform.parent = this.transform;
		m_equip_page_gui.transform.localPosition = new Vector3(0,0,0);
		m_equip_page_gui.transform.localScale = new Vector3(1,1,1);
		m_equip_page_gui.GetComponent<equip_page_gui>().init ();
		m_equip_page_gui.SetActive (false);

        m_treasure_page_gui = game_data._instance.ins_object_res("ui/treasure_page_gui");
        m_treasure_page_gui.transform.parent = this.transform;
        m_treasure_page_gui.transform.localPosition = new Vector3(0, 0, 0);
        m_treasure_page_gui.transform.localScale = new Vector3(1, 1, 1);
        m_treasure_page_gui.GetComponent<treasure_page_gui>().init();
        m_treasure_page_gui.SetActive(false);

		m_card_page_gui = game_data._instance.ins_object_res("ui/card_page_gui");
		m_card_page_gui.transform.parent = this.transform;
		m_card_page_gui.transform.localPosition = new Vector3(0,0,0);
		m_card_page_gui.transform.localScale = new Vector3(1,1,1);
		m_card_page_gui.SetActive (false);

		
		m_pet_page_gui = game_data._instance.ins_object_res("ui/pet_page_gui");
		m_pet_page_gui.transform.parent = this.transform;
		m_pet_page_gui.transform.localPosition = new Vector3(0,0,0);
		m_pet_page_gui.transform.localScale = new Vector3(1,1,1);
		m_pet_page_gui.SetActive (false);
		role_chongsheng();
		m_zhanhun_panel.SetActive(false);
		m_shop_panel.SetActive (false);
		m_jiyins.Clear ();
	}
	
	int comp(int x, int y)
	{
		if(x > y)
		{
			return 1;
		}
		return -1;
	}
	public void hide_page()
	{
		m_shop_panel.SetActive(false);
		Transform _obj = this.transform.Find("main_button");
        _obj.Find("equip_fenjie").GetComponent<UIToggle>().value = true;
	}
	
	public void click(GameObject obj)
	{

		if(obj.transform.name == "hide_equip")
		{
			hide_page();
			m_cards.Clear();
			m_jiyins.Clear();
			m_shells.Clear();
			transform.GetComponent<ui_show_anim>().hide_ui();
            s_message _message = new s_message();
            _message.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_message);
		}
		else if (obj.transform.name == "equip_fenjie")
		{
            equip_fenjie();
		}
        else if (obj.name == "equip_chongsheng")
        {
            equip_chongsheng();
        }
        else if (obj.transform.name == "role_chongsheng")
        {
            role_chongsheng();
        }
        else if (obj.name == "baowu_chongsheng")
        {
            baowu_chongshneg();
        }
        else if (obj.transform.name == "role_jiegu")
        {
            role_jiegu();
        }
		else if (obj.transform.name == "pet_chongsheng")
		{
			pet_chongsheng();
		}
		else if (obj.transform.name == "pet_fenjie")
		{
			pet_fenjie();
		}
        else if (obj.transform.name == "role_yijian_jiegu")
        {
            s_message _mes = new s_message();
            _mes.m_type = "role_yijian_jiegu";
            m_cards.Clear();
            List<ccard> _cards = m_card_page_gui.GetComponent<card_page_gui>().m_cards;
            for (int i = 0; i < _cards.Count; i++)
            {
                if (_cards[i].m_t_class.color < 4)
                {
                    m_cards.Add((ulong)_cards[i].m_t_class.id);
                    _mes.m_long.Add(_cards[i].get_guid());
                }

            }
            if (m_cards.Count <= 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_174_71"));//没有符合条件的伙伴
                return;
            }
            cmessage_center._instance.add_message(_mes);
        }
        else if (obj.name == "equip_yijian_fenjie")
        {
            s_message _mes = new s_message();
            _mes.m_type = "equip_yijian_fenjie";
            m_shells.Clear();
            List<dhc.equip_t> _cards = m_equip_page_gui.GetComponent<equip_page_gui>().m_equips;
            for (int i = 0; i < _cards.Count; i++)
            {
                if (game_data._instance.get_t_equip(_cards[i].template_id).font_color <= 2)
                {
                    m_shells.Add(_cards[i].guid);
                    _mes.m_long.Add(_cards[i].guid);
                }

            }
            if (m_shells.Count <= 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_196_71"));//没有符合条件的装备
                return;
            }
            cmessage_center._instance.add_message(_mes);
 
        }
        else if (obj.transform.name == "shui_pian")
        {
            m_zhanhun_panel.SetActive(true);

        }
        else if (obj.transform.name == "yijian_jiegu")
        {
            s_message _msg = new s_message();
            _msg.m_type = "select_dialog_box";
            _msg.m_string.Add(game_data._instance.get_t_language ("arena.cs_104_45"));//提示
            _msg.m_string.Add(game_data._instance.get_t_language ("recycle_gui.cs_212_30"));//是否解约所有蓝色和紫色质量伙伴？
            s_message _out_msg = new s_message();
            _out_msg.m_type = "yijian_jiegu";
            _msg.m_object.Add(_out_msg);
            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "sell")
        {
            if (m_shells.Count == 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_222_71"));//请先选择需要回收的装备
                return;
            }

            s_message _msg = new s_message();
            _msg.m_type = "select_dialog_box";
            _msg.m_string.Add(game_data._instance.get_t_language ("arena.cs_104_45"));//提示
            _msg.m_string.Add(game_data._instance.get_t_language ("recycle_gui.cs_229_30") + "？");//是否回收所选择的装备
            s_message _out_msg = new s_message();
            _msg.m_object.Add(_out_msg);
            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "fenjie")
        {
            if (m_jiyins.Count == 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_238_71"));//请选择回收的基因
                return;
            }

            s_message _msg = new s_message();
            _msg.m_type = "select_dialog_box";
            _msg.m_string.Add(game_data._instance.get_t_language ("arena.cs_104_45"));//提示
            _msg.m_string.Add(game_data._instance.get_t_language ("recycle_gui.cs_245_30"));//是否回收基因？

            s_message _out_msg = new s_message();
            _out_msg.m_type = "shell_jiyin";
            _msg.m_object.Add(_out_msg);
            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "jiegu_huoban")
        {
            if (m_cards.Count == 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_256_71"));//请选择解约伙伴
                return;
            }

            s_message _msg = new s_message();
            _msg.m_type = "select_dialog_box";
            _msg.m_string.Add(game_data._instance.get_t_language ("arena.cs_104_45"));//提示
            _msg.m_string.Add(game_data._instance.get_t_language ("recycle_gui.cs_263_30"));//是否解约伙伴？

            s_message _out_msg = new s_message();
            _out_msg.m_type = "jiegu_huoban";
            _msg.m_object.Add(_out_msg);
            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "yijian_sell")
        {
            s_message _msg = new s_message();
            _msg.m_type = "select_dialog_box";
            _msg.m_string.Add(game_data._instance.get_t_language ("arena.cs_104_45"));//提示
            _msg.m_string.Add(game_data._instance.get_t_language ("recycle_gui.cs_275_30") + "？");//是否回收所有绿色、蓝色质量装备

            s_message _out_msg = new s_message();
            _out_msg.m_type = "yijian_shell";
            _msg.m_object.Add(_out_msg);
            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "yijian_fenjie")
        {
            s_message _msg = new s_message();
            _msg.m_type = "select_dialog_box";
            _msg.m_string.Add(game_data._instance.get_t_language ("arena.cs_104_45"));//提示
            _msg.m_string.Add(game_data._instance.get_t_language ("recycle_gui.cs_287_30") + "？");//是否回收所有蓝色质量基因

            s_message _out_msg = new s_message();
            _out_msg.m_type = "yijian_fenjie";
            _msg.m_object.Add(_out_msg);
            cmessage_center._instance.add_message(_msg);
        }
	}
	void role_chongsheng()
	{
        m_info.text = game_data._instance.get_t_language ("recycle_gui.cs_297_22");//重生返还初始伙伴的基因以及养成消耗的大量资源
		m_desc.text = game_data._instance.get_t_language ("recycle_gui.cs_298_16");//伙伴重生后将返回初始状态，返还所有资源
		List<ulong> self = new List<ulong>();
		for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
		{
			//ccard _card = sys._instance.m_self.get_card_guid()
			if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
			{
				self.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
			}
		}
        for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; i++)
        {
            self.Add(sys._instance.m_self.m_t_player.houyuan[i]);
        }
        for (int i = 0; i < sys._instance.m_self.get_card_num(); i++)
        {
            ccard _card = sys._instance.m_self.get_card_index(i);
            bool flag = false;
            for (int j = 0; j < _card.get_role().jskill_level.Count; j++)
            {
                if (_card.get_role().jskill_level[j] > 1)
                {
                    flag = true;
                    break;
                }


            }
            if (_card.get_glevel() == 0 && _card.get_jlevel() == 0 && _card.get_level() == 1 && !flag)
            {
                self.Add(_card.get_guid());
            }

        }
        m_treasure_page_gui.SetActive(false);
		m_card_page_gui.SetActive(true);
		m_pet_page_gui.SetActive (false);
		m_equip_page_gui.SetActive (false);
		m_shop_panel.SetActive (false);
		m_zhanhun_panel.SetActive (false);
		m_card_page_gui.GetComponent<card_page_gui>().init();
		m_card_page_gui.GetComponent<card_page_gui>().m_hide_guids = self;
		m_card_page_gui.GetComponent<card_page_gui>().set_text("");
		m_card_page_gui.GetComponent<card_page_gui>().m_out_message = "show_cs_gui";
		m_card_page_gui.GetComponent<card_page_gui>().card_reset();
	}
    void baowu_chongshneg()
    {
        m_info.text = game_data._instance.get_t_language ("recycle_gui.cs_346_22");//重生返还初始饰品以及养成消耗的所有资源（钻石不返还）
        List<ulong> self = new List<ulong>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.treasures.Count; ++i)
        {

            dhc.treasure_t treasure = sys._instance.m_self.get_treasure_guid(sys._instance.m_self.m_t_player.treasures[i]);
            if ( treasure.jilian == 0 && treasure.enhance == 0 && treasure.star_exp == 0 && treasure.star == 0)
            {
                self.Add(sys._instance.m_self.m_t_player.treasures[i]);
            }
            else if (treasure.role_guid != 0)
            {
                if (!self.Contains(treasure.guid))
                {
                    self.Add(treasure.guid);
                }
            }
            else if (treasure.template_id == 12001 || treasure.template_id == 13001)
            {
                self.Add(treasure.guid);
            }
        }
       
        m_treasure_page_gui.SetActive(true);
        m_card_page_gui.SetActive(false);
		m_pet_page_gui.SetActive (false);
        m_equip_page_gui.SetActive(false);
        m_shop_panel.SetActive(false);
        m_zhanhun_panel.SetActive(false);
        m_treasure_page_gui.GetComponent<treasure_page_gui>().init();
        m_treasure_page_gui.GetComponent<treasure_page_gui>().m_show_remove = false;
        m_treasure_page_gui.GetComponent<treasure_page_gui>().m_show_lock = false;
        m_treasure_page_gui.GetComponent<treasure_page_gui>().set_text("");
        m_treasure_page_gui.GetComponent<treasure_page_gui>().m_hide_guids = self;
        m_treasure_page_gui.GetComponent<treasure_page_gui>().m_out_message = "show_treasure_cs";
        m_treasure_page_gui.GetComponent<treasure_page_gui>().treasure_reset();

    }

	void pet_chongsheng()
	{
		m_info.text = game_data._instance.get_t_language ("recycle_gui.cs_387_16");//重生返还养成消耗的大量资源
		m_desc.text = game_data._instance.get_t_language ("recycle_gui.cs_388_16");//宠物重生后将返回初始状态，返还所有资源
		List<ulong> self = new List<ulong>();
		if (sys._instance.m_self.m_t_player.pet_on != 0)
		{
			self.Add(sys._instance.m_self.m_t_player.pet_on);
		}
		for(int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count;++i)
		{
			ccard _card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[i]);
			if(_card == null)
			{
				continue;
			}
			if(_card.get_role().pet > 0)
			{
				self.Add(_card.get_role().pet);
			}
		}
		m_treasure_page_gui.SetActive(false);
		m_card_page_gui.SetActive(false);
		m_equip_page_gui.SetActive (false);
		m_shop_panel.SetActive (false);
		m_pet_page_gui.SetActive (true);
		m_zhanhun_panel.SetActive (false);
		m_pet_page_gui.GetComponent<pet_page_gui>().init();
		m_pet_page_gui.GetComponent<pet_page_gui>().m_hide_guids = self;
		m_pet_page_gui.GetComponent<pet_page_gui>().set_text("");
		m_pet_page_gui.GetComponent<pet_page_gui>().m_out_message = "show_pet_cs_gui";
		m_pet_page_gui.GetComponent<pet_page_gui>().pet_reset();
	}

	void pet_fenjie()
	{
		m_info.text = game_data._instance.get_t_language ("recycle_gui.cs_421_16");//分解闲置宠物可返还大量数码芯片和养成资源
		m_desc.text = game_data._instance.get_t_language ("recycle_gui.cs_422_16");//宠物解约后合成、升星消耗的碎片以半价的数码芯片形式返还
		List<ulong> self = new List<ulong>();
		if (sys._instance.m_self.m_t_player.pet_on != 0)
		{
			self.Add(sys._instance.m_self.m_t_player.pet_on);
		}
		for(int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count;++i)
		{
			ccard _card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[i]);
			if(_card == null)
			{
				continue;
			}
			if(_card.get_role().pet > 0)
			{
				self.Add(_card.get_role().pet);
			}
		}
		m_treasure_page_gui.SetActive(false);
		m_card_page_gui.SetActive(false);
		m_pet_page_gui.SetActive (true);
		m_equip_page_gui.SetActive (false);
		m_shop_panel.SetActive (false);
		m_zhanhun_panel.SetActive (true);
		m_add_zhanhun = 0;
		m_zhanhun.text = "0";
		m_cards.Clear ();
		m_jiegu.SetActive (true);
		
		m_treasure_page_gui.SetActive(false);
		m_card_page_gui.SetActive(false);
		m_equip_page_gui.SetActive (false);
		m_shop_panel.SetActive (false);
		m_pet_page_gui.SetActive (true);
		m_zhanhun_panel.SetActive (false);
		m_pet_page_gui.GetComponent<pet_page_gui>().init();
		m_pet_page_gui.GetComponent<pet_page_gui>().m_hide_guids = self;
		m_pet_page_gui.GetComponent<pet_page_gui>().set_text("");
		m_pet_page_gui.GetComponent<pet_page_gui>().m_out_message = "show_pet_fenjie_gui";
		m_pet_page_gui.GetComponent<pet_page_gui>().pet_reset();
	}

	void role_jiegu()
	{
        m_info.text = game_data._instance.get_t_language ("recycle_gui.cs_466_22");//分解闲置伙伴可返还大量战魂和养成资源
		m_desc.text = game_data._instance.get_t_language ("recycle_gui.cs_467_16");//伙伴解约后可获得战魂，一键解约所有蓝色和紫色质量伙伴
		List<ulong> self = new List<ulong>();
		for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
		{
			//ccard _card = sys._instance.m_self.get_card_guid()
			if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
			{
				self.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
			}
		}
        for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; i++)
        {
            self.Add(sys._instance.m_self.m_t_player.houyuan[i]);
        }
        m_treasure_page_gui.SetActive(false);
		m_card_page_gui.SetActive(true);
		m_pet_page_gui.SetActive (false);
		m_equip_page_gui.SetActive (false);
		m_shop_panel.SetActive (false);
		m_zhanhun_panel.SetActive (true);
		m_add_zhanhun = 0;
		m_zhanhun.text = "0";
		m_cards.Clear ();
		m_jiegu.SetActive (true);
		
		m_card_page_gui.GetComponent<card_page_gui>().init();
		m_card_page_gui.GetComponent<card_page_gui>().m_hide_guids = self;
		m_card_page_gui.GetComponent<card_page_gui>().set_text("");
		m_card_page_gui.GetComponent<card_page_gui>().m_out_message = "show_jiegu_gui";
		m_card_page_gui.GetComponent<card_page_gui>().jiegu_reset();
	}

    public void equip_chongsheng()
    {
        m_info.text = game_data._instance.get_t_language ("recycle_gui.cs_501_22");//重生返还初始装备的碎片以及养成消耗的大量资源
        m_desc.text = game_data._instance.get_t_language ("recycle_gui.cs_502_22");//装备回收获得合金，返还强化消耗金币，一键回收所有绿色和蓝色质量装备
        List<ulong> self = new List<ulong>();
		m_jiyins.Clear ();
		m_gold.GetComponent<UILabel>().text = "0";
		m_mw.GetComponent<UILabel>().text = "0";
		m_shop_panel.SetActive (false);
		m_zhanhun_panel.SetActive (false);
		m_equip_page_gui.SetActive(true);
		m_card_page_gui.SetActive (false);
		m_pet_page_gui.SetActive (false);
        m_treasure_page_gui.SetActive(false);
		m_equip_page_gui.GetComponent<equip_page_gui>().init();
		m_equip_page_gui.GetComponent<equip_page_gui>().set_text("");
        m_equip_page_gui.GetComponent<equip_page_gui>().m_hide_guids = self;
		m_equip_page_gui.GetComponent<equip_page_gui>().m_show_lock = false;
		m_equip_page_gui.GetComponent<equip_page_gui>().m_out_message = "show_equip_cs";
		m_equip_page_gui.GetComponent<equip_page_gui>().equip_reset();
    }
    public void equip_fenjie()
    {
        m_info.text = game_data._instance.get_t_language ("recycle_gui.cs_523_22");//分解闲置装备可返还大量合金和养成资源
        m_desc.text = game_data._instance.get_t_language ("recycle_gui.cs_502_22");//装备回收获得合金，返还强化消耗金币，一键回收所有绿色和蓝色质量装备
        List<ulong> self = new List<ulong>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.equips.Count; ++i)
        {

            dhc.equip_t equip = sys._instance.m_self.get_equip_guid(sys._instance.m_self.m_t_player.equips[i]);
            if (equip.role_guid != 0)
            {
                if (!self.Contains(equip.guid))
                {
                    self.Add(equip.guid);
                }
            }
        }
        m_jiyins.Clear();
        m_gold.GetComponent<UILabel>().text = "0";
        m_mw.GetComponent<UILabel>().text = "0";
        m_shop_panel.SetActive(true);
        m_zhanhun_panel.SetActive(false);
        m_equip_page_gui.SetActive(true);
        m_card_page_gui.SetActive(false);
		m_pet_page_gui.SetActive (false);
        m_treasure_page_gui.SetActive(false);
        m_equip_page_gui.GetComponent<equip_page_gui>().init();
        m_equip_page_gui.GetComponent<equip_page_gui>().set_text("");
        m_equip_page_gui.GetComponent<equip_page_gui>().m_show_lock = false;
        m_equip_page_gui.GetComponent<equip_page_gui>().m_hide_guids = self;
        m_equip_page_gui.GetComponent<equip_page_gui>().m_out_message = "show_equip_fenjie";
        m_equip_page_gui.GetComponent<equip_page_gui>().equip_reset();
    }
    public void treasure_shongsheng()
    {
 
    }
    public static bool is_injiyins(int id)
    {
        for (int i = 0; i < m_jiyins.Count; i++)
        {
            if (id == (int)m_jiyins[i])
            {
                return true;
            }
        }
        return false;
    }
	public static bool is_jiegus(ulong id)
	{
		for(int i = 0;i < m_cards.Count;i++)
		{
			if(id == (ulong)m_cards[i])
			{
				return true;
			}
		}
		return false;
	}

	void IMessage.net_message(s_net_message message)
	{

	  if(message.m_opcode == opclient_t.CMSG_EQUIP_SELL)
		{
            protocol.game.smsg_equip_init _msg = net_http._instance.parse_packet<protocol.game.smsg_equip_init>(message.m_byte);
            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i]);

            }
            for (int i = 0; i < _msg.roles.Count; i++)
            {
                sys._instance.m_self.add_card(_msg.roles[i]);

            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i]);
            }
            for (int i = 0; i < _msg.types.Count; i++)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("common_chongsheng_gui.cs_140_22"));//装备分解

            }
			for(int i = 0;i < m_shells.Count;i ++)
			{
				dhc.equip_t equip = sys._instance.m_self.get_equip_guid(m_shells[i]);
				sys._instance.m_self.remove_equip(m_shells[i]);
			}
			m_shells.Clear();
            equip_fenjie();
		}
		if(message.m_opcode == opclient_t.CMSG_ITEM_FENJIE)
		{
			for(int i = 0; i < m_jiyins.Count;i++)
			{
				s_t_item _item = game_data._instance.get_item ((int)m_jiyins[i]);
				int _num = sys._instance.m_self.get_item_num ((uint)m_jiyins[i]);
				sys._instance.m_self.remove_item((uint)m_jiyins[i],_num,game_data._instance.get_t_language ("recycle_gui.cs_632_60"));//物品分解消耗
			}
			sys._instance.m_self.add_att(e_player_attr.player_jjc_point,m_add_zhanhun);
		}
		if(message.m_opcode == opclient_t.CMSG_ROLE_FENJIE)
		{
			for(int i = 0;i < m_cards.Count;i ++)
			{
				sys._instance.m_self.remove_card(sys._instance.m_self.get_card_guid((ulong)m_cards[i]));
			}
            protocol.game.smsg_role_init _msg = net_http._instance.parse_packet<protocol.game.smsg_role_init>(message.m_byte);
            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i]);

            }
            for (int i = 0; i < _msg.roles.Count; i++)
            {
                sys._instance.m_self.add_card(_msg.roles[i]);

            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i]);
            }
            for (int i = 0; i < _msg.types.Count; i++)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("recycle_gui.cs_659_113"));//角色分解
            }
			role_jiegu();

		}
        if (message.m_opcode == opclient_t.CMSG_EQUIP_INIT)
        {
            protocol.game.smsg_equip_init _msg = net_http._instance.parse_packet<protocol.game.smsg_equip_init>(message.m_byte);
            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i],true);

            }
            for (int i = 0; i < _msg.roles.Count; i++)
            {
                sys._instance.m_self.add_card(_msg.roles[i]);

            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i]);
            }
            for (int i = 0; i < _msg.types.Count; i++)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("common_chongsheng_gui.cs_108_22"));//装备重生
 
            }
            for (int i = 0; i < m_shells.Count; i++)
            {
                if (equip.get_equip_frame_id(sys._instance.m_self.get_equip_guid(m_shells[i])) != -1)
                {
                    sys._instance.m_self.remove_equip((ulong)m_shells[i]);//特殊处理的

                }
                else
                {
                    dhc.equip_t _equip = sys._instance.m_self.get_equip_guid(m_shells[i]);
                    _equip.jilian = 0;
                    _equip.star = 0;
                    _equip.enhance = 0;
                    _equip.gaizao_counts = 0;
                    _equip.rand_ids.Clear();
                    _equip.rand_values.Clear();
 
                }
               
            }
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, 50, game_data._instance.get_t_language ("recycle_gui.cs_706_73"));//装备重生消耗
            equip_chongsheng();
        }
        if (message.m_opcode == opclient_t.CMSG_TREASURE_INIT)
        {
            protocol.game.smsg_treasure_init _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_init>(message.m_byte);
            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i]);
 
            }
            for (int i = 0; i < _msg.roles.Count; i++)
            {
                sys._instance.m_self.add_card(_msg.roles[i]);

            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i]);
            }
            for (int i = 0; i < _msg.types.Count; i++)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("recycle_gui.cs_728_113"));//宝物重生

            }
            for (int i = 0; i < m_shells.Count; i++)
            {
                sys._instance.m_self.remove_treasure((ulong)m_shells[i]);
            }
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, 50, game_data._instance.get_t_language ("recycle_gui.cs_735_73"));//宝物重生消耗
            baowu_chongshneg();
        }
        if (message.m_opcode == opclient_t.CMSG_ROLE_CHONGSHENG)
        {
            protocol.game.smsg_role_init _msg = net_http._instance.parse_packet<protocol.game.smsg_role_init>(message.m_byte);
            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i]);

            }
            for (int i = 0; i < _msg.roles.Count; i++)
            {
                sys._instance.m_self.add_card(_msg.roles[i]);

            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i]);
            }
            for (int i = 0; i < _msg.types.Count; i++)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("recycle_gui.cs_757_113"));//角色重生
            }
            m_card.get_role().glevel = 0;
            for (int i = 0; i < m_card.get_role().jskill_level.Count; i++)
            {
                m_card.get_role().jskill_level[i] = 1;
            }
			for(int i = 0; i < m_card.get_role().dress_ids.Count;)
			{
				s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress(m_card.get_role().dress_ids[i]);
				if(t_role_dress == null)
				{
					i++;
					continue;
				}
				if(t_role_dress.hq_condition == 5 || t_role_dress.hq_condition == 4)
				{
					i++;
					continue;
				}
				m_card.get_role().dress_ids.RemoveAt(i);
			}
            m_card.get_role().dress_on_id = 0;
            m_card.get_role().jlevel = 0;
            m_card.get_role().level = 1;
            m_card.get_role().pinzhi = m_card.m_t_class.pz * 100;
			int num = 0;
			for(int i = 0; i <= m_card.get_role().bskill_level ;++i)
			{
				s_t_role_skillunlock t_skillunlock = game_data._instance.get_t_role_skillunlock (m_card.get_template_id(),i);
				if(t_skillunlock == null)
				{
					continue;
				}
				for(int j =0 ;j < t_skillunlock.role_skillunlock_tasks.Count; ++j)
				{
					if(t_skillunlock.role_skillunlock_tasks[j].task_type <= m_card.get_role().bskill_counts.Count)
					{
						if(m_card.get_role().bskill_counts[t_skillunlock.role_skillunlock_tasks[j].task_type -1] < t_skillunlock.role_skillunlock_tasks[j].def1)
						{
							m_card.get_role().bskill_counts[t_skillunlock.role_skillunlock_tasks[j].task_type -1] = 0;
						}
					}
				}
				num++;
			}
			for(int i = num; i < m_card.get_role().bskill_counts.Count ;++i)
			{
				m_card.get_role().bskill_counts[i] = 0;
			}
			m_card.get_role().bskill_level = 0;
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, 50, game_data._instance.get_t_language ("recycle_gui.cs_808_73"));//角色重生消耗

            s_message _message1 = new s_message();
            _message1.m_type = "update_recycle";
            cmessage_center._instance.add_message(_message1);
            

        }

		if (message.m_opcode == opclient_t.CMSG_PET_INIT)
		{
			protocol.game.smsg_pet_init _msg = net_http._instance.parse_packet<protocol.game.smsg_pet_init>(message.m_byte);
			for (int i = 0; i < _msg.equips.Count; i++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
				
			}
			for (int i = 0; i < _msg.roles.Count; i++)
			{
				sys._instance.m_self.add_card(_msg.roles[i]);
				
			}
			for (int i = 0; i < _msg.treasures.Count; i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.types.Count; i++)
			{
				sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("recycle_gui.cs_836_101"));//宠物重生获得
			}
			sys._instance.m_self.remove_pet(m_pet);
	
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, 200,game_data._instance.get_t_language ("recycle_gui.cs_840_64"));//宠物重生消耗
			
			s_message _message1 = new s_message();
			_message1.m_type = "update_pet_chongsheng";
			cmessage_center._instance.add_message(_message1);
		}
		if (message.m_opcode == opclient_t.CMSG_PET_FENJIE)
		{
			protocol.game.smsg_pet_init _msg = net_http._instance.parse_packet<protocol.game.smsg_pet_init>(message.m_byte);
			for (int i = 0; i < _msg.equips.Count; i++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
				
			}
			for (int i = 0; i < _msg.roles.Count; i++)
			{
				sys._instance.m_self.add_card(_msg.roles[i]);
				
			}
			for (int i = 0; i < _msg.treasures.Count; i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.types.Count; i++)
			{
				sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("recycle_gui.cs_865_101"));//宠物分解获得
			}
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, 200,game_data._instance.get_t_language ("recycle_gui.cs_867_64"));//宠物分解消耗
			sys._instance.m_self.remove_pet(m_pet);
			s_message _message1 = new s_message();
			_message1.m_type = "update_pet_fenjie";
			cmessage_center._instance.add_message(_message1);
		}
	}
	void IMessage.message(s_message message)
	{

	    if(message.m_type == "select_equip")
		{
			m_shells.Clear ();
			int _gold = 0;
			int _mw = 0;
			for(int i = 0;i < sys._instance.m_select_equips.Count;i ++)
			{
				dhc.equip_t _equip = sys._instance.m_self.get_equip_guid(sys._instance.m_select_equips[i]);
				s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
				_gold += game_data._instance.get_total_enhance(_equip.enhance, t_equip.font_color);
				int mw = t_equip.sell;
				for (int j = 1; j <= _equip.star; ++j)
				{
					s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, j);
					_gold += t_equip_sx.gold;
					//mw = mw * 2 + t_equip_sx.mw_point;
				}
				_mw += mw;
				m_shells.Add(sys._instance.m_select_equips[i]);
			}
			m_gold.GetComponent<UILabel>().text = _gold.ToString();
			m_mw.GetComponent<UILabel>().text = _mw.ToString();
			m_add_gold = _gold;
			m_add_mw = _mw;
		}
		//select_jiyin
		if(message.m_type == "update_recycle")
		{
			role_chongsheng();
		}
		if(message.m_type == "update_pet_chongsheng")
		{
			pet_chongsheng();
		}
		if(message.m_type == "update_pet_fenjie")
		{
			pet_fenjie();
		}
		if(message.m_type == "select_jiyin")
		{
			int _zhanhun = 0;
			bool flag = false;
			for(int i = 0;i < m_jiyins.Count;i ++)
			{
				if((ulong)(int)message.m_long[0] == m_jiyins[i])
				{
					flag = true;
					m_jiyins.RemoveAt(i);
				}
			}
			if(!flag)
			{
				m_jiyins.Add((ulong)(int)message.m_long[0]);
			}
			for(int i = 0;i < m_jiyins.Count;i ++)
			{
				_zhanhun += (int)game_data._instance.get_item((int)m_jiyins[i]).gold * sys._instance.m_self.get_item_num((uint)m_jiyins[i]);
			}
			m_zhanhun.text = _zhanhun.ToString();
			m_add_zhanhun = _zhanhun;
		}
		if(message.m_type == "select_jiegu")
		{
			int _zhanhun = 0;
			bool flag = false;
			for(int i = 0;i < m_cards.Count;i ++)
			{
				if((ulong)(int)message.m_long[0] == m_cards[i])
				{
					flag = true;
					m_cards.RemoveAt(i);
					break;
				}
			}
			if(!flag)
			{
				m_cards.Add((ulong)(int)message.m_long[0]);
			}
			for(int i = 0;i < m_cards.Count;i ++)
			{

				_zhanhun += (int)game_data._instance.get_t_class((int)m_cards[i]).exp;
			}
			m_zhanhun.text = _zhanhun.ToString();
			m_add_zhanhun = _zhanhun;
		}
		else if(message.m_type == "shell_jiyin")
		{
			int _zhanhun = 0;
			protocol.game.cmsg_item_fenjie _msg1 = new protocol.game.cmsg_item_fenjie();;
			for(int i = 0;i < m_jiyins.Count;i ++ )
			{
				_msg1.item_id.Add((uint)m_jiyins[i]);
			}
			for(int i = 0;i < m_jiyins.Count;i ++)
			{
				_zhanhun += (int)game_data._instance.get_item((int)m_jiyins[i]).gold * sys._instance.m_self.get_item_num((uint)m_jiyins[i]);
			}
			m_add_zhanhun = _zhanhun;
			net_http._instance.send_msg<protocol.game.cmsg_item_fenjie> (opclient_t.CMSG_ITEM_FENJIE, _msg1);
		}
		else if(message.m_type == "jiegu_huoban")
		{
			protocol.game.cmsg_role_fj _msg = new protocol.game.cmsg_role_fj();
			for(int i = 0; i < m_cards.Count;i ++)
			{
				_msg.role_guid.Add(sys._instance.m_self.get_card_id((int)(m_cards[i])).get_guid());
			}
				
			net_http._instance.send_msg<protocol.game.cmsg_role_fj> (opclient_t.CMSG_ROLE_FENJIE, _msg);
		}
		else if(message.m_type == "shell_equip")
		{
			protocol.game.cmsg_equip_sell _msg = new protocol.game.cmsg_equip_sell ();			
			for(int c = 0;c < m_shells.Count;c ++)
			{
				if(m_shells[c] != 0)
				{
					_msg.equip_guids.Add(m_shells[c]);
				}
			}
			net_http._instance.send_msg<protocol.game.cmsg_equip_sell> (opclient_t.CMSG_EQUIP_SELL, _msg);
		}
		else if(message.m_type == "yijian_fenjie")
		{
			int _zhanhun = 0;
			List<uint> _ids = m_card_page_gui.GetComponent<card_page_gui>().m_sp_ids;
			protocol.game.cmsg_item_fenjie _msg1 = new protocol.game.cmsg_item_fenjie();;
			m_jiyins.Clear();
			for(int i = 0;i < _ids.Count;i++)
			{
				s_t_item _item = game_data._instance.get_item((int)_ids[i]);
				if(_item.font_color < 3 )
				{
					m_jiyins.Add((ulong)_ids[i]);
				}

			}
			if(m_jiyins.Count == 0)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_1017_59"));//没有可供回收的基因
				return;
			}
			for(int i = 0;i < _ids.Count;i ++ )
			{
				s_t_item _item = game_data._instance.get_item((int)_ids[i]);
				if(_item.font_color < 3 )
				{
					_msg1.item_id.Add(_ids[i]);
				}
			}
			for(int i = 0;i < _ids.Count;i ++)
			{
				s_t_item _item = game_data._instance.get_item((int)_ids[i]);
				if(_item.font_color < 3 )
				{
					_zhanhun += (int)game_data._instance.get_item((int)_ids[i]).gold * sys._instance.m_self.get_item_num((uint)_ids[i]);
				}
			}
			m_add_zhanhun = _zhanhun;
			net_http._instance.send_msg<protocol.game.cmsg_item_fenjie> (opclient_t.CMSG_ITEM_FENJIE, _msg1);

		}
		else if(message.m_type == "yijian_shell")
		{
			sys._instance.clear_select_equips();
			m_shells.Clear();
			
			List<dhc.equip_t> _list = m_equip_page_gui.GetComponent<equip_page_gui>().m_equips;
			for(int i = 0;i < _list.Count;i++)
			{
				if(game_data._instance.get_t_equip(_list[i].template_id).font_color <= 2)
				{
					sys._instance.select_equips (_list[i].guid);
				}

			}
			int _gold = 0;
			int _mw = 0;

			for(int i = 0;i < sys._instance.m_select_equips.Count;i ++)
			{
				dhc.equip_t _equip = sys._instance.m_self.get_equip_guid(sys._instance.m_select_equips[i]);
				s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
				_gold += game_data._instance.get_total_enhance(_equip.enhance, t_equip.font_color);
				int mw = t_equip.sell;
				for (int j = 1; j <= _equip.star; ++j)
				{
					s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, j);
					_gold += t_equip_sx.gold;
					//mw = mw * 2 + t_equip_sx.mw_point;
				}
				_mw += mw;
				m_shells.Add(sys._instance.m_select_equips[i]);
			}
			if(m_shells.Count == 0)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_1074_59"));//没有可供回收的装备
				return;
			}
			m_add_gold = _gold;
			m_add_mw = _mw;
			protocol.game.cmsg_equip_sell _msg = new protocol.game.cmsg_equip_sell ();			
			for(int c = 0;c < m_shells.Count;c ++)
			{
				if(m_shells[c] != 0)
				{
					_msg.equip_guids.Add(m_shells[c]);
				}
			}
			net_http._instance.send_msg<protocol.game.cmsg_equip_sell> (opclient_t.CMSG_EQUIP_SELL, _msg);
		}
		else if(message.m_type == "yijian_jiegu")
		{
			m_cards.Clear();
			role_jiegu();
			List<ccard> _cards = m_card_page_gui.GetComponent<card_page_gui>().m_cards;
			for(int i = 0; i < _cards.Count;i ++)
			{
				if(_cards[i].m_t_class.color < 4 )
				{
					m_cards.Add((ulong)_cards[i].m_t_class.id);
				}

			}
			m_add_zhanhun = 0;
			for(int i = 0;i < m_cards.Count;i ++)
			{
				m_add_zhanhun += (int)game_data._instance.get_t_class((int)m_cards[i]).exp;
			}
			if(m_cards.Count == 0)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("recycle_gui.cs_1109_59"));//没有可以解约的伙伴
				return;
			}
			protocol.game.cmsg_role_fj _msg = new protocol.game.cmsg_role_fj();
			for(int i = 0; i < m_cards.Count;i ++)
			{
				_msg.role_guid.Add(sys._instance.m_self.get_card_id((int)(m_cards[i])).get_guid());
			}
			
			net_http._instance.send_msg<protocol.game.cmsg_role_fj> (opclient_t.CMSG_ROLE_FENJIE, _msg);
		}
        else if (message.m_type == "chongsheng")
        {
            
            protocol.game.cmsg_role_init _msg = new protocol.game.cmsg_role_init();
            _msg.role_guid = (ulong)message.m_long[0];
            m_card = sys._instance.m_self.get_card_guid(_msg.role_guid);
            net_http._instance.send_msg<protocol.game.cmsg_role_init>(opclient_t.CMSG_ROLE_CHONGSHENG, _msg);
        }
        else if (message.m_type == "jiegu")
        {
            m_cards.Clear();

            if (message.m_long.Count <= 0)
            {
                return;
            }
            protocol.game.cmsg_role_fj _msg = new protocol.game.cmsg_role_fj();
            for (int i = 0; i < message.m_long.Count; i++)
            {
                ulong guid = (ulong)message.m_long[i];
                m_cards.Add(guid);
                _msg.role_guid.Add(guid);
 
            }
               
            net_http._instance.send_msg<protocol.game.cmsg_role_fj>(opclient_t.CMSG_ROLE_FENJIE, _msg);
        }
        else if (message.m_type == "equip_chongsheng")
        {
            m_shells.Clear();
            m_shells.Add((ulong)message.m_long[0]);
            protocol.game.cmsg_equip_init _msg = new protocol.game.cmsg_equip_init();
            _msg.equip_guids = (ulong)message.m_long[0];
            net_http._instance.send_msg<protocol.game.cmsg_equip_init>(opclient_t.CMSG_EQUIP_INIT, _msg);
        }
        else if (message.m_type == "treasure_chongsheng")
        {
            m_shells.Clear();
            m_shells.Add((ulong)message.m_long[0]);
            protocol.game.cmsg_treasure_init _msg = new protocol.game.cmsg_treasure_init();
            _msg.treasure_guids = (ulong)message.m_long[0];
            net_http._instance.send_msg<protocol.game.cmsg_treasure_init>(opclient_t.CMSG_TREASURE_INIT, _msg);
        }
        else if (message.m_type == "equip_fenjie")
        {
            protocol.game.cmsg_equip_sell _msg = new protocol.game.cmsg_equip_sell();
            m_shells.Clear();
            if (message.m_long.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < message.m_long.Count; i++)
            {
                m_shells.Add((ulong)message.m_long[i]);
                _msg.equip_guids.Add((ulong)message.m_long[i]);
            }
                
            net_http._instance.send_msg<protocol.game.cmsg_equip_sell>(opclient_t.CMSG_EQUIP_SELL, _msg);
        }
		else if (message.m_type == "pet_chongsheng")
		{
			ulong guid = (ulong)message.m_long[0];
			protocol.game.cmsg_pet_init _msg = new protocol.game.cmsg_pet_init();
			_msg.guid.Add((ulong)message.m_long[0]);
			m_pet = sys._instance.m_self.get_pet_guid(guid);
			net_http._instance.send_msg<protocol.game.cmsg_pet_init>(opclient_t.CMSG_PET_INIT, _msg);
		}
		else if (message.m_type == "pet_fenjie")
		{
			ulong guid = (ulong)message.m_long[0];
			protocol.game.cmsg_pet_init _msg = new protocol.game.cmsg_pet_init();
			_msg.guid.Add((ulong)message.m_long[0]);
			m_pet = sys._instance.m_self.get_pet_guid(guid);
			net_http._instance.send_msg<protocol.game.cmsg_pet_init>(opclient_t.CMSG_PET_FENJIE, _msg);
		}
        else if (message.m_type == "equip_yijian_fenjie")
        {


        }
	}
}
