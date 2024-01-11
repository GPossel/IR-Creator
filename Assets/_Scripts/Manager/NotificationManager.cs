using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NotificationManager : MonoBehaviour
{
    private Image nofiBg;
    private Text notificTxt;

    Color backGroundColor;
    Color txtColor;

    private void Start()
    {
        nofiBg = this.GetComponent<Image>();
        notificTxt = transform.GetChild(0).GetComponent<Text>();
        backGroundColor = nofiBg.color;
        backGroundColor.a = 0f;
        nofiBg.color = backGroundColor;
        txtColor = notificTxt.color;
        txtColor.a = 0f;
        notificTxt.color = txtColor;
    }

    public void ShowNotification(string message, float posXcenter)
    {
        this.transform.position = new Vector3(posXcenter, this.transform.position.y, this.transform.position.z);
        notificTxt.text = message;
        StartCoroutine(FadeIn(backGroundColor, txtColor));
        StartCoroutine(FadeOut(backGroundColor, txtColor));
    }

    IEnumerator FadeOut(Color background, Color txt)
    {
        for (float alpha = 0f; alpha <= 0; alpha += 0.1f)
        {
            background.a = alpha;
            nofiBg.color = background;

            txt.a = alpha;
            notificTxt.color = txt;
            yield return new WaitForSeconds(.1f);
        }

        background.a = 1;
        nofiBg.color = background;

        txt.a = 1;
        notificTxt.color = txt;
    }

    IEnumerator FadeIn(Color background, Color txt)
    {
        // remember we start from 1, so it 'pops-up' and then slowly fades
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            background.a = alpha;
            nofiBg.color = background;

            txt.a = alpha;
            notificTxt.color = txt;
            yield return new WaitForSeconds(.1f);
        }

        background.a = 0;
        nofiBg.color = background;

        txt.a = 0;
        notificTxt.color = txt;
    }
}