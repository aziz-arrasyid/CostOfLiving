using System.Collections.Generic;
using UnityEngine;
using Game.System;

namespace Work.Event
{
    public class WorkEventManager : MonoBehaviour
    {
        [Header("Math Question")]
        [SerializeField] private List<ModelMathQuestion> mathQuestions;

        private void Start()
        {
            RandomQuestion();
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

        private void TimerCountdown()
        {
            // Membuat durasi waktu kuis selama 15 detik saja
        }
    }
}
