<%@ Page Title="<%$ Resources:Resource, lRegisterOption %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SMIZEE.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><asp:literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, lRegisterOption %>"/></h1>
    </hgroup>

    <asp:CreateUserWizard runat="server" ID="RegisterUser" ViewStateMode="Disabled" OnCreatedUser="RegisterUser_CreatedUser">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="wizardStepPlaceholder" />
            <asp:PlaceHolder runat="server" ID="navigationPlaceholder" />
        </LayoutTemplate>
        <WizardSteps>
            <asp:CreateUserWizardStep runat="server" ID="RegisterUserWizardStep">
                <ContentTemplate>
                    <p class="message-info">
                        <asp:literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, lPasswordCriteria %>"/> <%: Membership.MinRequiredPasswordLength %> <asp:literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, lPasswordCriteriaEnd %>"/>
                    </p>

                    <p class="validation-summary-errors">
                        <asp:Literal runat="server" ID="ErrorMessage" />
                    </p>

                    <fieldset>
                        <legend><asp:literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, lAccountInformation %>"/></legend>
                        <ol>
                            <li>
                                <asp:Label runat="server" AssociatedControlID="UserName" Text="<%$ Resources:Resource, lUserName %>"></asp:Label>
                                <asp:TextBox runat="server" ID="UserName" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                    CssClass="field-validation-error" ErrorMessage="The user name field is required." />
                            </li>
                            <li>
                                <asp:Label runat="server" AssociatedControlID="Email" Text="<%$ Resources:Resource, lEmail %>"></asp:Label>
                                <asp:TextBox runat="server" ID="Email" TextMode="Email" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                    CssClass="field-validation-error" ErrorMessage="The email address field is required." />
                            </li>
                            <li>
                                <asp:Label runat="server" AssociatedControlID="Password" Text="<%$ Resources:Resource, lPassword %>"></asp:Label>
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                    CssClass="field-validation-error" ErrorMessage="The password field is required." />
                            </li>
                            <li>
                                <asp:Label runat="server" AssociatedControlID="ConfirmPassword" Text="<%$ Resources:Resource, lPasswordConfirm %>"></asp:Label>
                                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                     CssClass="field-validation-error" Display="Dynamic" ErrorMessage="<%$ Resources:Resource, mConfirmPasswordIsRequired %>" />
                                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                     CssClass="field-validation-error" Display="Dynamic" ErrorMessage="<%$ Resources:Resource, mPasswordsDoNotMatch %>" />
                            </li>
                        </ol>
                        <asp:Button runat="server" CommandName="MoveNext" Text="<%$ Resources:Resource, bRegister %>" />
                    </fieldset>
                </ContentTemplate>
                <CustomNavigationTemplate />
            </asp:CreateUserWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
</asp:Content>