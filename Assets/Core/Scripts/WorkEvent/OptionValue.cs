using UnityEngine;
using UnityEngine.UI;

namespace Work.Event
{
    [RequireComponent(typeof(Button))]
    public class OptionValue : MonoBehaviour
    {
        [Header("Data")]
        public int value;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => WorkEventManager.Instance.OnOptionSelected(value));
        }
    }
}
