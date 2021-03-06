declare @startDate dateTime = '1/1/2016'
declare @endDate dateTime = '1/1/2017'

SELECT
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-ActivityAttendanceByType-' + convert(varchar(5), min(a.name)) as id,
l.text_en [Activity], 
count(*) [Count],
count(distinct(asi.dwccardnum)) [Unique workers]
from
activities a
join activitysignins asi on a.id = asi.activityid
join lookups l on l.id = a.name
where a.datestart > @startdate
and a.datestart < @enddate
group by l.text_en

union
SELECT
convert(varchar(8), @startDate, 112) + '-' + convert(varchar(8), @endDate, 112) + '-ActivityAttendanceByType-TOTAL' as id,
'Total' as [Activity], 
count(*) [Count],
count(distinct(asi.dwccardnum)) [Unique workers]
from
activities a
join activitysignins asi on a.id = asi.activityid
join lookups l on l.id = a.name
where a.datestart > @startdate
and a.datestart < @enddate
order by count desc
