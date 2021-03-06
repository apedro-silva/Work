﻿<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SMI.03.01.aspx.cs" Inherits="SMIZEE.Forms.SMI_03_01" EnableEventValidation="true" %>
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
                <asp:Label ID="FormIdLabel" runat="server" Text="<%$ Resources:Resource, lCompany %>"></asp:Label>
            </asp:TableCell>
            <asp:TableCell id="TableCell6"  style="width: 220px;">
                <asp:DropDownList ID="ddlCompany" runat="server" Width="150px"/>
            </asp:TableCell>
            <asp:TableCell id="Td3" CssClass="entityLabel" style="width: 150px;">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, lProductionUnit %>"></asp:Label>
            </asp:TableCell>
            <asp:TableCell id="Td4">
                <asp:DropDownList ID="ddlProductionUnit" runat="server" Width="150px"/>
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
                    HeaderText="<%$ Resources:Resource, lFormDescription %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="FormDescriptionLabel" runat="server" Text='<%# Eval("FormDescription") %>'></asp:Label>
                       <asp:HiddenField id="FinancialExportFormIdHidden" runat="server" Value='<%# Eval("FinancialExportFormId") %>'></asp:HiddenField>
                       <asp:HiddenField id="PeriodNumberHidden" runat="server" Value='<%# Eval("PeriodNumber") %>'></asp:HiddenField>
                       <asp:HiddenField id="PeriodicityCodeHidden" runat="server" Value='<%# Eval("PeriodicityCode") %>'></asp:HiddenField>
                       <asp:HiddenField id="FormDateHidden" runat="server" Value='<%# Eval("FormDate") %>'></asp:HiddenField>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left" 
                    HeaderText="<%$ Resources:Resource, lCompany %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="CompanyNameLabel" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lProductionUnit %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="ProductionUnitDescriptionLabel" runat="server" Text='<%# Eval("ProductionUnitDescription") %>'></asp:Label>
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
                <asp:TableCell ColumnSpan="5">
                    <asp:Literal ID="DetailLabel" runat="server" Text="<%$ Resources:Resource, lDetail %>"></asp:Literal>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 200px;font-weight:bold" CssClass="entityLabel">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, lProductionUnitLabel %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell8" style="width: 220px"  ColumnSpan="2">
                    <asp:Literal ID="ProductionUnitLiteral" runat="server" Text=""></asp:Literal>
                </asp:TableCell>
                <asp:TableCell style="width: 150px;font-weight:bold" CssClass="entityLabel">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, lFillDateLabel %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="TableCell9" style="width: 160px">
                    <asp:Literal ID="FillDateLiteral" runat="server" Text="dd/MM/YYY"></asp:Literal>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5" style="font-weight:bold">
                    <asp:Literal ID="FormMessageLiteral" runat="server" Text="&nbsp;"></asp:Literal>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5" style="font-weight:bold">
                    <hr/>
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>
</asp:Panel>
<asp:Panel ID="InputPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
        <asp:Table ID="Table1" width="100%" runat="server" border="0">
            <asp:TableRow>
                <asp:TableCell style="width: 100px" CssClass="entityLabel">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, lCountry %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="Td5" style="width: 220px">
                    <asp:DropDownList ID="ddlCountry" runat="server" Width="200px" CssClass="dropDownList" AutoPostBack="False">
                    </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell style="width: 120px" CssClass="entityLabel">
                    <asp:Literal ID="AmountLiteral" runat="server" Text="<%$ Resources:Resource, lAmount %>"></asp:Literal>
                </asp:TableCell>
                <asp:TableCell id="Td6" style="width: 250px">
                    <asp:TextBox ID="AmountInput" ToolTip="<%$ Resources:ToolTips, MontanteExportacao %>" class="numbersAndCommasOnly" runat="server" Width="200px" MaxLength="16"></asp:TextBox>Kz
                </asp:TableCell>
                <asp:TableCell id="TableCell1" HorizontalAlign="right">
                    <asp:Button ID="btnAddCountry" runat="server" Text="<%$ Resources:Resource, bAddCountry %>" OnClick="OnAddCountry"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
</asp:Panel>

<asp:Panel ID="FormPanel" runat="server" Width="100%" Visible="False" ScrollBars="Auto">
        <asp:GridView ID="FormGridView" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both" Width="100%" CssClass="gridview"
            onselectedindexchanged="OnFormGridViewSelectedIndexChanged" OnRowDataBound="OnFormGridViewRowDataBound"
            AutoGenerateColumns="False" BorderWidth="0px" AllowPaging="False" PageSize="10">
                <Columns>
                    <asp:CommandField ItemStyle-Width="10%" HeaderText="<%$ Resources:Resource, lSelect %>" ShowSelectButton="True"  
                        SelectText="<%$ Resources:Resource, lDelete %>"/>
                    <asp:TemplateField ItemStyle-Width="80%" HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"
                        HeaderText="<%$ Resources:Resource, lCountry %>" ItemStyle-Wrap="False" 
                        SortExpression="">
                        <ItemTemplate>
                            <asp:Label id="CountryDescriptionLabel" runat="server" Text='<%# Eval("CountryDescription") %>'></asp:Label>
                            <asp:HiddenField id="CountryIdHidden" runat="server" Value='<%# Eval("CountryId") %>'></asp:HiddenField>
                        </ItemTemplate>
                        <HeaderStyle Wrap="False" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Right"   
                        HeaderText="<%$ Resources:Resource, lExportAmount %>" ItemStyle-Wrap="False" 
                        SortExpression="">
                        <ItemTemplate>
                            <asp:Label id="ExportAmountLabel" runat="server" Text='<%# Eval("ExportAmount") %>'></asp:Label>
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

<asp:Panel ID="ConfirmButtonPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table width="100%" runat="server" Border="0">
            <asp:TableRow>
                <asp:TableCell id="Td7" style="width: 100px">
                    <asp:Button Width="75px" ID="btnUpdate" runat="server" Text="<%$ Resources:Resource, btnUpdate %>" OnClick="OnUpdate"/>
                </asp:TableCell>
                <asp:TableCell id="TableCell7" style="width: 100px">
                    <asp:Button Width="75px" ID="btnSubmit" runat="server" Text="<%$ Resources:Resource, btnSubmit %>" OnClick="OnSubmit"/>
                </asp:TableCell>
                <asp:TableCell id="Td8">
                    <asp:Button Width="75px" ID="btnCancel" runat="server" Text="<%$ Resources:Resource, btnCancel %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="ApprovePanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table ID="Table2" width="100%" runat="server" Border="0">
            <asp:TableRow>
                <asp:TableCell id="TableCell10" style="width: 100px">
                    <asp:Button Width="75px" ID="btnApproveForm" runat="server" Text="<%$ Resources:Resource, btnApproveForm %>" OnClick="OnApproveForm"/>
                </asp:TableCell>
                <asp:TableCell id="TableCell11" style="width: 100px">
                    <asp:Button Width="75px" ID="btnRejectForm" runat="server" Text="<%$ Resources:Resource, btnRejectForm %>" OnClick="OnRejectForm"/>
                </asp:TableCell>
                <asp:TableCell id="TableCell12">
                    <asp:Button Width="75px" ID="btnCancelApproveForm" runat="server" Text="<%$ Resources:Resource, btnCancel %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="BackPanel" runat="server" Width="100%" Visible="False" CssClass="detail">
    <asp:Table width="100%" runat="server">
            <asp:TableRow>
                <asp:TableCell id="Td9" style="width: 75px">
                    <asp:Button Width="75px" ID="btnBack" runat="server" Text="<%$ Resources:Resource, btnBack %>" OnClick="OnCancel"/>
                </asp:TableCell>
            </asp:TableRow>
    </asp:Table>
</asp:Panel>

</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="GridView1" />
    </Triggers>
</asp:UpdatePanel>    

</asp:Content>
