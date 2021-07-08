using System.Collections.Generic;

namespace QuizFramework.Database
{
    public struct Question
    {
        public ushort QuestionsGroup;
        public string QuestionStr;
        public List<string> Answers;
        public byte IndexOfCorrectAnswer;
    }
}