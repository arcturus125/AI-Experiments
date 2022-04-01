using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private GameObject _containerTemplate;
    [SerializeField] private int _maxXLabels = 12;
    [SerializeField] private RectTransform _labelTemplateX;
    [SerializeField] private RectTransform _labelTemplateY;

    private List<GameObject> _graphs;
    private GameObject _container;

    // Start is called before the first frame update
    void Start()
    {
        _graphs = new List<GameObject>();
        foreach (var value in _trackedValues)
        {
            //Create new graph
            GameObject newGraph = Instantiate(_graphTemplate, this.transform);
            _graphs.Add(newGraph);
            newGraph.transform.localScale = Vector3.one;
            //Position graph
            RectTransform rectTransform = newGraph.GetComponent<RectTransform>();
            rectTransform.SetParent(_graphTemplate.GetComponent<RectTransform>().parent);
            rectTransform.anchoredPosition = _graphTemplate.GetComponent<RectTransform>().anchoredPosition;
            newGraph.SetActive(true);

            //Set line colour
            Graph graphComponent = newGraph.GetComponent<Graph>();
            graphComponent.LineColour = _lineColours[(_graphs.Count - 1) % _lineColours.Count];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _graphs.Count; i++)
        {
            Graph graphComponent = _graphs[i].GetComponent<Graph>();
            graphComponent.AddValue(_trackedValues[i].NewValue);
        }

        DrawAxes();
    }

    void DrawAxes()
    {
        if (_container)
        {
            Destroy(_container);
        }
        _container = Instantiate(_containerTemplate, this.transform);
        RectTransform containerRectTransform = _container.GetComponent<RectTransform>();
        containerRectTransform.SetParent(_containerTemplate.GetComponent<RectTransform>().parent);
        containerRectTransform.anchoredPosition = _containerTemplate.GetComponent<RectTransform>().anchoredPosition;
        _container.transform.localScale = Vector3.one;


        float graphWidth = containerRectTransform.sizeDelta.x;
        float graphHeight = containerRectTransform.sizeDelta.y;

        Graph graphComponent = _graphs[0].GetComponent<Graph>();
        float yMax = graphComponent.Values[0];
        float yMin = 0;

        for (int i = 1; i < _graphs.Count; i++)
        {
            graphComponent = _graphs[i].GetComponent<Graph>();
            foreach (var val in graphComponent.Values)
            {
                if (val > yMax)
                {
                    yMax = val;
                }
            }
        }

        float yDifference = yMax - yMin;
        if (yDifference <= 0)
        {
            yDifference = 5.0f;
        }
        yMax += yDifference * 0.1f; //Add 10% space at top of graph

        //X axis
        float labelStep = Mathf.Ceil(graphComponent.Values.Count / _maxXLabels) + 1;
        float xStep = graphWidth / (graphComponent.Values.Count + 1);
        int xIndex = 0;
        for (int i = 0; i < graphComponent.Values.Count; i++)
        {
            float xPos = xStep + xIndex * xStep;
            if (graphComponent.Values.Count <= _maxXLabels || i % labelStep == 0)
            {
                RectTransform labelX = Instantiate(_labelTemplateX);
                labelX.SetParent(_container.GetComponent<RectTransform>());
                labelX.gameObject.SetActive(true);

                labelX.anchoredPosition3D = new Vector3(xPos, -7,0); // set the position in 3d space rather than 2d
                labelX.localScale = Vector3.one; // reset the scale of the graph
                labelX.rotation = transform.parent.rotation; // the text should have the same rotation as the graph

                labelX.GetComponent<Text>().text = "Day " + i.ToString();

            }
            xIndex++;
        }

        //Y axis labels
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(_labelTemplateY);
            labelY.SetParent(_container.GetComponent<RectTransform>());
            labelY.gameObject.SetActive(true);

            float normalized = (float)i / separatorCount;
            labelY.anchoredPosition3D = new Vector3(-7, normalized * graphHeight, 0);
            labelY.localScale = Vector3.one;
            labelY.rotation = transform.parent.rotation;
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(yMin + (normalized * (yMax - yMin))).ToString();
        }

        containerRectTransform.SetParent(GetComponent<RectTransform>().parent);
        _container.SetActive(true);
    }
}
