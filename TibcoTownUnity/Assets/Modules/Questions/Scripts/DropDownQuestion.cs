using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicQuestion
{
    public class DropDownQuestion : MonoBehaviour, IQuestion
    {
        [SerializeField]
        Dropdown dropDown;

        QuestionAnswer               answer;
        private List<Option> options  = new List<Option>();

        public Question Question { get; set; }

        public void Init(Question question, string type)
        {
            Question = question;
            gameObject.SetActive(true);
            dropDown.options.Clear();
            options.Clear();
            options = new List<Option>(question.Options);
            dropDown.options.Add(new Dropdown.OptionData("Select Option"));
            if (question.Options != null || question.Options.Count > 0)
            {
                foreach (Option option in question.Options)
                {
                    dropDown.options.Add(new Dropdown.OptionData() { text = option.OptionDescription });
                }
                dropDown.captionText.text = dropDown.options[0].text;
            }

            // Reset
            dropDown.value = 0;

            if (answer != null)
            {
                answer = null;
            }
            answer              = new QuestionAnswer();
        }

        public QuestionAnswer Submit()
        {
            if (dropDown.value == 0)
            {
                answer = new QuestionAnswer(Question.QuestionId, -1);
            }
            else
            {
                answer = new QuestionAnswer(Question.QuestionId, options[dropDown.value - 1].OptionId);
            }
            Hide();
            return answer;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetupAnswer(Answer ans)
        {
            dropDown.value = dropDown.options.FindIndex(i => i.text.Equals(ans.value));
        }
    }
}

