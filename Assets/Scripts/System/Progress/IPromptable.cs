public interface IPromptable
{
    string PromptText { get; }
    bool CanShowPrompt { get; }
}