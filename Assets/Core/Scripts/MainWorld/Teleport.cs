using UnityEngine;

namespace Main.World
{
    public class Teleport : MonoBehaviour
    {
        [Header("Tujuan Teleport")]
        [Tooltip("Tarik objek GameObject kosong (Transform) yang jadi titik tujuan teleport ke sini")]
        public Transform teleportDestination;

        // Fungsi ini otomatis terpanggil saat ada objek yang menabrak area trigger
        private void OnTriggerEnter(Collider other) // Gunakan OnCollisionEnter jika tidak pakai Is Trigger
        {
            // Cek apakah yang masuk adalah Player (bisa dicocokkan pakai Tag)
            if (other.CompareTag("Player"))
            {
                // Matikan CharacterController sementara jika Player Anda menggunakannya,
                // agar Unity tidak protes saat posisi pemain dipindah paksa.
                CharacterController controller = other.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                }

                // PINDAHKAN POSISI PEMAIN KE TITIK TUJUAN
                other.transform.position = teleportDestination.position;

                // Nyalakan kembali CharacterController-nya
                if (controller != null)
                {
                    controller.enabled = true;
                }
            }
        }
    }
}
