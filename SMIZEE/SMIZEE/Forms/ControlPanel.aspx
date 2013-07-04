<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ControlPanel.aspx.cs" Inherits="SMIZEE.Forms.ControlPanel" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:Panel ID="OptionsPanel" runat="server" Width="100%" Visible="True">
    <asp:table id="Table4" border="0" runat="server" Width="100%">
        <asp:TableRow id="zonaopcoes">
            <asp:TableCell id="TableCell2" Width="100px" CssClass="entityLabel">
                <asp:Literal ID="CompanyLiteral" runat="server" Text="<%$ Resources:Resource, lCompany %>"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell id="TableCell3">
                <asp:DropDownList ID="ddlCompany" runat="server" Width="350px" AutoPostBack="true" OnSelectedIndexChanged="OnCompanySelectedIndexChanged">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell id="TableCell4" Width="100px" CssClass="entityLabel">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, lFormState %>"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell id="TableCell1">
                <asp:DropDownList ID="ddlFormState" runat="server" Width="350px" AutoPostBack="true" OnSelectedIndexChanged="OnFormStateSelectedIndexChanged">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:table>
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
    <asp:GridView ID="gvFormsInQueue" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both" Width="100%" CssClass="gridview"
        onselectedindexchanged="OnGridView1SelectedIndexChanged"  OnRowDataBound="OnFormsInQueueRowDataBound"
        AutoGenerateColumns="False" BorderWidth="0px" AllowPaging="False" PageSize="10" AllowSorting="False">
            <Columns>
                <asp:CommandField ItemStyle-Width="10%" HeaderText="<%$ Resources:Resource, lSelect %>" ShowSelectButton="True"  SelectText="<%$ Resources:Resource, lFillForm %>"/>
                <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Right"   
                    HeaderText="<%$ Resources:Resource, lId %>" ItemStyle-Wrap="False" 
                    SortExpression="CompanyFormID">
                    <ItemTemplate>
                       <asp:Label id="CompanyFormIDLabel" runat="server" Text='<%# Eval("CompanyFormID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lDescription %>" ItemStyle-Wrap="False" 
                    SortExpression="FormDescription">
                    <ItemTemplate>
                       <asp:Label id="CompanyNameLabel" runat="server" Text='<%# Eval("FormDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"
                    HeaderText="<%$ Resources:Resource, lProductionUnit %>" ItemStyle-Wrap="False"
                    SortExpression="ProductionUnit">
                    <ItemTemplate>
                       <asp:Label id="ProductionUnitLabel" runat="server" Text='<%# Eval("ProductionUnit") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lPeriodicity %>" ItemStyle-Wrap="False" 
                    SortExpression="Periodicity">
                    <ItemTemplate>
                       <asp:HiddenField id="FormDateHidden" runat="server" Value='<%# Eval("FormDate") %>'></asp:HiddenField>
                       <asp:HiddenField id="PeriodicityCodeHidden" runat="server" Value='<%# Eval("PeriodicityCode") %>'></asp:HiddenField>
                       <asp:HiddenField id="PeriodNumberHidden" runat="server" Value='<%# Eval("PeriodNumber") %>'></asp:HiddenField>
                       <asp:HiddenField id="FormPageHidden" runat="server" Value='<%# Eval("FormPage") %>'></asp:HiddenField>
                       <asp:Label id="PeriodicityLabel" runat="server" Text='<%# Eval("Periodicity") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lFormState %>" ItemStyle-Wrap="False" 
                    SortExpression="FormState">
                    <ItemTemplate>
                       <asp:Label id="FormStateLabel" runat="server" Text='<%# Eval("FormState") %>'></asp:Label>
                       <asp:HiddenField id="FormStateIdHidden" runat="server" Value='<%# Eval("FormStateId") %>'></asp:HiddenField>
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

</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvFormsInQueue" />
    </Triggers>
</asp:UpdatePanel>    

</asp:Content>
