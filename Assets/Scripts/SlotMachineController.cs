using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SlotMachineController : MonoBehaviour
{
    public List<Sprite> symbolSprites;
    public List<Image> slotImages;

    public Sprite[,] slotGrid = new Sprite[3, 3];
    public int playerScore = 0;

    public TMP_Text scoreText;
    public AudioClip spinSound;
    public AudioClip winSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpinSlots()
    {
        StartCoroutine(SpinAnimation());
    }

    void PopulateSlotGrid()
    {
        int index = 0;

        for(int row = 0; row < 3; row++)
        {
            for(int col = 0; col < 3; col++)
            {
                slotGrid[row, col] = slotImages[index].sprite;
                index++;
            }
        }
    }

    void CheckWinningCombination()
    {
        // Check rows
        for (int row = 0; row < 3; row++)
        {
            if (slotGrid[row, 0] == slotGrid[row, 1] && slotGrid[row, 1] == slotGrid[row, 2] && slotGrid[row, 0] != null)
            {
                // Row win logic
                Debug.Log("Row " + row + " Win!!");
                UpdateScore();
                audioSource.clip = winSound;
                audioSource.time = 9f;
  
                audioSource.Play();

                return;
            }
        }

        // Check columns
        for (int col = 0; col < 3; col++)
        {
            if (slotGrid[col, 0] == slotGrid[col, 1] && slotGrid[col, 1] == slotGrid[col, 2] && slotGrid[col, 0] != null)
            {
                //Column win logic
                Debug.Log("Column " + col + " Win!!");
                UpdateScore();
                audioSource.clip = winSound;
                audioSource.time = 9f;
                audioSource.Play();
                return;
            }
        }

        //Check Diagonal
        if (slotGrid[0,0] == slotGrid[1,1] && slotGrid[1,1] == slotGrid[2,2] && slotGrid[0,0] != null)
        {
            Debug.Log("Diagonal win !!");
            UpdateScore();
            audioSource.clip = winSound;
            audioSource.time = 9f;
            audioSource.Play();
            return;
        }
    }

    void UpdateScore()
    {
        playerScore += 10;
        scoreText.text = playerScore.ToString();
    }

    public IEnumerator SpinAnimation()
    {
        float spinDuration = 3f; 
        float spinInterval = 0.1f;
        float elapsed = 0f;

        // Loop to animate the spinning by briefly showing random symbols
        audioSource.PlayOneShot(spinSound);
        while (elapsed < spinDuration)
        {
            foreach (Image slot in slotImages)
            {
                // Show a random symbol in each slot
                Sprite randomSymbol = symbolSprites[Random.Range(0, symbolSprites.Count)];
                slot.sprite = randomSymbol;
            }

            elapsed += spinInterval;
            yield return new WaitForSeconds(spinInterval); // Short delay for each "spin"
        }

        // After the spin animation completes, stop with a final random set of symbols
        foreach (Image slot in slotImages)
        {
            Sprite finalSymbol = symbolSprites[Random.Range(0, symbolSprites.Count)];
            slot.sprite = finalSymbol;
        }

        audioSource.Stop();
        PopulateSlotGrid();
        CheckWinningCombination();
    }

}
