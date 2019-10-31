using System.Collections.Generic;
using UnityEngine;

public class bingyuan_scence : MonoBehaviour, IMessage
{
    public List<GameObject> m_items;
    public List<GameObject> m_objs;
    public GameObject m_main;
    public Camera m_camer;
    public GameObject gameObj;
    Vector3 strat_pos;
    Vector3 start_pos_child;
    private bool leader = false;
    public static bingyuan_scence _instance;

    void Start()
    {
        _instance = this;
        cmessage_center._instance.add_handle(this);
        for (int i = 0; i < 5; i++)
        {
            m_objs.Add(null);
        }
    }

    void Update()
    {
        if (bingyuan_gui._instance == null || net_tcp_bingyua._instance == null)
        {
            return;
        }
        if (net_tcp_bingyua._instance.m_connect)
        {
            return;
        }
        if (!bingyuan_gui._instance.m_can_move)
        {
            return;
        }
        if (!leader)
        {
            return;
        }
        Ray ray = m_camer.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hitInfo;
            ray = m_camer.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                gameObj = hitInfo.transform.gameObject;
                strat_pos = gameObj.transform.localPosition;

                start_pos_child = bingyuan_gui._instance.m_uis[int.Parse(gameObj.name)].transform.localPosition;
                if (bingyuan_gui._instance.m_team.players[int.Parse(gameObj.name)].guid == 0)
                {
                    gameObj = null;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (gameObj == null)
            {
                return;
            }
            ray = m_camer.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] _hits = Physics.RaycastAll(ray);
            if (_hits.Length > 1)
            {
                s_message _mes = new s_message();
                _mes.m_type = "change_pos";
                _mes.m_ints.Add(int.Parse(gameObj.name));
                for (int i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].collider.name != gameObj.name)
                    {
                        _mes.m_ints.Add(int.Parse(_hits[i].collider.name));
                    }
                }
                cmessage_center._instance.add_message(_mes);
            }
            else
            {
                if (gameObj)
                {
                    gameObj.transform.localPosition = strat_pos;
                    bingyuan_gui._instance.m_uis[int.Parse(gameObj.name)].transform.localPosition = start_pos_child;
                }

            }
            gameObj = null;
        }
        if (gameObj != null)
        {
            Vector3 Pos = m_camer.WorldToScreenPoint(gameObj.transform.position);
            Vector3 tar = new Vector3(Input.mousePosition.x, Pos.y, Pos.z);
            GameObject child = bingyuan_gui._instance.m_uis[int.Parse(gameObj.name)].gameObject;
            Vector3 Pos_child = UICamera.mainCamera.WorldToScreenPoint(child.transform.position);
            Vector3 tar_child = new Vector3(Input.mousePosition.x, Pos_child.y, Pos_child.z);
            if (tar.x < 0)
            {
                tar.x = 0;
                tar_child.x = 0;
            }
            if (tar.x > Screen.width)
            {
                tar.x = Screen.width;
                tar_child.x = Screen.width;
            }
            Vector3 v = m_camer.ScreenToWorldPoint(tar);
            Vector3 v_child = UICamera.mainCamera.ScreenToWorldPoint(tar_child);
            gameObj.transform.position = v;
            bingyuan_gui._instance.m_uis[int.Parse(gameObj.name)].transform.position = v_child;
        }
    }

    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "bingyuan_start")
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].transform.parent.gameObject.SetActive(false);
            }
            m_main.transform.parent.gameObject.SetActive(true);
            sys._instance.remove_child(m_main);
            ccard card = sys._instance.m_self.get_card_id((int)sys._instance.m_self.m_t_player.template_id);
            GameObject _unit = null;
            if (card != null)
            {
                _unit = sys._instance.create_class
                ((int)sys._instance.m_self.m_t_player.template_id, card.get_role().dress_on_id, sys._instance.m_self.m_t_player.guanghuan_id, m_main);
                _unit.transform.localPosition = new Vector3(-0.1f, 0, 0.1f);
            }
            else
            {
                _unit = sys._instance.create_class
               ((int)sys._instance.m_self.m_t_player.template_id, 0, sys._instance.m_self.m_t_player.guanghuan_id, m_main);
                _unit.transform.localPosition = new Vector3(-0.1f, 0, 0.1f);
            }
            _unit.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
        else if (mes.m_type == "bingyuan_pipei")
        {
            m_main.transform.parent.gameObject.SetActive(false);
            leader = mes.m_bools[0];
            bool create = mes.m_bools[1];
            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].transform.parent.gameObject.SetActive(true);
            }
            for (int i = 0; i < bingyuan_gui._instance.m_team.players.Count; i++)
            {
                if (!create)
                {
                    refresh_player(i);
                }
                else
                {
                    refresh_player_create(i);
                }
            }
        }
        else if (mes.m_type == "remove_unit")
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                sys._instance.remove_child(m_items[i]);
            }
        }
        else if (mes.m_type == "bingyuan_refresh_player")
        {
            int index1 = (int)mes.m_ints[0];
            int index2 = (int)mes.m_ints[1];

            m_objs[index1].name = index2 + "";
            m_objs[index2].name = index1 + "";

            m_objs[index1].transform.parent = m_items[index2].transform;
            m_objs[index2].transform.parent = m_items[index1].transform;
            m_objs[index1].transform.localPosition = Vector3.zero;
            m_objs[index2].transform.localPosition = Vector3.zero;

            GameObject obj = m_objs[index1];
            m_objs[index1] = m_objs[index2];
            m_objs[index2] = obj;
        }
    }

    public GameObject refresh_player_create(int index)
    {
        protocol.team.team_player _player = bingyuan_gui._instance.m_team.players[index];
        GameObject _unit = null;

        if (_player.guid != 0)
        {
            sys._instance.remove_child(m_items[index]);
            _unit = sys._instance.create_class(_player.id, _player.dress, _player.guanghuan, m_items[index]);
            CapsuleCollider _colide = _unit.AddComponent<CapsuleCollider>();
            _colide.center = new Vector3(0, 1, 0);
            _colide.radius = 0.5f;
            _colide.height = 2;
            _unit.name = index + "";
            _unit.transform.parent = m_items[index].transform;
            _unit.transform.localPosition = new Vector3(0, 0, 0.1f);
            _unit.transform.localEulerAngles = new Vector3(90, 0, 0);
            m_objs[index] = _unit;
        }
        else
        {
            sys._instance.remove_child(m_items[index]);
            _unit = new GameObject();
            CapsuleCollider _colide = _unit.AddComponent<CapsuleCollider>();
            _colide.center = new Vector3(0, 1, 0);
            _colide.radius = 0.5f;
            _colide.height = 2;
            _unit.name = index + "";
            _unit.transform.parent = m_items[index].transform;
            _unit.transform.localPosition = new Vector3(0, 0, 0.1f);
            _unit.transform.localScale = Vector3.one;
            _unit.transform.localEulerAngles = new Vector3(90, 0, 0);
            m_objs[index] = _unit;
        }
        return _unit;
    }
    GameObject refresh_player(int index)
    {
        protocol.team.team_player _player = bingyuan_gui._instance.m_team.players[index];
        GameObject _unit = null;
        if (_player.guid != 0)
        {
            if (m_objs[index] == null || m_objs[index].GetComponent<unit>() == null)
            {
                sys._instance.remove_child(m_items[index]);
                _unit = sys._instance.create_class(_player.id, _player.dress, _player.guanghuan, m_items[index]);
                CapsuleCollider _colide = _unit.AddComponent<CapsuleCollider>();
                _colide.center = new Vector3(0, 1, 0);
                _colide.radius = 0.5f;
                _colide.height = 2;
                _unit.name = index + "";
                _unit.transform.parent = m_items[index].transform;
                _unit.transform.localPosition = new Vector3(0, 0, 0.1f);
                _unit.transform.localEulerAngles = new Vector3(90, 0, 0);
                m_objs[index] = _unit;
            }
        }
        else
        {
            sys._instance.remove_child(m_items[index]);
            _unit = new GameObject();
            CapsuleCollider _colide = _unit.AddComponent<CapsuleCollider>();
            _colide.center = new Vector3(0, 1, 0);
            _colide.radius = 0.5f;
            _colide.height = 2;
            _unit.name = index + "";
            _unit.transform.parent = m_items[index].transform;
            _unit.transform.localPosition = new Vector3(0, 0, 0.1f);
            _unit.transform.localScale = Vector3.one;
            _unit.transform.localEulerAngles = new Vector3(90, 0, 0);
            m_objs[index] = _unit;
        }
        return _unit;
    }

    void IMessage.net_message(s_net_message mes)
    {
    }

    void OnDestroy()
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            sys._instance.remove_child(m_items[i]);
        }
        leader = false;
        sys._instance.remove_child(m_main);
        cmessage_center._instance.remove_handle(this);
    }
}
