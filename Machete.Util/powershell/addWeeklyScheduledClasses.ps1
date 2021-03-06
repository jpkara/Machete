##
##
## global variables
$dayNumber = @{
“Sunday” = 0
“Monday” = 1
“Tuesday” = 2
“Wednesday” = 3
“Thursday” = 4
“Friday” = 5
“Saturday” = 6
}
$defOffset = 0
$database = "machete"
$server = ".\SQLEXPRESS"
#$database = "machete" 
$insert_query = "INSERT INTO " + $database + ".dbo.Activities VALUES (@name, @type, @dateStart, @dateEnd, @teacher, @notes, @datecreated, @dateupdated, @Createdby, @Updatedby)"
if ($args.count -eq 0) {
    Write-host "CSV required as first argument"
    exit
    }

$file = $args[0]
$file

Function GetLookups{
    param ($category)
    #use the display name of the user to get their login name


    $select_activityNames = @"
SELECT id, text_EN 
FROM Lookups
WHERE category = '$($category)'
"@

    $adapter = new-object system.data.sqlclient.sqldataadapter ($select_activityNames, $connection)
    $table = new-object system.data.datatable
    $adapter.Fill($table) | out-null
    $array = @{}
    $table | Foreach-Object {
        $row = $_;
        $array.add($row.text_EN, $row.id)
    }
    return @($array)
}

Function get-FutureDate
{
    param([string]$day,[int]$offset,[string]$time)
    ##
    ## Date gymnastics to get datetime object with future times
    $today = get-date
    $dayIncr = $today.DayOfWeek.value__ + $offset + $dayNumber[$day]
    $schedDate = $today.AddDays($dayIncr)
    $scYear = $schedDate.Year
    $scMonth = $schedDate.Month
    $scDay = $schedDate.Day
    ##
    ## Re-constitute datetimme
    $startDateString = [String]::Format("{0}{1:d2}{2:d2} {3}", $scYear,$scMonth,$scDay,$time)
    $finalDate = [datetime]::ParseExact($startDateString, "yyyyMMdd %H:mm:ss", $null)
    $finalDate
}

function MAIN
{
    
    $connection=new-object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString="Server={0};Database={1};Integrated Security=True" -f $server,$database
    
    $activityNames = GetLookups 'activityName'
    $activityTypes = getlookups 'activityType'
    
    $cmd=new-object system.Data.SqlClient.SqlCommand($insert_query,$connection)
    $cmd.CommandTimeout=120
    $cmd.parameters.Add("@name", [System.Data.SqlDbType]"Int")| Out-Null
    $cmd.parameters.Add("@type", [System.Data.SqlDbType]"Int")| Out-Null
    $cmd.parameters.Add("@dateStart", [System.Data.SqlDbType]"DateTime")| Out-Null
    $cmd.parameters.Add("@dateEnd", [System.Data.SqlDbType]"DateTime")| Out-Null
    $cmd.parameters.Add("@teacher", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
    $cmd.parameters.Add("@notes", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
    $cmd.parameters.Add("@datecreated", [System.Data.SqlDbType]"DateTime")| Out-Null
    $cmd.parameters.Add("@dateupdated", [System.Data.SqlDbType]"DateTime")| Out-Null
    $cmd.parameters.Add("@Createdby", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
    $cmd.parameters.Add("@Updatedby", [System.Data.SqlDbType]"NVarChar", 4000)| Out-Null
    
    $records = Import-Csv $file -Delimiter ","
    $connection.Open()
    $records | Foreach-Object {
        $schedDate = get-date
        $row = $_;
        ##
        ## Date gymnastics to get datetime object with future times
        $startDate = get-FutureDate $row.day $defOffset $row.startTime 
        $endDate = get-FutureDate $row.day $defOffset $row.endTime
        
        $cmd.Parameters["@name"].Value = [int]$activityNames[$row.name]
        $cmd.Parameters["@type"].Value = [int]$activityTypes[$row.type]
        $cmd.Parameters["@dateStart"].Value = $startDate
        $cmd.Parameters["@dateEnd"].Value = $endDate
        $cmd.Parameters["@teacher"].Value = "volunteer"
        $cmd.Parameters["@notes"].Value = [DBNull]::Value
        $cmd.Parameters["@datecreated"].Value = get-date
        $cmd.Parameters["@dateupdated"].Value = get-date
        $cmd.Parameters["@Createdby"].Value = "PowerShellImport"
        $cmd.Parameters["@Updatedby"].Value = "PowerShellImport"
        $cmd.ExecuteNonQuery()
    }
    $connection.Close();
}
MAIN
