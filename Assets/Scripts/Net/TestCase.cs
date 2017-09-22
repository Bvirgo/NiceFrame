using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Specialized;
using Jhqc.EditorCommon;

public class TestCase : MonoBehaviour
{
    public MeshRenderer cube;

    void Start()
    {
        //TestStringHash();

        //WWWManager.Instance.RemoveCache();

        WWWManager.Instance.TimeOut = 50;
        WWWManager.Instance.Init("139.198.2.58:8000", Jhqc.EditorCommon.LogType.DebugAndGUI);
        WWWManager.Instance.Login("cs5", "1", resp =>
        {
            //Debug.Log(resp.ToString());

            MatManager.Instance.Init(false, true);
            StartCoroutine(Do());

            //WWWManager.Instance.Get("cities/fetch_city?city_name=+%e7%9f%b3%e5%ae%b6%e5%ba%84", new NameValueCollection(), resp2 =>
            //{
            //    Debug.LogWarning(resp2.WwwText);
            //});

            //WWWManager.Instance.GetFile(url, LocalCacheEntry.CacheType.Raw, (resp2, entry) =>
            //{
            //}, false);
        });
    }

    void TestStringHash()
    {
        var str = "http://api.map.baidu.com/panorama?width=1024&height=512&location=114.66514139895,33.6200375536284&fov=360&ak=lGAUfSA7xzwVYYo4szKif1Gb";
        var str2 = "http://stackoverflow.com/questions/742013/how-to-code-a-url-shortener";
        var str3 = "http://192.168.31.18/redmine/issues?assigned_to_id=me&set_filter=1&sort=priority%3Adesc%2Cupdated_on%3Adesc";

        Debug.Log(str.GetHashCode());
        Debug.Log(str2.GetHashCode());
        Debug.Log(str3.GetHashCode());
    }

    IEnumerator Do()
    {
        while (MatManager.Instance.IsLoading)
        {
            yield return new WaitForEndOfFrame();
        }

        var tags = MatManager.Instance.GetAllTags();
        Debug.Log(tags);

        MatManager.Instance.LoadMat("chuanghu_18", item => 
        {
            Debug.Log("done");
            cube.material = item.Material;
        });

        //var mat = MatManager.Instance.GetByName("men_03");
        //Debug.Log(mat);

        //MatManager.Instance.LoadMat("boli_13", item =>
        //{
        //    Debug.Log(1);
        //    Debug.Log(item.Material.preset);
        //    Debug.Log(Time.frameCount);
        //});
        //MatManager.Instance.LoadMat("boli_13", item =>
        //{
        //    Debug.Log(2);
        //    Debug.Log(item.Material.preset);
        //    Debug.Log(Time.frameCount);
        //});
        //MatManager.Instance.LoadMat("boli_13", item =>
        //{
        //    Debug.Log(3);
        //    Debug.Log(item.Material.preset);
        //    Debug.Log(Time.frameCount);
        //});
        //MatManager.Instance.LoadMat("boli_13", item =>
        //{
        //    Debug.Log(4);
        //    Debug.Log(item.Material.preset);
        //    Debug.Log(Time.frameCount);
        //});

        //Debug.Log(Time.frameCount); chuanghu_16


        //var fo = MatManager.Instance.GetInfoByTag("玻璃");
        //Debug.Log(fo.Count);

        ////GetMatPreview("chuanghu_14", tex => { });

        //for (int i = 0; i < 10; i++)
        //{
        //    var matInfo = MatManager.Instance.AllMatInfo[i];

        //    GetMatPreview(matInfo.name, tex =>
        //    {
        //        Debug.Log(string.Format("{0}:{1}*{2}", tex.name, tex.height, tex.width));
        //    });

        //    //MatManager.Instance.LoadMat()
        //}

        //MatManager.Instance.LoadMat("guanggao_01", item =>
        //{
        //    Debug.Log(item);
        //});

        //MatManager.Instance.
    }

    public static void GetMatPreview(string matName, Action<Texture2D> callback)
    {
        WWWManager.Instance.Get("/meterials/get_url", new NameValueCollection()
        {
            { "name", matName }
        }, resp =>
        {
            if (resp.Error == HttpResp.ErrorType.None)
            {
                var downloadInfo = JsonUtility.FromJson<DownloadInfo>(resp.WwwText);

                if (downloadInfo.thumbnail != null)
                {
                    WWWManager.Instance.GetFile(downloadInfo.thumbnail, LocalCacheEntry.CacheType.Texture, (innerResp, entry) =>
                    {
                        if (innerResp.Error == HttpResp.ErrorType.None)
                        {
                            callback(entry.Texture);
                        }
                    });
                }
            }
        });
    }

    [Serializable]
    public class DownloadInfo : JsonBase
    {
        public int id;
        public string name;
        public string crc;
        public string url;
        public string created_at;
        public string updated_at;
        public string thumbnail;
    }
}
