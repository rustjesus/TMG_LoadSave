using UnityEngine;

namespace TMG
{
    public class testload : MonoBehaviour
    {
        public GameObject prefabToLoadRed;
        public GameObject prefabToLoadWhite;
        public GameObject prefabToLoadBlue;
        public GameObject prefabToLoadRed_UI;
        public GameObject prefabToLoadWhite_UI;
        public GameObject prefabToLoadBlue_UI;
        public Transform canvas;
        public Transform cubeSpawn;
        void Start()
        {
            Debug.Log(Application.dataPath);
        }
        public void SpawnRedPrefab()
        {
            Vector3 pos = cubeSpawn.position;
            Instantiate(prefabToLoadRed, pos, Quaternion.identity);
        }
        public void SpawnWhitePrefab()
        {
            Vector3 pos = cubeSpawn.position;
            Instantiate(prefabToLoadWhite, pos, Quaternion.identity);
        }
        public void SpawnBluePrefab()
        {
            Vector3 pos = cubeSpawn.position;
            Instantiate(prefabToLoadBlue, pos, Quaternion.identity);
        }
        public void SpawnRedPrefab_UI()
        {
            Instantiate(prefabToLoadRed_UI, canvas);
        }
        public void SpawnWhitePrefab_UI()
        {
            Instantiate(prefabToLoadWhite_UI, canvas);
        }
        public void SpawnBluePrefab_UI()
        {
            Instantiate(prefabToLoadBlue_UI, canvas);
        }

    }

}
