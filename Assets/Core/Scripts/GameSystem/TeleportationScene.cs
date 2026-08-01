using UnityEngine;

namespace Game.System
{
    public class TeleportationScene : MonoBehaviour
    {
        public string sceneDestination;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TransitionManager.Instance.LoadScene(sceneDestination, true);
            }
        }
    }
}
