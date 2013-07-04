<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SMI.03.04.aspx.cs" Inherits="SMIZEE.Forms.SMI_03_04" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<script src="../Scripts/Validate.js"></script>
<script type="text/javascript">
    function pageLoad(sender, args) {
        $('div[id$="DetailPanel"]').validate();
        
    }

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:Panel ID="OptionsPanel" runat="server" Width="100%" Visible="True">
    <asp:table id="Table4" border="0" runat="server" Width="100%">
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
    <asp:table id="TableSearchPanel" border="0" runat="server" Width="100%">
        <asp:TableRow CssClass="sectiontitle">
            <asp:TableCell id="TableCell4" ColumnSpan="4">
                <asp:Literal ID="d_BaseData" runat="server" Text="<%$ Resources:Resource, lSearch %>"></asp:Literal>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell id="TableCell5" CssClass="entityLabel" style="width: 100px;">
                <asp:Label ID="FormIdLabel" runat="server" Text="<%$ Resources:Resource, lYear %>"></asp:Label>
            </asp:TableCell>
            <asp:TableCell id="TableCell6"  style="width: 220px;">
                <asp:DropDownList ID="ddlYear" runat="server" Width="150px"/>
            </asp:TableCell>
            <asp:TableCell id="Td3" CssClass="entityLabel" style="width: 150px;">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lPeriodicity %>"></asp:Label>
            </asp:TableCell>
            <asp:TableCell id="Td4">
                <asp:DropDownList ID="ddlPeriodicity" runat="server" Width="150px"/>
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
        onselectedindexchanged="OnGridView1SelectedIndexChanged" OnRowDataBound="OnFormsInQueueRowDataBound"
        AutoGenerateColumns="False" BorderWidth="0px" AllowPaging="False" PageSize="10">
            <Columns>
                <asp:CommandField ItemStyle-Width="10%" HeaderText="<%$ Resources:Resource, lSelect %>" ShowSelectButton="True"  SelectText="<%$ Resources:Resource, lDetail %>"/>

                <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Right"   
                    HeaderText="<%$ Resources:Resource, lId %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="OperationalFormIDLabel" runat="server" Text='<%# Eval("OperationalFormID") %>'></asp:Label>
                       <asp:HiddenField id="PeriodNumberHidden" runat="server" Value='<%# Eval("PeriodNumber") %>'></asp:HiddenField>
                       <asp:HiddenField id="PeriodicityCodeHidden" runat="server" Value='<%# Eval("PeriodicityCode") %>'></asp:HiddenField>
                       <asp:HiddenField id="FormDateHidden" runat="server" Value='<%# Eval("FormDate") %>'></asp:HiddenField>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left" 
                    HeaderText="<%$ Resources:Resource, lPeriodicity %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="PeriodicityDescriptionLabel" runat="server" Text='<%# Eval("PeriodicityDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lFormState %>" ItemStyle-Wrap="False" 
                    SortExpression="FormState">
                    <ItemTemplate>
                       <asp:Label id="FormStateLabel" runat="server" Text='<%# Eval("FormState") %>'></asp:Label>
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
    <br />
<asp:Panel ID="DetailPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
        <asp:HiddenField runat="server" ID="CompanyFormID" Value="0"/>
        <asp:Table width="100%" runat="server" border="0">
            <asp:TableRow CssClass="sectiontitle">
                <asp:TableCell ColumnSpan="3">
                    <asp:Literal ID="DetailLabel" runat="server" Text="<%$ Resources:Resource, lDetail %>"></asp:Literal>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="font-weight:bold">
                    <asp:Literal ID="FormMessageLiteral" runat="server" Text="&nbsp;"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell style="width: 150px;font-weight:bold" CssClass="entityLabel">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, lFillDateLabel %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell9" style="width: 160px">
                    <asp:Literal ID="FillDateLiteral" runat="server" Text="dd/MM/YYY"></asp:Literal>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell ColumnSpan="3" style="font-weight:bold">
                    <hr/>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell style="width: 300px" CssClass="entityLabel">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, lNumeroHectaresQuadrante1 %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell10" ColumnSpan="2">
                    <asp:TextBox ID="TextBox1" ToolTip="<%$ Resources:ToolTips, NumeroHectaresQuadrante1 %>" class="numbersOnly" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 300px" CssClass="entityLabel">
                    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, lNumeroHectaresQuadrante2 %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell11" ColumnSpan="2">
                    <asp:TextBox ID="TextBox2" ToolTip="<%$ Resources:ToolTips, NumeroHectaresQuadrante2 %>" class="numbersOnly" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 300px" CssClass="entityLabel">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, lNumeroHectaresQuadrante3 %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell12" ColumnSpan="2">
                    <asp:TextBox ID="TextBox3" ToolTip="<%$ Resources:ToolTips, NumeroHectaresQuadrante3 %>" class="numbersOnly" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 300px" CssClass="entityLabel">
                    <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Resource, lNumeroHectaresQuadrante4 %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell13" ColumnSpan="2">
                    <asp:TextBox ID="TextBox4" ToolTip="<%$ Resources:ToolTips, NumeroHectaresQuadrante4 %>" class="numbersOnly" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell style="width: 300px" CssClass="entityLabel">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, lNumeroHectaresEspacoUtilizado %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="Td5" ColumnSpan="2">
                    <asp:TextBox ID="NumeroHectaresEspacoUtilizadoInput" ToolTip="<%$ Resources:ToolTips, NumeroHectaresEspacoUtilizado %>" class="numbersOnly" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell CssClass="entityLabel">
                    <asp:Literal ID="AmountLiteral" runat="server" Text="<%$ Resources:Resource, lNumeroTotalUnidadesProdutivas %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="Td6" ColumnSpan="2">
                    <asp:TextBox ID="NumeroTotalUnidadesProdutivasInput" ToolTip="<%$ Resources:ToolTips, NumeroTotalUnidadesProdutivas %>" class="numbersOnly" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
</asp:Panel>


<asp:Panel ID="ConfirmButtonPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table ID="Table1" width="100%" runat="server">
            <asp:TableRow>
                <asp:TableCell id="Td7" style="width: 75px">
                    <asp:Button ID="btnUpdate" runat="server" CommandName="QRY" Text="<%$ Resources:Resource, btnUpdate %>" OnClick="OnUpdate"/>
                </asp:TableCell>
                <asp:TableCell id="TableCell7" style="width: 75px">
                    <asp:Button ID="btnSubmit" runat="server" CommandName="QRY" Text="<%$ Resources:Resource, btnSubmit %>" OnClick="OnSubmit"/>
                </asp:TableCell>
                <asp:TableCell id="Td8">
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Resource, btnCancel %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="ApprovePanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table ID="Table2" width="100%" runat="server" Border="0">
            <asp:TableRow>
                <asp:TableCell id="TableCell1" style="width: 100px">
                    <asp:Button Width="75px" ID="btnApproveForm" runat="server" Text="<%$ Resources:Resource, btnApproveForm %>" OnClick="OnApproveForm"/>
                </asp:TableCell>
                <asp:TableCell id="TableCell14" style="width: 100px">
                    <asp:Button Width="75px" ID="btnRejectForm" runat="server" Text="<%$ Resources:Resource, btnRejectForm %>" OnClick="OnRejectForm"/>
                </asp:TableCell>
                <asp:TableCell id="TableCell8">
                    <asp:Button Width="75px" ID="btnCancelApproveForm" runat="server" Text="<%$ Resources:Resource, btnCancel %>" OnClick="OnCancel"/>
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
