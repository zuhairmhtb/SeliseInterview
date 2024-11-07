# Write your MySQL query statement below
with report as (
    select id, 
    recordDate, 
    temperature, 
    lag(temperature) over (w) as prevTemperature,
    lag(recordDate) over (w) as prevDate
    from Weather window w as (order by recordDate)
) 
select id from report where 
temperature > prevTemperature and 
DateDiff(recordDate, prevDate) = 1;