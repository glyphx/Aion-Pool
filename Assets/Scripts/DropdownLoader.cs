using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Linq.Expressions;
using AionJsonFormat;

public class DropdownLoader : MonoBehaviour
{
    public UnityEngine.UI.Dropdown rigsDropList;  // Use this for initialization
    string[] keys = new string[100];
    Worker[] workers = new Worker[100];
    string[,] mKeys = new string[100,100];
    Worker[,] mWorkers = new Worker[100,100];

    void Start()
    {
        rigsDropList = GetComponent<UnityEngine.UI.Dropdown>();
        //rigsDropList.ClearOptions();
        string url = "https://aionmine.org:22200/api/pools/aion/miners/a09d458d49326f6f0e7722d22c668b55809a66ad175f5f796b96bab16889eb3d";
        UnityWebRequest www = UnityWebRequest.Get(url);
        StartCoroutine(enumerateAionJsonData(www));
    }

    IEnumerator enumerateAionJsonData(UnityWebRequest www)
    {
        {
            yield return www.SendWebRequest();  //send is deprecated, use SendWebRequest
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(jsonResult);
                    var root = Root.FromJson(jsonResult);
                    Debug.Log(root.Performance.Created);  //Most recent Creation date. Create variable -24hours
                    int count = 0;
                    foreach (KeyValuePair<string, Worker> kvp in root.Performance.Workers)
                    {
                        keys[count] = kvp.Key;
                        workers[count] = kvp.Value;
                        count++;
                    }
                   // Debug.Log(keys[1]);
                   // Debug.Log(workers[1].SharesPerSecond);

                    count = 0;
                    int element = 0;
                    foreach (Performance sample in root.PerformanceSamples)
                    {
                        int loopcounter = 0;
                        
                        foreach (KeyValuePair<string, Worker> kvp in sample.Workers)
                        {
                            loopcounter++;
                            Debug.Log(loopcounter);
                            
                            Debug.Log(kvp.Key);
                            mKeys[count, element] = kvp.Key;
                            mWorkers[count, element] = kvp.Value;
                            element++;
                            Debug.Log(count +"element" + element);
                        }
                        count++;
                        element = 0;
                    }
                    Debug.Log(mWorkers[0, 0].SharesPerSecond);
                    //Debug.Log(mWorkers[3, 1].SharesPerSecond);
                    Debug.Log(mWorkers[3, 4].SharesPerSecond);
                    Debug.Log(mWorkers[4, 3].SharesPerSecond);


                    /*  Debug.Log(mkeys[0, 0]);
                      rigsDropList.options.AddRange(
                          keys.Select(x =>
                              new UnityEngine.UI.Dropdown.OptionData()
                              {
                                  text = "rig1"
                              }).ToList());
                      rigsDropList.value = 0;
                      rigsDropList.captionText.text = "rig99";

                  }                
              }
          }
      }


      // Update is called once per frame
      void Update()
      {

      }
      */
                }
            }
        }
    }
}                   