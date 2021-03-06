﻿CREATE TABLE [dbo].[Account] (
    [ID] [int] NOT NULL IDENTITY,
    [EthereumAddress] [varchar](8000),
    [Login] [varchar](25) NOT NULL,
    [Password] [varchar](50) NOT NULL,
    [Created] [datetime] DEFAULT GETDATE(),
    [Updated] [datetime],
    [Active] [bit],
    CONSTRAINT [PK_dbo.Account] PRIMARY KEY ([ID])
)
CREATE TABLE [dbo].[EthereumContract] (
    [ID] [int] NOT NULL IDENTITY,
    [ContractID] [varchar](150),
    [Created] [datetime] DEFAULT GETDATE(),
    [Updated] [datetime],
    [Active] [bit],
    [Account_ID] [int],
    CONSTRAINT [PK_dbo.EthereumContract] PRIMARY KEY ([ID])
)
CREATE INDEX [IX_Account_ID] ON [dbo].[EthereumContract]([Account_ID])
ALTER TABLE [dbo].[EthereumContract] ADD CONSTRAINT [FK_dbo.EthereumContract_dbo.Account_Account_ID] FOREIGN KEY ([Account_ID]) REFERENCES [dbo].[Account] ([ID])
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201811272301085_InitialDatabase', N'SmartContractEthereum.Infrastructure.Data.Migrations.Configuration',  0x1F8B0800000000000400ED5ACD6EE33610BE17E83B083A1659CBCE22C036B077917592C2E8E60771B2E82D60A4B143542255924A6D147DB21EFA487D850E65FD92922C3B49B15B2C72B1A9996F38C36F38A371FEF9EBEFF18755143A4F2024E56CE28E0643D701E6F380B2E5C44DD4E2CD3BF7C3FBEFBF1B9F05D1CAF99CCBBDD572A8C9E4C47D542A3EF63CE93F4244E420A2BEE0922FD4C0E7914702EE1D0E873F7AA3910708E12296E38C6F12A66804E917FC3AE5CC87582524BCE00184325BC727F314D5B92411C898F83071E711110A159420BE3A538F2020890633B610442A91F82A113038258A0CAEF56EA54277C0754E424A70B3730817AE4318E38A2874E5F84EC25C09CE96F318174878BB8E01E516249490B9785C8AF7F57678A8BDF54AC51CCA4FA4E2D18E80A3B759F83C537DAF43708BF06280CFF020D45A7B9D0679E29EF83EC7D3711DD3D6F134145AAEED084E5198B2410A48410E2E08234B10830CF0C0E9523B28B88514D47F07CE3409F5594E1824A8111E38D7C94348FD9F617DCB7F0536614918565D4167F0596D0197AE058F41A8F50D2C320767A7AEE3D5F53C53B150ABE86C7C9F31F5F6D0752ED1387908A1604A254E73C505FC040C0451105C13A540E041CF0248636D59376CE5913909020152E68691A59893AE7341569F802DD5E3C4C58FAE734E5710E42BD966EE18C51446254C08D866EF135F52D661E5F0A897112B22DD56AF8994BF731174183E1ABE86E1A9007D2CB95DBC29E016AFA26D6A7771B08FDA89AFE813E45A1F390F81B0BAD2D82B73B033337362E429F4FC143511BFE568CF1CCD2354DA6CE0EFA82781BF32BEA2D22579A2CB3490967A563C6E204C9FCB471A6FAAAE45B6FB42F85CF0E886870D14CF65EEE73C11BA8CDFF22D82B7442C41F5CBB01329B94FD37DB6A458B1C7BAFF672C70FA6E78C38EA6002059307D688C09839B9BB83F5871EE6127F7B762A7886B1D7E38188CCCB85422605F3DDA16A63D882C388D7783FE0EABA6BB083BABEC3A9219D34DE7B4913928933BE561990E59E1A923D801B6A06C1103B3129176E092BA15F16D34376FBD1D485438DAE4A3B70770CE9A0A705B90BD7A441AD2AAE049D9B67B9BBE3DEFEFBD96067F7C41E218AFCD4AC39FAD38F34DB73F7D33DFBDC78D36189E2F1B5ADD62B785252C05D8A81A4FF51D1FC0391552E9978907A22FEE691059629D59D1C2D7DC749DF8F639E6E4CDE5F5E78E5CDCF6163468B15486FD1C231161094C83020DECB034D377341212D1508DA73C4C22D656D1BBB4AD1EB80A653DEC8F9BF5BA55B46CA93F46D9B95661CAD5FE484569AF02158BFD718A5A5FC52916FBE3E4C5BF0A93AFD92863CF208DC95FCF22B071B398F9D02B5BAC0BF0F5D3669BC91EF9B31DE27512A9DAA8D6585659FF46D83A21F7256CBD5076B3B6EC205E9A9C25F2AE1CD48D43CB3DDCD640D831EEC5E11CA5897D3ACAC5369EB1C3AC77DA73873BEF0CED07347D939849FDE259BCECF5F3DB6CB36C8259DD962952D0BBE8BA8CEE6A9C753ADB67AC56EBB311711DF4FE890669DBB346FE451B3ACE7F0BA72105CDB95CE08230BA00A9366300F770383A3466B05FCE3CD4933208F71D8ACE5800AB89FB87F3E77F3FE7A04596774D32769C8FB58C1F9F88F01F89A80D38DE0D87C32AFECED3C626503D6E7CE62CB109F668F8DC49A1AE4E2A1DA1ECE6B13189D917A63E9979A0AA1BE025A78AFF3B8EDBE3BB26CA8C8E7666F7574D992680B25E558EA39D205AEDD899FD725F6A1E3857020BC2B13344FAEC45D91718D3D5BAAD5DC7649D2395674EF8304D5083E95F1E5109DB3C2CED565F7A2D28F3694C42C31BBBB7E9937D3AD005A2F9E41462603AADDADCED63B3BBBD2B4C1877C2B650EC38ADB4C737CF1E3F6EBA20CCC7078EC7BEA1F1ABCE269B0C7E91134C23785D03C52DD9B4C3A8F365469376538C54ACFC7B02A684A4CB124277FA0CFC1A090B197C59E3793E183BCA458C2BF60214C1FB9D9C084517E8243EF6B1ED4A7F4BFA4CC24407297A8060C6AE1215270A5D86E821ACFD42A573AACB7E3A7FADEF797C15A73FCFBC840BB84DAA4BD415FB98D03028F67D6E97E236089DAC597DD767A9749D5FAE0BA44BCE7A0265E12BEE985B88E210C1E4159B135D0B77DFDB9D844FB024FE3A7FB76907D97E10F5B08F4F29590A12C90CA3D4C7AFC8E1205ABDFF1791BA7406A5230000 , N'6.2.0-61023')

