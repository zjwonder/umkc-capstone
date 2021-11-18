/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [ID]
      ,[customerID]
      ,[actID]
      ,[actType]
      ,[onDate]
      ,[balance]
      ,[transType]
      ,[amount]
      ,[description]
      ,[userEntered]
      ,[category]
  FROM [CommerceBankProject].[dbo].[Transaction]
  order by onDate;


Insert into [Transaction] (customerID, actID,actType,onDate,balance,transType,amount,[description],userEntered,category) VALUES ('999999999','411111111','Checking','2024-10-02',999999,'DR',-292,'starting amount',0,'Income');

SELECT   *, RunningTotal = SUM(amount )    OVER (  order by onDate rows unbounded preceding  )
FROM dbo.[Transaction]
where customerID = '999999999' and transType = 'CR' and actID = '411111111'

Union


SELECT   *, RunningTotal = SUM(-amount )   OVER (  order by onDate rows unbounded preceding  )
FROM dbo.[Transaction]
where customerID = '999999999' and transType = 'DR'and actID = '411111111'
order by onDate;

  CREATE TRIGGER update_balances ON [Transactions] for insert
FOR EACH
ROW
BEGIN
    UPDATE Customers SET balance = balance + NEW.Amount
      WHERE Customers.id = NEW.custid;
END;



