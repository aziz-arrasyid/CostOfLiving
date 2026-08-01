using UnityEngine;
using UnityEngine.SceneManagement;
using PrimeTween;
using System.Collections;

namespace Game.System
{
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager Instance;

        [Header("Referensi UI")]
        [Tooltip("Masukkan Canvas Group yang berisi gambar hitam full screen")]
        public CanvasGroup fadeCanvasGroup;

        [Header("Pengaturan")]
        public float fadeDuration = 0.5f;

        private void Awake()
        {
            // Memastikan hanya ada 1 Transition Manager di dalam game
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Jangan hancurkan objek ini saat pindah scene
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Pastikan saat game mulai, layar dalam keadaan terang dan bisa diklik
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }

        // Panggil fungsi ini dari Button atau Script lain
        public void LoadScene(string sceneName, bool showCursor)
        {
            if (showCursor)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            StartCoroutine(TransitionRoutine(sceneName));
        }

        private IEnumerator TransitionRoutine(string sceneName)
        {
            // 1. Blokir semua klik UI pemain saat transisi agar tidak spam tombol
            fadeCanvasGroup.blocksRaycasts = true;

            // 2. FADE OUT: Layar pelan-pelan menjadi hitam
            Tween.Alpha(fadeCanvasGroup, 1f, fadeDuration);

            // Tunggu sampai animasi hitamnya selesai
            yield return new WaitForSeconds(fadeDuration);

            // 3. LOAD SCENE: Menggunakan Async agar game tidak freeze (Not Responding)
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Tunggu di background sampai scene benar-benar 100% termuat
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // 4. FADE IN: Layar kembali terang perlahan
            Tween.Alpha(fadeCanvasGroup, 0f, fadeDuration);

            // Buka kembali blokir kliknya
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }
}

