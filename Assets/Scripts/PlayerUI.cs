using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuellFill;

    private PlayerController controller;

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    private void Update()
    {
        SetFuellAmount(controller.GetThrusterFuelAmount());
    }

    void SetFuellAmount(float _amount)
    {
        thrusterFuellFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
