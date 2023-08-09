using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RatingManager : MonoBehaviour
{

    public static RatingManager instance;


    private Transform entryContainer;
    private Transform entryTemplate;

    private List<Transform> rankListEntryTransformList;
    private List<OpponentData> opponentDataList;

    public float templateHeight = 40f;
    
    private void Awake()
    {
       
        if (RatingManager.instance)
            UnityEngine.Object.Destroy(gameObject);

        else
            RatingManager.instance = this;

        entryContainer = transform.Find("RankingEntryContainer");
        entryTemplate = entryContainer.Find("RankingEntryTemplate");
        entryTemplate.gameObject.SetActive(false);
        opponentDataList = new List<OpponentData>();
        StartCoroutine(PopulateOpponentDataList());

    }
    private IEnumerator PopulateOpponentDataList()
    {
        //Cycle through that object for sorting
        for (int i = 0; i < opponentDataList.Count; i++)
        {
            for (int j = i + 1; j < opponentDataList.Count; j++)
            {
                if (opponentDataList[j].Position > opponentDataList[i].Position)
                {
                    //swap
                    OpponentData tmp = opponentDataList[i];
                    opponentDataList[i] = opponentDataList[j];
                    opponentDataList[j] = tmp;
                }
            }
        }

        rankListEntryTransformList = new List<Transform>();
        foreach (OpponentData opponentData in opponentDataList)
        {
            CreateRankListEntryTransform(opponentData, entryContainer, rankListEntryTransformList);
        }
        yield return new WaitForSeconds(0.5f);
    }
    private void CreateRankListEntryTransform(OpponentData opponentData, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
       
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3ND"; break;
        }


        string name = /*rankListEntry*/opponentData.Tag;
        entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = name;

        entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>().text = rankString;
      

        transformList.Add(entryTransform);
       
    }
    private class OpponentClassObject
    {
        public List<OpponentData> opponentDataList;
    }
    [System.Serializable]
    public class OpponentData
    {
        public float Position { get; set; }
        public string Tag { get; set; }

        public OpponentData(float position, string tag)
        {
            Position = position;
            Tag = tag;
        }

    }

    public void SetOpponentData(float position, string name)
    {
        // Create a new instance 

        OpponentData opponentData1 = new OpponentData (position, name);
        opponentDataList.Add(opponentData1);

        StartCoroutine(PopulateOpponentDataList());
    }



}





