﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager
{
    private List<BaseMinigame> m_CurrentMinigames;
    private int m_CurrentMinigameID = 0;

    public void StartMinigame(MinigameDisplayComponent display, MinigameType type, int playerID, Action<MinigameTickResult> callback = null)
    {
        BaseMinigame minigame = null;
        switch (type)
        {
            case MinigameType.Sequence:
            
                minigame = new SequenceMinigame();
                minigame.Setup(m_CurrentMinigameID, display, new Rect(100, 100, 10, 10), playerID, callback);            
            break;
            case MinigameType.Screwdriver:
            
                minigame = new ScrewdriverMinigame();
                minigame.Setup(m_CurrentMinigameID, display, new Rect(200, 100, 10, 10), playerID, callback);
            break;
        }

        BeginMinigame(minigame);

        m_CurrentMinigameID++;
    }

    void BeginMinigame(BaseMinigame minigame)
    {
        minigame.Start();
        m_CurrentMinigames.Add(minigame);
    }

    public void Init()
    {
        m_CurrentMinigames = new List<BaseMinigame>();
    }

    ////////////////////////////////////////////////////////////////

    public void Tick()
    {
        for (int i = 0; i < m_CurrentMinigames.Count; i++)
        {
            MinigameTickResult result = m_CurrentMinigames[i].Tick();

            if (result != MinigameTickResult.ContinueTick)
            {
                m_CurrentMinigames[i].FinishCallback?.Invoke(result);
                m_CurrentMinigames[i].CleanUp();
                m_CurrentMinigames[i].Finish();
                m_CurrentMinigames.RemoveAt(i);
                i--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartMinigame(null, MinigameType.Sequence, 0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartMinigame(null, MinigameType.Screwdriver, 0);
        }

    }

    ////////////////////////////////////////////////////////////////
}
