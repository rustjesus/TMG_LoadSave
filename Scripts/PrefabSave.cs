using UnityEngine;

namespace TMG
{
    public class PrefabSave : MonoBehaviour
    {
        public bool ui_Obj = true;
        public GameObject prefab;
        public int prefabIndex = 0;
        public string spawnCountKey = "image0";
        private PrefabLoad prefabLoad;
        private void Start()
        {
            prefabLoad = FindObjectOfType<PrefabLoad>();

            PlayerPrefs.SetInt(spawnCountKey, PlayerPrefs.GetInt(spawnCountKey) + 1);

            // Check if the prefab was found in the array
            if (prefabIndex < 0)
            {
                Debug.LogError($"Prefab {prefab.name} was not found in the prefabsToLoad array.");
                return;
            }

            prefabLoad.objectList.Add(gameObject);

        }

        private void OnDestroy()
        {
            prefabLoad.objectList.Remove(gameObject);
            PlayerPrefs.DeleteKey(spawnCountKey);
        }
    }

}
