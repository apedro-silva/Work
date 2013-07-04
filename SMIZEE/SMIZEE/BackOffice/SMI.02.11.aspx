<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SMI.02.11.aspx.cs" Inherits="SMIZEE.Account.SMI_02_11" %>
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
            <asp:TableCell id="TableCell4" ColumnSpan="8">
                <asp:Literal ID="d_BaseData" runat="server" Text="<%$ Resources:Resource, lSearch %>"></asp:Literal>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell id="TableCell5" CssClass="entityLabel" style="width: 100px;">
                <asp:Label ID="AssetClassifierLabel" runat="server" Text="<%$ Resources:Resource, lID %>"></asp:Label></asp:TableCell>
            <asp:TableCell id="TableCell6">
                <asp:TextBox ID="ProductionUnitIdSearchInput" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
            </asp:TableCell>
              <asp:TableCell id="TableCell1" Width="100px" CssClass="entityLabel" BorderWidth="0">
                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, lArea %>"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell id="TableCell9" style="width: 160px;" BorderWidth="0">
                <asp:DropDownList ID="CBAreaIdSearchInput" runat="server" Width="150px">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell id="TableCell10" Width="100px" CssClass="entityLabel" BorderWidth="0">
                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, lCompany %>"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell id="TableCell11" style="width: 160px;" BorderWidth="0">
                <asp:DropDownList ID="CBCompanyIdSearchInput" runat="server" Width="150px">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell id="TableCell12" Width="100px" CssClass="entityLabel" BorderWidth="0">
                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, lSector %>"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell id="TableCell13" style="width: 160px;" BorderWidth="0">
                <asp:DropDownList ID="CBSectorIdSearchInput" runat="server" Width="150px">
                </asp:DropDownList>
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
                       <asp:Label id="ProductionUnitIDLabel" runat="server" Text='<%# Eval("ProductionUnitId") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                 <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lDescription %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="ProductionUnitDescriptionLabel" runat="server" Text='<%# Eval("ProductionUnitDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                 <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lArea %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="AreaIDLabel" runat="server" Text='<%# Eval("AreaDescription") %>'></asp:Label>
                       <asp:HiddenField id="AreaIdHidden" runat="server" Value='<%# Eval("AreaId") %>'></asp:HiddenField>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lCompany %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="CompanyIDLabel" runat="server" Text='<%# Eval("CompanyDescription") %>'></asp:Label>
                       <asp:HiddenField id="CompanyIdHidden" runat="server" Value='<%# Eval("CompanyId") %>'></asp:HiddenField>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>


                 <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lSector %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="SectorIDLabel" runat="server" Text='<%# Eval("SectorDescription") %>'></asp:Label>
                       <asp:HiddenField id="SectorIdHidden" runat="server" Value='<%# Eval("SectorId") %>'></asp:HiddenField>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Right"   
                    HeaderText="<%$ Resources:Resource, lHectares %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="HectaresLabel" runat="server" Text='<%# Eval("Hectares") %>'></asp:Label>
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
                    <asp:TextBox ID="IdInput" runat="server" Width="290px" MaxLength="4"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell id="TableCell18" CssClass="entityLabel" BorderWidth="0">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:Resource, lDescription %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell19">
                    <asp:TextBox ID="DescriptionInput" runat="server" Width="290px" MaxLength="200"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell id="TableCell7" CssClass="entityLabel" BorderWidth="0">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, lArea %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell8">
                    <asp:DropDownList ID="CBArea" runat="server" Width="300px">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                 <asp:TableCell id="TableCell14" CssClass="entityLabel" BorderWidth="0">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, lCompany %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell15">
                    <asp:DropDownList ID="CBCompany" runat="server" Width="300px">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow> 
             <asp:TableRow>
                <asp:TableCell id="TableCell16" CssClass="entityLabel" BorderWidth="0">
                <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Resource, lSector %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell17">
                    <asp:DropDownList ID="CBSector" runat="server" Width="300px">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>         
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, lHectares %>"></asp:Literal></asp:TableCell>
                <asp:TableCell id="TableCell20">
                    <asp:TextBox ID="HectaresInput" class="numbersOnly"  runat="server" Width="290px" MaxLength="9"></asp:TextBox>
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
