using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
class GameEdit : EditorWindow
{
	static GameEdit window;

	string m_save_name = "edit.sav";
	string m_xml_name;
	string m_part_name;

	List<string> m_action_name = new List<string>();
	string m_action_name1;
	string m_face_id;
	string m_face_name;
	string m_mission_id;
	string m_gmcommand_id;
	string m_gmcommand_id2;
	string m_juqing_name;
    string m_time;
    string m_name;
    string pass;

    [MenuItem("GameEdit/Test")]
    static void Execute()
    {
        if (window == null)
			window = (GameEdit)GetWindow(typeof (GameEdit));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("--- 读取角色脚本 ---");
        m_xml_name = EditorGUILayout.TextArea(m_xml_name);
        GUILayout.BeginHorizontal();
        bool _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            s_message _msg = new s_message();

            _msg.m_type = "loadxml";

            cmessage_center._instance.add_message(_msg);

        }

        GUILayout.Label("--- 装备测试 ---");
        m_part_name = EditorGUILayout.TextArea(m_part_name);
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            s_message _msg = new s_message();

            _msg.m_type = "part";
            _msg.m_string.Add(m_part_name);

            cmessage_center._instance.add_message(_msg);
        }

        read();
        GUILayout.Label("--- 行为测试 ---");
        for (int i = 0; i < m_action_name.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(m_action_name[i], GUILayout.Width(160f));
            _ok = GUILayout.Button("确定", GUILayout.Width(60f));
            bool _del = GUILayout.Button("删除", GUILayout.Width(60f));
            GUILayout.EndHorizontal();

            if (_ok)
            {
                s_message _msg = new s_message();

                _msg.m_type = "action";
                _msg.m_string.Add(m_action_name[i]);

                cmessage_center._instance.add_message(_msg);
            }

            if (_del)
            {
                m_action_name.RemoveAt(i);
                save();
                Repaint();
            }
        }

        m_action_name1 = EditorGUILayout.TextArea(m_action_name1);
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("增加", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            if (!m_action_name.Contains(m_action_name1))
            {
                m_action_name.Add(m_action_name1);
                save();
                Repaint();
            }
        }

        GUILayout.Label("--- 表情测试ID ---");
        m_face_id = EditorGUILayout.TextArea(m_face_id);
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            s_message _msg = new s_message();

            _msg.m_type = "face_ex_name";
            _msg.m_string.Add(m_face_id);

            cmessage_center._instance.add_message(_msg);
        }

        GUILayout.Label("--- 表情测试name ---");
        m_face_name = EditorGUILayout.TextArea(m_face_name);
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            s_message _msg = new s_message();

            _msg.m_type = "face_name";
            _msg.m_string.Add(m_face_name);

            cmessage_center._instance.add_message(_msg);
        }

        GUILayout.Label("--- Messageid ---");
        m_mission_id = EditorGUILayout.TextArea(m_mission_id);
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            s_message _msg = new s_message();

            _msg.m_type = m_mission_id;

            cmessage_center._instance.add_message(_msg);
        }

        GUILayout.Label("--- 剧情name ---");
        m_juqing_name = EditorGUILayout.TextArea(m_juqing_name);
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            root_gui._instance.action_guide(m_juqing_name);
        }

        GUILayout.Label("--- gm 命令 ---");
        m_gmcommand_id = EditorGUILayout.TextArea(m_gmcommand_id);
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            protocol.game.cmsg_gm_command _msg = new protocol.game.cmsg_gm_command();
            _msg.text = m_gmcommand_id;
            net_http._instance.send_msg<protocol.game.cmsg_gm_command>(opclient_t.CMSG_GM_COMMAND, _msg);
        }

        GUILayout.Label("--- 删除账号信息 ---");
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            PlayerPrefs.DeleteAll();
            string _file_name = Application.persistentDataPath + "/nsxq.sav";
            if (File.Exists(_file_name))
            {
                File.Delete(_file_name);
            }
            _file_name = Application.persistentDataPath + "/nsxq1.sav";
            if (File.Exists(_file_name))
            {
                File.Delete(_file_name);
            }
        }
        
        GUILayout.Label("--- 是否在代码里使用 ---");
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            DirectoryInfo direction1 = new DirectoryInfo("Assets/Resources/ui");
            FileInfo[] fileinfos1 = direction1.GetFiles("*", SearchOption.AllDirectories);
            for (int j = 0; j < fileinfos1.Length; ++j)
            {
                string path1 = fileinfos1[j].FullName;
                if (!path1.EndsWith(".prefab"))
                {
                    continue;
                }
                int index = path1.LastIndexOf("Resources");
                string s = path1.Substring(index + 10);
                s = s.Substring(0, s.Length - 7);
                string ss = "\"" + s + "\"";
                ss = ss.Replace("\\", "/");
                index = s.LastIndexOf("\\");
                string sss = "\"" + s.Substring(index + 1) + "\"";
                DirectoryInfo direction = new DirectoryInfo("Assets/script");
                FileInfo[] fileinfos = direction.GetFiles("*", SearchOption.AllDirectories);
                bool flag = false;
                for (int i = 0; i < fileinfos.Length; ++i)
                {
                    string path = fileinfos[i].FullName;
                    if (!path.EndsWith(".cs"))
                    {
                        continue;
                    }
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(fs);
                    string content = reader.ReadToEnd();
                    reader.Close();
                    fs.Close();
                    if (content.Contains(ss) || content.Contains(sss))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Debug.Log(ss);
                }
            }
        }

        GUILayout.Label("--- text混淆 ---");
        GUILayout.BeginHorizontal();
        _ok = GUILayout.Button("确定", GUILayout.Width(120f));
        GUILayout.EndHorizontal();

        if (_ok)
        {
            DirectoryInfo direction1 = new DirectoryInfo("Assets/Resources/ui");
            FileInfo[] fileinfos1 = direction1.GetFiles("*", SearchOption.AllDirectories);
            for (int j = 0; j < fileinfos1.Length; ++j)
            {
                string path1 = fileinfos1[j].FullName;
                if (!path1.EndsWith(".prefab"))
                {
                    continue;
                }
                int index = path1.IndexOf("Assets");
                string s = path1.Substring(index);
                s = s.Replace("\\", "/");
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(s);
                if (obj == null)
                {
                    continue;
                }
                bool f = find_text(obj.transform);
                if (f)
                {
                    EditorUtility.SetDirty(obj);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    IEnumerator loadassetbundle(string path)
    {
        using (WWW asset = new WWW(path))
        {
            yield return asset;
            AssetBundle bundle = asset.assetBundle;
            Debug.Log(bundle.mainAsset);
            bundle.Unload(false);
            yield return new WaitForSeconds(5);
        }  
    }
    private ulong DateTimeToStamp(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        double ti = (time - startTime).TotalMilliseconds;
        return (ulong)ti;
    }
	void read()
	{
		m_action_name.Clear ();
		string _file_name = Application.persistentDataPath + m_save_name;
		if(File.Exists(_file_name))
		{
			FileStream  _file = new FileStream(_file_name,FileMode.OpenOrCreate,FileAccess.ReadWrite);
			BinaryReader _br = new BinaryReader(_file);
			int num = _br.ReadInt32();
			for (int i = 0; i < num; ++i)
			{
				string s = _br.ReadString();
				m_action_name.Add(s);
			}
			_file.Close ();
		}
	}

	void save()
	{
		FileStream  _file = new FileStream(Application.persistentDataPath + m_save_name,FileMode.OpenOrCreate,FileAccess.ReadWrite);  
		BinaryWriter _bw = new BinaryWriter(_file);
		BinaryReader _br = new BinaryReader(_file);

		_bw.Write(m_action_name.Count);
		for (int i = 0; i < m_action_name.Count; ++i)
		{
			_bw.Write(m_action_name[i]);
		}
				
		_file.Close();
	}

    bool find_text(Transform t)
    {
        bool b = false;
        UILabel ul = t.GetComponent<UILabel>();
        if (ul != null)
        {
            if (ul.text != "..." && !ul.Confusion)
            {
                ul.text = "...";
                b = true;
            }
        }
        for (int i = 0; i < t.childCount; ++i)
        {
            if (find_text(t.GetChild(i)))
            {
                b = true;
            }
        }
        return b;
    }
}
