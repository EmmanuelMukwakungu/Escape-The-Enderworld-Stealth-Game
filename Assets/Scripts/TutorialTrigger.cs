using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
   public GameObject tutorialTrigger;

   private bool hasShown = false;

   private void OnTriggerEnter( Collider other )
   {
      if (other.CompareTag("Player") && !hasShown)
      {
         tutorialTrigger.SetActive(true);
         hasShown = true;
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         tutorialTrigger.SetActive(false);
      }
   }
   
  
}
