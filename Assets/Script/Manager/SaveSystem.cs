using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem _instance;
    public static SaveSystem Instance => _instance;

    [System.Serializable]
    public class SaveData
    {
        public int deathTotal = 2000; // Start with maximum value
        public float BGMVolume = 1f;
        public float SFXVolume = 1f;
    }

    private string savePath;
    private SaveData currentSaveData;
    public int Death = 0;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "death_save.json");

        Debug.Log($"JSON Save File Location: {savePath}");
        Debug.Log($"Persistent Data Path: {Application.persistentDataPath}");

        LoadDeathTotal();
    }

    public void CheckAndSaveDeathTotal(int currentScore)
    {
        if (currentScore < currentSaveData.deathTotal)
        {
            currentSaveData.deathTotal = currentScore;
            SaveDataToJson();
            Debug.Log($"New lowest death total saved: {currentScore}");
        }
        else
        {
            Debug.Log($"Current score {currentScore} is not lower than saved total {currentSaveData.deathTotal}");
        }
    }

    private void SaveDataToJson()
    {
        try
        {
            string json = JsonUtility.ToJson(currentSaveData, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"Death total saved: {currentSaveData.deathTotal}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save death total: {e.Message}");
        }
    }

    private void LoadDeathTotal()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                currentSaveData = JsonUtility.FromJson<SaveData>(json);
                Debug.Log($"Loaded death total: {currentSaveData.deathTotal}");
            }
            else
            {
                currentSaveData = new SaveData();
                Debug.Log("No save file found, created new save data");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load death total: {e.Message}");
            currentSaveData = new SaveData();
        }
    }

    // Public getter for the death total
    public int GetDeathTotal()
    {
        return currentSaveData.deathTotal;
    }

    public float GetBGMVolume()
    {
        return currentSaveData.BGMVolume;
    }

    public float GetSFXVolume()
    {
        return currentSaveData.SFXVolume;
    }

    public void SaveVolumes(float s, float b)
    {
        currentSaveData.SFXVolume = s;
        currentSaveData.BGMVolume = b;
        SaveDataToJson();
    }

    public void NextScene(int temp)
    {
        Death = Death + temp;
    }

    public void ResetDeathTotal()
    {
        currentSaveData.deathTotal = int.MaxValue;
        SaveDataToJson();
        Debug.Log("Death total reset");
    }
}
