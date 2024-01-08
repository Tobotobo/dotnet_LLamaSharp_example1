// https://github.com/SciSharp/LLamaSharp/blob/master/docs/ChatSession/basic-usages.md

using LLama.Common;
using LLama;

// https://huggingface.co/mmnga/ELYZA-japanese-Llama-2-7b-instruct-gguf/blob/main/ELYZA-japanese-Llama-2-7b-instruct-q5_0.gguf
string modelPath = @"C:/models/ELYZA-japanese-Llama-2-7b-instruct-q5_0.gguf";
var parameters = new ModelParams(modelPath)
{
    ContextSize = 1024
};
using var model = LLamaWeights.LoadFromFile(parameters);
using var context = model.CreateContext(parameters);
var ex = new InteractiveExecutor(context);
var session = new ChatSession(ex);

// プロンプト
var message = new ChatHistory.Message(
    AuthorRole.User,
    "[INST] <<SYS>>あなたは、役立つアシスタントです。<</SYS>C#言語の特徴を3つ箇条書きで、一つ20文字程度で回答してください。[/INST]"
);

// プロンプトを表示
Console.WriteLine(message.Content);

// プロンプトに対する応答を表示　※一文字一文字超ゆっくり表示される
await foreach (var text in session.ChatAsync(
        message,
        new InferenceParams()
        {
            Temperature = 0.6f,
            AntiPrompts = new List<string> { "User:" }
        }
    ))
{
    Console.Write(text);
}