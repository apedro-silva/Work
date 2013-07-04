<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="SMIZEE.Account.PasswordRecovery" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:PasswordRecovery CssClass="PasswordRecovery" id="PasswordRecovery1" runat="server" BorderStyle="Solid" BorderWidth="1px" BackColor="#F7F7DE"
                Font-Size="10pt" Font-Names="Verdana" BorderColor="#CCCC99" HelpPageTextx="Ajuda?" 
                HelpPageUrl="recoveryHelp.aspx" SubmitButtonText="<%$ Resources:Resource, bSubmit %>"
                UserNameInstructionText="<%$ Resources:Resource, mUserNameInstructionText%>"
                UserNameTitleText="<%$ Resources:Resource, mUserNameTitleText %>"
                UserNameLabelText="<%$ Resources:Resource, lGetPwdUserName %>">
                <successtemplate>
                    <table border="0" style="font-size:10pt;">
                        <tr>
                            <td><asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, mNewPwdSent %>"/></td>
                        </tr>
                    </table>
                </successtemplate>
                <titletextstyle font-bold="True" forecolor="White" backcolor="#6B696B">
                </titletextstyle>
    </asp:PasswordRecovery>

</asp:Content>
