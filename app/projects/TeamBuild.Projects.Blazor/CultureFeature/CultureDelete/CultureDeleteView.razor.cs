using Microsoft.AspNetCore.Components;
using TeamBuild.Core.Blazor;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureDelete;

public sealed partial class CultureDeleteView
{
    [Parameter]
    public Domain.CultureFeature.CultureDetails? Culture { get; set; }

    [Parameter]
    public Exception? SubmitException { get; set; }

    private IEnumerable<FieldValue> FieldValues()
    {
        yield return new FieldValue("Code", Culture?.CultureCode);
        yield return new FieldValue("English", Culture?.EnglishName);
        yield return new FieldValue("Native", Culture?.NativeName);
    }

    public class FormModel : FormModelBase<FormModel>
    {
        public override FormModel Clone()
        {
            return new FormModel();
        }

        public override bool IsEqualTo(FormModel? newOne)
        {
            if (newOne is null)
                return false;

            return true;
        }
    }
}
