declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByHouseholdComposition-' + myid as id, 
label as [Household], 
count(*) as [Count]
FROM 
(
  select  
  CASE 
	WHEN W.livewithchildren = 1 then 'Households with minors - details unknown'
	when W.livealone = 1 then 'Single person household - details unknown'
	when (W.livealone = 0 or w.livealone is null) and (W.livewithchildren = 0 or w.livewithchildren is null) then 'Household composition unknown'
  END as label,
  CASE 
	WHEN W.livewithchildren = 1 then '1'
	when W.livealone = 1 then '2'
	when (W.livealone = 0 or w.livealone is null) and (W.livewithchildren = 0 or w.livewithchildren is null) then '3'
  END as myid
  from Workers W
  JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
  WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
  group by W.ID, W.livewithchildren, W.livealone
) as WW
group by label, myid
union 
select 
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-WorkersByHouseholdComposition-TOTAL'  as id,
'Total' as [Household], 
count(*) as [Count]
from (
   select W.ID, min(dateforsignin) firstsignin
   from workers W
   JOIN dbo.WorkerSignins WSI ON W.ID = WSI.WorkerID
   WHERE dateforsignin >= @startDate and dateforsignin <= @endDate
   group by W.ID
) as WWW