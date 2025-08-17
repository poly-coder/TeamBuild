#pragma warning disable S1694
namespace TeamBuild.Core.Blazor;

public abstract class FormModelBase<TFormModel>
    where TFormModel : FormModelBase<TFormModel>, new()
{
    public abstract bool IsEqualTo(TFormModel? other);
    public abstract TFormModel Clone();
}
