# This sample is related to https://github.com/OData/odata.net/issues/3580


## Root cause

Asp.NET Core OData Minimal API so far can only handle the POCO result, and cannot process the result of aggregation ($apply, $compute...).


## Test

1) Run the ODataTestService
2) Run the ODataTestClient, click the 'load' button, you can get the message box as:



3) Run the ODataTestClient9x, you can see the following output