using System.ComponentModel.DataAnnotations;
using TeamBuild.Core.Blazor;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureNew;

public sealed partial class CultureNewFormView
{
    public class FormModel : FormModelBase<FormModel>
    {
        [Required]
        public string CultureCode { get; set; } = "";

        [Required]
        public string EnglishName { get; set; } = "";

        [Required]
        public string NativeName { get; set; } = "";

        public bool GoToDetails { get; set; }

        public override FormModel Clone()
        {
            return new FormModel
            {
                CultureCode = CultureCode,
                EnglishName = EnglishName,
                NativeName = NativeName,
                GoToDetails = GoToDetails,
            };
        }

        public override bool IsEqualTo(FormModel? newOne)
        {
            if (newOne is null)
                return false;

            return CultureCode == newOne.CultureCode
                && EnglishName == newOne.EnglishName
                && NativeName == newOne.NativeName
                && GoToDetails == newOne.GoToDetails;
        }
    }
}
