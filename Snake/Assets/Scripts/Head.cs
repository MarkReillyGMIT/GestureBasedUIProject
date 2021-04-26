using UnityEngine;

/*
 *  The following class is used to manage which item
 *  is colliding with the head of the snake.
 * 
 */
public class Head : MonoBehaviour
{
    public GameController gameController;

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Food":
                gameController.Eat();
                break;
            case "Tail":
                gameController.GameOver();
                break;
        }
    }
}
