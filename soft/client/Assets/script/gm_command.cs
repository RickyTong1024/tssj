
using UnityEngine;
using System.Collections;

public class gm_command : MonoBehaviour {

	public GameObject m_des;
	
	public GameObject m_gm_command;
	// Use this for initialization
	void Start () {
	}

	public void click(GameObject obj)
	{
		/*
		if (Application.platform == RuntimePlatform.WindowsEditor) {
						if (obj.name == "show_att" && UnityEditor.Selection.gameObjects.Length > 0) {
								string _des = "";

								GameObject _select = UnityEditor.Selection.gameObjects [0];

								unit _unit = _select.GetComponent<unit>();

								if (_unit) {
										_des += _unit.m_card.m_t_class.name + '\n';
										for (int i = 0; i < game_data._instance.m_t_value.get_y(); i ++) {
												string _name = game_data._instance.m_t_value.get (1, i);
												int _att = int.Parse (game_data._instance.m_t_value.get (0, i));
												_des += _name + (int)_unit.get_att (_att) + '\n';
										}

										for (int i = 0; i < _unit.m_buffer.Count; i ++) {
												_des += _unit.m_buffer [i].m_t_skill.name + '\n';
										}
								}

								m_des.GetComponent<UILabel>().text = _des;
						} else if (obj.name == "gm_command") {
								protocol.game.cmsg_gm_command _msg = new protocol.game.cmsg_gm_command ();
			
								_msg.text = m_gm_command.GetComponent<UILabel>().text;
								_msg.guid = sys._instance.m_self.m_guid;
								_msg.sig = sys._instance.m_self.m_sig;

								net_http._instance.send_msg<protocol.game.cmsg_gm_command> (opclient_t.CMSG_GM_COMMAND, _msg);
						} else if (obj.name == "close") {
								this.transform.gameObject.SetActive (false);
						}
				}
				*/
	}
	// Update is called once per frame
	void Update () {
	
	}
}
