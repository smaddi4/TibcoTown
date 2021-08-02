using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicQuestion
{
    public class ImageMCQQuestion : MonoBehaviour,IQuestion
    {
        [SerializeField]
        GameObject radioOption, answersParent;

        [SerializeField] 
        private ToggleGroup toggleGroup;
        
        List<Toggle>     toggles             = new List<Toggle>();
        List<GameObject> instantiatedObjects = new List<GameObject>();
        Option           selectedAnswers;
        List<Option>     options = new List<Option>();
        QuestionAnswer   answer;
        public bool      isRadio = false;

        public Question Question { get; set; }

        public void Init(Question question, string type)
        {
            Reset();
            Question = question;
            gameObject.SetActive(true);
            selectedAnswers = new Option();
            options.Clear();
            options = new List<Option>(question.Options);
            
            if(isRadio)
            {
                radioOption.SetActive(false);
                radioOption.GetComponentInChildren<Text>().text = question.Options[0].OptionDescription;
                toggleGroup.allowSwitchOff = true;
                for (int i = 0; i < question.Options.Count; i++)
                {
                    GameObject toggleObj = Instantiate(radioOption, answersParent.transform);
                    toggleObj.SetActive(true);
                    instantiatedObjects.Add(toggleObj);
                    toggleObj.GetComponentInChildren<Text>().text = question.Options[i].OptionDescription;
                    toggles.Add(toggleObj.GetComponentInChildren<Toggle>());
                    toggleObj.GetComponentInChildren<Toggle>().isOn = false;

                    StartCoroutine(SetupImage(question.Options[i].OptionDescription, sprite =>
                    {
                        toggleObj.GetComponentInChildren<Image>().sprite = sprite;
                    }));
                }
                toggleGroup.allowSwitchOff = true;
            }
            answer = new QuestionAnswer(Question.QuestionId, -1);
        }

        IEnumerator SetupImage(string id, Action<Sprite> sprite)
        {
            var _sprite = GameManagerBehaviour.Instance.getSprite(id);
            yield return new WaitForEndOfFrame();
            yield return null;
            sprite?.Invoke(_sprite);
        }

        public void SelectRadioButton()
        {
            /*foreach (Toggle toggle in toggles)
            {
                toggle.isOn = false;
            }*/
            if (toggleGroup.allowSwitchOff)
            {
                toggleGroup.allowSwitchOff = false;
            }
            //EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>().isOn = true;
        }

        void Reset()
        {
            foreach(Toggle toggle in toggles)
            {
                Destroy(toggle);
            }
            
            foreach(GameObject _go in instantiatedObjects)
            {
                Destroy(_go);
            }
            
            toggles.Clear();
            radioOption.GetComponentInChildren<Text>().text = "";
        }

        void CheckAnswer()
        {
            foreach (Toggle toggle in toggles)
            {
                if(toggle.isOn)
                {
                    selectedAnswers = (Question.Options.Find(x => x.OptionDescription.Equals(toggle.transform.parent.GetComponentInChildren<Text>().text))); 
                }
            }
        }

        public QuestionAnswer Submit()
        {
            CheckAnswer();
            if (selectedAnswers == null)
            {
                answer = new QuestionAnswer(Question.QuestionId, -1);
            }
            else
            {
                answer = new QuestionAnswer(Question.QuestionId, selectedAnswers.OptionId);
            }
            
            Debug.LogFormat("Answer : {0}", answer);
            Hide();
            return answer;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetupAnswer(Answer answer)
        {
            
        }

        public void SetupAnswer(QuestionAnswer ans)
        {
            if (ans.QuestionId == -1)
            {
                answer.QuestionId = -1;
                return;
            }

            foreach (Toggle _toggle in toggles)
            {
                if (_toggle.transform.parent.GetComponentInChildren<Text>().text.Equals(ans.QuestionId))
                {
                    _toggle.isOn = true;
                }
            }
        }
    } 
}
