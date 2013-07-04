<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SMIZEE.Account.Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <section id="loginForm">
        <h2><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, lAuthenticationMessage%>" /></h2>
        <asp:Login runat="server" ViewStateMode="Disabled" RenderOuterTable="false" FailureText="<%$ Resources:Resource, lLoginFailureText%>">
            <LayoutTemplate>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
                    <legend>Log in Form</legend>
                    <ol>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="UserName" Text="<%$ Resources:Resource, lUsername %>"></asp:Label>
                            <asp:TextBox runat="server" ID="UserName" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="<%$ Resources:Resource, lUserNameRequired %>" />
                        </li>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="Password" Text="<%$ Resources:Resource, lPassword %>"></asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="<%$ Resources:Resource, lPasswordRequired %>" />
                        </li>
                        <li>
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox" Text="<%$ Resources:Resource, lRememberAuthentication %>"></asp:Label>
                        </li>
                    </ol>
                    <asp:Button runat="server" CommandName="Login" Text="<%$ Resources:Resource, bLoginConfirm %>" />
                    <a href="PasswordRecovery.aspx"><asp:Literal runat="server" Text="<%$ Resources:Resource, mUserNameTitleText %>"></asp:Literal></a>
                </fieldset>
            </LayoutTemplate>
        </asp:Login>
    </section>

</asp:Content>
