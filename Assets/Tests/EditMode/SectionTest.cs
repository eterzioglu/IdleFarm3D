using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

public class SectionTest 
{
    private List<Section> sections = new List<Section>();
    private GoldManager goldManager;
    private GameObject go;

    [SetUp]
    public void Setup()
    {
        go = new GameObject();
        goldManager = go.AddComponent<GoldManager>();

        for (int i = 0; i < 3; i++)
        {
            GameObject sectionGo = new GameObject($"Section_{i}");
            sectionGo.AddComponent<WorkTask>();
            Section section = sectionGo.AddComponent<Section>();
            section.SetIdForTesting(i.ToString());
            section.SetUnlockCostForTesting(50 * (i + 1));
            sections.Add(section);
        }
    }

    [TearDown]
    public void TearDown()
    {
        foreach (var section in sections)
        {
            Object.DestroyImmediate(section.gameObject);
        }
        sections.Clear();
    }

    [Test]
    public void TestUnlockSection()
    {
        goldManager.ResetGold();
        goldManager.AddGold(200);

        Section sectionToUnlock = sections[1];
        bool unlocked = sectionToUnlock.TryUnlockForTesting(goldManager);

        Assert.IsTrue(unlocked);
        Assert.IsTrue(sectionToUnlock.IsUnlocked);
        Assert.AreEqual(100, goldManager.Gold);
    }
}
