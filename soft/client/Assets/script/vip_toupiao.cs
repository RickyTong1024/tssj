
using UnityEngine;
using System.Collections;

public class vip_toupiao : MonoBehaviour {
	public UILabel m_desc1;
	public UILabel m_desc2;
	public UILabel m_desc3;
	public UILabel m_desc4;
	public UILabel m_desc5;
	public UILabel m_desc6;
	public UILabel m_desc7;
	public UILabel m_desc8;
	public UILabel m_desc9;
	public UILabel m_desc10;
	public UILabel m_desc11;
	public UILabel m_desc12;
	public UILabel m_desc0;

	public UILabel LabelText;  
	//把UILabel拖给LabelText  

	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	void click(GameObject obj)
	{
		if(obj.name == "copy")
		{
			try 
			{
                platform._instance.copy(LabelText.text);
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("vip_toupiao.cs_38_59"));//复制成功
			}
			catch (System.Exception) 
			{
				return;	
			}
		}
		else if(obj.name == "close")
		{
			this.GetComponent<ui_title_anim>().hide_ui();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
	}
	void CopyText()
	{  
		TextEditor te = new TextEditor();//很强大的文本工具  
		te.content = new GUIContent(LabelText.text);  
		te.OnFocus();  
		te.Copy();  
	}  
}
