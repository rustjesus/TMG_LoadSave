using UnityEngine;
using TMPro;

namespace TMG
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] float UpdateTime = 0.3f;
        TextMeshProUGUI Text;

        int FpsCount = 0;
        float Timer = 0;

        void Start()
        {
            Text = GetComponent<TextMeshProUGUI>();
            Text.color = Color.yellow;
        }

        void LateUpdate()
        {
            if (Timer >= UpdateTime)
            {
                Text.text = ((int)(FpsCount / Timer)).ToString();
                Timer = 0;
                FpsCount = 0;
            }
            else
            {
                Timer += Time.deltaTime;
                FpsCount++;
            }

 
        }
    }
}