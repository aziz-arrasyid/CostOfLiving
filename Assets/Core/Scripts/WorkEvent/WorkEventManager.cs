using System.Collections.Generic;
using UnityEngine;
using Game.System;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections;

namespace Work.Event
{
    public class WorkEventManager : MonoBehaviour
    {
        public static WorkEventManager Instance { private set; get; }

        [Header("Timer Coutdown Setting")]
        [SerializeField] private float timeRemaining;
        private bool timerIsRunning;

        [Space]

        [Header("Money Get Setting")]
        [SerializeField] private int baseMoneyGet;

        [Space]

        [Header("Math Question")]
        [SerializeField] private List<ModelMathQuestion> mathQuestions;
        private int currentIndex = 0;
        private ModelMathQuestion currentMathQuestion;
        private int currentAnswerSelected;
        private int moneyEarned;

        [Space]

        [Header("Diki Expresso")]
        [SerializeField] private List<Sprite> dikiExpression;
        [SerializeField] private List<Sprite> optionSelecting;
        private Coroutine dikiExpressionRoutine;

        [Space]

        [Header("UI")]
        [SerializeField] private Image dikiWorking;
        [SerializeField] private TextMeshProUGUI moneyEarnedText;
        [SerializeField] private TextMeshProUGUI totalMathQuestionsText;
        [SerializeField] private TextMeshProUGUI timerCountdownText;
        [SerializeField] private TextMeshProUGUI mathQuestionText;
        [SerializeField] private Button nextMathQuestionBtn;
        [SerializeField] private List<Button> mathOptions = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            nextMathQuestionBtn.onClick.AddListener(NextQuestion);

            RandomQuestion();
            UpdateCurrentMathQuestion();
            UpdateTimerUI();
            UpdateMoneyUI();

            Waiting();
            StartQuiz();
        }

        private void Update()
        {
            if (!timerIsRunning) return;

            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                StopQuiz();
            }

            UpdateTimerUI();
        }

        #region Public Function
        public void SetTimerCoutdownActive(bool status) { timerIsRunning = status; }

        public void OnOptionSelected(int value)
        {
            GameObject optionObj = EventSystem.current.currentSelectedGameObject;

            OptionReset();

            if (optionObj == null) return;

            optionObj.GetComponentInChildren<Image>().sprite = optionSelecting[1];
            currentAnswerSelected = value;

            nextMathQuestionBtn.interactable = true;
        }

        public void StopQuiz()
        {
            for (int i = 0; i < mathOptions.Count; i++)
            {
                mathOptions[i].interactable = false;
                mathOptions[i].GetComponentInChildren<Image>().sprite = optionSelecting[0];
            }
            nextMathQuestionBtn.interactable = false;

            PlayerData newPlayerData = GameManager.Instance.LoadData<PlayerData>("playerData");
            newPlayerData.money += moneyEarned;
            GameManager.Instance.SaveData(newPlayerData, "playerData");

            SetTimerCoutdownActive(false);

            ChangeScene("MainWorld", false);
        }

        public void ChangeScene(string sceneName, bool showCursor) { TransitionManager.Instance.LoadScene(sceneName, showCursor); }

        public void StartQuiz()
        {
            for (int i = 0; i < mathOptions.Count; i++)
            {
                mathOptions[i].interactable = true;
            }

            SetTimerCoutdownActive(true);
        }
        #endregion

        private void Waiting()
        {
            for (int i = 0; i < mathOptions.Count; i++)
            {
                mathOptions[i].interactable = false;
            }

            nextMathQuestionBtn.interactable = false;
        }


        private void NextQuestion()
        {
            CheckAnswer();
            if (currentIndex < mathQuestions.Count - 1)
            {
                currentIndex++;
                UpdateCurrentMathQuestion();
            }
        }

        private void UpdateTimerUI() { timerCountdownText.text = Mathf.FloorToInt(timeRemaining).ToString(); }
        private void UpdateMoneyUI() { moneyEarnedText.text = $"RP. {GameManager.Instance.CurrencyFormat(moneyEarned)}"; }

        private void UpdateMathUI()
        {
            totalMathQuestionsText.text = $"Question:{currentIndex + 1}/{mathQuestions.Count}";
            mathQuestionText.text = currentMathQuestion.question;

            for (int i = 0; i < mathOptions.Count; i++)
            {
                int optionValue = currentMathQuestion.options[i];

                mathOptions[i].GetComponent<OptionValue>().value = optionValue;

                TextMeshProUGUI mathOptionsText = mathOptions[i].GetComponentInChildren<TextMeshProUGUI>();
                char optionLetter = (char)(65 + i);
                mathOptionsText.text = $"{optionLetter}. {optionValue}";
            }

            nextMathQuestionBtn.interactable = false;
        }

        private void UpdateDikiWorkingUI(bool statusAnswer)
        {
            if (dikiExpressionRoutine != null)
            {
                StopCoroutine(dikiExpressionRoutine);
            }

            dikiExpressionRoutine = StartCoroutine(ExpressionRoutine(statusAnswer));
        }

        private IEnumerator ExpressionRoutine(bool statusAnswer)
        {
            if (statusAnswer)
            {
                dikiWorking.sprite = dikiExpression.Find(img => img.name == "correct");
            }
            else
            {
                dikiWorking.sprite = dikiExpression.Find(img => img.name == "incorrect");
            }

            yield return new WaitForSeconds(1.5f);

            dikiWorking.sprite = dikiExpression.Find(img => img.name == "neutral");

            dikiExpressionRoutine = null;
        }

        private void OptionReset()
        {
            for (int i = 0; i < mathOptions.Count; i++)
            {
                Image optionText = mathOptions[i].GetComponentInChildren<Image>();
                optionText.sprite = optionSelecting[0];
            }
        }

        private void CheckAnswer()
        {
            if (currentAnswerSelected == currentMathQuestion.correctAnswer)
            {
                moneyEarned += baseMoneyGet;
                UpdateDikiWorkingUI(true);
                UpdateMoneyUI();
            }
            else
            {
                UpdateDikiWorkingUI(false);
            }

            OptionReset();
            currentAnswerSelected = 999;

        }

        private void UpdateCurrentMathQuestion()
        {
            if (mathQuestions[currentIndex] == null) return;

            currentMathQuestion = mathQuestions[currentIndex];
            UpdateMathUI();
        }

        private void RandomQuestion()
        {
            mathQuestions.Clear();
            int totalQuestion = 8;

            for (int i = 0; i < totalQuestion; i++)
            {
                // Membuat Soal
                int num1 = Random.Range(1, 100);
                int num2 = Random.Range(1, 100);

                bool isAddition = Random.value > 0.5f;

                if (!isAddition && num1 < num2)
                {
                    (num2, num1) = (num1, num2);
                }

                string mathOperator = isAddition ? "+" : "-";
                string question = $"{num1} {mathOperator} {num2}";
                int correctAnswer = isAddition ? num1 + num2 : num1 - num2;
                // Membuat Soal

                // Membuat Opsi Jawaban
                List<int> options = new()
                {
                    correctAnswer
                };

                while (options.Count < 4)
                {
                    int fakeAnswer = correctAnswer + Random.Range(-10, 10);

                    if (fakeAnswer != 0 && fakeAnswer != correctAnswer && !options.Contains(fakeAnswer))
                    {
                        options.Add(fakeAnswer);
                    }
                }
                // Membuat Opsi Jawaban

                // Shuffle Opsi Jawaban
                for (int j = 0; j < options.Count; j++)
                {
                    int randomIndex = Random.Range(j, options.Count);
                    (options[randomIndex], options[j]) = (options[j], options[randomIndex]);
                }
                // Shuffle Opsi Jawaban

                ModelMathQuestion newMathQuestion = new(question, options, correctAnswer);
                mathQuestions.Add(newMathQuestion);
            }
        }
    }
}
