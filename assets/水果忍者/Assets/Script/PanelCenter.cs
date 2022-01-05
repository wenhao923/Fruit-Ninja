using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCenter : MonoBehaviour
{

    public RectTransform firstPanelClone;
    public RectTransform gamePanelClone;

    // Start is called before the first frame update
    void Start()
    {
        createFirstPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void createFirstPanel()
    {
        RectTransform firstPanel = Instantiate(firstPanelClone, firstPanelClone.position, firstPanelClone.rotation) as RectTransform;
        firstPanel.parent = transform;
        firstPanel.anchoredPosition3D = new Vector3(0, 0, 0);
        firstPanel.localScale = new Vector3(1, 1, 1);
        firstPanel.offsetMin = new Vector2(0, 0);
        firstPanel.offsetMax = new Vector2(0, 0);
    }

    private void createGamePanel()
    {
        RectTransform gamePanel = Instantiate(gamePanelClone, gamePanelClone.position, gamePanelClone.rotation) as RectTransform;
        gamePanel.parent = transform;
        gamePanel.anchoredPosition3D = new Vector3(0, 0, 0);
        gamePanel.localScale = new Vector3(1, 1, 1);
        gamePanel.offsetMin = new Vector2(0, 0);
        gamePanel.offsetMax = new Vector2(0, 0);
    }

    public void showGamePanel()
    {
        createGamePanel();
    }

    public void showFirstPanel()
    {
        createFirstPanel();
    }
}
