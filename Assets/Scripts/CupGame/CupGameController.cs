using System.Collections;
using UnityEngine;

public class CupGameController : MonoBehaviour
{
    public Transform[] cups;
    public Transform ball;
    public float shuffleSpeed = 3f;
    public int shuffleCount = 5;
    public float liftHeight = 1f;

    private int correctCupIndex;
    private bool canChoose = false;
    private bool isAnimating = false;

    void Start()
    {
        if (cups == null || cups.Length == 0)
        {
            Debug.LogError("Nie przypisano kubków w Inspectorze");
            return;
        }

        if (ball == null)
        {
            Debug.LogError("Nie przypisano kulki w Inspectorze");
            return;
        }

        correctCupIndex = Random.Range(0, cups.Length);

        Vector3 ballPos = cups[correctCupIndex].position;
        ballPos.y -= 0.5f;
        ball.position = ballPos;

        StartCoroutine(RevealBallAtStart());
    }

    IEnumerator RevealBallAtStart()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Transform cup in cups)
        {
            StartCoroutine(MoveCupY(cup, cup.position.y + liftHeight, 0.5f));
        }

        yield return new WaitForSeconds(1.2f);

        foreach (Transform cup in cups)
        {
            StartCoroutine(MoveCupY(cup, cup.position.y - liftHeight, 0.5f));
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(ShuffleCups());
    }

    IEnumerator ShuffleCups()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < shuffleCount; i++)
        {
            int cupA = Random.Range(0, cups.Length);
            int cupB = Random.Range(0, cups.Length);
            while (cupB == cupA)
                cupB = Random.Range(0, cups.Length);

            yield return StartCoroutine(SwapCups(cups[cupA], cups[cupB]));
        }

        canChoose = true;
        Debug.Log("🎮 Kliknij na kubek!");
    }

    IEnumerator SwapCups(Transform cupA, Transform cupB)
    {
        isAnimating = true;

        Vector3 posA = cupA.position;
        Vector3 posB = cupB.position;
        float t = 0f;


        bool ballUnderA = Mathf.Abs(ball.position.x - posA.x) < 0.01f;
        bool ballUnderB = Mathf.Abs(ball.position.x - posB.x) < 0.01f;

        while (t < 1f)
        {
            t += Time.deltaTime * shuffleSpeed;
            float smoothT = Mathf.SmoothStep(0, 1, t);


            cupA.position = Vector3.Lerp(posA, posB, smoothT);
            cupB.position = Vector3.Lerp(posB, posA, smoothT);


            if (ballUnderA)
            {
                ball.position = new Vector3(
                    Mathf.Lerp(posA.x, posB.x, smoothT),
                    ball.position.y,
                    ball.position.z
                );
            }
            else if (ballUnderB)
            {
                ball.position = new Vector3(
                    Mathf.Lerp(posB.x, posA.x, smoothT),
                    ball.position.y,
                    ball.position.z
                );
            }

            yield return null;
        }

        if (ballUnderA)
            ball.position = new Vector3(posB.x, ball.position.y, ball.position.z);
        else if (ballUnderB)
            ball.position = new Vector3(posA.x, ball.position.y, ball.position.z);

        isAnimating = false;
    }

    public void ChooseCup(int index)
    {
        if (!canChoose || isAnimating) return;

        canChoose = false;
        StartCoroutine(RevealChosenCup(index));
    }

    IEnumerator RevealChosenCup(int index)
    {
        Transform chosenCup = cups[index];

        yield return StartCoroutine(MoveCupY(chosenCup, chosenCup.position.y + liftHeight, 0.4f));

        if (index == correctCupIndex)
            Debug.Log("Trafiłeś!");
        else
            Debug.Log("Pudło!");

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(MoveCupY(chosenCup, chosenCup.position.y - liftHeight, 0.4f));
    }

    IEnumerator MoveCupY(Transform cup, float targetY, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = cup.position;
        Vector3 endPos = new Vector3(cup.position.x, targetY, cup.position.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cup.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }
    }
}
