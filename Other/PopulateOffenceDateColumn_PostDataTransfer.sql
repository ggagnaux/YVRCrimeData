use YvrCrimeData
go


Update Crime set hour = 0 where hour is null
Go

update crime set minute = 0 where minute is null
Go


Update Crime set OffenceDate = DATETIMEFROMPARTS(Year, Month, Day, Hour, Minute, 0, 0)
where hour is not null and minute is not null
Go

