using UnityEngine;
using System.Collections;

public class gq_sub : MonoBehaviour {

	// Use this for initialization

    public int m_gq_id;
	void Start () {
	
	}
    public void init(string sprintname)
    {
        this.gameObject.GetComponent<UISprite>().spriteName = sprintname;
    }

    public void select_id()
    {
        s_message _msg_ = new s_message();
        _msg_.m_type = "select_nationality";
        _msg_.m_ints.Add(m_gq_id);
        cmessage_center._instance.add_message(_msg_);

    }
	// Update is called once per frame
	void Update () {
	
	}
}
