using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    private bool collected;

    private void OnEnable()
    {
        collected = false;
    }

    public void Collect(Player playerTransform)
    {
        if (collected)
            return;

        collected = true;

        StartCoroutine(MoveTowardsPlayer(playerTransform));
    }

    IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0;
        Vector2 initialPosition = transform.position;
        //Vector2 targetPosition = player.GetCenter();

        while (timer < 1)
        {
            Vector2 targetPosition = player.GetCenter();

            //Moves towards the player
            transform.position = Vector2.Lerp(initialPosition, targetPosition, timer);

            timer += Time.deltaTime;

            yield return null;
        }

        Collected();
    }

    protected abstract void Collected();
}
