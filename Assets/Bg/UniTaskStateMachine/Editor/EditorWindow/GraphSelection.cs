namespace Bg.UniTaskStateMachine.Editor
{
    public class GraphSelection
    {
        private readonly Context context;

        public GraphSelection(Context context)
        {
            this.context = context;
        }

        public void Delete()
        {
            ICommand command = new DeleteSelectionCommand(context.StateMachine, context.SelectedNodes);
            command.Execute();
        }
    }
}