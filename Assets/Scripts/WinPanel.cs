using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public GameObject winPanel;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winPanel.SetActive(true);
        }
    }
}
