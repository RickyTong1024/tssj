using UnityEngine;

public class login_scene : MonoBehaviour,IMessage {

	public GameObject m_anim;
	public GameObject m_mm;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
		Physics.gravity = new Vector3 (0, -1, 0);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);

		Physics.gravity = new Vector3 (0, -20, 0);
	}
	void IMessage.net_message(s_net_message message)
	{

	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "login_anim")
		{
			m_anim.GetComponent<Animator>().SetBool("anim_0",true); 

			s_message _msg = new s_message();
			
			_msg.m_type = "face_ex_name";
			_msg.m_string.Add("su_xing");
			
			cmessage_center._instance.add_message(_msg);


		}
	}

	void show_anim()
	{
		m_mm.GetComponent<Animation>().CrossFade("dl_cj02");
	}
	public void load_scene(string name)
	{
		sys._instance.m_game_state = "hall";
		sys._instance.load_scene (sys._instance.m_hall_name);
	}
}
