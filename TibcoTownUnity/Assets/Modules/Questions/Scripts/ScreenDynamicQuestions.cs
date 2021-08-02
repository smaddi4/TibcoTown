using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicQuestion
{
    public class ScreenDynamicQuestions : UIBehaviour
    {
        #region Declarations

        public TextAsset dataJson;

        public Text        questionText;

        [SerializeField]
        GameObject
            continueBtn,
            dropDownQuestionObj,
            mcqQuestionObj,
            imageMcqQuestionObj,
            textQuestionObj,
            phoneQuestionObj,
            timeQuestionObj,
            parentObject,
            gobackBtn;

        internal GetQuizQuestionsResponse   questionsData;
        QuestionFactory questionFactory;

        private int currentQuestion,
                    totalQuestions,
                    noOfQuestionsAnswered;

        public event Func<string, Answer> OnGetAnswer; 

        public int NoOfQuestionsAnswered { get => noOfQuestionsAnswered; set => noOfQuestionsAnswered = value; }

        private IQuestion                  currentQuestionObj;
        private List<QuestionAnswer> answers = new List<QuestionAnswer>(); 

        private Dictionary<QuestionType, IQuestion> questionObjPool =
            new Dictionary<QuestionType, IQuestion>();

        [SerializeField]
        private GameObject
            btnMaximize,
            btnMinimize;

        private bool isMinimized;

        [SerializeField]
        private Text header;

        [SerializeField] private string dataFromServer;

        private QuestionType type;

        #endregion

        #region ScreeBase Implementation

        public void OnButtonClick(string buttonName)
        {
            switch (buttonName)
            {
                case "Button-Submit":
                    OnSubmit();
                    break;
                case "Button-Minimize":
                    ToggleButtonActivity();
                    break;
                case "Button-Maximize":
                    ToggleButtonActivity();
                    break;
                case "Button-Goback":
                    ShowPreviousQuestion();
                    break;
                case "Button-Back":
                    answers.Clear();
                    answers = new List<QuestionAnswer>();
                    if (currentQuestionObj != null)
                        currentQuestionObj.Hide();
                    break;

            }
        }

        #endregion

        #region Non-Mono Methods

        public void Init(GetQuizQuestionsResponse data, string header = " ")
        {
            Reset();
            questionFactory = GetComponent<QuestionFactory>();
            questionsData = data;
            totalQuestions = questionsData.Questions.Count;
            ShowQuestion();
            this.header.text = header;
        }

        void GetDataFromJson()
        {
            questionsData = JsonUtility.FromJson<GetQuizQuestionsResponse>(dataJson.text);
        }

        /// <summary>
        /// Set value of <see cref="questionsData"/> using value received from server.
        /// </summary>
        public void SetQuestions(GetQuizQuestionsResponse data)
        {
            questionsData = data;
        }

        public void SetCurrentQuestionId(int questionNumber)
        {
            currentQuestion = questionNumber;
        }

        void CreateQuestionObj(QuestionType type)
        {
            //questionObjPool.Add(type, questionFactory.GetQuestion(type));
        }

        private void ShowQuestion()
        {
            Question question = questionsData.Questions[currentQuestion];
            questionText.text = question.QuestionDescription;
            string _type = "mcq_radio";

            type = question.Options[0].IncludesImage ? QuestionType.image : QuestionType.radio;
            
            if (questionObjPool.ContainsKey(type))
            {
                currentQuestionObj = questionObjPool[type];
            }
            else
            {
                if (type == QuestionType.image)
                {
                    currentQuestionObj = Instantiate(imageMcqQuestionObj, parentObject.transform).GetComponent<ImageMCQQuestion>();
                    ImageMCQQuestion _radio =  currentQuestionObj as ImageMCQQuestion;
                    _radio.isRadio = true;
                    _type = "mcq_image";
                }
                else
                {
                    currentQuestionObj = Instantiate(mcqQuestionObj, parentObject.transform).GetComponent<MCQQuestion>();
                    MCQQuestion _radio =  currentQuestionObj as MCQQuestion;
                    _radio.isRadio = true;
                    _type = "mcq_radio";
                }
                
                questionObjPool.Add(type, currentQuestionObj);
                currentQuestionObj = questionObjPool[type];
            }

            currentQuestionObj.Init(question,_type);
            continueBtn.transform.SetAsLastSibling();
        }

        /// <summary>
        /// Submit answer for current question
        /// </summary>
        void OnSubmit()
        {
            QuestionAnswer currentAnswer = currentQuestionObj.Submit();
            
            Debug.LogFormat("Selected answer - {0}", currentAnswer.AnswerId);
            //Answer validations
            if (currentAnswer.AnswerId == -1)
            {
                GenericPopupBehaviour.Instance.SetValues("Alert!", "Please select an option.", "OKAY");
                ShowCurrentQuestion();
                return;
            }
            
            GenericPopupBehaviour.Instance.SetValues(questionsData.Questions[currentQuestion].CorrectAnswerId == currentAnswer.AnswerId, () =>
            {
                //Check in answers dictionary already answered for current question? If yes, replace answer. No then add as a new answer.
                if (answers.Count > 0 && answers.Exists(x => x.QuestionId == currentAnswer.QuestionId))
                {
                    QuestionAnswer _temp = new QuestionAnswer();
                    _temp = answers.Find(y => y.QuestionId == currentAnswer.QuestionId);
                    answers.Remove(_temp);
                    answers.Add(currentAnswer);
                }
                else
                {
                    answers.Add(currentAnswer);
                    noOfQuestionsAnswered++;
                }

                currentQuestion++;


                if (currentQuestion < totalQuestions)
                {
                    ShowQuestion();
                }
                else
                {
                    SubmitAnsweToServer();
                }
                TogglePreviousQnoButton(currentQuestion > 0);
            });
        }

        private void SubmitAnsweToServer()
        {
            GameManagerBehaviour.Instance.ApiStateChanged(ApiState.Request);
            Debug.LogFormat("Answers : {0}", JsonConvert.SerializeObject(answers));
            SubmitQuizRequest _submitQuiz = new SubmitQuizRequest(GameManagerBehaviour.Instance.UserID, answers);
            SubmitQuiz _data = new SubmitQuiz(_submitQuiz);
        
            ApiManager.Instance.Post(_data, rest =>
            {
                GameManagerBehaviour.Instance.ApiStateChanged(ApiState.Response);
                GenericPopupBehaviour.Instance.SetValues("Quiz",
                $"Quiz answer successfully submitted to server.", "OKAY",() => GameManagerBehaviour.Instance.GameStateChanged(GameState.Game));
            }, (statusCode, resp) =>
            {
                GenericPopupBehaviour.Instance.SetValues("Quiz",
                    $"Failed to send quiz answer to server.", "OKAY",() => GameManagerBehaviour.Instance.GameStateChanged(GameState.Game));

            });
        }

        private void Reset()
        {
            currentQuestion = 0;
            TogglePreviousQnoButton(false);
        }

        /// <summary>
        ///  Enable or disable maximize button.
        /// </summary>
        private void ToggleButtonActivity()
        {
            isMinimized = !isMinimized;
            btnMaximize.SetActive(isMinimized);
        }

        /// <summary>
        /// Shows previous question.
        /// </summary>
        private void ShowPreviousQuestion()
        {
            currentQuestion--;
            currentQuestionObj.Hide();
            TogglePreviousQnoButton(currentQuestion > 0);
            ShowQuestion();
            if (OnGetAnswer != null)
            {
                currentQuestionObj.
                    SetupAnswer(OnGetAnswer.Invoke(questionsData.Questions[currentQuestion].QuestionId.ToString()));
            }
        }

        /// <summary>
        /// Show current question again.
        /// </summary>
        public void ShowCurrentQuestion()
        {
            currentQuestionObj.Hide();
            TogglePreviousQnoButton(currentQuestion > 0);
            ShowQuestion();
        }

        /// <summary>
        /// Enable or disable prevoius question button.
        /// </summary>
        /// <param name="isOn"></param>
        private void TogglePreviousQnoButton(bool isOn)
        {
            gobackBtn.SetActive(false);
        }

        /// <summary>
        /// Set final question.
        /// </summary>
        public void SetFinalQuestions()
        {
            Init(questionsData, "Quiz");
        }

        #endregion

        public override void GameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Quiz:
                    transform.GetChild(0).gameObject.SetActive(true);
                    GetQuestionFromServer();
                    break;
                default:
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
        }

        private void GetQuestionFromServer()
        {
            GameManagerBehaviour.Instance.ApiStateChanged(ApiState.Request);
            GetQuizQuestions quizQuestions = new GetQuizQuestions(new GetQuizQuestionsRequest(13));
            ApiManager.Instance.Post(quizQuestions, OnSuccess, OnFailue);
        }

        private void OnFailue(int statusCode, string resp)
        {
            GameManagerBehaviour.Instance.ApiStateChanged(ApiState.Response);
        }

        private void OnSuccess(string resp)
        {
            dataFromServer = resp;
            QuizResponse _response = JsonConvert.DeserializeObject<QuizResponse>(resp);
            Init(_response.GetQuizQuestionsResponse, "Quiz");
            ShowQuestion();
            GameManagerBehaviour.Instance.ApiStateChanged(ApiState.Response);
        }
    }

    public enum QuestionType
    {
        text,
        mcq,
        dropwdown,
        tel,
        number,
        radio,
        time,
        image
    }

    public class QuestionsData
    {
        public List<Question> questions;
    }
}
