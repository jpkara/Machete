declare @startDate DateTime = '1/1/2016'
declare @endDate DateTime = '1/1/2017'

select 
convert(varchar(24), @startDate, 126) + '-' + convert(varchar(23), @endDate, 126) + '-' + convert(varchar(5), business) as id,
case
  when business = 0 then 'Individual'
  when business = 1 then 'Business'
end as [Business type], 
[count] as [Count]
FROM (
SELECT
business, count(*) as [count]
FROM dbo.Employers Es
WHERE Es.datecreated >= @startdate AND Es.datecreated <= @EnDdate
group by business
) as WW