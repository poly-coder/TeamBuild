using Microsoft.AspNetCore.Components;

namespace TeamBuild.Core.Blazor;

public abstract class TbCustomFormView<TFormModel> : TbCustomView
    where TFormModel : TbCustomFormView<TFormModel>.FormModelBase, new()
{
    [Parameter]
    public Func<TFormModel, Task>? OnSave { get; set; }

    [Parameter]
    public TFormModel? InitialForm { get; set; }

    protected TFormModel? Form { get; set; }
    private TFormModel? previousInitialForm;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (
            Form is null
            || previousInitialForm is null
            || !previousInitialForm.IsEqualTo(InitialForm)
        )
        {
            previousInitialForm = InitialForm?.Clone() ?? new TFormModel();
            Form = InitialForm?.Clone() ?? new TFormModel();
        }
    }

    protected bool IsSaving { get; set; }
    protected Exception? Exception { get; set; }

    protected async Task HandleValidSubmit()
    {
        if (Form is null || IsSaving || OnSave is null)
            return;

        IsSaving = true;
        Exception = null;

        StateHasChanged();

        try
        {
            await OnSave(Form);
        }
        catch (Exception ex)
        {
            Exception = ex;
        }
        finally
        {
            IsSaving = false;
        }
    }

    public abstract class FormModelBase
    {
        public abstract bool IsEqualTo(TFormModel? other);
        public abstract TFormModel Clone();
    }
}
