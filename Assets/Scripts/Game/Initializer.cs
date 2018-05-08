using UnityEngine;

public class Initializer :MonoBehaviour
{
	void Start()
	{
        DontDestroyOnLoad(this.gameObject);

        StartGame();

        
	}
	
    void OnDestroy()
    {
    }

	
    void StartGame()
    {
        this.gameObject.AddComponent<ResMgr>();
        this.gameObject.AddComponent<UIMgr>();
        this.gameObject.AddComponent<AppMgr>();
        this.gameObject.AddComponent<TableDataMgr>();
    }

   
}
