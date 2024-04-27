using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{ 
   [ShowOnly][SerializeField] private bool isInRange;
   [SerializeField] private string sceneName;
   private void OnTriggerEnter2D(Collider2D other)
   {
       if (!other.gameObject.CompareTag("Player")) return;
       ChangeScene();
   }

   private void ChangeScene() {
       if (sceneName != "") {
           SceneManager.LoadScene(sceneName);
       } else {
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       }
   }
   
   public void ChangeScene(string scene) {
       SceneManager.LoadScene(scene);
   }
}
