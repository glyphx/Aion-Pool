using AionJsonFormat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AionJsonData
{
    public class AionJsonData : MonoBehaviour
    {
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
                        var jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                        Debug.Log(jsonResult);
                        var root = Root.FromJson(jsonResult);
                        Debug.Log(
                            root.PendingBalance + "\n" +
                            root.PendingShares + "\n" +
                            root.LastPayment + "\n" +
                            root.Performance.Created); //Most recent Creation date. Create variable -24hours

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
                            Debug.Log(now.SharesPerSecond + "\n" + now.Hashrate);
                        }

                        foreach (Worker sample in lPerformanceSampleWorkers)
                        {
                            lPerfSamWorkerShares.Add(sample.SharesPerSecond);
                            lPerfSamWorkerHashrate.Add(sample.Hashrate);
                            Debug.Log(sample.SharesPerSecond + "\n" + sample.Hashrate);
                        }
                    }
                }
            }
        }
    }
}