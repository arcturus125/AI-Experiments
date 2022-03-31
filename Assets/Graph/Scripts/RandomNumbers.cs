using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumbers : MonoBehaviour
{
    [SerializeField] private List<Track> _trackables = new List<Track>();
    [SerializeField] private Track _startLate;
    private int frame;

    // Start is called before the first frame update
    void Start()
    {
        frame = 0;

        //Reset values
        foreach (var track in _trackables)
        {
            track.NewValue = 0;
        }
        _startLate.NewValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var track in _trackables)
        {
            track.NewValue = (Random.Range(0, 100));
        }

        if (frame >= 50)
        {
            Debug.Log(frame);
            _startLate.NewValue = (Random.Range(0, 100));
        }

        frame++;
    }
}
