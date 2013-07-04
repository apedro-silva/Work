<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SMI.04.01.aspx.cs" Inherits="SMIZEE.Management.SMI_04_01" %>

<%@ Register Src="~/UserControls/ProductionUnitGrid.ascx" TagPrefix="uc1" TagName="ProductionUnitGrid" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:Panel ID="OptionsPanel" runat="server" Width="100%" Visible="True">
    <asp:table id="Table4" border="0" runat="server" Width="100%">
        <asp:TableRow id="zonaopcoes" BorderWidth="0">
            <asp:TableCell id="TableCell2" Width="100px" CssClass="entityLabel" BorderWidth="0">
                <asp:Literal ID="d_Option" runat="server" Text="<%$ Resources:Resource, lOptions %>"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell id="TableCell3" style="width: 160px;" BorderWidth="0">
                <asp:DropDownList CssClass="DropDownList" ID="Options" runat="server" Width="150px">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" BorderWidth="0">
                <asp:Button ID="OptionsBtn" runat="server"
                    OnClick="OnOptions" Text="<%$ Resources:Resource, btnOK %>" Width="75px">
                </asp:Button>
            </asp:TableCell>
        </asp:TableRow>
    </asp:table>
</asp:Panel>

<asp:Panel ID="SearchPanel" runat="server" Width="100%" Visible="False">
    <asp:table id="TableSearchPanel" border="0" runat="server" Width="100%">
        <asp:TableRow CssClass="sectiontitle">
            <asp:TableCell id="TableCell4" ColumnSpan="4">
                <asp:Literal ID="d_BaseData" runat="server" Text="<%$ Resources:Resource, lSearch %>"></asp:Literal>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell id="TableCell5" CssClass="entityLabel" style="width: 100px;">
                <asp:Label ID="AssetClassifierLabel" runat="server" Text="<%$ Resources:Resource, lID %>"></asp:Label></asp:TableCell>
            <asp:TableCell id="TableCell6"  style="width: 160px;">
                <asp:TextBox ID="UserNameSearchInput" runat="server" MaxLength="25" Width="140px"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell id="Td3" CssClass="entityLabel">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lFirstName %>"></asp:Label></asp:TableCell>
            <asp:TableCell id="Td4" style="width: 300px;">
                <asp:TextBox ID="FisrtNameSearchInput" runat="server" MaxLength="25" Width="300px"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="SearchPanelButtons" runat="server" Width="100%" Visible="False">
    <asp:Table id="Table5" width="100%" class="inputtable" border="0" runat="server">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="right" ColumnSpan="3">
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Resource, btnConfirm %>" Width="75px" OnClick="OnQuery">
                </asp:Button>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="right" style="width: 85px;">
                <asp:Button CssClass="button" ID="btnCancelSearch" runat="server" Text="<%$ Resources:Resource, btnCancel %>" OnClick="OnCancel" Width="75px">
                </asp:Button>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="MessagePanel" runat="server" Width="100%" Visible="False">
    <br />
    <asp:Table ID="Table6" width="100%" runat="server">
        <asp:TableRow>
            <asp:TableCell id="Td1" runat="server">
                <asp:Label ID="InfoMessage" CssClass="sucessMessage" Text="Information Message" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="ErrorPanel" runat="server" Width="100%" Visible="False">
    <br />
    <asp:Table ID="Table7" width="100%"  runat="server">
        <asp:TableRow>
            <asp:TableCell id="Td2" runat="server" >
                <asp:Label ID="ErrorMessage" CssClass="errorMessage" Text="Error Message" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>

<asp:Panel ID="ListPanel" runat="server" Width="100%" Visible="False" ScrollBars="Auto">
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both" Width="100%" CssClass="gridview"
        onselectedindexchanged="OnGridView1SelectedIndexChanged"
        AutoGenerateColumns="False" BorderWidth="0px" AllowPaging="False" PageSize="10">
            <Columns>
                <asp:CommandField ItemStyle-Width="10%" HeaderText="<%$ Resources:Resource, lSelect %>" ShowSelectButton="True"  SelectText="<%$ Resources:Resource, lDetail %>"/>
                <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Right"   
                    HeaderText="<%$ Resources:Resource, lId %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lEmail %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="EmailLabel" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lFirstName %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="FirstNameLabel" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lLastName %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="LastNameLabel" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lLastActivityDate %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="LastActivityDateLabel" runat="server" Text='<%# Eval("LastActivityDate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lLastLoginDate %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="LastLoginDateLabel" runat="server" Text='<%# Eval("LastLoginDate") %>'></asp:Label>
                       <asp:HiddenField id="CompanyIDHidden" runat="server" Value='<%# Eval("CompanyID") %>'></asp:HiddenField>
                       <asp:HiddenField id="FunctionalAreaIdHidden" runat="server" Value='<%# Eval("FunctionalAreaId") %>'></asp:HiddenField>
                       <asp:HiddenField id="IsManagerHidden" runat="server" Value='<%# Eval("IsManager") %>'></asp:HiddenField>
                       <asp:HiddenField id="IsExecutiveHidden" runat="server" Value='<%# Eval("IsExecutive") %>'></asp:HiddenField>
                       <asp:HiddenField id="IsLockedOutHidden" runat="server" Value='<%# Eval("IsLockedOut") %>'></asp:HiddenField>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lLastLockoutDate %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="LastLockoutDateLabel" runat="server" Text='<%# Eval("LastLockoutDate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

            </Columns>

        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle CssClass="gvfooter" />
        <HeaderStyle CssClass="gvheader" />
        <PagerStyle CssClass="gvpager" />
        <RowStyle CssClass="gvrow" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
</asp:Panel>
<asp:Panel ID="PagingPanel" runat="server" Width="100%" Visible="False">
    <asp:Table id="Table3" width="100%" runat="server">
        <asp:TableRow>
            <asp:TableCell style="width:100%; text-align:right">
                <asp:DropDownList ID="ddlPaging" runat="server" Width="50px" 
                    CssClass="dropDownList" 
                    onselectedindexchanged="OnPagingSelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="DetailPanel" runat="server" Width="100%" Visible="False">
        <asp:Table width="100%" runat="server" border="0">
            <asp:TableRow CssClass="sectiontitle">
                <asp:TableCell ColumnSpan="2">
                    <asp:Literal ID="DetailLabel" runat="server" Text="<%$ Resources:Resource, lDetail %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, lProductionUnit %>"></asp:Literal>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 150px" CssClass="entityLabel">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, lUserName %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="Td5" style="width: 150px">
                    <asp:TextBox ID="UserNameInput" runat="server" Width="395px" MaxLength="25"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell id="TableCell13" RowSpan="11" VerticalAlign="Top">
                    <uc1:ProductionUnitGrid runat="server" id="ProductionUnitGrid"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="ExpenseBudgetRatingDescriptionLiteral" runat="server" Text="<%$ Resources:Resource, lEmail %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="Td6">
                    <asp:TextBox ID="EmailInput" runat="server" Width="395px" MaxLength="75"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, lFirstName %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell1">
                    <asp:TextBox ID="FirstNameInput" runat="server" Width="395px" MaxLength="75"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, lLastName %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell7">
                    <asp:TextBox ID="LastNameInput" runat="server" Width="395px" MaxLength="75"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, lPassword %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell8">
                    <asp:TextBox ID="PasswordInput" runat="server" Width="395px" MaxLength="75"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Resource, lConfirmPassword %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell11">
                    <asp:TextBox ID="ConfirmPasswordInput" runat="server" Width="395px" MaxLength="75"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:Resource, lCompany %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell12">
                    <asp:DropDownList ID="ddlCompany" AutoPostBack="true" runat="server" Width="405px" OnSelectedIndexChanged="OnCompanySelectedIndexChanged"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, lFunctionalArea %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell9">
                    <asp:DropDownList ID="ddlFunctionalArea" AutoPostBack="true" runat="server" Width="405px"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, lMembershipRole %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell10">
                    <asp:HiddenField runat="server" Id="CurrentRoleNameHidden"/>
                    <asp:DropDownList ID="ddlMembershipRole" runat="server" Width="405px"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:Resource, lManager %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell14">
                    <asp:CheckBox ID="cbManager" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:Resource, lExecutive %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell15">
                    <asp:CheckBox ID="cbExecutive" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:Resource, lIsLockedOut %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell16">
                    <asp:CheckBox ID="cbIsLockedOut" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
</asp:Panel>
<asp:Panel ID="ConfirmButtonPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table width="100%" runat="server">
            <asp:TableRow>
                <asp:TableCell id="Td7" style="width: 75px">
                    <asp:Button ID="btnConfirm" runat="server" CommandName="Query" Text="<%$ Resources:Resource, btnConfirm %>" OnClick="OnConfirm"/>
                </asp:TableCell>
                <asp:TableCell id="Td8">
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Resource, btnCancel %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="BackPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table width="100%" runat="server">
            <asp:TableRow>
                <asp:TableCell id="Td9" style="width: 75px">
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Resource, btnBack %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>    
</asp:Content>
