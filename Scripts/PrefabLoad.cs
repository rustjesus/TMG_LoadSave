using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TMG
{
    public class PrefabLoad : MonoBehaviour
    {
        [SerializeField] private bool _deleteBeforeSaving = true;
        [SerializeField] private bool _loadOnAwake = false;
        public GameObject[] prefabsToLoad;
        [SerializeField] private Transform _canvas;
        public List<Vector3> positionlist;
        public List<GameObject> objectList = new List<GameObject>();
        private void Awake()
        {
            if (_loadOnAwake)
            {
                LoadAll();
            }
        }
        public void LoadAll()
        {
            // Get all JSON files in the data path
            string[] filePaths;
            // Construct the file path
            if (Application.isEditor)
            {
                filePaths = Directory.GetFiles(Application.dataPath + "/TMG_LoadSave/Scripts/Jsons", "*.json");
            }
            else
            {
                filePaths = Directory.GetFiles(Application.persistentDataPath, "*.json");
            }
            // Load each file
            foreach (string filePath in filePaths)
            {
                Load(filePath);
            }

        }
        private void Load(string filePath)
        {
            // Load the JSON data from the specified file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON data into a PrefabData object
            PrefabData prefabData = JsonUtility.FromJson<PrefabData>(json);

            // Check if the index is within the bounds of the prefabsToLoad array
            if (prefabData.prefabindex < 0 || prefabData.prefabindex >= prefabsToLoad.Length)
            {
                Debug.LogError($"Prefab index {prefabData.prefabindex} is out of range.");
                return;
            }

            // Get the prefab at the specified index
            GameObject prefab = prefabsToLoad[prefabData.prefabindex];

            // Check if the prefab with the matching name already exists in the scene
            GameObject existingPrefab = GameObject.Find(prefab.name);
            if (existingPrefab != null && existingPrefab.GetComponent<PrefabSave>() != null && existingPrefab.GetComponent<PrefabSave>().prefabIndex == prefabData.prefabindex)
            {
                Debug.Log($"Prefab {prefab.name} already exists in the scene, skipping loading from JSON.");
                return;
            }
            else
            {
                for (int i = 0; i < prefabData.positions.Count; i++)
                {
                    int index = prefabData.prefabindex;
                    Debug.Log("spawn");
                    // Instantiate the prefab at the corresponding position
                    GameObject prefabnew = Instantiate(prefabsToLoad[index], prefabData.positions[i], Quaternion.identity);
                    if (prefabnew.GetComponent<PrefabSave>().ui_Obj == true)
                    {
                        prefabnew.transform.SetParent(_canvas.transform);
                    }
                }
            }
        }
        public void DeleteSavedGameData()
        {
            string[] filePaths;
            // Construct the file path
            if (Application.isEditor)
            {
                filePaths = Directory.GetFiles(Application.dataPath + "/TMG_LoadSave/Scripts/Jsons", "*.json");
            }
            else
            {
                filePaths = Directory.GetFiles(Application.persistentDataPath, "*.json");
            }

            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
            PlayerPrefs.DeleteAll();
        }
        private bool doOnce = false;
        private void RemovePositions()
        {
            if (doOnce == false)
            {
                doOnce = true;
                positionlist.Clear();
            }
        }

        private void Update()
        {
            doOnce = false;
        }

        private void Sort()
        {
            //Sorting list 
            if (objectList.Count > 0)
            {
                objectList.Sort(delegate (GameObject a, GameObject b) {
                    return (a.GetComponent<PrefabSave>().prefabIndex).CompareTo(b.GetComponent<PrefabSave>().prefabIndex);
                });
            }
        }
        public void Save()
        {
            if (_deleteBeforeSaving)
            {
                DeleteSavedGameData();
            }
            Sort();
            RemovePositions();
            int previousPrefabIndex = 0;
            for (int i = 0; i < objectList.Count; i++)
            {
                var prefabToSave = objectList[i].GetComponent<PrefabSave>();

                // Check if the prefab index has changed
                if (previousPrefabIndex != prefabToSave.prefabIndex)
                {
                    positionlist.Clear();
                    Debug.Log("DIFFERENT");
                    // Update the previous prefab index
                    previousPrefabIndex = prefabToSave.prefabIndex;
                }
                string filePath;
                // Construct the file path
                if (Application.isEditor)
                {
                    filePath = Path.Combine(Application.dataPath, "TMG_LoadSave/Scripts/Jsons", prefabToSave.spawnCountKey + ".json");
                }
                else
                {
                    filePath = Path.Combine(Application.persistentDataPath, prefabToSave.spawnCountKey + ".json");
                }

                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    // Create a new JSON string with default values
                    string defaultJson = JsonUtility.ToJson(new PrefabData(prefabToSave.prefabIndex, 0)
                    {
                        positions = new List<Vector3>() { prefabToSave.transform.position } // Add current position to list
                    });

                    // Write the JSON string to the file
                    File.WriteAllText(filePath, defaultJson);
                }

                // Load the JSON data from the file
                string json = File.ReadAllText(filePath);


                // Deserialize the JSON data into a PrefabData object
                PrefabData prefabData = JsonUtility.FromJson<PrefabData>(json);

                //need to check prefabindex value and remove from loop so it doesnt write multiples
                Debug.Log("data index = " + prefabData.prefabindex);
                Debug.Log("save index = " + prefabToSave.prefabIndex);
                // Add the current position to the list
                positionlist.Add(prefabToSave.transform.position);

                // Serialize the data to JSON
                string jsonstring = JsonUtility.ToJson(new PrefabData(prefabToSave.prefabIndex, PlayerPrefs.GetInt(prefabToSave.spawnCountKey))
                {
                    positions = positionlist
                });

                // Overwrite the JSON data for the current prefab
                File.WriteAllText(filePath, jsonstring);

                Debug.Log("index match");
            }
        }

        private class PrefabData
        {
            public int prefabindex;
            public int spawnCount;
            public List<Vector3> positions;

            public PrefabData(int prefabIndex, int spawnCount)
            {
                this.prefabindex = prefabIndex;
                this.spawnCount = spawnCount;
                this.positions = new List<Vector3>();
            }
        }
        private class PrefabDataList
        {
            public List<PrefabData> prefabDataList;

            public PrefabDataList(List<PrefabData> prefabDataList)
            {
                this.prefabDataList = prefabDataList;
            }
        }
    }
}
