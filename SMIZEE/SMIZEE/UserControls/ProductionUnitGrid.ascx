<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductionUnitGrid.ascx.cs" Inherits="SMIZEE.UserControls.ProductionUnitGrid" %>

<asp:Panel ID="ListPanel" runat="server" Width="100%" Visible="True" ScrollBars="Both" Height="400px">
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Both" Width="100%" CssClass="gridview"
        OnRowDataBound="OnGridView1RowDataBound"
        AutoGenerateColumns="False" BorderWidth="0px" AllowPaging="False" PageSize="10">
            <Columns>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Center"   HeaderText="<%$ Resources:Resource, lSelect %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                        <asp:CheckBox ID="SelectRowCheckBox"  runat="server"/>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-Wrap="False" ItemStyle-HorizontalAlign="Left"   
                    HeaderText="<%$ Resources:Resource, lDescription %>" ItemStyle-Wrap="False" 
                    SortExpression="">
                    <ItemTemplate>
                       <asp:Label id="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                       <asp:HiddenField id="ProductionUnitIDHidden" runat="server" Value='<%# Eval("ProductionUnitID") %>'></asp:HiddenField>
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
