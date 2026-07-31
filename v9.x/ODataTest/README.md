# This sample is related to https://github.com/OData/odata.net/issues/3580


## Root cause

Asp.NET Core OData Minimal API so far can only handle the POCO result, and cannot process the result of aggregation ($apply, $compute...).


## Test

1) Run the ODataTestService
<img width="1720" height="625" alt="image" src="https://github.com/user-attachments/assets/8c523c4b-2f31-4769-9301-537978be65a7" />
  
2) Run the ODataTestClient, click the 'load' button, you can get the message box as:
<img width="1132" height="440" alt="image" src="https://github.com/user-attachments/assets/a23e6df2-4215-4eef-a400-df61c150da99" />

3) Run the ODataTestClient9x, you can see the following output
<img width="1660" height="217" alt="image" src="https://github.com/user-attachments/assets/9138b15e-2b39-4a09-ba9d-ea1e48d30526" />

   
