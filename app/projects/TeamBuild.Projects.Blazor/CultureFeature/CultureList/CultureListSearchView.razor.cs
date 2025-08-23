using TeamBuild.Core.Blazor;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureList;

public sealed partial class CultureListSearchView
{
    public class FormModel : FormModelBase<FormModel>
    {
        public string Search { get; set; } = "";

        public override FormModel Clone()
        {
            return new FormModel { Search = Search };
        }

        public override bool IsEqualTo(FormModel? newOne)
        {
            if (newOne is null)
                return false;

            return Search == newOne.Search;
        }
    }
}
