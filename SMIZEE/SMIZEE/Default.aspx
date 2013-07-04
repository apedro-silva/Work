<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SMIZEE._Default" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
	<div>
		<div id="LeftPane">
			<div class="banner"><img id="Img1" src="~/Images/banner1.jpg" runat="server"></div>
			<div class="content">
				<h2>Sobre <br><span>o Sistema</span></h2>
				<p>A plataforma de recolha de informação, para o Sistema de Monitorização de Indicadores da ZEE, visa a recolha e posterior validação dos dados obtidos.</p>
				<p>O modo de funcionamento preconizado consiste na introdução dos dados identificados através de vários formulários específicos.</p>
				<p>Após o preenchimento desses formulários a informação neles contida será gravada automaticamente na base de dados e sujeita a uma validação por parte da área responsável na SDZEE.  Esta validação irá confirmar a fiabilidade dos dados apresentados.</p>
				<p>Em caso de inconsistência de informação, será solicitada a correção da mesma no respectivo formulário.</p>
			</div>
		</div>
		<div id="RightPane">
			<div class="form">
                <section id="loginForm">
                    <h2><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, lAuthenticationMessage%>" /></h2>
                    <asp:Login ID="Login1" runat="server" ViewStateMode="Disabled" RenderOuterTable="false"  FailureText="<%$ Resources:Resource, lLoginFailureText%>">
                        <LayoutTemplate>
                            <fieldset>
                                <legend>Log in Form</legend>
                                <ol>
                                    <li>
                                        <asp:Label ID="Label1" runat="server" AssociatedControlID="UserName" Text="<%$ Resources:Resource, lUsername %>"></asp:Label>
                                        <asp:TextBox runat="server" ID="UserName" />
                                    </li>
                                    <li>
                                        <asp:Label ID="Label2" runat="server" AssociatedControlID="Password" Text="<%$ Resources:Resource, lPassword %>"></asp:Label>
                                        <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                                    </li>
                                </ol>
                                <div class="button">
                                    <asp:Button ID="Button1" runat="server" CommandName="Login" Text="<%$ Resources:Resource, bLoginConfirm %>" />
                                    <span><a href="Account/PasswordRecovery.aspx"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, mUserNameTitleText %>"></asp:Literal></a></span>
                                </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="<%$ Resources:Resource, lUserNameRequired %>" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="<%$ Resources:Resource, lPasswordRequired %>" />
                            </fieldset>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>

                        </LayoutTemplate>
                    </asp:Login>
                </section>
			</div>
			<div><img id="Img2" src="~/Images/banner2.jpg" runat="server">
			</div>
		</div>
	</div>
</asp:Content>
