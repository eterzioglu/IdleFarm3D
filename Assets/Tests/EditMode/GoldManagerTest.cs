using NUnit.Framework;
using UnityEngine;

public class GoldManagerTest
{
    private GameObject go;
    private GoldManager goldManager;

    [SetUp]
    public void Setup()
    {
        go = new GameObject();
        goldManager = go.AddComponent<GoldManager>();
    }

    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    [Test]
    public void TestAddGold()
    {
        goldManager.ResetGold();
        goldManager.AddGold(10);
        goldManager.AddGold(25);
        Assert.AreEqual(35, goldManager.Gold);
    }

    [Test]
    public void TestTrySpendGold()
    {
        goldManager.ResetGold();
        goldManager.AddGold(50);
        bool result = goldManager.TrySpendGold(30);
        Assert.IsTrue(result);
        Assert.AreEqual(20, goldManager.Gold);

        result = goldManager.TrySpendGold(25);
        Assert.IsFalse(result);
        Assert.AreEqual(20, goldManager.Gold);
    }

    [Test]
    public void TestResetGold()
    {
        goldManager.AddGold(100);
        goldManager.ResetGold();
        Assert.AreEqual(0, goldManager.Gold);
    }

    [Test]
    public void TestAddNegativeGold()
    {
        goldManager.ResetGold();
        goldManager.AddGold(-10);
        Assert.AreEqual(0, goldManager.Gold);
    }
}
