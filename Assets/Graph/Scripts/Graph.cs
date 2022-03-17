using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite _pointSprite;
    [SerializeField] private GameObject _containerTemplate;
    [SerializeField] private RectTransform _labelTemplateX;
    [SerializeField] private RectTransform _labelTemplateY;
    [SerializeField] private int maxXLabels = 12;

    private GameObject _container;
    private List<int> _values;
    private float _time;

    private void Awake()
    {
        _values = new List<int>() {0};
        CreateGraph(_values);
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 1)
        {
            _values.Add(Random.Range(0, 100));
            CreateGraph(_values);

            _time = 0;
        }
    }

    private void CreateGraph(List<int> values)
    {
        //Create new container
        if (_container)
        {
            Destroy(_container);
        }
        _container = Instantiate(_containerTemplate);
        RectTransform containerRectTransform = _container.GetComponent<RectTransform>();
        containerRectTransform.SetParent(_containerTemplate.GetComponent<RectTransform>().parent);
        containerRectTransform.anchoredPosition = _containerTemplate.GetComponent<RectTransform>().anchoredPosition;
        _container.SetActive(true);

        //Get max height
        float graphWidth = containerRectTransform.sizeDelta.x;
        float graphHeight = containerRectTransform.sizeDelta.y;
        
        float yMax = values[0];
        float yMin = 0;

        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] > yMax)
            {
                yMax = values[i];
            }
        }

        float yDifference = yMax - yMin;
        if (yDifference <= 0)
        {
            yDifference = 5.0f;
        }
        yMax += yDifference * 0.1f; //Add 10% space at top of graph

        //Create points
        float labelStep = Mathf.Ceil(values.Count / maxXLabels) + 1;
        Debug.Log("Num: " + values.Count + " Step: " + labelStep);
        float xStep = graphWidth / (values.Count + 1);
        int xIndex = 0;
        GameObject prevPoint = null;
        for (int i = 0; i < values.Count; i++)
        {
            float xPos = xStep + xIndex * xStep;
            float yPos = ((values[i] - yMin) / (yMax - yMin)) * graphHeight;
            GameObject point = CreatePoint(new Vector2(xPos, yPos));

            if (prevPoint)
            {
                ConnectPoints(prevPoint.GetComponent<RectTransform>().anchoredPosition, point.GetComponent<RectTransform>().anchoredPosition);
            }

            prevPoint = point;

            //X axis labels
            if (values.Count <= maxXLabels || i % labelStep == 0)
            {
                RectTransform labelX = Instantiate(_labelTemplateX);
                labelX.SetParent(_container.GetComponent<RectTransform>());
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPos, -7);
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

            float normalized = (float) i / separatorCount;
            labelY.anchoredPosition = new Vector2(-7, normalized * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(yMin + (normalized * (yMax - yMin))).ToString();
        }
    }

    private GameObject CreatePoint(Vector2 position)
    {
        GameObject pointObject = new GameObject("Point", typeof(Image));
        pointObject.transform.SetParent(_container.GetComponent<RectTransform>(), false);
        pointObject.GetComponent<Image>().sprite = _pointSprite;

        RectTransform rectTransform = pointObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        //pointObject.SetActive(false);

        return pointObject;
    }

    private void ConnectPoints(Vector2 pointA, Vector2 pointB)
    {
        //Create & position line
        GameObject gameObject = new GameObject("PointConnector", typeof(Image));
        gameObject.transform.SetParent(_container.GetComponent<RectTransform>(), false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 direction = (pointB - pointA).normalized;
        float distance = Vector2.Distance(pointA, pointB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3);
        rectTransform.anchoredPosition = pointA + direction * distance * 0.5f;

        //Get connector gradient
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }

        rectTransform.localEulerAngles = new Vector3(0, 0, n);
    }
}
