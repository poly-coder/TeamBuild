using MudBlazor;

namespace TeamBuild.Core.MudBlazor;

public class TbMudGoToAddButton : MudButton
{
    public TbMudGoToAddButton()
    {
        Color = Color.Primary;
        StartIcon = TbMudIcons.Add;
        Variant = Variant.Filled;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Add");
        };
    }
}

public class TbMudGoToDetailsButton : MudButton
{
    public TbMudGoToDetailsButton()
    {
        Color = Color.Default;
        StartIcon = TbMudIcons.View;
        Variant = Variant.Text;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Details");
        };
    }
}

public class TbMudGoToDeleteButton : MudButton
{
    public TbMudGoToDeleteButton()
    {
        Color = Color.Error;
        StartIcon = TbMudIcons.Delete;
        Variant = Variant.Text;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Delete");
        };
    }
}

public class TbMudGoToEditButton : MudButton
{
    public TbMudGoToEditButton()
    {
        Color = Color.Default;
        StartIcon = TbMudIcons.Edit;
        Variant = Variant.Text;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Edit");
        };
    }
}

public class TbMudCancelButton : MudButton
{
    public TbMudCancelButton()
    {
        Color = Color.Default;
        StartIcon = TbMudIcons.Cancel;
        Variant = Variant.Text;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Cancel");
        };
    }
}

public class TbMudReloadButton : MudButton
{
    public TbMudReloadButton()
    {
        Color = Color.Primary;
        StartIcon = TbMudIcons.Reload;
        Variant = Variant.Outlined;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Reload");
        };
    }
}

public class TbMudResetButton : MudButton
{
    public TbMudResetButton()
    {
        Color = Color.Default;
        StartIcon = TbMudIcons.Reset;
        Variant = Variant.Text;
        ButtonType = ButtonType.Reset;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Reset");
        };
    }
}

public class TbMudSaveButton : MudButton
{
    public TbMudSaveButton()
    {
        Color = Color.Primary;
        StartIcon = TbMudIcons.Save;
        Variant = Variant.Filled;
        ButtonType = ButtonType.Submit;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Save");
        };
    }
}

public class TbMudDeleteButton : MudButton
{
    public TbMudDeleteButton()
    {
        Color = Color.Error;
        StartIcon = TbMudIcons.Delete;
        Variant = Variant.Filled;
        ButtonType = ButtonType.Submit;
        ChildContent = tree =>
        {
            tree.AddContent(0, "Delete");
        };
    }
}
