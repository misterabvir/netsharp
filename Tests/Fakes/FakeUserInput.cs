using Core.UIWrappers;

namespace Tests.Fakes;

internal class FakeUserInput : IUserInput
{
    public string? Expected { get; set; }

    public bool SendFlag { get; set; } = false;


    public async Task<string?> ReadLineAsync()
    {
        while(!SendFlag)
        {
            await Task.Delay(1);
        }
        SendFlag = false;
        return Expected;
    }
}
