using System.Reflection;
using PhantomKit.Helpers;
using PhantomKit.Models.Commands;
using Terminal.Gui;

namespace PhantomKit.Models.Menus;

public class PhantomKitMainMenu : MenuBar
{
    protected readonly HostedGuiCommandBase GuiCommand;

    public PhantomKitMainMenu(HostedGuiCommandBase guiCommand) : base(menus: BuildMenus(guiCommand))
    {
        this.GuiCommand = guiCommand;
    }

    private static MenuItemDetails[] BuildMenuItems()
    {
        MenuItemDetails[] menuItems =
        {
            new(title: "F_ind",
                help: "",
                action: null!),
            new(title: "_Replace",
                help: "",
                action: null!),
            new(title: "_Item1",
                help: "",
                action: null!),
            new(title: "_Also From Sub Menu",
                help: "",
                action: null!),
        };

        menuItems[0].Action = () => ShowMenuItem(mi: menuItems[0]);
        menuItems[1].Action = () => ShowMenuItem(mi: menuItems[1]);
        menuItems[2].Action = () => ShowMenuItem(mi: menuItems[2]);
        menuItems[3].Action = () => ShowMenuItem(mi: menuItems[3]);
        return menuItems;
    }

    private static MenuBarItem[] BuildMenus(HostedGuiCommandBase guiCommand)
    {
        var menuItems = BuildMenuItems();
        return new MenuBarItem[]
        {
            new MenuBarItem("_File",
                new MenuItem[]
                {
                    new MenuItem("_Quit",
                        "",
                        GuiUtilities.QuitClick,
                        null,
                        null,
                        Key.AltMask | Key.Q),
                }),
            new MenuBarItem("_Edit",
                new MenuItem[]
                {
                    new MenuItem("_Copy",
                        "",
                        guiCommand.Copy,
                        null,
                        null,
                        Key.C | Key.CtrlMask),
                    new MenuItem("C_ut",
                        "",
                        guiCommand.Cut,
                        null,
                        null,
                        Key.X | Key.CtrlMask),
                    new MenuItem("_Paste",
                        "",
                        guiCommand.Paste,
                        null,
                        null,
                        Key.Y | Key.CtrlMask),
                    new MenuBarItem(title: "_Find and Replace",
                        children: new MenuItem[] {menuItems[0], menuItems[1]}),
                    menuItems[3],
                }),
        };
    }

    public static void ShowMenuItem(MenuItem mi)
    {
        var flags = BindingFlags.Public | BindingFlags.Static;
        var minfo = typeof(MenuItemDetails).GetMethod(name: "Instance",
            bindingAttr: flags);
        var mid = (PhantomMail.PhantomKit.MenuItemDelegate) Delegate.CreateDelegate(
            type: typeof(PhantomMail.PhantomKit.MenuItemDelegate),
            method: minfo ?? throw new InvalidOperationException());
        MessageBox.Query(width: 70,
            height: 7,
            title: mi.Title.ToString(),
            message: $"{mi.Title} selected. Is from submenu: {mi.GetMenuBarItem()}",
            "Ok");
    }
}