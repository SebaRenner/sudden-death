using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform healthBarFill;

    private Player player;
    private PlayerController controller;


    void Update()
    {
        SetHealthAmount(player.GetHealthPct());
    }


    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
    }

}
