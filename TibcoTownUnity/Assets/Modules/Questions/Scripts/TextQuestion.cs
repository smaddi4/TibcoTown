using System;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicQuestion
{

    public class TextQuestion : MonoBehaviour, IQuestion
    {
        [SerializeField]
        protected Text answerTxt, placeHolderTxt;

        [SerializeField]
        protected InputField anwerInputField;

        QuestionAnswer answer;

        public Question Question { get; set; }

        public virtual void Init(Question question, string type)
        {
            Question = question;
           
                anwerInputField.contentType = InputField.ContentType.Standard;

            gameObject.SetActive(true);
            answerTxt.text                 = String.Empty;
            anwerInputField.text           = string.Empty;

            if(answer != null)
            {
                answer = null;
            }
            answer              = new QuestionAnswer();
        }

        public QuestionAnswer Submit()
        {
            answer = new QuestionAnswer(Question.QuestionId, -1);
            Hide();
            return answer;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetupAnswer(Answer ans)
        {
            anwerInputField.text = ans.value.ToString();
        }
    } 
}
