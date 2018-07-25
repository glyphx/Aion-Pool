using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Linq.Expressions;
using AionJsonFormat;
using System;

public class DropdownLoader : MonoBehaviour
{
    public UnityEngine.UI.Dropdown rigsDropList;  // Use this for initialization

    List<DateTimeOffset> lPerformanceSampleCreatedTime = new List<DateTimeOffset>();
    List<string> lPerformanceSampleKeys = new List<string>();
    List<string> lPerformanceKeys = new List<string>();    
    List<Worker> lPerformanceWorkers = new List<Worker>();
    List<Worker> lPerformanceSampleWorkers = new List<Worker>();    
    List<double> lPerfSamWorkerShares = new List<double>();
    List<double> lPerfSamWorkerHashrate = new List<double>();
    List<double> lPerfWorkerShares = new List<double>();
    List<double> lPerfWorkerHashrate = new List<double>();
    //List<List<Worker>> llWorkers = new List<List<Worker>>();

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
                    
                    foreach (KeyValuePair<string, Worker> workers in root.Performance.Workers)
                    {
                        Debug.Log(workers.Key);
                        lPerformanceKeys.Add(workers.Key);                        
                        lPerformanceWorkers.Add(workers.Value);                        
                    }                  
                    
                    foreach (Performance sample in root.PerformanceSamples)
                    {
                        lPerformanceSampleCreatedTime.Add(sample.Created);
                        Debug.Log(sample.Created);
                        foreach (KeyValuePair<string, Worker> workers in sample.Workers)
                        {                            
                            lPerformanceSampleKeys.Add(workers.Key);
                            Debug.Log(workers.Key);
                            lPerformanceSampleWorkers.Add(workers.Value);                            
                        }                       
                    }

                    foreach (Worker now in lPerformanceWorkers)
                    {                        
                        lPerfWorkerShares.Add(now.SharesPerSecond);
                        lPerfWorkerHashrate.Add(now.Hashrate);
                        Debug.Log(now.SharesPerSecond);
                        Debug.Log(now.Hashrate);
                    }

                    foreach (Worker sample in lPerformanceSampleWorkers)
                    {
                        lPerfSamWorkerShares.Add(sample.SharesPerSecond);
                        lPerfSamWorkerHashrate.Add(sample.Hashrate);
                        Debug.Log(sample.SharesPerSecond);
                        Debug.Log(sample.Hashrate);
                    }
                    
                                  

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