
using UnityEngine;
using System.Collections;

public class five_star : MonoBehaviour {

	public void click(GameObject obj)
	{
        if (obj.transform.name == "nogo")
        {
            this.transform.gameObject.SetActive(false);
        }
        else if (obj.transform.name == "gonow")
        {
            platform._instance.game_update();  
            this.transform.gameObject.SetActive(false);
        }
	}
}
