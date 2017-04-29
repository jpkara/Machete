declare @startDate dateTime = '1/1/2016'
declare @endDate dateTime = '1/1/2017'

--SELECT CONVERT(VARCHAR(7), wo.dateTimeofWork, 111) yearmonth,
--	lskill.text_en  AS workType
--FROM [dbo].WorkAssignments as WA 
--join [dbo].lookups as lskill on (wa.skillid = lskill.id)
--join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
--join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
--WHERE wo.dateTimeOfWork < (@end) 
--and wo.dateTimeOfWork > (@start)
--and lstatus.text_en = 'Completed'


SELECT
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), min(wa.skillid)) as id,
lskill.text_en  AS label,
count(lskill.text_en) value
FROM [dbo].WorkAssignments as WA 
join [dbo].lookups as lskill on (wa.skillid = lskill.id)
join [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)
join [dbo].lookups as lstatus on (WO.status = lstatus.id) 
WHERE wo.dateTimeOfWork < (@endDate) 
and wo.dateTimeOfWork > (@startDate)
and lstatus.text_en = 'Completed'
group by lskill.text_en
