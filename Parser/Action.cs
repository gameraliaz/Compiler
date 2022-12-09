namespace Parser
{
    public class Action
    {
        public TypeOfAction Act { get; set; }
        public int State { get; set; }
        public Action()
        {
            Act = TypeOfAction.error;
            State = -1;
        }
        public override string ToString()
        {
            return ((int)Act).ToString();
        }
    }

}
