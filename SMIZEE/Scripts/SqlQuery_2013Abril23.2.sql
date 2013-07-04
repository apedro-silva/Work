﻿ALTER TABLE [dbo].[FinancialExportFormCountries] DROP CONSTRAINT [FK_dbo.FinancialExportFormCountries_dbo.Countries_Country_CountryID_Country_SmallDescription_Country_Description_Country_ISOCode]
DROP INDEX [IX_Country_CountryID_Country_SmallDescription_Country_Description_Country_ISOCode] ON [dbo].[FinancialExportFormCountries]
EXECUTE sp_rename @objname = N'dbo.FinancialExportFormCountries.Country_CountryID', @newname = N'CountryID', @objtype = N'COLUMN'
ALTER TABLE [dbo].[Countries] ALTER COLUMN [CountryID] [int] NOT NULL
ALTER TABLE [dbo].[Countries] DROP CONSTRAINT [PK_dbo.Countries]
ALTER TABLE [dbo].[Countries] ADD CONSTRAINT [PK_dbo.Countries] PRIMARY KEY ([CountryID])
ALTER TABLE [dbo].[FinancialExportFormCountries] ADD CONSTRAINT [FK_dbo.FinancialExportFormCountries_dbo.Countries_CountryID] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Countries] ([CountryID]) ON DELETE CASCADE
CREATE INDEX [IX_CountryID] ON [dbo].[FinancialExportFormCountries]([CountryID])
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FinancialExportFormCountries')
AND col_name(parent_object_id, parent_column_id) = 'Country_SmallDescription';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FinancialExportFormCountries] DROP CONSTRAINT ' + @var0)
ALTER TABLE [dbo].[FinancialExportFormCountries] DROP COLUMN [Country_SmallDescription]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FinancialExportFormCountries')
AND col_name(parent_object_id, parent_column_id) = 'Country_Description';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FinancialExportFormCountries] DROP CONSTRAINT ' + @var1)
ALTER TABLE [dbo].[FinancialExportFormCountries] DROP COLUMN [Country_Description]
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FinancialExportFormCountries')
AND col_name(parent_object_id, parent_column_id) = 'Country_ISOCode';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FinancialExportFormCountries] DROP CONSTRAINT ' + @var2)
ALTER TABLE [dbo].[FinancialExportFormCountries] DROP COLUMN [Country_ISOCode]
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201304231540050_country1', 0x1F8B0800000000000400ED5D5F6FDC38927F3FE0BE83D14F77076C9C647680DD81B30B4FEC6C8C75126F3A330BEC8B2177D3B6306AA9575267E3FB6AF7701FE9BEC291FAD71459248B14F5CFD1CB4C2C9245B2EAC7AA229BACFABFFFF9DFB33F7FDB45275F499A8549FC66F5EAC5CBD5098937C9368C1FDEAC0EF9FDEFFEB0FAF39FFEFDDFCE2EB7BB6F27BFD6F57E60F568CB387BB37ACCF3FD4FA7A7D9E691EC82ECC52EDCA44996DCE72F36C9EE34D826A7AF5FBEFCE3E9AB57A7849258515A2727679F0F711EEE48F107FDF36D126FC83E3F04D187644BA2ACFA4E4BD605D5938FC18E64FB6043DEACD61FAEFE7179F9A2ACB83A398FC2800E624DA2FBD5C9FEF73FFD9291759E26F1C37A1FE461107D79DA135A7E1F4419A986FCD3FEF7D851BF7CCD467D1AC47192537249EC34EB55331F3AA34B3AF3FC890DAB98D59BD5F9611BE67C155AE9AFE4A9F5817EBA49933D49F3A7CFE49E6F7875B13A396D373E155B376DC5866C146F565771FEC3EBD5C9C74314057711699845B9B9CE9394FC85C4240D72B2BD09F29CA454FC575B52CC42EA5AE8E80B95719607BB7DDDD50525C33E02BDE92951B1A6EC5F35212A638AD0D5C9BBF01BD95E93F8217F6C287D08BED55F5EBDA438FD250E29A069A33C3DD8F77CBEDF47E1A610FE4DF030C2003EED19F769F76689E9097D26D921CAED27603F838FC1D7F0A16499622EAB93CF242AFE953D86FB72FDBE287079CBD5799726BBCF495443F65874BB4E0EE986092381CBBF04E903C9DB033B3B3DAE3DED8A3C8EC06155B6C465BB322D64DD7D755E906C9386FB92D5FDA21ACDFA3549BF8654B00E8CAF9ABAB09D6BDA37D34757275394FADB64B70FE22717A9574D5DA4CE35ED5BEA5557E358B0BE245EFE8D9AF82F9FAF2DBB7E6DD9B505D6A8EF993A62AD68EA86B5A669DF585BEF82287297F9EB1FA78A3754E757EB4F6F696BCB8E7FE849B15D0777D451A143BC0F2327A3D622E0023C89C0C4E137B6BEEBCDC69DA72470DAE5D1764E9BBCAADDC4E53D7375F39E6CF220259992CF1D2CD5BB43BC61B30A2257ECB429B8A048A6B0E069946DD18672D56D57C45ABA6D8AEA968BC84710F955BC65FB4437A9378D5D04DF6ABCC87E04D95F90AF244AF63BCAC29BC72073F21C451A2E4880682C8018011037240D13BA2A19FF1CB0C035778181D0FC199D4B421B75EBCDE3EBDE2CC05792E56CF1B18F6E6680A7E0660B440ACBFA1F63FDA7C9F65038E294BCD38F856D0A4E5A40A2305D45A03F3DC4FDEE87DAEFA04E3FBB92C17AE2DEB7AABA9FF28A3D29F42B5E1B27B765BDE32F7940B1F46B1E5407FA454F37BEFA3705C4109BAACA5156354C03ADABD98EB5DADE21865AD7548EB4AC601A6855ABD3AFA4EB9C2E6AA72D296BE8B423AD1B2E3668041BC4EE6D4461EC24F2BAAD8BD4F9B68BE04710FCBB24DDB97AA0755BA7B347AEED745D0D0F7B0E36D1712E03D81EEF620D9A70740D19B65ABAB762DDA3695354918C9BAA5E27F3C688BA22DE15ED3350713F4EF63A027AA9E1748A9E8ED5918861A31105E9EE73103F1076C9D1CE3B5792FA10C454A1F8A276F98D6C0E79F89578DA3B341645A5186E8F35DAEAA0290095C0B1D4D603E70FD894836A5512C6C595C143E32B74534C611CC49B30882EBFED933477D653321927B50593E95D8BE1B6026665D05911589E8A60D4CAC7C3EE4EB376D17363B7AD3BDFBC5E1FEE76616E2285D227FB7D9A7C0D221FB498AA3C8FE8071FC42A65E98D5EA32EBD512C85E078099EFEB3E32DBE5A72C30D407D54529C3A802A5AD644B7556D4E57AB2AC94A5B59D3D6B6141A1A3BE2B2B27EC0E55810E32DFE636D0ADB67CDD8818BCDF45310CFA31093119A584B4126595ECA0CD991286E92D52DCE5BC804EB27AC69D9CC4433795DF39A77BE9C890E1764D5D43CB916035EA375726C4C7681513ADFB1493416816C42BA235B9DDCA4F45FD52BBC3FAC4ED69B80D1B576DAD11CEAB06EC65D2E085D81596DF8DF11CAE56039E7A6196A9EF5FF2DE656FFDFCF5EA2F32EA2F3FE61D9394C64E7B0A68A27FB487AD653D593AB0B12513F397DEABDBFB749967F49F220EAB79BE201DFA63882643DF6DB5985A0B6C07E3E6CA93E20DB9E05376CA73F1FB2302659D62F3FBFA44118D30D4DCFC80FA2207DEAB98FA4321A24CE48CF5CFB943F9294AA9F2C894901FA9EFBBB08299DBC7CD7324077C71B50BF06D181F4863DB2798C932879781AA8C3A3F9393FE4C98ED9D2B709F5289268FD94E564D7E9E878397D5A4E9FBED3D327C3B913E6C4A9D7B326FD2913E27C69989325EC9992D56912F61CC97EFFB4BEF8C7E565E74D5443A5D34EAA45E5FBD94E2D1B20FD06E890A654A4E75946F2AC57CFA2EAE93A0CEEC228CC43D26F77C5AE8E3982BDF652CFE6A9D75E0AE9F4DAC3E2992D9ED977EA99357651E39E0975D4AE8458D1BFA376EC41E9ADB5AB6006ABF4DBEC836305D135B55FEC7CC1D5EF5190EA14384B26F5FD7840133D505E4CCE6272BE2F93A3D046B2DDD15694F4B9BEB6370BA4EA463443BA7AE8B1F77590A0EA4F7DA4806B819E98CF6306451FCEB10174F43C5ADFC16206385B7FD34D7574F803BB13778688CC605AED7491CA2BB3581AC5146F95848C0B45DB1EBB6CF4446CF5841447C39A1D3205241FC486760C905AFB521F1E5C750F2EFAF7E79A7B73A6A9DFB35DB348E89D54D1DF0EC1360DE2FC55FD28DC45192968BEEE81E60F3DD0FCBD479A826A2F8E243D905D764ECBCEE9BBDD3919774CB89D52CF3B24D3CE08B523F27434F7FEB00BE2CF242B3A72B5F41211175B0F12F97EACFD440FE2EAFB6D85797A4F6563190B4620F7F724FDED43986594E245F0D49D5645E72A2E67DC89DEE56E1F254F8464EFC3947822754DEEF3C59CAB692DE67C31E782A194EC806CD015552473A9AAE7CDA8CB1D88661DAE8118695F879C724FEAE34D535DC4347C1E69B6A8D32D5914DE576951BC382E12C5CE5E0C48717169467669AE934D1031E781A47F0FF3C7EBE45F2D397532D702ED0F641B1E76BD917F1F3E3CFA237EF98D01AD37D6C8E47D7347EEC12F83165F6CF1C5BE635F4CB26606C74C515FEF34A81AF5E3B2C9BD69FD37B8BAED8406F1ECE46E916E9EA9A1ED6C7B73003B5D9F87297576F8966BF4DD289D6F988A6F4E4F267206E3F160A83846ABCFD4BA1FA72D1EC9E2917CC71E89E676B6AE9EDE84F5764B5BD18BD6E330DCD6D6D5ED64698BB0920E7695B5BBDADA5BD1BA5D89B8BF1CC2ADCB2D07BAF9F30C5B5C8C9E5D1046832FD79B20CBFE95A4DBE1E7FB2E4CB37C1415751D8CD4F1DB64C72E560DDEEF55566AE5A3DBF17342577E10DB3B69155CDE51AC1E5292ADC37843183FD787CD86646A3F0477A64609093DF8B0888C6CE11352EDE48BDE75B2F92D397831D825B98730F641EC6D12DF87E9AEB01A5F92DF886DA4630F204F091DBB8FB95C658CCB64FBE99077C52D8FACB78F2CB2F0D6C7086B92BFD2BD4BBD5F1D87EBCA915C7EDB87E56D0F3FF032E595416D38B0C1DF712811424E0318C1D191824D632929BD38E65FC1311E59C92D733632DE6B3B7E956233724550DC45DD28582BC4C98F5CEDB68C0B7E1C9FA28A345855BD4E11231969176F92B573F126EB761DBC4946C2BBA7D173AC7B9F3BB00AE02EF8173729C0D270DE93088BC1718722FF32E9B25FB1FD7DB3FB895FB14BBAE884EB8E3FCA6AF1E25B37894032E9308F67E94057EAF3736365CC4C3A9C939F67198BDDC6DAD64739872DA5D1DC1615D6C965BC3D29C721D43B8EB3543B8C134515AA660E511EEEA32217C19BD57F49CC56D36C2CCC91E6716002DD572B71017F8A2F48447272725EF086FA3141B609B63232296FB6ED2F74CD1316542464A13EE22C67A7AEB9AC20E83E28DC07917EF4423350B3348D6485C2C6D7F424965C903D899906D00BC5C3109A9E04D699387576CA414C8F3C01C64016FA235280BA1002C585878722D40100C73287D0D490A8193C060AB57FDE01881AF9741BC1F038ACD335229122E56EEC018D62E247AE8F26B9E4C431294C01030A6E1BEA0F9982B83A8F63787C56393A91D0111376F6804E21DB27D7459D4E74E2D86C4F00038963CA5C7FC86C4BAAEB2806C025FBE988095B4CB1A7828DA23E84CDBAAA0D2A55E4015C8A3901A7864FC35430D8904FDF3A20D520395FE31908B3C7EC6F3A2801A9E0DAF8B4C5A69C3D0E01F86960511A3A4AE2892A37AB35F6245974EF7F28ACDD7049FDB4E8B88132FC7544DC0D901490F700F8848393C41D30018CE86FDAE933BBA20F108D97510C814165D6322570CC29CC385442A9772C406A4C82C63B93E5BD9DC9E1D43407942707E7ACB783AA49701D0732125A816BBB7A00B56F3FF58AD5D6E52983B29E2652F919608D6A0F38E565D66D1823A154FB63931E44AAF3FF5E91ABF809017D3C304D34C3B342D96AC3CF7C9D110E4BD9D7D0C641BD2E019F051851D9F8B4ABA1C986D76951A092685AADCA49AE12C434512A1893E0BBEBB24140C3EB58C75D47706A541466A50C8F03AE17315564EBD709B0A349AF0B613AB85F081459687DE15F106FE7310D89739B3D2772B7E9EC01CD7B873981BDE5AC779516FB49DC4EB21B0E67BA7B1C7DDF38E31DA3D35ED17697D80D95CF686738C93DE133DB0D8AEF2A8D20533EB004907C7CB7EF0267D5EBCC19597CC5140637FB0AA1CDC8F60B8F682DC063F4027CA174A6FE003C81A19D02585E93F70C9A5BB8602A171578B4AD20AC2A1AD80056DFE77C742B6A1E4329589420A7AF6555D3D0AA5A5DA381313C2FCD8B99C540EA1723C3D9EA60E43E0DD77C6044CF7F176737B3C1F77376529FD1CE4E31317DFA264B7822733919574A9533A9F372C1A585B25FB9935D45A8096340AB20D1CFE242C1C6FBA8C75E73528E302770AB13860DBCCA94B9C7B881C869D1DA9DBF7CF1E295D4BFFFF5A11A2A06625072BFBE56844AB6FEC639EC1AB0DB01A377BE1D7CABB9EF7427B1C39DF9CED66A478BDDC976C5E46C77AE13D8B1CE6FA7AACA77A2428B31F9C9113072722F0B549A92A74C5F571A663094B634086CFAFA5291E5060F1C95CEF48CCF79E94DFDF807D29C7A59CD507722CFF7F0998D7A43EBFC4FF3B0731AFC1C0F2BDD199DE0A13271A0C0674CCBA140BC9C70CB15FEA61C1F33F32C0CD319C5CD300879663E87224D8B23D850DE489F689FB19FA29FCC184E8B5EBEF3F2608C497B1C0167E9DBF489FD67E6F5602738AE0B8445C45CFD21EC7D555C5210C5AA70BA1188CA2C32337F67E4FBAB1821CECCBF41DE634565861900BD33F65F46BDD78A90DFE4FD1536932AFAB70A35C72A102A59A90D06396A00E4CA50D25A6A169353C5AA568DCD18B8BA3D50F7E08CC6E8FD7A8E8CBF080D13C0C01E4E2D65B5000DF2428F62ACC5678E4A6E0120F3A6C03B66E7EFFCA32735B8C38F96F0649DFCCB22B83D6D93D31624ADDDB45DF8DF84B0AFE49B8C6FD6624D723EEC7DB63AB96CC2E4B783D84B586D376F7E4686487031EB0D64D624FD1A6E0844A42A329228A31387208D2638B491068B15A0A251857030D0B80EEE2868281AEEC30824D4AA6024C7A28482E22902B61A1AB7838D4264C408B04639B148BBB098CA10C7060257F196EDA9611A4DA1918C785B0BA2265F9E33103DC6298411D00A13699CE75792E5AC6F46009E2C5FC33CB8967A018727D8019328D95E0E9464B99F363467F9B422AA73200A7599199F49199A14446652478245105111303796638D80B4A01847F6A475FA45179E06DB9371F876036FF65B5A9ADC36196B2CDA9737B5A643BC58EED6855249E8AFFAE23B43CC03357EE967588828F0ABBA0D59E968D3D80770BA6DD3A11647AA2317430755362D895CB997333466EE1FD4B8DC68237A366B6368232010E67C3AC11FE3D2109D70B578CF4C95A9A8B59F50E52A6AA674F4FF2467554DA5DE837054783750DC45B4A7896081E083978E89CC06A89A7A12406D881D9260357C814802BCA95C2FDF6C697C5A2367C0442EBA9988A95CFCF0474CDEC251E57C77DF6CAAFD522397A07422BAE9080945FCF0484821C2116D3CEFCE0CAA1D2B315D05C0225555F57C142D203671CE9F86412A82008BA4DD8E17561DB3242818A448A320CD424EA42030C3CC08397502869FAE136FED771473BF5185F597C70E448F77E0001009BFB5F6DA7BB8EE8C50C68D8738820B32DF9E9431CC3CCF2370B7A2639931B03CAF62AA1D612F4C2B9D3A1CCFE45FF48C936AFDA0E79B63AD1FEF4C90F5C42FD177C4714E77B46D9CA6E274DB37371527DA7803EA87C3BAA0CD386EA3C33E9B588209FCAC970277B060270D4CAC674B00F8154F73066225123082309617620CE19E592F860D6EF9C50ADA5D788CB46176D60B69B72C34C6D0B60A67A5ACEC13CE32D9F264306B646B871C2D90ADEDB1E5D7A8F646BC66A7E39FF64A1E3C41D5853C886FFC291C8679AAAB77BDAE42E15A178E5DD8F5A8B8FFE597597D2F4F7DA43C806116A1F55A73C305D7830E01A5E37D0D0771E1F4FAC09C36589B051F0DE8438577EB8B8B6381D16C355C828DA1A66CB6245E393C8675710A4D85978145882B0C8B7041AECCF2A97FC3B317122EAC9503427CCB4EBEB26029347D8C243CAF945192FA1793322E12D73570FDC3A760D0F6D4D68EA2EDA7953E1ADE5E62EDA4A57DC4DA457BEEF46D0755014900F6A06297B426638A5EC2CD07BA3EA06193295E491F305244C64071CA002543140D9F7C1A1E506697CA2EB28361826637AA330FC7709D5031014CBC45C512504FDE144D40C563E8DE0E96E1A6F801BDAF74C57B747B56DBE800C343F661183DA8AA303E87B667B89B1241BFA81E4608A3EB1AC44120FE7DAE7AE6880341E5D53C2C77873C18D4BDF9C4B3D04667980F093D32B06FDDC03D3D04B8A57A98D81A39F034911B687593523373E03122D7BE1A5BE789AA9E2102B346BD586C4DC1F46651988FCDFD2FD32BC5615865B61196CFE84C7334DB020F8CEC53E7B3C7718C4CF3F4AA293B3B5D6F1EC92EA83E9C9DD22A1BB2CFA949FA906C4994D5051F82FD3E8C1FB263CBEACBC97A1F6CE894DEFE6EBD3AF9B68BE2ECCDEA31CFF73F9D9E6605E9ECC52EDCA44996DCE72F36C9EE34D826A7AF5FBEFCE3E9AB57A7BB92C6E9A6B5A6C587624D4F7992060F4428653F3C6FC9BB30CDF28B200FEE8A93A8B7DB9D540DF5D0ACEEABF5DE4C16647D7FBAAECEFE5DBD54F97045F5E58B927BD5BB34A1FD9179EFE87CD8B9503135225E7496DBD196EB4D100569FDA28F1F277BC5F736890EBB58F82802514D87BD85C9F260B76F53E23EE369318CB37FB5491DBFE2299DEFD903D102D73754FEC22CC5423CDDE6DC45E45CAB004F8F5ACC4394B749D5DF642A67A7020844949D4A3013D6BB085A14A4B9CBEFEEB0565EE747405BD37628415D906C9386FB62102D7AAD82C988AC796FEA2EB0FA5DAABDB8942D55CCAD1A88A2E23E8FBFF2670680E38B03770428DE552010A06CA9E26ED5404400F7192FA9AA916C485A05FD4BDE30BE5F3E5F83C32BBE4F0847F50DBD2E38022F2BA270A468A9E66DD140C651F3192FA7F52E8822A5F0E5D2F11075B5FEC41CDA36ADE6E364B024440E7007543BC2803DAC0CED556C6E3513212615CE19682301A47CD6D461EB04BCDDC2EC9CC0664A0783D696F64DD5B739CB5C45EF3DD9E441CA0EC17862C7AF93418FF83CCE1D47FAD78008449908A878DD6E27A24C2E9D33DE46DB1995EF4CBB6C8CA087B4A87D11DC50BD2D62F5E55D51FD7591BEB5F4B9F03CEE003886F1B1C780A6ADD2BBAB9B884868152C60B006837CCBCE1D13A68B8908689849A8F92EE7F66BB3DF9CFB6FC18B112FEDB7D7EE58D1BC2947C044DB5AC569AE91080EA168CC7311710B3BB1FDAB18F3AC8B0569C5467331237A026A5BC2B7930D8A58BA68097B2D21FCE6DA414F687F80C6A80A0301A5B690427DB614863110A87F59F6BB19F779FAECCF699FCDC6BBBA0CD5614705DD00C36CA8E0764AD1D4D1FD5B925185FCD7D059545D39AA638C4C77D937B134EDC5AF6EAAE26FDD420401FF7DC181FDC95B524766EA70E696C041A630A76DCAA6CA73B6AA8574C2C67D1FCFA8156FC5A4DFAE8F5F2D28753E4F1C11515DD1E488243B1441089ABB161966D5F4B1253DA79F769F83F8819497635B6EA150E642F54310D315A824DC14BBD0BEFC4636873CFC2A5E59812A4C679D42F1923A2C5B73A429CC2AC65051825C6E2CE11DAE62B1E43D39A27E14507FBBBD721D7F3CECEEC435D32EB19BF145F1BC449CF30510D45D2B83C3DD2ECC655AFC77AB5B6769F23588647AED12BBBBB0E711FD2093148AF0342BFDA4202B97E22937BA49411B2AB795157C3B582CB397194C572E9DB2D2F5714D4B13A0DF8B0EB6BECCA5A681D0C84E57BE8650FE65BBF31D1B9FB848F8129BE32BFB4B6F6343D79BA7D0D54770F40EB4D0583C82E13C82751091EC230117945866738A5B5C3DBF2011B559E9938ABEB296CDE2CDF22F494EF7E0400752A1E54B954DB1BB675420EA8A2AD63869B3F9E7C3F681E4640B824655D55A32983E0D55F17DFE7CC8D829670631512CB37839C5B2A0511D055115CBACD643903E29560357624131A9CC5D11C808A40CD6B0C06AFE4852AA00B224260514C15ED4B52C8E44C2946CF2F2AEAEB22365258B1BD9CD2FA9BF06D141BC992D165A60866C1EE3244A1E9EB43D68AAB95881F3439EECA82DDAB297906912AD9FB29CEC545641557BD9E52DBB3CB3AC965D9EE86A7211183CF8CBC70C541D9C660D11A3E7DCB455BACFAD1AB3F5A1174F57F4740F29CB017B9E6524CF00A305955B53BF0E83BB302A6EE8A9BB902B595860E68E33AF0032BB42199E6A3DA22780AA5866A1FB182B018AFCF7C52A2F56D92CABC52AF3DBEAF247F376E45C77D3AC20D9254E828992F6D04026A08CA120579BADB99ED191D7A2881745BC28625807757EC8A3A3EB4F255B3EF0D151412A6787873F435A837E9F30B5F577062A63B8CA14F1EED1E1E8EE68383B180628CDDEA1F06EF6A912DE1691F32443587FC6D3FADB21D8A6419CBFAA9F1340035556B2EFE735A61FA9927D3F3F60FA912AD9F7F37B4C3F52256785551C2C20B456ABDEE2502E0EA559568B43590D1C08F7EF6E6225620E461641432504A9A968CFC00AB335B533DABBD73FEC179AFA3D65BFF0BC0F2AC753FF7B92FEF621CC324AE0227812484B857674AB665771397599B654C142D1EDF651F24448F63E4CC55B1C629903D56B722F5EB56B172DA672319566592DA612322F401E0B4F7653A2DCD5882208A22CAA44476B5EC1DA8BADEDDFD65E27F42BB34C24FD7B983F5E27FF6A8942F895D554D9B9DF0F641B1E76F8AEC1FACEBDBF0F1F1EF17D03B56D74704ED218CF724CFD2EBD1B198F6CD2650C06F6A31A2CDEC9E29D9865B5782790A9F672790EA6D8D51B71B84407B7D77A1FCB653A18DD1BB6DC9A6DA0006EB170DC3D673FBBE3E284A13E6E804E2280F2C5122D96C82CABC51271D0E86277A0B46D082B0337D3E1F76A2B03F7CA4A9BB016B1249BE3571B6D178491A8E48A4F16F622C8B27F25A9F806ADF96AA1C75892301974DC678B9D5900913A7EB5B8EC9CEC98E485CBCDF5478B674A59B97A44ABC27FB7E7FA3B2AAD434AB275186F089BDEFAB0D9902C83A5A1AE6DC759819EACDA9495ECFA299C03BAD4E00EDAA57694E95EFCB7E4002865A9D096EE4318ABA8364536E88BEFC37457EC4EBF24BF9158C4A1546C413B257434F260F9EF36F8666C23DB4F875C043857E086B3B78F2C28CF568FB35625FB95F42BF56EEB730080D39A6A1EFABAFCB60FCBDB38F21CD18DAC749AB7209CFDE551B8CAC01850DC671B5A8A984FAD82C97833652A5C776F86B577F066E0662A9EB2DAA237537FC34B86B5902DF5F1ABCDBDC659457284B2D276F35F6FBA0631C610D179A5FA5F2CE01A767E2F4473E85F55FAC64B3B51B0AC18EA24DCBAC55FD78156B82A4335B030DBC9BA654E58E9855B9572600C697AB71A589587193930D5F6EB56B507530F4C4CE72C0B52CAEA2C566960547D69FE6EB23A5719955BA99E8BB9B1C4CDC59CB22ABBB39862B9ACB23AA163FF1A6E8BF4CA45648017ACC28BF53FA3B75158EC5FEA0AD4A486F7844540A18EC59BD5EB972FFFB03A398FC2207B5324EDB64F1E4DB6BBD32CDB4640EA6806DCFABC41CEA47CF6572281A096C16772CF3794D7E7D9A9D8FA4C0041D3908DE2CD2A645C2856C95F48CC6E4493ED4D90B35F8AA86FB025C57857271F0F5114DCB1DCE0F74194495649EC83CBC85CF6B2A574F39099334B4AC793969250FC3548378F01F57D3E04DFAE49FC903FBE59BD7AF9D29AF0B9989ED533FDE67AB9C06A4B32758266FDE84CC3E335B2168ECDA85D20D99AB22D2C55FCF20DCD962FD449E668A682398A712C55A6253633946BDA233BFB5E4623880B4C288C13977203691617D7B44771B5D2124F4354792A8706500CBB4857ACA1FCDA44D9020340FC482C0614A1103118689AF6880139CCB78EA53F8EB86465DA4DA2610DDD1F7C29024DEA5E1C140C797ACD8090084C05169EF5C37FEC826FFFE94B6C721A54A4470D1ED0211CEAAADD546433B1257B4C22047AC11D94B42EF32D4EE4A6D359B3F0650A0B0C6C6408E5A9C5BAC6709A2B8C675CB75C6465232B453E599CB8340964CD126B355E846623347DA6579CECCC0111CC2284682C92B491E48D2A192B4E8837BA443766F909CD67714203EDB00C9B87D7FE94A53A6B2A5663EA33A462D4A648615971562B4EF3532072D1197E4E43AC3B89C20496DE6B970377D93F763BBCEA460474FE26B3A501B28C22BD61F8BA3CC219AE1B2EAAC14650703E509CACD41940CDE2E2DB2E12B391189CB913794EA0CC3A883821E0DA4E4075BB784DC774A0BE09EB4E4FFC89DD55E4AEE29ED2E2FC71CCDFDCD0ABC1928A7A3362EB9444ED449D487BAE23D45CCEF4408BBB9AE9FDF0D494B812B94C3029CA10AB0626D3E72202FC1E072C77C4B1CEB9775A13F5EBC46EB32AEF5377BCBAC33F9A5393422D88D673B96EB4847772DD88C9AFE3BAD1835EC475A328BE8433FCEE66238D0E44BB68A90E3FCDDBA46E74D259C3FC806F56969604DBC9202BC0914D48FD0C764392FE2BA32A8A1AFC3FD0C96C0246F7B52D6A60D678F2F1D409162D81D1D97E2D966B3CCB2526A6E907C8EAF4347D2D1C212F633FDD14F72FE5048DFD7476A3CFD0D8ABE086ED544CD9D84F2F620AC79E18C89E143CF5DC0798DEB127C82B933CF6D39F32D7633FDD49E9187BC29E3AFF63AFCA4393FDD179E7BDEC7D96BDCFF4F63E8AE04D96AEAD263893857FDBA2F2EC9DDCC531D539A6400EC55E7B925329F664D184D48AFDF422A65AECA7173EFD623F3D2C3673B19993B199C52ED69495D0F235A329D594C5DB4699D4B337A1133C275A34D6A2B1A6AEB19CEF6AEBE879D45D43DCE1C6A94DEBCB0DDA7BE8AE4711AD447EBE7FABE718E1C19A79B062DF8DF5F26471B8BC79CE585366C8EB4C51CA85D799A294F5AE334529BF9DAF35DB4A63B71C1C2AF1BBB814B371290CC9DC70060391B5CD6C324022CFDE684C70CB03A57A7356765272B74E94A4546ECED4C4F8F4DD0995D1F317AB00D35AACC26CAD82213B99838940641EB3B41720C5C5780C6F3C8CE9CA9C1524261B992FE240B22B770381C826E691B85FBEA0F2802D360FA6B5D8BCD9DABC4ED728B079AE2C6DDC729DC2E5D19698326B7447DFDBCE034A84B5A86298D6A28AE7A48AE5A0EB38C50B872637ABD9BA5D39A3431CFEF340C2427DDE874CFDD89FE057D9A50C2CC23DB829934C7910E131CF948F7171A9A63C8CED986DCA03B126E394075A7CCAA992DC5DE860CE8C49A53A6CB954F9A3BAE910396B54777AAD64513EC83559A2BA11035243F98021971BAADBF85A69A1DC51A8CC00D56D749A744F1EB59631A5535708E80306A19C285C880A9CC485D80082BC7134A49800462A681B2DA73FC2D96838AF89D946D7ED3CD9E863CE241FB6D0265C7147BFE8A6739C314C6A219CCFA43D80F4BD2DADB317790280FBE1A9D512717565F5398060A12A52F3E0D6D6ADE705D61A90134D159FB96442F5DE84A5A9B96DEE1C09FCBE8CB7276C727CC6926A442C5BCF0BEEEB874394872C4905EDF6CDEA9594F8E9537C4122929393F30239D46A04D926D8CACC60B99054A328D3F8F023A8BEB47BFF2F89289536616F1B42F6E220CE72B6F597F269DDA4D4B50CF74124CD59A809C2A8A90D65F33A3B6D888B2517644F6226D8D61C3DF4D81016F86BE2452BE7941E3D6D4D700BC407E764C70A5BA22B3E0C821B41F3F3A3108B7A4192CC18854855D92B2DF0A3B372D6BD0E8F21382DCC519475392FC3E6DB77012690430AC96A52ADF60B296DC7C3A30A8C7F7F146955CC8BB2FEF45D400A628F42B0AA7C007D034AD7EF0078AA03EADD6AB3611C252A54E3252A160D82B0262C676B24CDC75E50A5639542CAE63CCE162883439176E879209CDD2A62A83A4AB3474449FDF785242B492AA3C85AA3C7437F4361E64699B68033335C9D968DE1BF3F3BF02839A390E78D2E83433F10327639048AE47077B7509C74CE6BCAB9C3E3D2692ABF0C032120B26A0B5150793FDE93CC25859C15E1E3ED40658A286BDFF748E8022E09596A88670F2D2B233412B0D45D8F842BED39BF874DD7B3479DC3F6CCF4ABC82048C40C621C4C5661596FCDE1C0BDE26138A4D6E1820D036CAAF5A32D5D351826BA7A5704830195BB0E685C34C331A2F9B35A19140A047C6748B501833276F69088D40E624814CE675FA256D8CF6F2F32B75DC84CF61FA34068943DC7CC761BCF629F310ABA26B3B798EDAEA279CC340F33787C0F06428D2B7E4E0651F10ACEBEEB7190357DD33832AC46319276A01AD5523677D3DA41E526ADB21463866F3C8A759E81F252CD6DFA1A4C85B6C9AAB1E9416D4885D6056893D46A33DD094C0F8563EF0EBA207322FB04C5148A28ACB7A8F9F5839061315C06F6450CB3AAD8EF0302073429DAF6836F200CB28F618D8D763108B112E652451E3872611B2C2F5FBC7825E1651E00D53348010128B4F39890C48D67582CCE69AFA353E8CF716F33BF3DCD6CF632234169A4BDCB3CF62C5270E049EB2539A8323F12A0F419E826432069FB9E4741D564F5D3342035A48E7203D4C4B4D44CCF54A681B6B1CF51DC10389113147D78E4F9984F39FAB4128A40D5E766580DB1B8ED87313E12E761722708C3D18CB11B08A76399650C3E07333D41844ECA80BBA1768AD67C1E77A714F1B495A07C5E77A830D1C4EDBB1F0F69F3B0D21380D968567946F7AAE4D881B7409CBCA38C8B425EA2E58741F005C45C14473248741B99410AC9C24100AD70658A33A9EB7942989AA9573715C48DEDBD39A270221E1B9B43A1D5B2DB75A19F95F863FF6D49B8FC3008D6D87F25ED7AFCD80BAE8AE96124C92A76D464CD54B0FDA963AE0E0C9A2F41FA40D44A6B4C9338066886347F56A0D104DEED053497454C5CDA26A72D48DABC6CDD922221C345900777D02D21D66A4D723E62EEEAE4B209B0DB8E44BBDE3C925DF066B5BD4BA890CB08BD455106DC116A53E6A2E94AD4B932A887A6D8DCCB9AA45F4396225BEAA329817AA80ACDF49B609C12FDA604A25F1686A80EAA97D940075509DC012BC474701DDC51B343F17A1F46109F8472A8B35615738F6520401952C5671051B4C44C568C342875205680BA6AD7C1E0AB0C9C09C0AB2C80D1C5CACCC4AFE22D3B8602E9736550174DB1B917F9C69FD4995C05EA53AC65EEBA158A4DEAB5550A7578AC8001FA55FC9564391B5D1937106069BB02CC57BE0E628682032E4F52A800CEB39D5DDE0CCAF2F44EC664F91D84242B3253FE12EE4844CD0940FC5804D1AF4B11CB38A9033BCA0BB82902976E558AEB42415E4D1A41160A5123F702D5023B952B3A8D416D4174959123C2DB19E1B9B57A30088E58F2823BCA5377CB55D2F6DDD4B37071C4B7026A8747ACA9757FDA959DC7A3D2FAFAEA1623435A03E9FAA46E4428F6E0D8025C53917A06EA407D4BD52C7B077E70D30F0568601C97D4C67290BAE5A4AA681C94C5B22A778F52D7E567A8A362A36C245B9EA64864CBCF105956821BADD10B802AA96662EB0D1C37DCE0DCD46CAB4BE52EB82DA7B055E492AB9C70B5F85DA32AFF4AEBE840DA1DD27E5A5FA51DF4B16D6BCF5AB4ABBE884717ED6920A6289C1B97DB08799A5035CD70B9DD4A39DAE2836682309C8AB66291EF2937BB5AE3AC15891BA0E416DCF89B6FD39C7EBDAB33CE1ECE31002461E0065F7F9AC8D46B7F5A4C02004C5E55553D1178B35E4C442CD2B043DC2B94ED9B8F5E58708C4FAF98B82280BDFB40819FCC8556BEA6D6DA612B66A70928AE0AB6CEA392FF3EC6349571BEA1F9E28282B7D731BFD72E9771F9453759F546B19C3B54DE0B2B4AD2384E4057066C2538513688BA13C710FD6F8E3EB4F634D9A50BEF8B631D3A40B06F0698E80967252AB24DB53ED9DB7462C552459853284E6DCBED0227345D76211579FF2A5C8D42DF0B10A7B07B56D5034FD742318FAA9287638B78A757C712EDFD5F7F4B403A0F6A73802BEE830DC615A1BB9BEA6B590CC8027DAC39801116C1E93A63C270E2DD3EC411EBF4C61A1544F081D43AE2649A6C312B579708603DA8D949B24F1F620ACF4C8B50557D310543B7F523918E7855B15FB6CB17122CF96D8A05A40D97C431412E9C176BD1A6630893A1C3AFFFC58C350DBD9B8401A6AD0A8B02CC1C1541A5B3D495BFB916ED81D21E58A0923D26DA4747E94F61FA66F36F17A6A207C33F329B54D1104CAC424551F0BB84941707644E0155FB661B6AA5615EFCFB5C76D36699E5EAB47BAADEF7529D126B110726F847D37E172D7C6AA0AAD2175B508BB3C743943159A27A600AB003F516B53531FED24D318DF2838615EABB424D7BEF3FF39BDF43E29831B0BE198355F2533D803586F77CAD49F057CE8A61971F34D3162F7335AD206C759B62F5B04C3F45E8F559C745E06B8AEC691A6BDF3C816ACACE4ECBEB6ED507FA679EA4C103F9906C4994155FCF4E3F1F68EB1D29FFBA2059F87024714669C6A480D691685DE72ABE4FEAE75FC288EA2A7571C5F40F240FB6411E9CA779781F6C725ABC215916C60FAB935F83E840AB5CEEEEC8F62AFE74C8F7879C4E99ECEEA2D60F6AEC0599AEFFB35369CC679FF6ECAFCCC714E830433A05F229FEF91046DB66DCEF82483CB25091604FD3FE42E26AEBBBCEE9FFC9C35343E96312230955EC6B5ED47D21BB7DC49E377C8AD7C157A21E9B99876D8E9D5D84C1431AECB28AC6B13DFD93C26FBBFBF6A7FF07333859855C920200, '5.0.0.net45')
