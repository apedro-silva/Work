using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMIZEE.Models;
using SMIZEE.CodeFirstMembership;

namespace SMIZEE.UserControls
{
    public partial class AplMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = WebSecurity.GetUserInfo(Page.User.Identity.Name);
            try
            {
                CodeFirstMembership.CodeFirstRoleProvider x = new CodeFirstMembership.CodeFirstRoleProvider();
                string[] myRoles = x.GetRolesForUser(Page.User.Identity.Name);

                switch (myRoles[0])
                {
                    case "Admin": SetAdminMenu(user); break;
                    case "User": SetUserMenu(user); break;
                }

            }
            catch (Exception)
            {
                SetUserMenu(user);
            }
            
        }

        private void SetUserMenu(User user)
        {
            NavigationMenu.Items.Clear();
            // Create First Level Menu
            MenuItem controlPanelMenuItem = createMenuItem(Resources.MainMenu.lControlPanel, null, null, "~/Forms/ControlPanel.aspx", null);
            MenuItem formsMenuItem = createMenuItem(Resources.MainMenu.IM_3_00_01, null, null, null, null);

            NavigationMenu.Items.Add(controlPanelMenuItem);
            addFormsMenu(user, formsMenuItem);
        }

        private void SetAdminMenu(User user)
        {
            NavigationMenu.Items.Clear();
            // Create First Level Menu
            MenuItem controlPanelMenuItem = createMenuItem(Resources.MainMenu.lControlPanel, null, null, "~/Forms/ControlPanel.aspx", null);
            MenuItem backOfficeMenuItem = createMenuItem(Resources.MainMenu.lBackOffice, null, null, null, null);
            MenuItem formsMenuItem = createMenuItem(Resources.MainMenu.IM_3_00_01, null, null, null, null);
            MenuItem managementMenuItem = createMenuItem(Resources.MainMenu.IM_4_00_01, null, null, null, null);

            if (user.FunctionalAreaID!=null && user.FunctionalAreaID!=0)
                NavigationMenu.Items.Add(controlPanelMenuItem);

            addBackOfficeMenu(backOfficeMenuItem);
            addFormsMenu(user, formsMenuItem);
            addManagementMenu(managementMenuItem);
        }

        private void addBackOfficeMenu(MenuItem backOfficeMenuItem)
        {
            // BackOffice Menu
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_01, null, null, "~/BackOffice/SMI.02.01.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_02, null, null, "~/BackOffice/SMI.02.02.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_03, null, null, "~/BackOffice/SMI.02.03.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_04, null, null, "~/BackOffice/SMI.02.04.aspx", null);
            //addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_05, null, null, "~/BackOffice/SMI.02.05.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_06, null, null, "~/BackOffice/SMI.02.06.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_07, null, null, "~/BackOffice/SMI.02.07.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_08, null, null, "~/BackOffice/SMI.02.08.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_09, null, null, "~/BackOffice/SMI.02.09.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_10, null, null, "~/BackOffice/SMI.02.10.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_11, null, null, "~/BackOffice/SMI.02.11.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_12, null, null, "~/BackOffice/SMI.02.12.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_13, null, null, "~/BackOffice/SMI.02.13.aspx", null);
            addMenuItem(backOfficeMenuItem, Resources.MainMenu.IM_02_14, null, null, "~/BackOffice/SMI.02.14.aspx", null);
            NavigationMenu.Items.Add(backOfficeMenuItem);
        }

        private void addManagementMenu(MenuItem managementMenuItem)
        {
            // Management Menu
            addMenuItem(managementMenuItem, Resources.MainMenu.IM_04_01, null, null, "~/Management/SMI.04.01.aspx", null);
            //addMenuItem(managementMenuItem, Resources.MainMenu.IM_04_02, null, null, "~/Management/SMI.04.02.aspx", null);
            addMenuItem(managementMenuItem, Resources.MainMenu.IM_04_03, null, null, "~/Management/SMI.04.03.aspx", null);

            NavigationMenu.Items.Add(managementMenuItem);
        }
        private void addFormsMenu(User user, MenuItem formsMenuItem)
        {
            //user with company does not sees ZEE forms
            int? companyId = (user.CompanyID == null ? 0 : user.CompanyID);


            // Forms Menu
            addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_01, null, null, "~/Forms/SMI.03.01.aspx", null);
            addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_02, null, null, "~/Forms/SMI.03.02.aspx", null);

            if (companyId == 0)
            {
                addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_03, null, null, "~/Forms/SMI.03.03.aspx", null);
                addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_04, null, null, "~/Forms/SMI.03.04.aspx", null);
            }
            addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_05, null, null, "~/Forms/SMI.03.05.aspx", null);
            addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_06, null, null, "~/Forms/SMI.03.06.aspx", null);

            if (companyId == 0)
            {
                addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_07, null, null, "~/Forms/SMI.03.07.aspx", null);
                addMenuItem(formsMenuItem, Resources.MainMenu.IM_03_08, null, null, "~/Forms/SMI.03.08.aspx", null);
            }
            NavigationMenu.Items.Add(formsMenuItem);
        }

        private MenuItem createMenuItem(string text, string value, string imageUrl, string navigateUrl, string target)
        {
            MenuItem menuItem = null;

            if (navigateUrl != null)
                menuItem = new MenuItem(text, value, imageUrl, navigateUrl, target);
            else
            {
                menuItem = new MenuItem(text);
                menuItem.Selectable = false;
            }
            return menuItem;
        }
        private void addMenuItem(MenuItem menu, string text, string value, string imageUrl, string navigateUrl, string target)
        {
            MenuItem menuItem = new MenuItem(text, value, imageUrl, navigateUrl, target);
            menu.ChildItems.Add(menuItem);
        }

    }
}