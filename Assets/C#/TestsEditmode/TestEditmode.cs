using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestEditmode
{
    private GameObject _gameObj;
    private PlayerController _player;

    [Test]
    [SetUp]
    public void SystemSetup()
    {
        // Khởi tạo đối tượng trước mỗi bài test
        _gameObj = new GameObject();
        _player = _gameObj.AddComponent<PlayerController>();

        // Giả lập giá trị ban đầu (vì Start() không chạy trong EditMode Test)
        _player.maxHealth = 100f;
        _player.currentHealth = 100f;
    }
    [Test]
    public void Test_Heal_IncreasesHealth()
    {
        // Setup: Để máu thấp xuống trước
        _player.currentHealth = 50f;
        _player.Heal(20f);

        // Assert: 50 + 20 = 70
        Assert.AreEqual(70f, _player.currentHealth, "Hàm Heal không tăng máu đúng!");
    }
    [Test]
    public void Test_Heal_NotExceedMaxHealth()
    {
        // Setup: Máu đang gần đầy
        _player.currentHealth = 90f;

        // Action: Hồi 50 máu (Tổng là 140)
        _player.Heal(50f);

        // Assert: Máu không được vượt quá maxHealth (100)
        Assert.AreEqual(100f, _player.currentHealth, "Máu vượt quá giới hạn tối đa sau khi hồi!");
    }
}
