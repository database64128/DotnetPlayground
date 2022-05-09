using System.CommandLine.Binding;

namespace SystemCommandLineUtils;

public class CancellationTokenBinder : BinderBase<CancellationToken>
{
    protected override CancellationToken GetBoundValue(BindingContext bindingContext) =>
        (CancellationToken)bindingContext.GetService(typeof(CancellationToken))!;
}
