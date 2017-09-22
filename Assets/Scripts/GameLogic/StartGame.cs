using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyFrameWork;
using System;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // 加载数据模块
		RegisterAllModules();

        // 打开指定UI
		UIManager.Instance.OpenUI(EnumUIType.TestOne);

		float time = System.Environment.TickCount;

		Debug.Log("Times： " + (System.Environment.TickCount - time) * 1000);

		StartCoroutine(AutoUpdateGold());
	}

	private void RegisterAllModules()
	{
		LoadModule(typeof(TestOneModule));
		//.....add
	}

    /// <summary>
    /// 创建指定M，初始化
    /// </summary>
    /// <param name="moduleType"></param>
	private void LoadModule(Type moduleType)
	{
		BaseModule bm = System.Activator.CreateInstance(moduleType) as BaseModule;
		bm.Load();
	}

    /// <summary>
    /// 金币 :1s add 1
    /// </summary>
    /// <returns></returns>
	private IEnumerator AutoUpdateGold()
	{
		int gold = 0;
		while(true)
		{
			gold ++;
			yield return new WaitForSeconds(1.0f);
			Message message = new Message(MessageType.Net_MessageTestOne.ToString(), this);
			message["gold"] = gold;
			MessageCenter.Instance.SendMessage(message);
//			Message message = new Message(MessageType.Net_MessageTestOne, this);
//			message["gold"] = gold;
//			message.Send();
		}
	}

	private IEnumerator<int> AsyncLoadData()
	{
		int i = 0;
		while(true)
		{
			Debug.Log("------> " + i);
			yield return i;
			i++;
		}
	}

}
