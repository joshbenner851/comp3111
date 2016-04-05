drop table dbo.Application;

create table [dbo].[Account](
	[applicationID]		int	NOT NULL	IDENTITY (1,1) PRIMARY KEY, 
	[firstName] 		NCHAR(35)	NOT NULL,
	[lastName]			NCHAR(35)	NOT NULL,
	[dateOfBirth]		DATE 		NOT NULL,
	[email]				NCHAR(50)	NOT NULL,
	[building]			NCHAR(50)	NOT NULL,
	[street]			NCHAR(35)	NOT NULL,
	[district]			NCHAR(19) 	NOT NULL,
	[homePhone]			NCHAR(8)	NOT NULL,
	[homeFax]			NCHAR(8)	NOT NULL,
	[businessPhone] 	NCHAR(8)	NOT NULL,
	[mobilePhone]		NCHAR(8)	NOT NULL,
	[citizenship]		NCHAR(70)	NOT NULL,
	[legalResidence]	NCHAR(70) 	NOT NULL,
	[HKIDPassportNumber]		NCHAR(8)	NOT NULL,
	[passportCountry]		NCHAR(70) 	NOT NULL,

	[coFirstName] 		NCHAR(35)	 NULL,
	[coLastName]			NCHAR(35)	 NULL,
	[coDateOfBirth]		DATE 		 NULL,
	[coEmail]				NCHAR(50)	 NULL,
	[coBuilding]			NCHAR(50)	 NULL,
	[coStreet]			NCHAR(35)	 NULL,
	[coDistrict]			NCHAR(19) 	 NULL,
	[coHomePhone]			NCHAR(8)	 NULL,
	[coHomeFax]			NCHAR(8)	 NULL,
	[coBusinessPhone] 	NCHAR(8)	 NULL,
	[coMobilePhone]		NCHAR(8)	 NULL,
	[coCitizenship]		NCHAR(70)	 NULL,
	[coLegalResidence]	NCHAR(70) 	 NULL,
	[coHKIDPassportNumber]		NCHAR(8) NULL,
	[coPassportCountry]		NCHAR(70)  NULL,

	[employeeStatus]		NCHAR(13)	NOT NULL,
	[occupation]		NCHAR(20)	NOT NULL,
	[years]				NCHAR(2)	NOT NULL,
	[employerName]		NCHAR(25)	NOT NULL,
	[employerPhone]		NCHAR(8)	NOT NULL,
	[natureBusiness]	NCHAR(20) 	NOT NULL,

	[coEmployeeStatus]		NCHAR(13)	 NULL,
	[coOccupation]		NCHAR(20)	 NULL,
	[coYears]				NCHAR(2)	 NULL,
	[coEmployerName]		NCHAR(25)	 NULL,
	[coEmployerPhone]		NCHAR(8)	 NULL,
	[coNatureBusiness]	NCHAR(20) 	 NULL,

	[isEmployedFinance]		BIT			NOT NULL,
	[isPubliclyTraded]		BIT 		NOT NULL,
	

	[coIsEmployedFinance]	BIT NULL,
	[coIsPubliclyTraded]		BIT 		 NULL,

	[primaryFundingSource]	NCHAR(30)	NOT NULL,
	[otherInformation]		NCHAR(50)	NULL,

	[investmentObjective]	NCHAR(20)	NOT NULL,
	[investmentKnowledge]	NCHAR(9)	NOT NULL,
	[investmentExperience]	NCHAR(9)	NOT NULL,
	[annualIncome]			NCHAR(30)	NOT NULL,
	[liquidNetWorth]		NCHAR(30) 	NOT NULL,

	[accountFeature]	BIT 		NOT NULL,

	[chequeAmount]			NCHAR(12) 		NULL,
	[accountTransferForm]	NCHAR(12) 	NULL,

	[signedOn]			DATE 		NOT NULL,
	[coSignedOn]		DATE		NULL

);