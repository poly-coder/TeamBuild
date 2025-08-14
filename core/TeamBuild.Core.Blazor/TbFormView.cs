using Microsoft.AspNetCore.Components;

namespace TeamBuild.Core.Blazor;

public abstract class TbFormView<TFormModel> : ComponentBase
    where TFormModel : FormModelBase<TFormModel>, new()
{
    [Parameter]
    public EventCallback<TFormModel> OnSubmit { get; set; }

    [Parameter]
    public bool IsSubmitting { get; set; }

    [Parameter]
    public TFormModel? InitialForm { get; set; }

    protected TFormModel Form { get; set; } = new();
    private TFormModel? previousInitialForm;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (previousInitialForm is null || !previousInitialForm.IsEqualTo(InitialForm))
        {
            previousInitialForm = InitialForm?.Clone() ?? new TFormModel();
            Form = InitialForm?.Clone() ?? new TFormModel();
        }
    }

    protected async Task HandleSubmit()
    {
        if (IsSubmitting)
            return;

        await OnSubmit.InvokeAsync(Form);
    }
}
