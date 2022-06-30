select * from BookInfo

select * from IssueBooks

select * from studentInfos

Update IssueBooks set returnDate = '2022-06-29' where stID = '12345' and bkID = 1
Update IssueBooks set returnDate = null where stID = 12345 and bkID = 1

 Select IB.stID as ID, stName as 'Student Name', bkName as 'Book Name', bkAuthor as 'Author',   
                     issueDate as 'Issue Date', returnDate as 'Return Date', bkQuantity as 'Quantity' from IssueBooks as IB  
                     join StudentInfos as SI ON IB.stID = SI.stID  
                     join BookInfo as BI ON IB.bkID = BI.bkID   
                     where returnDate is null

 Select IB.stID as ID, stName as 'Student Name', bkName as 'Book Name', bkAuthor as 'Author',   
                     issueDate as 'Issue Date', returnDate as 'Return Date', bkQuantity as 'Quantity' from IssueBooks as IB  
                     join StudentInfos as SI ON IB.stID = SI.stID  
                     join BookInfo as BI ON IB.bkID = BI.bkID  
                     where returnDate is not null