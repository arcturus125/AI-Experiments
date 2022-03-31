using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite _pointSprite;
    [SerializeField] private GameObject _containerTemplate;
    [SerializeField] private bool _showDots = false;
    [SerializeField] private Color _lineColour = new Color(1, 1, 1, 0.5f);

    private GameObject _container;
    private List<float> _values;
    
    public Color LineColour
    {
        get { return _lineColour; }
        set { _lineColour = value; }
    }

    public List<float> Values
    {
        get { return _values; }
    }

    public void AddValue(float value)
    {
        _values.Add(value);
        CreateGraph(_values);
    }

    private void Awake()
    {
        _values = new List<float>() {0};
        CreateGraph(_values);
    }

    private void CreateGraph(List<float> values)
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

            xIndex++;
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
        pointObject.SetActive(_showDots);

        return pointObject;
    }

    private void ConnectPoints(Vector2 pointA, Vector2 pointB)
    {
        //Create & position line
        GameObject gameObject = new GameObject("PointConnector", typeof(Image));
        gameObject.transform.SetParent(_container.GetComponent<RectTransform>(), false);
        gameObject.GetComponent<Image>().color = _lineColour;

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
