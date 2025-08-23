using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using TeamBuild.Core.Blazor;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureEdit;

public sealed partial class CultureEditView
{
    [Parameter]
    public Exception? SubmitException { get; set; }

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
