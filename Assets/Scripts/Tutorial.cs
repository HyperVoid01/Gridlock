using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Stats stats;
    
    public GameObject[] tutorialPanels;
    public int currentPanel = 0;

    private void Start()
    {
        if (!stats.doneTutorial)
            tutorialPanels[0].SetActive(true);
    }
    
    private void Update()
    {
        if (stats.doneTutorial) return;
        
        if (Input.anyKeyDown)
            NextPanel();
    }

    public void NextPanel()
    {
        tutorialPanels[currentPanel].SetActive(false);

        if (currentPanel < tutorialPanels.Length - 1)
        {
            currentPanel++;
            tutorialPanels[currentPanel].SetActive(true);
            return;
        }
        
        stats.doneTutorial = true;
    }
}
