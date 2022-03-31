using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    [SerializeField] private GameObject _graphTemplate;
    [SerializeField] private List<Track> _trackedValues;
    [SerializeField] private List<Color> _lineColours = new List<Color>()
    {
        new Color(1.0f, 0.0f, 0.0f, 0.5f),
        new Color(0.0f, 1.0f, 0.0f, 0.5f),
        new Color(0.0f, 0.0f, 1.0f, 0.5f),
        new Color(1.0f, 1.0f, 0.0f, 0.5f),
        new Color(1.0f, 1.0f, 1.0f, 0.5f)
    };

    private List<GameObject> _graphs;

    // Start is called before the first frame update
    void Start()
    {
        _graphs = new List<GameObject>();
        foreach (var value in _trackedValues)
        {
            //Create new graph
            GameObject newGraph = Instantiate(_graphTemplate);
            _graphs.Add(newGraph);

            //Position graph
            RectTransform rectTransform = newGraph.GetComponent<RectTransform>();
            rectTransform.SetParent(_graphTemplate.GetComponent<RectTransform>().parent);
            rectTransform.anchoredPosition = _graphTemplate.GetComponent<RectTransform>().anchoredPosition;
            newGraph.SetActive(true);

            //Set line colour
            Graph graphComponent = newGraph.GetComponent<Graph>();
            graphComponent.LineColour = _lineColours[(_graphs.Count - 1) % _lineColours.Count];
        }

        _graphs[0].GetComponent<Graph>().DrawAxis = true;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _graphs.Count; i++)
        {
            Graph graphComponent = _graphs[i].GetComponent<Graph>();
            graphComponent.AddValue(_trackedValues[i].NewValue);
        }
    }
}
