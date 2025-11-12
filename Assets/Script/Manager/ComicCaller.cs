using UnityEngine;

public class ComicCaller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject comic;
    void Start()
    {
        Time.timeScale = 0f;
        comic.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
