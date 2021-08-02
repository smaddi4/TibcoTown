using System.Collections.Generic;

namespace DynamicQuestion
{
	public interface IQuestion
	{
		/// <summary>
		/// Dynamic question get from server.
		/// </summary>
		Question Question { get; set; }

		/// <summary>
		/// Initialize questions. 
		/// </summary>
		/// <param name="question"></param>
		void Init(Question question, string type);
		
		/// <summary>
		/// Return response json type & disable question.
		/// </summary>
		/// <returns></returns>
		QuestionAnswer Submit();

		/// <summary>
		/// Temparory disable current question.
		/// </summary>
		void Hide();

		/// <summary>
		/// Setup answer manually for when loading from checkpoint.
		/// </summary>
		void SetupAnswer(Answer answer);
	}

	public class Answer
	{
		public object value;
		public Answer () {}

		public Answer(object questionQuestion,  string questionCorrectValue, string options)
		{
		}
	}
}
