<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AplMenu.ascx.cs" Inherits="SMIZEE.UserControls.AplMenu" %>

    <asp:Menu ID="NavigationMenu" runat="server" CssClass="menuApl" EnableViewState="false" IncludeStyleBlock="false" Orientation="Horizontal">
        <Items>
            <asp:MenuItem NavigateUrl="~/Forms/ControlPanel.aspx" Text="<%$ Resources:MainMenu, lControlPanel %>"/>
            <asp:MenuItem Text="<%$  Resources:MainMenu, lBackOffice %>" NavigateUrl="~/Account/BackOffice.aspx">
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.01.aspx" Text="<%$ Resources:MainMenu, IM_02_01 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.02.aspx" Text="<%$ Resources:MainMenu, IM_02_02 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.03.aspx" Text="<%$ Resources:MainMenu, IM_02_03 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.04.aspx" Text="<%$ Resources:MainMenu, IM_02_04 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.05.aspx" Text="<%$ Resources:MainMenu, IM_02_05 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.06.aspx" Text="<%$ Resources:MainMenu, IM_02_06 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.07.aspx" Text="<%$ Resources:MainMenu, IM_02_07 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.08.aspx" Text="<%$ Resources:MainMenu, IM_02_08 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.09.aspx" Text="<%$ Resources:MainMenu, IM_02_09 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.10.aspx" Text="<%$ Resources:MainMenu, IM_02_10 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.11.aspx" Text="<%$ Resources:MainMenu, IM_02_11 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.12.aspx" Text="<%$ Resources:MainMenu, IM_02_12 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.13.aspx" Text="<%$ Resources:MainMenu, IM_02_13 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Account/SMI.02.14.aspx" Text="<%$ Resources:MainMenu, IM_02_14 %>"></asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem NavigateUrl="~/Forms/ControlPanel.aspx" Text="<%$ Resources:MainMenu, IM_3_00_01 %>">
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.01.aspx" Text="<%$ Resources:MainMenu, IM_03_01 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.02.aspx" Text="<%$ Resources:MainMenu, IM_03_02 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.03.aspx" Text="<%$ Resources:MainMenu, IM_03_03 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.04.aspx" Text="<%$ Resources:MainMenu, IM_03_04 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.05.aspx" Text="<%$ Resources:MainMenu, IM_03_05 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.06.aspx" Text="<%$ Resources:MainMenu, IM_03_06 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.07.aspx" Text="<%$ Resources:MainMenu, IM_03_07 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Forms/SMI.03.08.aspx" Text="<%$ Resources:MainMenu, IM_03_08 %>"></asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem NavigateUrl="~/Management/SMI.04.01.aspx" Text="<%$ Resources:MainMenu, IM_4_00_01 %>">
                <asp:MenuItem NavigateUrl="~/Management/SMI.04.01.aspx" Text="<%$ Resources:MainMenu, IM_04_01 %>"></asp:MenuItem>
                <asp:MenuItem NavigateUrl="~/Management/SMI.04.02.aspx" Text="<%$ Resources:MainMenu, IM_04_02 %>"></asp:MenuItem>
            </asp:MenuItem>

        </Items>
    </asp:Menu>

