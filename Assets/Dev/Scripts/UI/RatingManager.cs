using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class RatingManager : MonoBehaviour
{
    public static RatingManager instance;

    public List<TMP_Text> uiList; // Reference to the UI Text elements
    private List<OpponentData> opponentDataList = new List<OpponentData>(); // List of opponent data

    private void Awake()
    {
        if (RatingManager.instance != null)
            UnityEngine.Object.Destroy(gameObject);
        else
            RatingManager.instance = this;
    }
    private void Update()
    {
        SortOpponentsByZPositionAndDisplayTags();
    }
    public void SetOpponentData(Transform opponentTransform, string tag)
    {
        OpponentData opponentData = new OpponentData(opponentTransform, tag);
        opponentDataList.Add(opponentData);
    }

    public void SortOpponentsByZPositionAndDisplayTags()
    {
        // Sorting opponents by Z position
        List<OpponentData> sortedOpponents = opponentDataList.OrderByDescending(od => od.OpponentTransform.position.z).ToList();

        // Displaying sorted opponent tags in UI
        for (int i = 0; i < uiList.Count && i < sortedOpponents.Count; i++)
        {
            uiList[i].text = sortedOpponents[i].Tag;
        }
    }

    [System.Serializable]
    public class OpponentData
    {
        public Transform OpponentTransform { get; set; }
        public string Tag { get; set; }

        public OpponentData(Transform opponentTransform, string tag)
        {
            OpponentTransform = opponentTransform;
            Tag = tag;
        }
    }
}




