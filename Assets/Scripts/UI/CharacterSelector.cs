using UnityEngine;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private Transform previewPoint;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private TMP_Text nameText;

    private int index = 0;
    private GameObject currentObj;

    private void Start()
    {
        ShowCharacter();
    }

    public void Next()
    {
        index++;
        if (index >= characters.Length)
            index = 0;

        ShowCharacter();
    }

    public void Previous()
    {
        index--;
        if (index < 0)
            index = characters.Length - 1;

        ShowCharacter();
    }

    void ShowCharacter()
    {
        if (currentObj != null)
            Destroy(currentObj);

        currentObj = Instantiate(characters[index], previewPoint.position, previewPoint.rotation);

        currentObj.transform.SetParent(previewPoint);

        nameText.text = characters[index].name;

        PlayerPrefs.SetInt("CharacterIndex", index);

        Debug.Log("Personaje seleccionado: " + characters[index].name);
    }
}