using UnityEngine;
using UnityEngine.UI;

public class ObjectTransformer : MonoBehaviour
{
    public Button rotatePositiveButton;
    public Button rotateNegativeButton;
    public Button moveXPositiveButton;
    public Button moveXNegativeButton;
    public Button moveZPositiveButton;
    public Button moveZNegativeButton;

   public ARGameManager gameManager;

    public float rotationAmount = 90f;
    public float moveAmount = 10f;

    void Start()
    {
        // Add click listeners to the buttons
        rotatePositiveButton.onClick.AddListener(RotatePositive);
        rotateNegativeButton.onClick.AddListener(RotateNegative);
        moveXPositiveButton.onClick.AddListener(MoveXPositive);
        moveXNegativeButton.onClick.AddListener(MoveXNegative);
        moveZPositiveButton.onClick.AddListener(MoveZPositive);
        moveZNegativeButton.onClick.AddListener(MoveZNegative);

        
    }

    void RotatePositive()
    {
        if(gameManager.selectedPiece != null)
        {
            // Rotate the object by the positive rotation amount
             gameManager.selectedPiece.transform.Rotate(Vector3.up, rotationAmount);
        }
    }

    void RotateNegative()
    {
        if (gameManager.selectedPiece != null)
        {
            // Rotate the object by the negative rotation amount
            gameManager.selectedPiece.transform.Rotate(Vector3.up, -rotationAmount);
        }
    }

    void MoveXPositive()
    {
        if (gameManager.selectedPiece != null)
        {
            // Move the object in the positive X direction
            Vector3 movement = new Vector3(moveAmount, 0f, 0f);
            gameManager.selectedPiece.transform.position += movement;
        }
    }

    void MoveXNegative()
    {
        if (gameManager.selectedPiece != null)
        {
            // Move the object in the negative X direction
            Vector3 movement = new Vector3(-moveAmount, 0f, 0f);
            gameManager.selectedPiece.transform.position += movement;
        }
    }

    void MoveZPositive()
    {
        if (gameManager.selectedPiece != null)
        {
            // Move the object in the positive Z direction
            Vector3 movement = new Vector3(0f, 0f, moveAmount);
            gameManager.selectedPiece.transform.position += movement;
        }
    }

    void MoveZNegative()
    {
        if (gameManager.selectedPiece != null)
        {
            // Move the object in the negative Z direction
            Vector3 movement = new Vector3(0f, 0f, -moveAmount);
            gameManager.selectedPiece.transform.position += movement;
        }
    }
}
