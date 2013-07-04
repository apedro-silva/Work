<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SMI.02.10.aspx.cs" Inherits="SMIZEE.Account.SMI_02_10" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:Panel ID="BigPanel" DefaultButton="btnConfirm" runat="server" Width="100%" Visible="true">
<asp:Table ID="Table1" runat="server"></asp:Table>
    <asp:Panel ID="OptionsPanel" runat="server" Width="100%" Visible="True">
    <asp:table id="Table4" BorderWidth="0" runat="server" Width="100%" BorderStyle="Solid" CellPadding="0" CellSpacing="0">
        <asp:TableRow id="zonaopcoes" BorderWidth="0">
            <asp:TableCell id="TableCell2" Width="100px" CssClass="entityLabel" BorderWidth="0">
                <asp:Literal ID="d_Option" runat="server" Text="<%$ Resources:Resource, lOptions %>"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell id="TableCell3" style="width: 160px;" BorderWidth="0">
                <asp:DropDownList ID="Options" runat="server" Width="150px">
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
    <asp:Table id="Table2" width="100%" CssClass="inputtable" border="0" runat="server">
        <asp:TableRow CssClass="sectiontitle">
            <asp:TableCell id="TableCell4" ColumnSpan="6">
                <asp:Literal ID="d_BaseData" runat="server" Text="<%$ Resources:Resource, lSearch %>"></asp:Literal>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell id="TableCell5" CssClass="entityLabel" style="width: 100px;">
                <asp:Label ID="AssetClassifierLabel" runat="server" Text="<%$ Resources:Resource, lID %>"></asp:Label></asp:TableCell>
            <asp:TableCell id="TableCell6">
                <asp:TextBox ID="InvestmentTypeIdSearchInput" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell id="TableCell7" CssClass="entityLabel" style="width: 100px;">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lSmallDescription %>"></asp:Label></asp:TableCell>
            <asp:TableCell id="TableCell8">
                <asp:TextBox ID="InvestmentTypeSmallDescriptionSearchInput" runat="server" MaxLength="25" Width="150px"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell id="Td3" CssClass="entityLabel">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, lDescription %>"></asp:Label></asp:TableCell>
            <asp:TableCell id="Td4" style="width: 300px;">
                <asp:TextBox ID="InvestmentTypeDescriptionSearchInput" runat="server" MaxLength="75" Width="300px"></asp:TextBox>
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
    <asp:GridView ID="GridView1" runat="server" CellPadding="6" ForeColor="#333333" GridLines="Both" Width="100%"  CssClass="gridview"
        onselectedindexchanged="OnGridView1SelectedIndexChanged"
        AutoGenerateColumns="False" BorderWidth="0px" AllowPaging="False" PageSize="10">
            <Columns>
                <asp:CommandField ItemStyle-Width="10%" HeaderText="<%$ Resources:Resource, lSelect %>" ShowSelectButton="True"  SelectText="<%$ Resources:Resource, lDetail %>"/>
                <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Right"   
                    HeaderText="<%$ Resources:Resource, lId %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="InvestmentTypeIDLabel" runat="server" Text='<%# Eval("InvestmentTypeId") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                 <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lSmallDescription %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="InvestmentTypeSmallDescriptionLabel" runat="server" Text='<%# Eval("SmallDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lDescription %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="InvestmentTypeDescriptionLabel" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
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
<asp:Panel ID="DetailPanel" runat="server" Width="100%" Visible="False" CssClass="xdetail">
        <asp:Table width="100%" runat="server">
            <asp:TableRow CssClass="sectiontitle">
                <asp:TableCell ColumnSpan="2">
                    <asp:Literal ID="DetailLabel" runat="server" Text="<%$ Resources:Resource, lDetail %>"></asp:Literal>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 100px" CssClass="entityLabel">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, lID %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="Td5">
                    <asp:TextBox ID="IdInput" runat="server" Width="295px" MaxLength="4"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 100px" CssClass="entityLabel">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, lSmallDescription %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="Td6">
                    <asp:TextBox ID="SmallDescriptionInput" runat="server" Width="295px" MaxLength="25"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 100px" CssClass="entityLabel">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, lDescription %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="Td7">
                    <asp:TextBox ID="DescriptionInput" runat="server" Width="295px" MaxLength="75"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>      
        </asp:Table>
</asp:Panel>
<asp:Panel ID="ConfirmButtonPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table width="100%" runat="server">
            <asp:TableRow>
                <asp:TableCell id="Td8" style="width: 75px">
                    <asp:Button ID="btnConfirm" runat="server" CommandName="Query" Text="<%$ Resources:Resource, btnConfirm %>" OnClick="OnConfirm"/>
                </asp:TableCell>
                <asp:TableCell id="Td9">
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Resource, btnCancel %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="BackPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table width="100%" runat="server">
            <asp:TableRow>
                <asp:TableCell id="Td10" style="width: 75px">
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Resource, btnBack %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>    
</asp:Content>
