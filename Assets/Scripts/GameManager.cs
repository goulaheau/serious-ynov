using System.Collections;
using System.Collections.Generic;
using Json;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Text _warningText;
    [SerializeField] private GameObject _situationPanel;
    [SerializeField] private Text _situationText;
    [SerializeField] private GameObject _choiceParent;
    [SerializeField] private GameObject _choicePrefab;
    [SerializeField] private Transform _player;

    private List<Situation> _situations;
    private readonly List<GameObject> _instantiatedChoices = new List<GameObject>();

    private int _warningCount;

    public void Start()
    {
        foreach (var instantiatedChoice in _instantiatedChoices)
        {
            Destroy(instantiatedChoice);
        }
        
        _warningCount = 0;
        SetWarningText();
        _situationPanel.SetActive(true);
        _situations = JsonObject.GetSituations();
        StartCoroutine(LaunchSituation(_situations[0], 0f));
    }

    public void Choose(Situation situation, Choice choice)
    {
        if (choice.consequence.warning)
        {
            ++_warningCount;
            SetWarningText();
        }

        _situationText.text = choice.consequence.description;

        foreach (var instantiatedChoice in _instantiatedChoices)
        {
            Destroy(instantiatedChoice);
        }

        _situations.Remove(situation);

        if (_situations.Count > 0)
        {
            StartCoroutine(LaunchSituation(_situations[0], 5f));
        }
        else
        {
            StartCoroutine(FinishGame());
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }

    private IEnumerator LaunchSituation(Situation situation, float time)
    {
        yield return new WaitForSeconds(time);

        _player.position = situation.position;
        _player.localEulerAngles = situation.rotation;

        _situationText.text = situation.description;

        while (situation.choices.Count > 0)
        {
            var choice = situation.choices[Random.Range(0, situation.choices.Count - 1)];

            var choiceInstance = Instantiate(
                _choicePrefab.gameObject,
                _choiceParent.transform
            );

            var choiceText = choiceInstance.GetComponentInChildren<Text>();
            choiceText.text = choice.description;

            var choiceButtonController = choiceInstance
                .GetComponentInChildren<Button>()
                .GetComponent<ButtonController>();
            choiceButtonController.Situation = situation;
            choiceButtonController.Choice = choice;

            _instantiatedChoices.Add(choiceInstance);

            situation.choices.Remove(choice);
        }
    }

    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(5f);

        _situationText.text = "Vous avez fini le jeu avec " + _warningCount + " avertissement(s).\n";

        if (_warningCount >= 3)
        {
            _situationText.text += "Vous êtes renvoyés de l'école !";
        }
        else if (_warningCount == 2)
        {
            _situationText.text += "Vous pouvez mieux faire !";
        }
        else
        {
            _situationText.text += "Vous ne pouviez rien faire de mieux, pas de chance !";
        }

        var choiceInstance = Instantiate(
            _choicePrefab.gameObject,
            _choiceParent.transform
        );

        var choiceText = choiceInstance.GetComponentInChildren<Text>();
        choiceText.text = "Pour rejouer cliquez sur le bouton ci-dessous";
        
        var choiceButtonController = choiceInstance
            .GetComponentInChildren<Button>()
            .GetComponent<ButtonController>();
        choiceButtonController.Situation = null;
        choiceButtonController.Choice = null;

        _instantiatedChoices.Add(choiceInstance);
    }

    private void SetWarningText()
    {
        _warningText.text = "Avertissement(s) : " + _warningCount;
    }
}