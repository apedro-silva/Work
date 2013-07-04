﻿CREATE TABLE [dbo].[Operations] (
    [OperationID] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](100) NOT NULL,
    CONSTRAINT [PK_dbo.Operations] PRIMARY KEY ([OperationID])
)
CREATE TABLE [dbo].[Services] (
    [ServiceID] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](100) NOT NULL,
    CONSTRAINT [PK_dbo.Services] PRIMARY KEY ([ServiceID])
)
ALTER TABLE [dbo].[Audits] ADD [OperationID] [int] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Audits] ADD CONSTRAINT [FK_dbo.Audits_dbo.Operations_OperationID] FOREIGN KEY ([OperationID]) REFERENCES [dbo].[Operations] ([OperationID]) ON DELETE CASCADE
CREATE INDEX [IX_OperationID] ON [dbo].[Audits]([OperationID])
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Audits')
AND col_name(parent_object_id, parent_column_id) = 'Operation';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Audits] DROP CONSTRAINT ' + @var0)
ALTER TABLE [dbo].[Audits] DROP COLUMN [Operation]
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](255) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId])
)
BEGIN TRY
    EXEC sp_MS_marksystemobject 'dbo.__MigrationHistory'
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201304171335158_audits', 0x1F8B0800000000000400ED5DDD6EDC3896BE5F60DFC1A8ABDD05264ED2D3C04CC399813B7626C638892795EE01E6C690AB685B68955423A932F1BEDA5EEC23ED2B2CA9BFA2C843F290A2FE1CDD74C72279481E7E3C3F2CF29CFFFB9FFF3DFBF3B75D74F295A45998C46F56AF5EBC5C9D9078936CC3F8E1CDEA90DFFFEE0FAB3FFFE9DFFFEDEC72BBFB76F26B5DEF07568FB68CB337ABC73CDFFF747A9A6D1EC92EC85EECC24D9A64C97DFE6293EC4E836D72FAFAE5CB3F9EBE7A754A288915A5757272F6F910E7E18E147FD03FDF26F186ECF343107D48B624CAAAEFB4645D503DF918EC48B60F36E4CD6AFDE1EA1F97972FCA8AAB93F3280CE820D624BA5F9DEC7FFFD32F1959E769123FACF7411E06D197A73DA1E5F74194916AC83FED7F8F1DF5CBD76CD4A7411C27392597C44EB35E35F3A133BAA433CF9FD8B08A59BD599D1FB661CE57A195FE4A9E5A1FE8A79B34D993347FFA4CEEF9865717AB93D376E353B175D3566CC846F1667515E73FBC5E9D7C3C44517017918659949BEB3C49C95F484CD22027DB9B20CF494A97FF6A4B8A59485D0B1D7DA16B9CE5C16E5F777541C9B08F406F7A4A745953F6AF9A105D638AD0D5C9BBF01BD95E93F8217F6C287D08BED55F5EBDA438FD250E29A069A33C3DD8F77CBEDF47E1A658FC9BE06184017CDA33EED3EECD2BA627F499648728B79F80FD0C3E065FC38792658AB9AC4E3E93A8F857F618EECBFDFBA2C0E52D57E75D9AEC3E27510DD963D1ED3A39A41BB618095CFE25481F48DE1ED8D9E971EF6977E471040EBBB2B55CB63BD362ADBBEFCE0B926DD2705FB2BA5F54A359BF26E9D7902EAC03E3ABA62E6CE79A7E8F4C7F9BECF641FCE4C2F4AAA90BD3B9A67D33BDEA6A1C05D2D78A977FA326FECBE76BCBAE5F5B766D81356AFAA58E582B9ABA61AD69DA37D6D6BB208ADCD7FCF58F53C51BAAF3ABF5A7B7B4B565C73FF424D8AE833B6A27D021DE8791934E691170019E4460E2F01B5BDEF5A6E3CE5312383959B49D938F55B59BF87ACF5CDCBC279B3C4849A6E473074DF5EE106FD8AC82C8153B6D0A2E2892292C781AC52BD950AEBA3925ACA59B4F52B75C967C8425BF8AB7ECD4C76DD59BC62E0BDF6ABCACFD086B7F41BE9228D9EF280B6F1E83CCC9721469B82001A2B100620440DC90344CE8AE64FC73C002D7DC050642F36774420539EAD6CEE3EBDE34C05792E56CF3B18F6E6A80A7E0A60B440ACBFE1F63FFA7C9F65018E294BCD36F756D0A4E5240A2305D41A03F3DC4FDEC86F27750A79F5DC9602D71EFAEAAEE97B4C227857E446BE3E4B6AC77FC210D28967E4C83EA403FA8E9C657FFA6801862535539CAAA8669A07535DBB156EE1D62A8754DE548CB0AA68156B53AFD48B9CEE9A67672495943278FB46EB8E8A0117410BB361185B1D392D76D5D569D6FBB2CFC080BFF2E4977AE1668DDD6E9EC916B3B5D53C383CFC1263ACED51EDBE35DAC42138EAE21C556AFEEAD58F7A8DA145524E5A6AAD749BD31A2AE887745FB0C44DC8F93BD8E80DE6A3899A2A7637524627034A220DD7D0EE207C2EE18DA59E74A521F82980A145FD42EBF91CD210FBF124FBE43A3515482E1F658A32D0E9A0250081C4B6D2D70FE804D39A85625615C5C193C34BE4237C114C641BC0983E8F2DB3E49736739259371125B3099DEA518CE15300B83CE82C0F2540423563E1E76779ABD8B9E1BBBECDCF9E2F3FA70B70B731329943CD9EFD3E46B10F9A0C544E579443FF82056094B6FF41A71E98D62B9088E77D0E93F3BDEE2AB576EB801A88F4A8A53075044CB92E8B6AACDC96A552559682B6BDAEA96424263475C56D60FB81C0B62BCC57FAC5561FBAC193B70B1997E0AE2791462324213EB5590499697324376248A9B64758BF31652C1FA096B5A3633D14C5ED7BCE69D2F63A2C3055935354FA6C580D7689D0C1B935E6094CE776C128D46209B907A64AB939B94FEAB7A04F787D5C97A1330BAD6463B9A431DF6CDB8DB05212B30BB0DFF3B42B91D2CE7DC3443CDB3FEBFC5DCEAFFFBF1253A7B119DFD87C5739888E7B0A68227FB487A9653D58BA70B12513B397DEABDBFB749967F49F220EAB79BE2FDDCA63882643DF6DB5985A0F682FD7CD8527940B63D2FDCB09DFE7CC8C2986459BFFCFC9206614C1D9A9E911F4441FAD4731F49A534489C919EB9F6297F2429153F59129302F43DF77711523A79F9AE6580EE8E37A07E0DA203E90D7B64F3182751F2F034508747F5737EC8931DD3A56F136A5124D1FA29CBC9AED3D1F172FAB49C3E7DA7A74F867327CC8953AF674DFA5326C4F9D230274BD83325ABD324EC3992BDFFB4BEF8C7E5656727AAA1D2C9936A51F97EDCA9C501D23B408734A54B7A9E6524CF7AB52CAA9EAEC3E02E8CC23C24FD76577875CC10ECB5977A364FBDF652AC4EAF3D2C96D962997DA79659A31735E69950476D4A8815FD1B6AC71E94D65ABB0A66B04ABBCD3E3655105D53FDC5CE175CED1E05A94E71AB6452DF8F0534D103E545E52C2AE7FB52390A6924EB1D6D45499EEB6B7BD340AA6E4435A4AB871E7B5F0709AAFED4470AB816E889F93C6650F4E11C1B4047CFA3F61D2C6680B3F637DD5447873FB03B716788C80CAAD54E16A9AC328BAD514CF15649C8B851B4EDB1DB464FC4564E487134ACD9215340F2416C68C700A9B52FF1E1C154F760A27F7FA6B937639ADA3DDB350B44DE4914FDED106CD320CE5FD58FC25D849182E6EB1E68FED003CDDF7BA42988F6E248D203D9C5735A3CA7EFD673327A4C384FA9670FC9E419A13C224F4773EF0FBB20FE4CB2A223574D2F1171D1F52091EF47DB4FF420AEBEDF56A8A7F7746D2C63C108E4FE9EA4BF7D08B38C52BC089EBAD3AAE85CC5E58C3BD1BBDCEDA3E48990EC7D98124FA4AEC97DBEA87335AD459D2FEA5C5094921E9015BAA28AA42E55F5BC2975B90351ADC3351023EDEB9053EE497DBC69AA8B9886CF23CD1675EA9245E17D95E4C88BE12251EC6CC580141793666493E63AD90411331E48FAF7307FBC4EFED55AA74EEA5AA0FD816CC3C3AE37F2EFC387477FC42FBF31A0F5C61A99BC6FEEC83DF865D0628B2DB6D8776C8B49DACC609829EAEB8D0655A37E4C36B937ADFD0657B79DD020969DDC2DD2CC3335B49D6D6F0660A7EBF330A5CE06DF728DBE1BA5F30D13F1CDE9C944CE603C1E0C15C768F5995AF7E3B4C522592C92EFD822D1DCCED6D5D3ABB0DE6E692B7AD15A1C86DBDABABA9D346D1156D241AFB276575B7B2D5AB72B11F79743B875B9E5409D3FCFB0C5C5E8D9056134F876BD09B2EC5F49BA1D7EBEEFC234CB471151D7C1481DBF4D76EC62D5E0FD5E65A5543E9A1D3F2774E707B1BD9156C1E51DC5EA2125D93A8C3784F1737DD86C48A6B64370676A9490D0830F8DC8C8163621954EBEE85D279BDF928317855D927B08631FC4DE26F17D98EE0AADF125F98DD8463AF600F294D0B1FB98CB55C6B84CB69F0E7957DCF2C87AFBC8220B6F7D8CB026F92BF55D6A7F751CAE2B4772F96D1F96B73DFCC0CB945706E5706083BFE35022849C063082A323059BC652525A71CCBE82633CB2925B666C64BCD576FC2AC566E48AA0B88BBA51B05688931FB9DA6D1917FC383E451569B0AA7A9D224632D22ED6246BE7624DD6ED3A58938C84774BA3E758F73E3DB00AE02EF8179D14606B38FB24C26670F450E45F265DFC15DBDF37BB9FF8155ED245275C77FC51568B17DFB24904924986793C4B07BA529F9F1B2B6366D2E19CFC3CCB58EC36D6B63ECA396C298DE6B6A8B04F2EE3ED49390EA1DE719CA5D8619C28AA50317388F2701F15B908DEACFE4B62B69A66A3618E348F0313E8BE5A891BF8537C4122929393F38237D48E09B24DB095914979B36D7FA17B9EB0A022210BF51167393B75CD650141FDA0701F44FAD10BCD40C9D23492050A1B5FD393587241F624661240BF281E86D0F424B0CEC4A9B3530E627AE4093006B2D01F9102D48510286E3C3C14A10E0038963984A68644CDE03150A8EDF30E40D4AC4FB7110C8FC33A5D23122952EEC61ED028267EE4FA68924B4E1C93C21430A0E0DC507FC81496ABF33886C76795A313091D3161670FE814B27D725DD4E944278ECDF604309038A6CCF587CCF64A751DC500B8643F1DB1C51653ECA960A3A80F61B3AE6A834A15790097624EC0A9E1D330150C36E4D3B70E4835AC9CAFF10C84D963F6371D948054706D7CDA6253CE1E8700FC34B0280D1DB5E2892A37AB35F6A4B5E8DEFF5058BBE192FA69D1710365F8EB88B81B2029206F01F0090727893B600298A5BF69A7CFEC8A3E6069BC8C62080C2AB3962981634E61C6A1124ABD630152631234DE982CEFED4C0EA7A639A02C393867BD1D544D0BD7712023A115B8B6AB0750FBF653AF586D5D9E3208EB6922959F0156A9F680537ECDBA0D6324946A7F6CD2834875FEDF2B72153F21A08F07A689667856285D6DF899AF33C2E155F635B47150AF4BC067014654363EED6E68B2E175DA14A8249A56BB7292BB04314D9408C624F8EEBA6D10D0F03AD671F7119C1A15855929C3E380FB454C15D9FA7502EC68D2FB42980EEE170245165A5FF81796B7F39886C4B98DCF89F4369D2DA0797B9813F02D67ED555AF893384FB21B0E67EA3D8EEE37CED86374F2156DBDC46EA87C469EE1247DC267E60D8AEF2A8D20533EB004907C7CB7EF0267D5EBCC19697CC5140657FB8A459B91EE171ED15A80C76805F842E94CED017802431B05F07A4DDE32686EE182A95C54E0D1B682B0AA686003587D9FF391ADA8790C2560510B397D29AB9A8656D4EA1A0D8CE179495ECC2C0612BF98359CAD0C46FA69B8E603237AFE5E9CDDCC06F7E7EC567D469E9D6262FAF44D96F044E67232EE942A6752E7ED824B0B65BF7327BB8B5013C6805641A29FCD85828DF7518FBDE7A41C614EE056270C1B789729738F710391D3A2B53B7FF9E2C52BA97FFFFB4335540CC4A0E47E7DED08D5DAFA1BE7B07BC0CE03467BBE1D6CABB97BBA93F07067EED95A79B4584FB62B2667EBB94EC0639D9FA7AACA77A2428B31F9C9113072722F0B549A92A74C5F561A663094B4342CD8F4E5A522CB0D1E382A99E9199FF3929BFAF10F2439F56B3543D9893CDFC36736EA0DADF33FCDC3CE69F0733CECEACEE8040F958903053E635A0E05E2E5845BAEF037E5F89899656198CE286686619167667328D2B438820D658DF489F619DB29FAC98C61B4E8D7775E168C31698F23E02C6D9B3EB1FFCCAC1EEC04C73581B08898AB3D84BDAF8A4B0AA2D8154E37025199456666EF8C7C7F15B38833B36F90F7585199610640EF8CED9751EFB522D66FF2F60A9B4915FD5B859A63150895ACD406831C3500726528692D358BC9A96255ABC6660C5CDD1EA87B704663F47E3D47C6DF84860960600FA796B2DA8086F5428F62ACCD678E4A6E0120B353E01DB3F337FED1931ADCE047AFF0648DFCCB22B83D6D93D31624ADCDB45DF8DF84B0AFE49B8C6FD6624D723EEC7DB63AB96CC2E4B783D84B586D376F7E4686487031EB0D64D624FD1A6E0844A42A329228A31387208D2638B491068B15A0A251857030D0B80EEE2868281AEEC30824D4AA6024C7A28482CB53046C35346E071B85C88811608DEBC422EDC2CB5486383610B88AB7CCA7866934854632E26D2D889A7C79CE40F418A71046402B4CA4719E5F4996B3BE190178B27C0DF3E05AE2051C9EA0074C4BC97C3970254B7FDAD09CE5D38AA8CC8128D465667C266568521099491D0916414445C0DC588E3502D282621CD993D6C9175D781A6C4FC6E1DB0DBCF1B7B434393719AB2CDA9737B5AA43BC58EED6855248E8AFFAE23B43CC03357EE967588828F0ABBA0D59E968D3D80770BA6DD3A11647AA2317430755362D895CE9CB191A33F30F6A5C3ADA889ECDD218720404C29C4D27D8635C1AA213AE166F99A93215B5FC0955AEA2664A47FB4F3256D5546A1F84A3C29B81A217D19E268205820D5E1A26321BA06AEA4900B52176480BABE10B4412E04D657AF9664B63D31A39032672D1CD444CE5E2873F62F2168E2A67BBFB66536D971AB904A513D14D474828E28747420A118E6863797766506D5889E92A0016A9AAAAE7A36801B18933FE340C5211045824793B5E5875CC92A06090228D82340B399182C00C3323E4D409187EBA4EBCE5EF28E67EA30AEB2F8F1D881EEFC00120127E6BEFB57DB8EE8C50C68D8738820B32DF9E9431CC3CCF23D05BD1B1CC18589E17319547D80BD34AA30EC733F9173DE3A45A3FE8F9E658EBC73B13643DF14BB41D719CD31D6D1BA7A938DDF6CD4DC589365E81FAE1B02E68338EDBE8B0CF269660023FEB57813B58B05B0D4CAC674B00F85D9EE60CC46A49C008C2585E8831847B66BD1836B865172B6877E1315287D9692FA4DEB2901843EB2A9C96B2D24F38CD64CB93C1B491AD1E72D440B6BAC7965FA3EA1BF19A9D8E7FDA2B79F0045517F220BEF1A77018E6A9AEDEF5BA0B856B5D387661F7A3E2FE975F66F5BD3DF591F200865984D66BCD0D175C0F3A04948EF7351CC485D3EB0373DA606D167C34A00F15DEAD2F2E8E0546B3D6700936869AB2599378E5F018DAC52934157E0D2C425C6158840B72655E9FFA373CFB45C285B5724088EFB593AF2C582E9A3E46129E57CA2849FD2F93322E12D73570FDC3E7C2A0F5A9AD1E45EB4F2B7934BCBEC4EA494BFD88D58BF6DCE95B0FAA029200EC41C52E694DC614BD849B0F747D40C32653BC923E60A4888C81E294014A86281A3EF9343CA0CC26955D6407C304CD6654671E8E613AA1620298788B8A25A09EBC299A808AC7D0BD1D2CC34DF1037ADFE98AF7E8F6ACB691018687ECC3307A5051617C0E6DCF703721827E513DCC228C2E6B100781F8F7B9EA99230E049557F3B0DC1DF26050F7E613CF421B99613E24F4C8C0BE6503F7F410E096EA61626BE4C0D3446EA0D54D4ACDCC81C7885CFB6A6C9D27AA7A8608CC1AF562B13505D39B45613E36F7BF4CAF148761955947583EA333CDD1AC0B3C30B24F99CF1EC73132CDD3ABA6ECEC74BD7924BBA0FA70764AAB6CC83EA72AE943B2255156177C08F6FB307EC88E2DAB2F27EB7DB0A1537AFBBBF5EAE4DB2E8AB337ABC73CDFFF747A9A15A4B317BB70932659729FBFD824BBD3609B9CBE7EF9F28FA7AF5E9DEE4A1AA79BD69E161F8A353DE5491A3C10A194FDF0BC25EFC234CB2F823CB82B4EA2DE6E775235D443B3BAAFD67B337921EBFBD37575F6EFEAA5CA872B2A2F5F94DCABDEA509ED8FCC7B47E7C3CE858AA911F1A2B3DC8EB65C6F822848EB177DFC38D92BBEB74974D8C5C24711886A3AEC2D4C9607BB7D9B12F7194F8B619CFDAB4DEAF8154FE97CCF1E8816B8BEA1EB2FCC522CC4D36DCE5D44CEB50AF0F4A8C63C44799B54FD4DA672762A804044D9A9043361BF8BA045419ABBFCEE0E6BE5757E04B4356D875AA80B926DD2705F0CA245AF553099256BDE9BBA2F58FD2ED57EB9942D55CCAD1A884BC57D7EB60B757C19E0BE528AF70F889552B65471B76A20AE14F719BF52552359E0B70AFA5F79C3F87EF97C0D0EAFF83E211CD537E9BAE008BC5488C291A2A59AB74DD2FA366B95B9ECD5B4D6BB208A948B2F978E87A8ABF5276678B669351F278325E185BF3BA0DA9100EC616568AF6273AB990831A970CE401B0920E5F3A30E2E0EF0C60AE3E180CD942E00AD2DF937D5B739AFB98ADE7BB2C983941D56F1C48E5F27831EF1199B3B8EF4AFF61088321150F1BADD4E44995C3A67BC8DE6C194EF41BB3830D0835794FF023754BB2FACBEECBDD45F97D5B75E7D2E8C8E3B008EE176EC31A069ABB4EEEA2622125A050B18ACC120DF8673C784E90221021A66126ABECB39F8DAEC37E7E85BF062C44BFB8DB43B56346FBF1130D1B656719A6B248243281AF35C44746127E6BF8AB1C9BA6890560C331735A227A0D6257C3B59A188A58B94B09712C26FA31DE484F687628CA83010504A0B2924674B60180376FA5FCB7E9D719FA7CFFE8CF6D938DED5A5A50E1E1574530BE350C1ED944B5347E16FAD8C2A34BF86CE22EACA511D6359BAAF7D13F3D27EF9D54D55FCAD5B8820E0BF2F38B03F794BEA084A1DCEDC12381814E6B44DD95479CE56B5904ED8B8EFE329B5E24D9774BBE4F8D58252E7F3C41111D5154D8E48B2431184A0B94B9161764D1F2EE939FDB4FB1CC40FA4BCC4DA320B853217AA1F8298EE4025E1A6D885F6E537B239E4E157F1521954613AFB148A6BD461DB9A2342617631868A12E4726309EF70158B2DEFC910F52380FAF3F6CA7DFCF1B0BB13F74CBBC46EC617C5331071CE1740F075ED1A1CEE76612ED3E2BF5BDD0B4D93AF4124D36B97D8DD593D8FE80799A45084A759C9270559B9144FB9914D0ADA50B9ED5AC1B778C532FB3583E9CAA55316BA3EAE696902E97B91C1D697B9D4341012D9E9CAD710C2BF6C77BE63E31337095F62737C657FE96D6CE87AB314BADA088ED681161A8B45309C45B00E22927D24E08612CB6C4E718B2BE21724A23A2B7D52D157D6B2D9BC59FE25C9A90F0E7420155ABE28D914DE3DA302515754B1C6499BCD3F1FB60F24275B1034AAAAD62B83E9D35015DFE7CF878C9D72661013C5328B174E2C5B1995511055B1CC6A3F04E993623770251614934ADD15018740CA600D0BACE68F24A502204B62524011EC455DCBE248244CC9262FEFEA2A3B5256B2B891DDFC92FA6B101DC49BD962A10566C8E6314EA2E4E149DB83A69A8B16383FE4C98EEAA22D7BB19826D1FA29CBC94EA51554B5172F6FF1F2CC6BB57879A2A9C9454AF0602F1F334575309A35448C9673D356693EB76ACCD6865E2C5DD1D23DA42C57EB7996913C039416546E4DFD3A0CEEC2A8B8A1A7EE42AE64A1819939CEAC0248ED0A6578AAF5889E00AA629985EC63AC0428F2DF17ADBC6865F35A2D5A9977ABCB1FCDDB116EDD55B38264977806264ADA4303998032D6815C6DB6EA7A46475E8B205E04F122886119D4F9218F8EAE3F916CF9C0474705299C1D1EFE0CA90DFA7DC2D496DF19288CE12A53C4BB4783A3BBA1E16C6018A0347B83C2BBDAA742785B44B8931461FD194FEB6F87609B0671FEAA7E4E000D5459C9BE9FD7987EA44AF6FDFC80E947AA64DFCFEF31FD48959C055671B080905AAD7A8B41B91894E6B55A0CCA6AE040587E77152B117350B2081AAA45909A8AFA0CAC305B553B23DFBDFE61BF90D4EF29FB85E77D50399EFADF93F4B70F6196510217C193405A2AB4A35B35BB8ACBA9CBB4A50A16826EB78F922742B2F7612ADEE210CB1CA85E937BF1AA5DBB6851958BAA34AFD5A22A21F502E49BF0A43725CA5D952882204AA34A74B4EA15ACBDE8DAFE75ED7542BF32CD44D2BF87F9E375F2AFD65208BFB29A2A3BF7FB816CC3C30EDF3558DFB9F7F7E1C323BE6FA0B68D0CCE491AE3598EA9DFA57723E3914DBA8CC1C07E5483C53A59AC13F35A2DD609A4AABD5C9E832976B5461C2ED1C1EDB5D6C772990E46F7866DB7C60D14C02D168EEB73F6E31D17270CF57103741201942F9A68D144E6B55A3411078D2E7A074AAF86D03270331D7EAFB63270AFACA4096B114B6B73FC6A23ED823012855CF1C9425F0459F6AF2415DFA0355F2DE4184BE625838EFB6CE1990510A9E3578BCBCEC98EADBC70B9B9FE68F14C292B778FA855F8EFF65C7F4757EB90926C1DC61BC2A6B73E6C3624CBE0D550D7B6E3AC404F166DCA4A76FD14C601DD6A7007ED523BCAD417FF2D390042592AB4A5FB10C62AAA4D910DFAE2FB30DD15DEE997E437128B38948A2D68A7848E461E2CFFDD06DF8C6D64FBE9908B00E70ADC70F6F69105E5D9EA71D6AA64BF937EA5D66D7D0E00705A53CD435F97DFF661791B479E23BA91954CF31684B3BF3C0A571918038AFB6C434B11F3A95530196BA64C59EB6ECDB0F60ED60CDC4CC553565BB466EA6FF895612D644D7DFC6A73AF7156911CA1ECB1DDECD79BAE418C31447456A9FE170BB8869DDD0BD11CFA5795BEF1D24EE82B0B863A59B66EF3D775A01DAECA240D6CCC76526D99135672E156251C18439ADEAD0656E54B460E4CE57EDDAA7C30F5C0C4B4CBF2424AD997C52A0D8CAA2FCDDF4DF6E52AF3712B257331379660B998535665611653219755562774EC5FC36D9106B9880CF0825578B1FE67F4360A0BFFA5AE40556A784F5804146A58BC59BD7EF9F20FAB93F3280CB23745726DFB24CF64BB3BCDB26D04A47866C0ADCF1BE48CC7677F251208EA35F84CEEF986F2FE3C3B155B9F0920681AB251BC59858C0BC52EF90B89D98D68B2BD0972F64B11B50DB6A418EFEAE4E3218A823B96C3FB3E8832492B897D709993CB5EB6946E1E32756649E978D252128ABF06E9E631A0B6CF87E0DB35891FF2C737AB572F5F5A133E1713287BA6DF5C2F17586D49A64EA4AC1F9D6978BC44D6C2B119B50B245B53B685A58A5FBEA1D9B2853AAD399AA9602E611C4B95E983CD0CE59A3E2F7682097F71EC543A786676724D7B64672B6DB06789E4B854792A3FDD570CBB4827ACA1FCDA44D90203407C472C0614A10A3118689AF68801390CB78EA53F8EB86565DA4D22600DDD1F7C09024D6A5D1C140C7974CD8090084C05169EE5C37FEC826FFFE96BD9E434A5488B173C404318BC55BBA9ACCDC4B6EC31C90F68A57610D2BACCB4B825379D9E9A175FA6B0C0C0660DA13CB258D3154E4385B15CEB96CB5AD9AC9522DF2B6EB934095ECD2BD66ABC2C9ACDA2E933B1E2D6CE1CB0C0BC84108D65256D56F246952C15B78837BA4434E6F5139ACFC2E5873C2C83F3F0DA9FB0546735C54A4C7D06538CD814292C3BCE6AC7697EAA436E3AC3CF5D887D275198C0D67BED72202EDBC76E8757DD8880C6DF645C1A200B28D21A86AFB3238CE1BAE1221A6C160ACED7895B2B75864EF372F16D9715B3593138B326F29C409915107142C0B59D80E876B19A8EE93A7D13D69D9EF85B76D725775DEE296DCE1F07FB21070D2560375852513B23B64649D44EA489D4E73A42CDE5490FB4B8AB93DE0F4F4D892591DB0493420CB16B60327D6E22C0EE71C072471CEB8C7BA73D51BF1EEC36ABF2BE73C7AB35FCA3363529D486683D67EB464B78C7D68D98FC7AAD1B3DE8C55A378AE24B35C3EF6E36ABD181681729D5E1A7799BD48A4E326B981FF0CDC2D292603B59630538B209A99DC16E30D27F6554445185FF073A994DC0E8BEB6450DCC1A4F369E3A01A225303AEBAF45738DA7B9C4C431FD00599D3EA6AF8D23E44DECA79BE27EA49C40B19FCE6EF419147B5DB8613B15532AF6D38B9862B12706B22BFF4F3DF701A65FEC09F2CA248CFDF4A7CCC5D84F7752BAC49EB0A7CECFD8ABF0D0646774F6BC17DF67F17DA6E7FB28822B599AB69AE04916F66D8BCAB3377217C354679802390E7BED494E75D8934613521FF6D38B980AB19F5EF8F488FDF4B0E8CC45674E4667165EAC296BA0E56B43532A288BB78732A967AF4227784EB448AC45624D5D6239DFD5D6D1F328BB86B8C38D139BD6971BB4F7D05D8F225A89F67CFF56CF31C28336F3A0C5BE1BEDE549E37079ED9CB1A6CC60D799A294ABAE3345292B5D678A52FE395F7BB695666E393854E27731296663521892ADE1140622AB9A596580449EBDD298A0CB03A56273167652F2B54E94A4546BCED4C4F8F1DD0995D1ED17AD00D35AB4C26CB582217B98838A406406B3D41720C545790CAF3C8CE9C49C0524265B982FE240322A770581C8F6E591B85FBEA0F2742D3A0FA6B5E8BCD9EABC4ED728B079A82C75DC729DC2E5D19698D26A7443DF9BE70125AA5A44314C6B11C57312C57250749CE08543879BC56CDDAE9CD1210EFF792061213EEF43267EEC4FF0ABEC4F0616E11EDC9449A03C2CE1310F948F7171A9A03C8CED980DCA03B1262394075A7C4AA892DC5DE8A0CE8C499F3AB85CAAFC4EDD64889CD5A93BBD5632271FE49A2C4EDD8801A99B7CC090CBDDD46D7CADB44DEE28546668EA363A4D3A268F52CB9872A92B04F401835046142E44056EC585D800C27AE3684831018C54D03A5A4E4F84D3D170DE11B38EAEDB79D2D1C79C463E74A14DB8E28E76D14DE7386398D43F389B497B00E9DB2DADB30B790280FBE1A9D516713565F5397AE04555A4CEC1EDAD5BCF1BAC3520279A2A3E73C97E6ADF84A591B96DEE1C09FCBE8CB7276C727C46916A442C9BCE0BEEEB874394872C170BEDF6CDEA959498E9537C4122929393F30239546B04D926D8CACC60B98A54A328D3ECF023A8BEB47BFF2F89285D6DC2DE3684ECC5419CE5CCF597F25DDDA4D4B40CF74124CD59A809C2A8A90D65DB3A3B6D888B2517644F62B6B0AD397AE8B1212CF0D7C48B564E283D7ADA92E016880FCEAD1D2B6C2D5DF16110DC08929F1F8558D40B9264C6289654955DD2023F3A2D67DDEBF01882D3C21C97B22EE7D7B0F9F65D8009E490626535A950FB8594B6E3E15105C6BF3F2E6955CC2F65FDE9BB8014C41EC5C2AAF201F40D285DBF03E0A90EA877ABCD86715C51A11ABFA262D120086BC272B646D27CEC05553A562956D99C67D902657028D20E3D0F84B35B450C55C7D5EC115152FF7D21C96A25955164ADD1E3A1BFA13073A34C5BC0A919AE4E4BC7F0DF9F1D78949C51ACE78D2E83433F10327639048AE47077B7509C74CE6ACAB9C3E3D2682ABF0C032120B26A0B5150793FD693CC25C53A2BC2C7DB81CA1451D6BEEF91D0055C12B29410CF1E5A564A682460A9BB1E0957DA737E0F4ED7B3479D837B66FA556410246206310E26ABB0ACB7E670E05EF1301C52EB70C1860136D5FA9196AE120C135DBD2B82C180CA5D07342E9AE118D1FC59AD0C0A0502BE33A4DA8041193B7B48446A0731240AE7E397A805F6F3F345E6E685CCC4FF180542A3F81C33F3369E859F310ABA26E35BCCD6AB681E33CD430D1EDF838150E38A9F934254BC82B3EF7A1C644D5F358E0CAB5194A41DA846D594CDDDB47650B9498B2CC598E11B8F629D6720BC54739BBE0453A16DB2626C7A501B52A07501DA24A5DA4C3D81E9A1706CEFA00B3227E22728A6504461BD45CDAF1F840C8BE132B02F629855C57E1F1038A049D1B61F7C0361907D0C6B6CB48B41889530972AF2C0910BDB6079F9E2C52B092FF300A89E410A0840A19DC784246E3CC362714EBE8E4EA03F47DF667E3ECD6C7C9991A03492EF320F9F450A0E3C69B9240755E64702943E03D96408246DDFF328A89AAC7C9A06A48694516E809A98949AE999CA34D036F6398A1B02277282A20F8F3C1FF529479F564211A8FADC14AB2116B7FD30C647E23C54EE0461389A327603E17434B38CC1E7A0A62788D049297037D44E519BCFE3EE94229EB61294CFEB0E15269AB87DF7E3216D1E5A7A02301B4D2BCFE85E951C3BF0168893775CE3A2905FD1F2C320F802622E8A231924BA8DCC20C5CAC24100AD70658A33A9EB7942989AA9553715C48D6DBD39A27022161B9B4321D5B2DB75219F95F863FF6DAD70F96110ACB1FF4AD2F5F8B1175C15D3C3AC24ABD851923553C1F6A78EB93A3068BE04E903510BAD3155E218A01952FD5981461378B717D05C163171699B9CB62069F3B2754B8A840C17411EDC41B78458AB35C9F988B9AB93CB26C06E3B12ED7AF34876C19BD5F62EA18B5C46E82D8A32E08E509B32174D57A2CE95413D34C5E65ED624FD1AB214D9521F4D09D4435568A6DF04E394E8372510FDB230447550BDCC063AA84AE00E5821A683EBE08EAA1D8AD7FB3082F82494439DB5AA987B2C0301CA902A3E8388A22566B262A441A903B102D455BB0E065F65E04C005E65018C2E5666267E156FD93114489F2B83BA688ACDBDC837FEA4CEE42A509F622D73D7AD506C52AFAD52A8C363050CD0AFE2AF24CBD9E8CAB881004BDB1560BEF2751033140C7079924205709EEDECF2665096A7773226CBEF2024599199F297704722AA4E00E2C722887E5D8AD8C6491DD851DEC04D11B875AB525C170AF26AD208B250881AB917A816D8A95CD1690C6A0DA2AB8C1C115ECF08CFADD5834170C49217DC519EBA5BAE92B6EFA69E858923BE15501B3C624DADF9D3AEEC3C1E95D4D757B71819521B48D727752342B107C716E09A8AD4335007EA5BAA66D93BF0839B7E284003E3B8A4369683D46D275545E3A02CB655E93D4A5D979FA18E0A47D948B63C4D91C8969F21B2AC04375AA315005552CDC4D61A383ADCE0DCD46CAB4BE52E3897537015B9E42A275C2DDE6B54E55F691D1D48DE21EDA7F555F2A08F6D5B3E6BD1AEFA221E5DB4A78198A2706E5CBA11F234A16A9AE172DE4A39DAE2836682309C8AB66291EF29375EAD71D68AC40D50720B6EFCCDB7694EBFF6EA8CB387730C004918B8C1D79F2632F5DA9E16930000935755554F0476D68B8988451A7688BE42D9BEF9E88505C7F8F48A892B02D8BB0F14F8C95C68E56B6A2D0F5B313B4D407155B0751E95FCF731A6A98CF30DCD171714BCBD8F795FBBDCC6E517DD64D58E623977A8BC175694A4719C80AE0CD8AEE044D920CA4E1C43F4BF39FA90DAD364972EBC2F8E75E800C1BE1960A2279C95A8C836D5FA646FD389154B15614EA138B52DB30B9CD074D98514E4FD8B70350A7D6F409CC0EE59540F3C5D0BC13CAA481E8E2DE29D5E1D4BB4F77FFD6D01E93CA8CD01AEB80F36187784EE6EAAAF6D31200BF4B1E600465804A7EB8C09C38977FB1047ACD31B6B5410C10752EB889369B2C52C5C5D2280F5206627C93E7D88293C332D4255F5C5140CDDD68F443AE255C57ED92E5F48B0E4B7291690365C12C704B9705EAC45AB8E2154860EBFFE37335635F4AE120698B62A2C0A3073540495CEABAEFCCDB5680F94F6C002D5DA63A27D745CFD294CDFACFEEDC254F4A0F84766932A1A828955A8280A7EB790F2E280CC29A06ADF6C43ED34CC8B7F9FDB6EDA2CB3DC9D764FD5FBDEAA53622DE2C004FF68DAEFA6854F0D5455FA620B6A73F6788832264B540F4C0176A0DEA2B626C65FBA29A6517ED0B0427D57A869EFFD677EF37B481C3306963763B04A7EAA07B0C6F09EAF3509FECA5931ECF28366DAE265AEA61584AD6E53AC1E96E9A708BD3EEBB8097C4D913D4D63ED9B27504DD9D96979DDADFA40FFCC933478201F922D89B2E2EBD9E9E7036DBD23E55F17240B1F8E24CE28CD9814D03A12ADEB5CC5F749FDFC4B18515DA52EAE98FE81E4C136C883F3340FEF834D4E8B3724CBC2F86175F26B101D6895CBDD1DD95EC59F0EF9FE90D32993DD5DD4FA418DBD20D3F57F762A8DF9ECD39EFD95F998021D6648A7403EC53F1FC268DB8CFB5D108947162A12EC69DA5F485CB9BEEB9CFE9F3C3C35943E26319250C5BEE645DD17B2DB47EC79C3A7781D7C25EAB19979D8E6D8D945183CA4C12EAB681CDBD33F29FCB6BB6F7FFA7F18DBE83023910200, '5.0.0.net45')